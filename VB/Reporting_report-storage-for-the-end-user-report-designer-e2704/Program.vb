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

            ' Uncomment these lines to register a report storage that uses XPO.
            'XpoDefault.DataLayer = XpoDefault.GetDataLayer(DevExpress.Xpo.DB.AccessConnectionProvider.GetConnectionString("ReportStorage.mdb"), DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema)
            'reportStorage_Renamed = New XpoReportStorage(New UnitOfWork())

            ' Uncomment this line to register a report storage, which uses Zip file.
            'reportStorage_Renamed = New ZipReportStorage()

            ReportStorageExtension.RegisterExtensionGlobal(reportStorage_Renamed)
			Application.Run(New Form1())
		End Sub
	End Class
End Namespace
