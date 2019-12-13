Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.IO.Compression
Imports System.Collections.Generic
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraReports.Extensions
Imports DevExpress.Utils.Zip
' ...

Namespace ReportStorageSample
	Friend Class ZipReportStorage
		Inherits ReportStorageExtension
		Private Class ZipFilesHelper
			Implements IDisposable
			Private stream As Stream
			Private zipFiles_Renamed As New InternalZipFileCollection()
			Public ReadOnly Property ZipFiles() As InternalZipFileCollection
				Get
					Return zipFiles_Renamed
				End Get
			End Property
			Public Sub New(ByVal path As String)
				If File.Exists(path) Then
					stream = File.OpenRead(path)
					zipFiles_Renamed = InternalZipArchive.Open(stream)
				End If
			End Sub
			Public Overridable Sub Dispose() Implements IDisposable.Dispose
				If stream IsNot Nothing Then
					stream.Dispose()
				End If
			End Sub
		End Class
		Private Const fileName As String = "ReportStorage.zip"
		Public Sub New()
		End Sub
		Private ReadOnly Property StoragePath() As String
			Get
				Dim dirName As String = Path.GetDirectoryName(Application.ExecutablePath)
				Return Path.Combine(dirName, fileName)
			End Get
		End Property
		Public Overrides Function CanSetData(ByVal url As String) As Boolean
			' Always return true to confirm that the SetData method is available.
			Return True
		End Function
		Public Overrides Function IsValidUrl(ByVal url As String) As Boolean
			Return Not String.IsNullOrEmpty(url)
		End Function
		Public Overrides Function GetData(ByVal url As String) As Byte()
			' Open ZIP archive.
			Using helper As New ZipFilesHelper(StoragePath)
				' Read a file with a specified URL from the archive.
				Dim zipFile As InternalZipFile = GetZipFile(helper.ZipFiles, url)
				If zipFile IsNot Nothing Then
					Return GetBytes(zipFile)
				End If
				Return New Byte() { }
			End Using
		End Function
		Private Shared Function GetBytes(ByVal zipFile As InternalZipFile) As Byte()
			Return GetBytes(zipFile.FileDataStream, CInt(Fix(zipFile.UncompressedSize)))
		End Function
		Private Shared Function GetBytes(ByVal stream As Stream, ByVal length As Integer) As Byte()
			Dim result(length - 1) As Byte
			stream.Read(result, 0, result.Length)
			Return result
		End Function
		Private Shared Function GetZipFile(ByVal zipFiles As InternalZipFileCollection, ByVal url As String) As InternalZipFile
			For Each item As InternalZipFile In zipFiles
				If StringsEgual(item.FileName, url) Then
					Return item
				End If
			Next item
			Return Nothing
		End Function
		Private Shared Function StringsEgual(ByVal a As String, ByVal b As String) As Boolean
			Return String.Equals(a, b, StringComparison.OrdinalIgnoreCase)
		End Function
		Public Overrides Sub SetData(ByVal report As XtraReport, ByVal url As String)
			report.Extensions("StorageID") = url
			SaveArchive(url, GetBuffer(report))
		End Sub
		Private Sub SaveArchive(ByVal url As String, ByVal buffer() As Byte)
			Dim tempPath As String = Path.ChangeExtension(StoragePath, "tmp")
			' Create a new ZIP archive.
			Using arch As New InternalZipArchive(tempPath)
				' Open a ZIP archive where report files are stored.
				Using helper As New ZipFilesHelper(StoragePath)
					Dim added As Boolean = False
					' Copy all report files to a new archive.
					' Update a file with a specified URL.
					' If the file does not exist, create it.
					For Each item As InternalZipFile In helper.ZipFiles
						If StringsEgual(item.FileName, url) Then
							arch.Add(item.FileName, DateTime.Now, buffer)
							added = True
						Else
							arch.Add(item.FileName, DateTime.Now, GetBytes(item))
						End If
					Next item
					If (Not added) Then
						arch.Add(url, DateTime.Now, buffer)
					End If
				End Using
			End Using
			' Replace the old ZIP archive with the new one.
			If File.Exists(StoragePath) Then
				File.Delete(StoragePath)
			End If
			File.Move(tempPath, StoragePath)
		End Sub
		Private Function GetBuffer(ByVal report As XtraReport) As Byte()
			Using stream As New MemoryStream()
				report.SaveLayout(stream)
				Return stream.ToArray()
			End Using
		End Function
		Public Overrides Function GetNewUrl() As String
			' Show the report selection dialog and return a URL for a selected report.
			Dim form As StorageEditorForm = CreateForm()
			form.textBox1.Enabled = False
			If form.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
				Return form.textBox1.Text
			End If
			Return String.Empty
		End Function
		Private Function CreateForm() As StorageEditorForm
			Dim form As New StorageEditorForm()
			For Each item As String In GetUrls()
				form.listBox1.Items.Add(item)
			Next item
			Return form
		End Function
		Public Overrides Function SetNewData(ByVal report As XtraReport, ByVal defaultUrl As String) As String
			Dim form As StorageEditorForm = CreateForm()
			form.textBox1.Text = defaultUrl
			form.listBox1.Enabled = False
			' Show the save dialog to get a URL for a new report.
			If form.ShowDialog() = DialogResult.OK Then
				Dim url As String = form.textBox1.Text
				If (Not String.IsNullOrEmpty(url)) AndAlso (Not form.listBox1.Items.Contains(url)) Then
					TypeDescriptor.GetProperties(GetType(XtraReport))("DisplayName").SetValue(report, url)
					SetData(report, url)
					Return url
				Else
					MessageBox.Show("Incorrect report name", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
				End If
			End If
			Return String.Empty
		End Function
		Public Overrides Function GetStandardUrlsSupported(ByVal context As ITypeDescriptorContext) As Boolean
			' Always return true to confirm that the GetStandardUrls method is available.
			Return True
		End Function
		Public Overrides Function GetStandardUrls(ByVal context As ITypeDescriptorContext) As String()
			If context IsNot Nothing AndAlso TypeOf context.Instance Is XRSubreport Then
				Dim xrSubreport As XRSubreport = TryCast(context.Instance, XRSubreport)
				If xrSubreport.RootReport IsNot Nothing AndAlso xrSubreport.RootReport.Extensions.TryGetValue("StorageID", storageID) Then
					Dim result As List(Of String) = GetUrlsCore(AddressOf CanPassId)
					Return result.ToArray()
				End If
			End If
			Return GetUrls()
		End Function
		Private storageID As String
		Private Function CanPassId(ByVal id As String) As Boolean
			Return id <> storageID
		End Function
		Private Function GetUrls() As String()
			Return GetUrlsCore(Nothing).ToArray()
		End Function
		Private Function GetUrlsCore(ByVal method As Predicate(Of String)) As List(Of String)
			Dim list As New List(Of String)()
			Using helper As New ZipFilesHelper(StoragePath)
				For Each item As InternalZipFile In helper.ZipFiles
					If method Is Nothing OrElse method(item.FileName) Then
						list.Add(item.FileName)
					End If
				Next item
				Return list
			End Using
		End Function
	End Class
End Namespace
