Imports Microsoft.VisualBasic
Imports System
Namespace ReportStorageSample
	Partial Public Class Form1
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
            Me.buttonDesign = New System.Windows.Forms.Button()
            Me.buttonPreview = New System.Windows.Forms.Button()
            Me.listBox1 = New System.Windows.Forms.ListBox()
            Me.SuspendLayout()
            '
            'buttonDesign
            '
            Me.buttonDesign.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.buttonDesign.Location = New System.Drawing.Point(31, 196)
            Me.buttonDesign.Name = "buttonDesign"
            Me.buttonDesign.Size = New System.Drawing.Size(123, 31)
            Me.buttonDesign.TabIndex = 2
            Me.buttonDesign.Text = "Show Designer"
            Me.buttonDesign.UseVisualStyleBackColor = True
            '
            'buttonPreview
            '
            Me.buttonPreview.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.buttonPreview.Enabled = False
            Me.buttonPreview.Location = New System.Drawing.Point(173, 196)
            Me.buttonPreview.Name = "buttonPreview"
            Me.buttonPreview.Size = New System.Drawing.Size(121, 31)
            Me.buttonPreview.TabIndex = 0
            Me.buttonPreview.Text = "Show Preview"
            Me.buttonPreview.UseVisualStyleBackColor = True
            '
            'listBox1
            '
            Me.listBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.listBox1.FormattingEnabled = True
            Me.listBox1.Location = New System.Drawing.Point(12, 18)
            Me.listBox1.Name = "listBox1"
            Me.listBox1.Size = New System.Drawing.Size(301, 160)
            Me.listBox1.TabIndex = 1
            '
            'Form1
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(325, 239)
            Me.Controls.Add(Me.listBox1)
            Me.Controls.Add(Me.buttonPreview)
            Me.Controls.Add(Me.buttonDesign)
            Me.Name = "Form1"
            Me.Text = "Form1"
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private WithEvents buttonDesign As System.Windows.Forms.Button
		Private WithEvents buttonPreview As System.Windows.Forms.Button
		Private WithEvents listBox1 As System.Windows.Forms.ListBox
	End Class
End Namespace

