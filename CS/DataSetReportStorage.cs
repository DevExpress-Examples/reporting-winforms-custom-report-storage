using System;
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Extensions;
// ...

namespace ReportStorageSample {
    class DataSetReportStorage : ReportStorageExtension {
        const string fileName = "ReportStorage.xml";
        StorageDataSet dataSet;
        public DataSetReportStorage() {
        }
        string StoragePath {
            get {
                string dirName = Path.GetDirectoryName(Application.ExecutablePath);
                return Path.Combine(dirName, fileName);
            }
        }
        StorageDataSet DataSet {
            get {
                if (dataSet == null) {
                    dataSet = new StorageDataSet();
                    // Populate a dataset from an XML file specified in fileName.
                    if (File.Exists(StoragePath))
                        dataSet.ReadXml(StoragePath, XmlReadMode.ReadSchema);
                }
                return dataSet;
            }
        }
        StorageDataSet.ReportStorageDataTable ReportStorage {
            get {
                return DataSet.ReportStorage;
            }
        }

        public override bool CanSetData(string url) {
            // Always return true to confirm that the SetData method is available.
            return true;
        }
        public override bool IsValidUrl(string url) {
            return !string.IsNullOrEmpty(url);
        }
        public override byte[] GetData(string url) {
            // Get a dataset row containing the report.
            StorageDataSet.ReportStorageRow row = FindRow(url);
            if (row != null)
                return row.Buffer;
            return new byte[] { };
        }
        StorageDataSet.ReportStorageRow FindRow(string url) {
            DataRow[] result = ReportStorage.Select(string.Format("Url = '{0}'", url));
            if (result.Length > 0)
                return result[0] as StorageDataSet.ReportStorageRow;
            return null;
        }
        public override void SetData(XtraReport report, string url) {
            StorageDataSet.ReportStorageRow row = FindRow(url);
            // Write the report to a corresponding row in the dataset.
            // If a row with a specified URL field value does not exist, create a new one.
            if (row != null)                
                row.Buffer = GetBuffer(report);
            else {                
                int id = ReportStorage.Rows.Count;
                report.Extensions["StorageID"] = id.ToString();
                row = ReportStorage.AddReportStorageRow(id, url, GetBuffer(report));
            }
            DataSet.WriteXml(StoragePath, XmlWriteMode.WriteSchema);
        }
        byte[] GetBuffer(XtraReport report) {
            using (MemoryStream stream = new MemoryStream()) {
                report.SaveLayout(stream);
                return stream.ToArray();
            }
        }
        public override string GetNewUrl() {
            // Show the report selection dialog and return a URL for a selected report.
            StorageEditorForm form = CreateForm();
            form.textBox1.Enabled = false;
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                return form.textBox1.Text;
            return string.Empty;
        }
        StorageEditorForm CreateForm() {
            StorageEditorForm form = new StorageEditorForm();
            foreach (string item in GetUrls())
                form.listBox1.Items.Add(item);
            return form;
        }
        public override string SetNewData(XtraReport report, string defaultUrl) {
            StorageEditorForm form = CreateForm();
            form.textBox1.Text = defaultUrl;
            form.listBox1.Enabled = false;
            // Show the save dialog to get a URL for a new report.
            if (form.ShowDialog() == DialogResult.OK) {
                string url = form.textBox1.Text;
                if (!string.IsNullOrEmpty(url) && !form.listBox1.Items.Contains(url)) {
                    TypeDescriptor.GetProperties(typeof(XtraReport))["DisplayName"].SetValue(report, url);
                    SetData(report, url);
                    return url;
                }
                else {
                    MessageBox.Show("Incorrect report name", "Error",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
            }
            return string.Empty;
        }

        // The following code is intended to support selection of a value for 
        // the Report Source Url property of Subreport controls.
        // (Use this code to avoid assigning the master report as a 
        // detail report's source.)

        public override bool GetStandardUrlsSupported(ITypeDescriptorContext context) {
            // Always return true to confirm that the GetStandardUrls method is available.
            return true;
        }
        public override string[] GetStandardUrls(ITypeDescriptorContext context) {
            if (context != null && context.Instance is XRSubreport) {
                XRSubreport xrSubreport = context.Instance as XRSubreport;
                if (xrSubreport.RootReport !=
                    null && xrSubreport.RootReport.Extensions.TryGetValue("StorageID", out storageID)) {
                    List<string> result = GetUrlsCore(CanPassId);
                    return result.ToArray();
                }
            }
            return GetUrls();
        }
        string storageID;
        bool CanPassId(string id) {
            return id != storageID;
        }
        string[] GetUrls() {
            return GetUrlsCore(null).ToArray();
        }
        List<string> GetUrlsCore(Predicate<string> method) {
            List<string> list = new List<string>();
            foreach (StorageDataSet.ReportStorageRow row in ReportStorage.Rows)
                if (method == null || method(row.ID.ToString()))
                    list.Add(row.Url);
            return list;
        }
    }
}
