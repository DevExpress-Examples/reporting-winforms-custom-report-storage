Imports Microsoft.VisualBasic
Imports System
Namespace ReportStorageSample
	Partial Public Class StorageEditorForm
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
			Me.panel1 = New System.Windows.Forms.Panel()
			Me.listBox1 = New System.Windows.Forms.ListBox()
			Me.panel3 = New System.Windows.Forms.Panel()
			Me.textBox1 = New System.Windows.Forms.TextBox()
			Me.label1 = New System.Windows.Forms.Label()
			Me.panel2 = New System.Windows.Forms.Panel()
			Me.buttonCancel = New System.Windows.Forms.Button()
			Me.buttonOK = New System.Windows.Forms.Button()
			Me.panel1.SuspendLayout()
			Me.panel3.SuspendLayout()
			Me.panel2.SuspendLayout()
			Me.SuspendLayout()
			' 
			' panel1
			' 
			Me.panel1.Controls.Add(Me.listBox1)
			Me.panel1.Controls.Add(Me.panel3)
			Me.panel1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.panel1.Location = New System.Drawing.Point(0, 0)
			Me.panel1.Name = "panel1"
			Me.panel1.Padding = New System.Windows.Forms.Padding(5, 5, 5, 0)
			Me.panel1.Size = New System.Drawing.Size(604, 301)
			Me.panel1.TabIndex = 0
			' 
			' listBox1
			' 
			Me.listBox1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.listBox1.FormattingEnabled = True
			Me.listBox1.IntegralHeight = False
			Me.listBox1.Location = New System.Drawing.Point(5, 5)
			Me.listBox1.Name = "listBox1"
			Me.listBox1.Size = New System.Drawing.Size(594, 271)
			Me.listBox1.Sorted = True
			Me.listBox1.TabIndex = 0
'			Me.listBox1.SelectedIndexChanged += New System.EventHandler(Me.listBox1_SelectedIndexChanged);
			' 
			' panel3
			' 
			Me.panel3.Controls.Add(Me.textBox1)
			Me.panel3.Controls.Add(Me.label1)
			Me.panel3.Dock = System.Windows.Forms.DockStyle.Bottom
			Me.panel3.Location = New System.Drawing.Point(5, 276)
			Me.panel3.Name = "panel3"
			Me.panel3.Padding = New System.Windows.Forms.Padding(0, 5, 0, 0)
			Me.panel3.Size = New System.Drawing.Size(594, 25)
			Me.panel3.TabIndex = 2
			' 
			' textBox1
			' 
			Me.textBox1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.textBox1.Location = New System.Drawing.Point(76, 5)
			Me.textBox1.Name = "textBox1"
			Me.textBox1.Size = New System.Drawing.Size(518, 20)
			Me.textBox1.TabIndex = 1
'			Me.textBox1.TextChanged += New System.EventHandler(Me.textBox1_TextChanged);
			' 
			' label1
			' 
			Me.label1.Dock = System.Windows.Forms.DockStyle.Left
			Me.label1.Location = New System.Drawing.Point(0, 5)
			Me.label1.Name = "label1"
			Me.label1.Size = New System.Drawing.Size(76, 20)
			Me.label1.TabIndex = 2
			Me.label1.Text = "Report Name :"
			Me.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
			' 
			' panel2
			' 
			Me.panel2.Controls.Add(Me.buttonCancel)
			Me.panel2.Controls.Add(Me.buttonOK)
			Me.panel2.Dock = System.Windows.Forms.DockStyle.Bottom
			Me.panel2.Location = New System.Drawing.Point(0, 301)
			Me.panel2.Name = "panel2"
			Me.panel2.Size = New System.Drawing.Size(604, 36)
			Me.panel2.TabIndex = 1
			' 
			' buttonCancel
			' 
			Me.buttonCancel.Anchor = (CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
			Me.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
			Me.buttonCancel.Location = New System.Drawing.Point(524, 6)
			Me.buttonCancel.Name = "buttonCancel"
			Me.buttonCancel.Size = New System.Drawing.Size(75, 23)
			Me.buttonCancel.TabIndex = 1
			Me.buttonCancel.Text = "Cancel"
			Me.buttonCancel.UseVisualStyleBackColor = True
			' 
			' buttonOK
			' 
			Me.buttonOK.Anchor = (CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
			Me.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK
			Me.buttonOK.Enabled = False
			Me.buttonOK.Location = New System.Drawing.Point(425, 6)
			Me.buttonOK.Name = "buttonOK"
			Me.buttonOK.Size = New System.Drawing.Size(75, 23)
			Me.buttonOK.TabIndex = 0
			Me.buttonOK.Text = "OK"
			Me.buttonOK.UseVisualStyleBackColor = True
'			Me.buttonOK.Click += New System.EventHandler(Me.buttonOK_Click);
			' 
			' StorageEditorForm
			' 
			Me.AcceptButton = Me.buttonOK
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.CancelButton = Me.buttonCancel
			Me.ClientSize = New System.Drawing.Size(604, 337)
			Me.Controls.Add(Me.panel1)
			Me.Controls.Add(Me.panel2)
			Me.Name = "StorageEditorForm"
			Me.Text = "Storage Editor"
'			Me.Load += New System.EventHandler(Me.StorageEditorForm_Load);
			Me.panel1.ResumeLayout(False)
			Me.panel3.ResumeLayout(False)
			Me.panel3.PerformLayout()
			Me.panel2.ResumeLayout(False)
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private panel1 As System.Windows.Forms.Panel
		Private panel2 As System.Windows.Forms.Panel
		Private buttonCancel As System.Windows.Forms.Button
		Private WithEvents buttonOK As System.Windows.Forms.Button
		Public WithEvents textBox1 As System.Windows.Forms.TextBox
		Public WithEvents listBox1 As System.Windows.Forms.ListBox
		Private panel3 As System.Windows.Forms.Panel
		Private label1 As System.Windows.Forms.Label
	End Class
End Namespace