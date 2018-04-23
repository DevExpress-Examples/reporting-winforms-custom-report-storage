Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Collections.Generic
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraReports.Extensions
' ...

Namespace ReportStorageSample
	Friend Class XpoReportStorage
		Inherits ReportStorageExtension
		Private items_Renamed As XPCollection(Of StorageItem)
		Private ReadOnly Property Session() As UnitOfWork
			Get
				Return CType(items_Renamed.Session, UnitOfWork)
			End Get
		End Property
		Public Function FindItem(ByVal name As String) As StorageItem
			Return Session.FindObject(Of StorageItem)(New BinaryOperator("Url", name))
		End Function
		Public ReadOnly Property Items() As XPCollection(Of StorageItem)
			Get
				Return items_Renamed
			End Get
		End Property
		Public Sub New(ByVal session As UnitOfWork)
			items_Renamed = New XPCollection(Of StorageItem)(session)
		End Sub

		Public Overrides Function CanSetData(ByVal url As String) As Boolean
			' Always return true to confirm that the SetData method is available.
			Return True
		End Function
		Public Overrides Function IsValidUrl(ByVal url As String) As Boolean
			Return Not String.IsNullOrEmpty(url)
		End Function
		Public Overrides Function GetData(ByVal url As String) As Byte()
			' Get a StorageItem containing the report.
			Dim item As StorageItem = FindItem(url)
			If item IsNot Nothing Then
				Return item.Layout
			End If
			Return New Byte() { }
		End Function

		Public Overrides Sub SetData(ByVal report As XtraReport, ByVal url As String)
			' Write the report to a corresponding StorageItem.
			' If a StorageItem with a specified Url property value does not exist, create a new one.
			Dim item As StorageItem = FindItem(url)
			If item IsNot Nothing Then
				item.Layout = GetBuffer(report)
			Else
				item = New StorageItem(Session)
				item.Url = url
				Session.CommitChanges()

				report.Extensions("StorageID") = item.Oid.ToString()
				item.Layout = GetBuffer(report)
			End If
			Session.CommitChanges()
			items_Renamed.Reload()
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
			If form.ShowDialog() = DialogResult.OK Then
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
			If form.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
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
			For Each item As StorageItem In Items
				If method Is Nothing OrElse method(item.Oid.ToString()) Then
					list.Add(item.Url)
				End If
			Next item
			Return list
		End Function
	End Class
	Public Class StorageItem
		Inherits XPObject
		Private url_Renamed As String
		Private layout_Renamed() As Byte = Nothing
		Public Property Url() As String
			Get
				Return url_Renamed
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Url", url_Renamed, value)
			End Set
		End Property
		Public Property Layout() As Byte()
			Get
				Return layout_Renamed
			End Get
			Set(ByVal value As Byte())
				SetPropertyValue("Layout", layout_Renamed, value)
			End Set
		End Property
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
	End Class
End Namespace
