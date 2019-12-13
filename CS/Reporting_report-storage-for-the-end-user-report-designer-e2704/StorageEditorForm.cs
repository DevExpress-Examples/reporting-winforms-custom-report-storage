using System;
using System.Windows.Forms;
// ...

namespace ReportStorageSample {
    public partial class StorageEditorForm : Form {
        public StorageEditorForm() {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {
            textBox1.Text = listBox1.SelectedItem.ToString();
        }

        private void StorageEditorForm_Load(object sender, EventArgs e) {
            if (listBox1.Items.Count > 0 && string.IsNullOrEmpty(textBox1.Text))
                listBox1.SelectedIndex = 0;
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            buttonOK.Enabled = !string.IsNullOrEmpty(textBox1.Text);
        }

        private void buttonOK_Click(object sender, EventArgs e) {

        }
    }
}
