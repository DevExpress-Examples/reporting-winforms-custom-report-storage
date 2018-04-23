Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports DevExpress.Xpo
Imports DevExpress.XtraReports.Extensions
' ...

Namespace ReportStorageSample
	Friend NotInheritable Class Program
		Private Shared reportStorage_Renamed As ReportStorageExtension
		Private Sub New()
		End Sub
		Public Shared ReadOnly Property ReportStorage() As ReportStorageExtension
			Get
				Return reportStorage_Renamed
			End Get
		End Property

		<STAThread> _
		Shared Sub Main()
			Application.EnableVisualStyles()
			Application.SetCompatibleTextRenderingDefault(False)

			' This code registers a report storage that uses System.DataSet.
			reportStorage_Renamed = New DataSetReportStorage()

			' By default, XPO uses an MS Access database. Uncomment these two lines if you are using another database type.
			' string conn = DevExpress.Xpo.DB.MSSqlConnectionProvider.GetConnectionString("(local)", "..", "...", "....");
			' XpoDefault.DataLayer = XpoDefault.GetDataLayer(conn, AutoCreateOption.DatabaseAndSchema);

            ' Uncomment this line to register a report storage that uses XPO.
            ' reportStorage = new XpoReportStorage(new UnitOfWork());

			' Uncomment this line to register a report storage, which uses Zip file.
			' reportStorage = new ZipReportStorage();


			ReportStorageExtension.RegisterExtensionGlobal(reportStorage_Renamed)
			Application.Run(New Form1())
		End Sub
	End Class
End Namespace