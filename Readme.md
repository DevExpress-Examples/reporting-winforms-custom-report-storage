<!-- default file list -->
*Files to look at*:

* [DataSetReportStorage.cs](./CS/Reporting_report-storage-for-the-end-user-report-designer-e2704/DataSetReportStorage.cs) (VB: [DataSetReportStorage.vb](./VB/Reporting_report-storage-for-the-end-user-report-designer-e2704/DataSetReportStorage.vb))
* [Form1.cs](./CS/Reporting_report-storage-for-the-end-user-report-designer-e2704/Form1.cs) (VB: [Form1.vb](./VB/Reporting_report-storage-for-the-end-user-report-designer-e2704/Form1.vb))
* [Program.cs](./CS/Reporting_report-storage-for-the-end-user-report-designer-e2704/Program.cs) (VB: [Program.vb](./VB/Reporting_report-storage-for-the-end-user-report-designer-e2704/Program.vb))
* [StorageDataSet.cs](./CS/Reporting_report-storage-for-the-end-user-report-designer-e2704/StorageDataSet.cs) (VB: [StorageDataSet.vb](./VB/Reporting_report-storage-for-the-end-user-report-designer-e2704/StorageDataSet.vb))
* [StorageEditorForm.cs](./CS/Reporting_report-storage-for-the-end-user-report-designer-e2704/StorageEditorForm.cs) (VB: [StorageEditorForm.vb](./VB/Reporting_report-storage-for-the-end-user-report-designer-e2704/StorageEditorForm.vb))
* [XpoReportStorage.cs](./CS/Reporting_report-storage-for-the-end-user-report-designer-e2704/XpoReportStorage.cs) (VB: [XpoReportStorage.vb](./VB/Reporting_report-storage-for-the-end-user-report-designer-e2704/XpoReportStorage.vb))
* [ZipReportStorage.cs](./CS/Reporting_report-storage-for-the-end-user-report-designer-e2704/ZipReportStorage.cs) (VB: [ZipReportStorage.vb](./VB/Reporting_report-storage-for-the-end-user-report-designer-e2704/ZipReportStorage.vb))
<!-- default file list end -->
# Report Storage for the End-User Report Designer


<p>The following example demonstrates how to implement a report storage to persist <a href="http://documentation.devexpress.com/XtraReports/CustomDocument2592.aspx"><u>report definitions</u></a> in a database or in any other custom location. This may be useful when providing end-users with the capability to create and customize reports using XtraReports End-User Designer, if it is necessary to have a common target for saving and sharing all reports.</p><p>The default report serialization mechanism does not support the serialization of a report's data source. The following examples illustrate how you can provide a custom XML serialization logic for a report's data source as well as its custom parameters.</p><p>- <a href="https://www.devexpress.com/Support/Center/p/E3157">How to implement custom XML serialization of a report that is bound to a dataset</a>;</p><p>- <a href="https://www.devexpress.com/Support/Center/p/E3169">How to serialize an XPO data source</a>;</p><p>- <a href="https://www.devexpress.com/Support/Center/p/E3186">How to serialize parameters of custom types</a>.</p>

<br/>


