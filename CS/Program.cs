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

            // By default, XPO uses an MS Access database. Uncomment these two lines if you are using another database type.
            // string conn = DevExpress.Xpo.DB.MSSqlConnectionProvider.GetConnectionString("(local)", "..", "...", "....");
            // XpoDefault.DataLayer = XpoDefault.GetDataLayer(conn, AutoCreateOption.DatabaseAndSchema);

            // Uncomment this line to register a report storage that uses XPO.
            // reportStorage = new XpoReportStorage(new UnitOfWork());

            // Uncomment this line to register a report storage, which uses Zip file.
            // reportStorage = new ZipReportStorage();


            ReportStorageExtension.RegisterExtensionGlobal(reportStorage);
            Application.Run(new Form1());
        }
    }
}