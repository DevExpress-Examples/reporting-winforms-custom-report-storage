using System;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraReports.UserDesigner;
using DevExpress.XtraReports.UI;
// ...

namespace ReportStorageSample {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void buttonDesign_Click(object sender, EventArgs e) {
            XRDesignForm form = new XRDesignForm();
            string url = GetSelectedUrl();
            if (!string.IsNullOrEmpty(url))
                form.OpenReport(url);
            form.ShowDialog(this);

            object selectedItem = listBox1.SelectedItem;
            FillListBox();
            if (selectedItem != null && listBox1.Items.Contains(selectedItem))
                listBox1.SelectedItem = selectedItem;
        }

        private void buttonPreview_Click(object sender, EventArgs e) {
            XtraReport report = GetSelectedReport();
            if (report != null)
                report.ShowPreviewDialog();
        }
        string GetSelectedUrl() {
            return listBox1.SelectedItem as string;
        }
        XtraReport GetSelectedReport() {
            string url = GetSelectedUrl();
            if (string.IsNullOrEmpty(url))
                return null;
            using (MemoryStream stream = new MemoryStream(Program.ReportStorage.GetData(url))) {
                return XtraReport.FromStream(stream, true);
            }
        }
        private void Form1_Load(object sender, EventArgs e) {
            FillListBox();
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;
        }
        void FillListBox() {
            listBox1.Items.Clear();
            string[] urls = Program.ReportStorage.GetStandardUrls(null);
            foreach (string url in urls) {
                listBox1.Items.Add(url);
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {
            buttonPreview.Enabled = listBox1.SelectedItem != null;
        }
    }
}