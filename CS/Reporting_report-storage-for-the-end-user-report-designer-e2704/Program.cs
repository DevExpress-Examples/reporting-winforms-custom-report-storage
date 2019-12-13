using System;
using System.Windows.Forms;
using DevExpress.Xpo;
using DevExpress.XtraReports.Extensions;
// ...

namespace ReportStorageSample {
    static class Program {
        static ReportStorageExtension reportStorage;
        public static ReportStorageExtension ReportStorage {
            get {
                return reportStorage;
            }
        }

        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // This code registers a report storage that uses System.DataSet.
            reportStorage = new DataSetReportStorage();

            // Uncomment these lines to register a report storage that uses XPO.
            //XpoDefault.DataLayer = XpoDefault.GetDataLayer(DevExpress.Xpo.DB.AccessConnectionProvider.GetConnectionString("ReportStorage.mdb"), DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema);
            // reportStorage = new XpoReportStorage(new UnitOfWork());

            // Uncomment this line to register a report storage, which uses Zip file.
            // reportStorage = new ZipReportStorage();

            ReportStorageExtension.RegisterExtensionGlobal(reportStorage);
            Application.Run(new Form1());
        }
    }
}
