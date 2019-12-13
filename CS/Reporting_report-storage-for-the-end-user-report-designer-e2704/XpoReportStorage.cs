using System;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Extensions;
// ...

namespace ReportStorageSample {
    class XpoReportStorage : ReportStorageExtension {
        XPCollection<StorageItem> items;
        UnitOfWork Session {
            get { return (UnitOfWork)items.Session; }
        }
        public StorageItem FindItem(string name) {
            return Session.FindObject<StorageItem>(new BinaryOperator("Url", name));
        }
        public XPCollection<StorageItem> Items {
            get { return items; }
        }
        public XpoReportStorage(UnitOfWork session) {
            items = new XPCollection<StorageItem>(session);
        }

        public override bool CanSetData(string url) {
            // Always return true to confirm that the SetData method is available.
            return true;
        }
        public override bool IsValidUrl(string url) {
            return !string.IsNullOrEmpty(url);
        }
        public override byte[] GetData(string url) {
            // Get a StorageItem containing the report.
            StorageItem item = FindItem(url);
            if (item != null)
                return item.Layout;
            return new byte[] { };
        }

        public override void SetData(XtraReport report, string url) {
            // Write the report to a corresponding StorageItem.
            // If a StorageItem with a specified Url property value does not exist, create a new one.
            StorageItem item = FindItem(url);
            if (item != null)
                item.Layout = GetBuffer(report);
            else {
                item = new StorageItem(Session);
                item.Url = url;
                Session.CommitChanges();

                report.Extensions["StorageID"] = item.Oid.ToString();
                item.Layout = GetBuffer(report);
            }
            Session.CommitChanges();
            items.Reload();
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
            if (form.ShowDialog() == DialogResult.OK)
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
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
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
            foreach (StorageItem item in Items)
                if (method == null || method(item.Oid.ToString()))
                    list.Add(item.Url);
            return list;
        }
    }
    public class StorageItem : XPObject {
        string url;
        byte[] layout = null;
        public string Url {
            get { return url; }
            set { SetPropertyValue("Url", ref url, value); }
        }
        public byte[] Layout {
            get { return layout; }
            set { SetPropertyValue("Layout", ref layout, value); }
        }
        public StorageItem(Session session)
            : base(session) {
        }
    }
}
