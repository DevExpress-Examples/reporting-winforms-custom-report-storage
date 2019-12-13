Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Windows.Forms
Imports DevExpress.XtraReports.UserDesigner
Imports DevExpress.XtraReports.UI
' ...

Namespace ReportStorageSample
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
		End Sub


		Private Sub buttonDesign_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonDesign.Click
			' Open a selected report in the report designer.
			Dim form As New XRDesignForm()
			Dim url As String = GetSelectedUrl()
			If (Not String.IsNullOrEmpty(url)) Then
				form.OpenReport(url)
			End If
			form.ShowDialog(Me)

			Dim selectedItem As Object = listBox1.SelectedItem
			FillListBox()
			If selectedItem IsNot Nothing AndAlso listBox1.Items.Contains(selectedItem) Then
				listBox1.SelectedItem = selectedItem
			End If
		End Sub

		Private Sub buttonPreview_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonPreview.Click
			' Show a preview for a selected report.
			Dim report As XtraReport = GetSelectedReport()
			If report IsNot Nothing Then
				report.ShowPreviewDialog()
			End If
		End Sub
		Private Function GetSelectedUrl() As String
			Return TryCast(listBox1.SelectedItem, String)
		End Function
		Private Function GetSelectedReport() As XtraReport
			' Return a report by a URL selected in the ListBox.
			Dim url As String = GetSelectedUrl()
			If String.IsNullOrEmpty(url) Then
				Return Nothing
			End If
			Using stream As New MemoryStream(Program.ReportStorage.GetData(url))
				Return XtraReport.FromStream(stream, True)
			End Using
		End Function
		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			FillListBox()
			If listBox1.Items.Count > 0 Then
				listBox1.SelectedIndex = 0
			End If
		End Sub
		Private Sub FillListBox()
			listBox1.Items.Clear()
			Dim urls() As String = Program.ReportStorage.GetStandardUrls(Nothing)
			For Each url As String In urls
				listBox1.Items.Add(url)
			Next url
		End Sub
		Private Sub listBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles listBox1.SelectedIndexChanged
			buttonPreview.Enabled = listBox1.SelectedItem IsNot Nothing
		End Sub
	End Class
End Namespace
