Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Data
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Collections.Generic
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraReports.Extensions
' ...

Namespace ReportStorageSample
	Friend Class DataSetReportStorage
		Inherits ReportStorageExtension
		Private Const fileName As String = "ReportStorage.xml"
		Private dataSet_Renamed As StorageDataSet
		Public Sub New()
		End Sub
		Private ReadOnly Property StoragePath() As String
			Get
				Dim dirName As String = Path.GetDirectoryName(Application.ExecutablePath)
				Return Path.Combine(dirName, fileName)
			End Get
		End Property
		Private ReadOnly Property DataSet() As StorageDataSet
			Get
				If dataSet_Renamed Is Nothing Then
					dataSet_Renamed = New StorageDataSet()
					' Populate a dataset from an XML file specified in fileName.
					If File.Exists(StoragePath) Then
						dataSet_Renamed.ReadXml(StoragePath, XmlReadMode.ReadSchema)
					End If
				End If
				Return dataSet_Renamed
			End Get
		End Property
		Private ReadOnly Property ReportStorage() As StorageDataSet.ReportStorageDataTable
			Get
				Return DataSet.ReportStorage
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
			' Get a dataset row containing the report.
			Dim row As StorageDataSet.ReportStorageRow = FindRow(url)
			If row IsNot Nothing Then
				Return row.Buffer
			End If
			Return New Byte() { }
		End Function
		Private Function FindRow(ByVal url As String) As StorageDataSet.ReportStorageRow
			Dim result() As DataRow = ReportStorage.Select(String.Format("Url = '{0}'", url))
			If result.Length > 0 Then
				Return TryCast(result(0), StorageDataSet.ReportStorageRow)
			End If
			Return Nothing
		End Function
		Public Overrides Sub SetData(ByVal report As XtraReport, ByVal url As String)
			Dim row As StorageDataSet.ReportStorageRow = FindRow(url)
			' Write the report to a corresponding row in the dataset.
			' If a row with a specified URL field value does not exist, create a new one.
			If row IsNot Nothing Then
				row.Buffer = GetBuffer(report)
			Else
				Dim id As Integer = ReportStorage.Rows.Count
				report.Extensions("StorageID") = id.ToString()
				row = ReportStorage.AddReportStorageRow(id, url, GetBuffer(report))
			End If
			DataSet.WriteXml(StoragePath, XmlWriteMode.WriteSchema)
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

		' The following code is intended to support selection of a value for 
		' the Report Source Url property of Subreport controls.
		' (Use this code to avoid assigning the master report as a 
		' detail report's source.)

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
			For Each row As StorageDataSet.ReportStorageRow In ReportStorage.Rows
				If method Is Nothing OrElse method(row.ID.ToString()) Then
					list.Add(row.Url)
				End If
			Next row
			Return list
		End Function
	End Class
End Namespace
