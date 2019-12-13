namespace ReportStorageSample {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.buttonDesign = new System.Windows.Forms.Button();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // buttonDesign
            // 
            this.buttonDesign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDesign.Location = new System.Drawing.Point(50, 196);
            this.buttonDesign.Name = "buttonDesign";
            this.buttonDesign.Size = new System.Drawing.Size(123, 31);
            this.buttonDesign.TabIndex = 2;
            this.buttonDesign.Text = "Show Designer";
            this.buttonDesign.UseVisualStyleBackColor = true;
            this.buttonDesign.Click += new System.EventHandler(this.buttonDesign_Click);
            // 
            // buttonPreview
            // 
            this.buttonPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPreview.Enabled = false;
            this.buttonPreview.Location = new System.Drawing.Point(192, 196);
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Size = new System.Drawing.Size(121, 31);
            this.buttonPreview.TabIndex = 0;
            this.buttonPreview.Text = "Show Preview";
            this.buttonPreview.UseVisualStyleBackColor = true;
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 18);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(301, 160);
            this.listBox1.TabIndex = 1;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 239);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.buttonPreview);
            this.Controls.Add(this.buttonDesign);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonDesign;
        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.ListBox listBox1;
    }
}

