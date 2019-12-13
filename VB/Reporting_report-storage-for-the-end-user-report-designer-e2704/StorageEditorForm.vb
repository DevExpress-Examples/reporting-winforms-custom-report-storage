Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
' ...

Namespace ReportStorageSample
	Partial Public Class StorageEditorForm
		Inherits Form
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub listBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles listBox1.SelectedIndexChanged
			textBox1.Text = listBox1.SelectedItem.ToString()
		End Sub

		Private Sub StorageEditorForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			If listBox1.Items.Count > 0 AndAlso String.IsNullOrEmpty(textBox1.Text) Then
				listBox1.SelectedIndex = 0
			End If
		End Sub

		Private Sub textBox1_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles textBox1.TextChanged
			buttonOK.Enabled = Not String.IsNullOrEmpty(textBox1.Text)
		End Sub

		Private Sub buttonOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonOK.Click

		End Sub
	End Class
End Namespace
