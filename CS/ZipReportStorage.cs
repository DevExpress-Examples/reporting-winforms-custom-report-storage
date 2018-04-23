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
using DevExpress.Utils.Zip;
// ...

namespace ReportStorageSample {
    class ZipReportStorage : ReportStorageExtension {
        class ZipFilesHelper : IDisposable {
            Stream stream;
            ZipFileCollection zipFiles = new ZipFileCollection();
            public ZipFileCollection ZipFiles {
                get {
                    return zipFiles;
                }
            }
            public ZipFilesHelper(string path) {
                if (File.Exists(path)) {
                    stream = File.OpenRead(path);
                    zipFiles = ZipArchive.Open(stream);
                }
            }
            public virtual void Dispose() {
                if (stream != null)
                    stream.Dispose();
            }
        }
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
            return true;
        }
        public override bool IsValidUrl(string url) {
            return !string.IsNullOrEmpty(url);
        }
        public override byte[] GetData(string url) {
            using (ZipFilesHelper helper = new ZipFilesHelper(StoragePath)) {
                ZipFile zipFile = GetZipFile(helper.ZipFiles, url);
                if (zipFile != null)
                    return GetBytes(zipFile);
                return new byte[] { };
            }
        }
        static byte[] GetBytes(ZipFile zipFile) {
            return GetBytes(zipFile.FileDataStream, (int)zipFile.UncompressedSize);
        }
        static byte[] GetBytes(Stream stream, int length) {
            byte[] result = new byte[length];
            stream.Read(result, 0, result.Length);
            return result;
        }
        static ZipFile GetZipFile(ZipFileCollection zipFiles, string url) {
            foreach (ZipFile item in zipFiles) {
                if (StringsEgual(item.FileName, url))
                    return item;
            }
            return null;
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
            using (ZipArchive arch = new ZipArchive(tempPath)) {
                using (ZipFilesHelper helper = new ZipFilesHelper(StoragePath)) {
                    bool added = false;
                    foreach (ZipFile item in helper.ZipFiles) {
                        if (StringsEgual(item.FileName, url)) {
                            arch.Add(item.FileName, DateTime.Now, buffer);
                            added = true;
                        }
                        else
                            arch.Add(item.FileName, DateTime.Now, GetBytes(item));
                    }
                    if (!added)
                        arch.Add(url, DateTime.Now, buffer);
                }
            }
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
            using (ZipFilesHelper helper = new ZipFilesHelper(StoragePath)) {
                foreach (ZipFile item in helper.ZipFiles)
                    if (method == null || method(item.FileName))
                        list.Add(item.FileName);
                return list;
            }
        }
    }
}
