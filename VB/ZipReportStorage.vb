Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.IO.Compression
Imports System.Collections.Generic
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraReports.Extensions
' ...

Namespace ReportStorageSample
    Public Class ZipReportStorage
        Inherits ReportStorageExtension

        Private Const fileName As String = "ReportStorage.zip"
        Public Sub New()
        End Sub
        Private ReadOnly Property StoragePath As String
            Get
                Dim dirName As String = Path.GetDirectoryName(Application.ExecutablePath)
                Return Path.Combine(dirName, fileName)
            End Get
        End Property
        Public Overrides Function CanSetData(url As String) As Boolean
            ' Always return true to confirm that the SetData method is available.
            Return True
        End Function
        Public Overrides Function IsValidUrl(url As String) As Boolean
            Return Not String.IsNullOrEmpty(url)
        End Function
        Public Overrides Function GetData(url As String) As Byte()
            ' Open ZIP archive.
            If Not File.Exists(StoragePath) Then
                Return New Byte() {}
            End If

            Using fileStream = File.OpenRead(StoragePath)
                Using archive = New ZipArchive(fileStream, ZipArchiveMode.Read)
                    Dim entry = archive.GetEntry(url)
                    If entry Is Nothing Then
                        Return New Byte() {}
                    End If

                    Using entryStream = entry.Open()
                        Using ms As New MemoryStream()
                            entryStream.CopyTo(ms)
                            Return ms.ToArray()
                        End Using
                    End Using
                End Using
            End Using
        End Function
        Private Shared Function StringsEgual(a As String, b As String) As Boolean
            Return String.Equals(a, b, StringComparison.OrdinalIgnoreCase)
        End Function
        Public Overrides Sub SetData(report As XtraReport, url As String)
            report.Extensions("StorageID") = url
            SaveArchive(url, GetBuffer(report))
        End Sub
        Private Sub SaveArchive(url As String, buffer As Byte())
            Dim tempPath As String = Path.ChangeExtension(StoragePath, "tmp")
            ' Create a new ZIP archive.
            Using destStream As New FileStream(tempPath, FileMode.Create)
                Using destArchive As New ZipArchive(destStream, ZipArchiveMode.Create)

                    ' Open existing archive if it exists
                    If File.Exists(StoragePath) Then
                        Using srcStream = File.OpenRead(StoragePath)
                            Using srcArchive As New ZipArchive(srcStream, ZipArchiveMode.Read)
                                Dim added As Boolean = False

                                For Each item In srcArchive.Entries
                                    Dim newEntry = destArchive.CreateEntry(item.FullName)

                                    If StringsEgual(item.FullName, url) Then
                                        Using newStream = newEntry.Open()
                                            newStream.Write(buffer, 0, buffer.Length)
                                        End Using
                                        added = True
                                    Else
                                        Using entryStream = item.Open()
                                            Using newStream = newEntry.Open()
                                                entryStream.CopyTo(newStream)
                                            End Using
                                        End Using
                                    End If
                                Next

                                If Not added Then
                                    Dim newEntry = destArchive.CreateEntry(url)
                                    Using newStream = newEntry.Open()
                                        newStream.Write(buffer, 0, buffer.Length)
                                    End Using
                                End If
                            End Using
                        End Using
                    Else
                        Dim newEntry = destArchive.CreateEntry(url)
                        Using newStream = newEntry.Open()
                            newStream.Write(buffer, 0, buffer.Length)
                        End Using
                    End If
                End Using
            End Using
            ' Replace the old ZIP archive with the new one.
            If File.Exists(StoragePath) Then
                File.Delete(StoragePath)
            End If
            File.Move(tempPath, StoragePath)
        End Sub
        Private Function GetBuffer(report As XtraReport) As Byte()
            Using stream As New MemoryStream()
                report.SaveLayout(stream)
                Return stream.ToArray()
            End Using
        End Function
        Public Overrides Function GetNewUrl() As String
            Dim form As StorageEditorForm = CreateForm()
            form.textBox1.Enabled = False

            If form.ShowDialog() = DialogResult.OK Then
                Return form.textBox1.Text
            End If
            Return String.Empty
        End Function
        Private Function CreateForm() As StorageEditorForm
            Dim form As New StorageEditorForm()
            For Each item As String In GetUrls()
                form.listBox1.Items.Add(item)
            Next

            Return form
        End Function
        Public Overrides Function SetNewData(report As XtraReport, defaultUrl As String) As String
            Dim form As StorageEditorForm = CreateForm()
            form.textBox1.Text = defaultUrl
            form.listBox1.Enabled = False

            If form.ShowDialog() = DialogResult.OK Then
                Dim url As String = form.textBox1.Text

                If Not String.IsNullOrEmpty(url) AndAlso Not form.listBox1.Items.Contains(url) Then
                    TypeDescriptor.GetProperties(GetType(XtraReport))("DisplayName").SetValue(report, url)
                    SetData(report, url)
                    Return url
                Else
                    MessageBox.Show("Incorrect report name", "Error",
                                    MessageBoxButtons.OKCancel, MessageBoxIcon.[Error])
                End If
            End If
            Return String.Empty
        End Function
        Public Overrides Function GetStandardUrlsSupported(context As ITypeDescriptorContext) As Boolean
            ' Always return true to confirm that the GetStandardUrls method is available.
            Return True
        End Function
        Public Overrides Function GetStandardUrls(context As ITypeDescriptorContext) As String()
            If context IsNot Nothing AndAlso TypeOf context.Instance Is XRSubreport Then
                Dim xrSubreport As XRSubreport = CType(context.Instance, XRSubreport)

                If xrSubreport.RootReport IsNot Nothing AndAlso
                   xrSubreport.RootReport.Extensions.TryGetValue("StorageID", storageID) Then

                    Dim result As List(Of String) = GetUrlsCore(AddressOf CanPassId)
                    Return result.ToArray()
                End If
            End If
            Return GetUrls()
        End Function
        Private storageID As String
        Private Function CanPassId(id As String) As Boolean
            Return id <> storageID
        End Function
        Private Function GetUrls() As String()
            Return GetUrlsCore(Nothing).ToArray()
        End Function
        Private Function GetUrlsCore(method As Predicate(Of String)) As List(Of String)
            Dim list As New List(Of String)()

            If Not File.Exists(StoragePath) Then
                Return list
            End If

            Using fileStream = File.OpenRead(StoragePath)
                Using archive As New ZipArchive(fileStream, ZipArchiveMode.Read)
                    For Each entry In archive.Entries
                        If method Is Nothing OrElse method(entry.FullName) Then
                            list.Add(entry.FullName)
                        End If
                    Next
                End Using
            End Using

            Return list
        End Function
    End Class
End Namespace
