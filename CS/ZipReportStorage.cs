using System;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO.Compression;
using System.Collections.Generic;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;        
using DevExpress.XtraReports.UI;                 
using DevExpress.XtraReports.Extensions;           
// ...

namespace ReportStorageSample {
    class ZipReportStorage : ReportStorageExtension {
        const string fileName = "ReportStorage.zip";
        public ZipReportStorage() {
        }
        string StoragePath {
            get {
                string dirName = Path.GetDirectoryName(Application.ExecutablePath);
                return Path.Combine(dirName, fileName);
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
            // Open ZIP archive.
            if (!File.Exists(StoragePath))
                return new byte[] { };
            using (var fileStream = File.OpenRead(StoragePath))
            using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Read)) {
                var entry = archive.GetEntry(url);
                if (entry == null)
                    return new byte[] { };
                using (var entryStream = entry.Open())
                using (var ms = new MemoryStream()) {
                    entryStream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }
        static bool StringsEgual(string a, string b) {
            return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        }
        public override void SetData(XtraReport report, string url) {
            report.Extensions["StorageID"] = url;
            SaveArchive(url, GetBuffer(report));
        }
        void SaveArchive(string url, byte[] buffer) {
            string tempPath = Path.ChangeExtension(StoragePath, "tmp");
            // Create a new ZIP archive.
            using (var destStream = new FileStream(tempPath, FileMode.Create))
            using (var destArchive = new ZipArchive(destStream, ZipArchiveMode.Create)) {
                // Open a ZIP archive where report files are stored.
                if (File.Exists(StoragePath)) {
                    using (var srcStream = File.OpenRead(StoragePath))
                    using (var srcArchive = new ZipArchive(srcStream, ZipArchiveMode.Read)) {
                        bool added = false;
                        // Copy all report files to a new archive.
                        // Update a file with a specified URL.
                        // If the file does not exist, create it.
                        foreach (var item in srcArchive.Entries) {
                            if (StringsEgual(item.FullName, url)) {
                                var newEntry = destArchive.CreateEntry(item.FullName);
                                using (var newStream = newEntry.Open())
                                    newStream.Write(buffer, 0, buffer.Length);
                                added = true;
                            }
                            else {
                                var newEntry = destArchive.CreateEntry(item.FullName);
                                using (var entryStream = item.Open())
                                using (var newStream = newEntry.Open())
                                    entryStream.CopyTo(newStream);
                            }
                        }
                        if (!added) {
                            var newEntry = destArchive.CreateEntry(url);
                            using (var newStream = newEntry.Open())
                                newStream.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
                else {
                    var newEntry = destArchive.CreateEntry(url);
                    using (var newStream = newEntry.Open())
                        newStream.Write(buffer, 0, buffer.Length);
                }
            }
            // Replace the old ZIP archive with the new one.
            if (File.Exists(StoragePath))
                File.Delete(StoragePath);
            File.Move(tempPath, StoragePath);
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
            if (!File.Exists(StoragePath))
                return list;
            using (var fileStream = File.OpenRead(StoragePath))
            using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Read)) {
                foreach (var entry in archive.Entries)
                    if (method == null || method(entry.FullName))
                        list.Add(entry.FullName);
            }
            return list;
        }
    }
}
