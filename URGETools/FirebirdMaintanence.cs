using DevExpress.XtraEditors;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList;
using System.Threading.Tasks;
using System.Drawing;
using DevExpress.XtraRichEdit;

namespace URGETools
{
    public partial class FirebirdMaintanence : XtraForm
    {
        private string sourceFilePath;
        private readonly string fb25path = @"C:\Program Files\Firebird\Firebird_2_5\bin\";
        private readonly string fb5path = @"C:\Program Files\Firebird\Firebird_5_0\";

        public FirebirdMaintanence()
        {
            InitializeComponent();
        }

        private async void FirebirdMaintanence_Load(object sender, EventArgs e)
        {
            LoadSettings();
            gridControl1.DataSource = fileList;

            await Task.Delay(10); // UI hazır olsun
            LoadDrives();         // doğrudan UI thread'de çalıştır
        }

        private void LoadSettings()
        {
            chkCreateBackupCopy.IsOn = Properties.Settings.Default.CreateBackupCopy;
            checkFb5.IsOn = Properties.Settings.Default.UseFirebird5;
            chkFullValidation.IsOn = Properties.Settings.Default.FullValidation;
            chkMend.IsOn = Properties.Settings.Default.Mend;
            checkRewriteFile.Checked = Properties.Settings.Default.CheckRewriteFile;
        }

        private void LoadDrives()
        {
            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();

            // En hızlı ve risksiz şekilde sürücüleri al (0 disk erişimi)
            foreach (string drive in Environment.GetLogicalDrives())
            {
                // Örn: drive = "C:\"
                TreeNode node = new TreeNode(drive.TrimEnd('\\')) // "C:" olarak göster
                {
                    Tag = drive
                };

                node.Nodes.Add(""); // Lazy loading için sahte node
                treeView1.Nodes.Add(node);
            }

            treeView1.EndUpdate();

            // C:\Fenix kontrolü ve varsa genişletme
            TryExpandFenixNode();
        }

        private void TryExpandFenixNode()
        {
            string fenixPath = @"C:\Fenix";
            if (!Directory.Exists(fenixPath))
                return;

            // C: node'unu bul
            TreeNode cNode = treeView1.Nodes
                .Cast<TreeNode>()
                .FirstOrDefault(n => string.Equals(n.Tag?.ToString(), @"C:\", StringComparison.OrdinalIgnoreCase));

            if (cNode == null)
                return;

            treeView1.SelectedNode = cNode;
            cNode.Expand(); // Lazy yüklemeyi tetikler

            // Lazy yüklemenin tamamlanmasını bekle (kısa gecikmeyle)
            Task.Delay(100).ContinueWith(_ =>
            {
                this.Invoke(new Action(() =>
                {
                    foreach (TreeNode child in cNode.Nodes)
                    {
                        if (string.Equals(child.Tag?.ToString(), fenixPath, StringComparison.OrdinalIgnoreCase))
                        {
                            child.Expand();
                            child.EnsureVisible();
                            treeView1.SelectedNode = child;
                            break;
                        }
                    }
                }));
            });

        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            string path = e.Node.Tag?.ToString();
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
                return;

            e.Node.Nodes.Clear(); // Her zaman yeniden yükle

            var excludedDirs = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "System Volume Information", "$Recycle.Bin", "Recycle.Bin",
                "Config.Msi", "ProgramData", "$WinREAgent", "$Windows.~WS", "$Windows.~BT"
            };

            try
            {
                foreach (string dir in Directory.GetDirectories(path))
                {
                    string dirName = Path.GetFileName(dir);
                    if (excludedDirs.Contains(dirName) || dirName.StartsWith("$") || dirName.StartsWith("."))
                        continue;

                    TreeNode dirNode = new TreeNode(dirName) { Tag = dir };
                    dirNode.Nodes.Add(""); // Lazy loading
                    e.Node.Nodes.Add(dirNode);
                }

                var files = Directory.GetFiles(path)
                                     .Where(f => f.EndsWith(".fdb", StringComparison.OrdinalIgnoreCase) ||
                                                 f.EndsWith(".fbk", StringComparison.OrdinalIgnoreCase));

                foreach (string file in files)
                {
                    TreeNode fileNode = new TreeNode(Path.GetFileName(file)) { Tag = file };
                    e.Node.Nodes.Add(fileNode);
                }
            }
            catch { }
        }

        private void RefreshSelectedTreeNode()
        {
            TreeNode selectedNode = treeView1.SelectedNode;
            if (selectedNode == null) return;

            string path = selectedNode.Tag?.ToString();
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
                return;

            // Aynı işlemi BeforeExpand’teki gibi uygula
            selectedNode.Nodes.Clear();

            var excludedDirs = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "System Volume Information", "$Recycle.Bin", "Recycle.Bin",
                "Config.Msi", "ProgramData", "$WinREAgent", "$Windows.~WS", "$Windows.~BT"
            };

            try
            {
                foreach (string dir in Directory.GetDirectories(path))
                {
                    string dirName = Path.GetFileName(dir);
                    if (excludedDirs.Contains(dirName) || dirName.StartsWith("$") || dirName.StartsWith("."))
                        continue;

                    TreeNode dirNode = new TreeNode(dirName) { Tag = dir };
                    dirNode.Nodes.Add("");
                    selectedNode.Nodes.Add(dirNode);
                }

                var files = Directory.GetFiles(path)
                                     .Where(f => f.EndsWith(".fdb", StringComparison.OrdinalIgnoreCase) ||
                                                 f.EndsWith(".fbk", StringComparison.OrdinalIgnoreCase));

                foreach (string file in files)
                {
                    TreeNode fileNode = new TreeNode(Path.GetFileName(file)) { Tag = file };
                    selectedNode.Nodes.Add(fileNode);
                }

                selectedNode.Expand();
            }
            catch { }
        }




        private async void btnBackup_Click(object sender, EventArgs e)
        {
            isWorking(true);
            txtOutput.Clear();
            AppendOutput("========== Yedekleme Başladı ==========\n");

            int index = 0;

            foreach (var entry in fileList)
            {
                if (Path.GetExtension(entry.FullPath).ToLower() != ".fdb")
                {
                    AppendOutput($"[Atlandı] '{entry.FullPath}' bir .fdb dosyası değil.");
                    continue;
                }

                int currentIndex = index++;
                AppendOutput($"[Yedekleme Başlıyor] {entry.FullPath}");
                await Task.Run(() => BackupDatabase(entry.FullPath, currentIndex));
                AppendOutput("----------------------------------------\n");
            }

            AppendOutput("========== Yedekleme Tamamlandı ==========\n");
            isWorking(false);

            RefreshSelectedTreeNode();
        }



        private async void btnRestore_Click(object sender, EventArgs e)
        {
            isWorking(true);
            txtOutput.Clear();
            AppendOutput("========== Geri Yükleme Başladı ==========\n");

            foreach (var entry in fileList)
            {
                if (Path.GetExtension(entry.FullPath).ToLower() != ".fbk")
                {
                    AppendOutput($"[Atlandı] '{entry.FullPath}' bir .fbk dosyası değil.");
                    continue;
                }

                AppendOutput($"[Geri Yükleme Başlıyor] {entry.FullPath}");
                await Task.Run(() => RestoreDatabase(entry.FullPath));
                AppendOutput("----------------------------------------\n");
            }

            AppendOutput("========== Geri Yükleme Tamamlandı ==========\n");
            isWorking(false);

            // Backup işlemi bittiğinde veya restore işlemi bittiğinde
            RefreshSelectedTreeNode();
        }
        private void BackupDatabase(string sourceFile, int index = 0)
        {
            string targetFolder = Path.GetDirectoryName(sourceFile);
            string fileName = Path.GetFileNameWithoutExtension(sourceFile);
            string timestamp = DateTime.Now.ToString("dd_MM_yyyy__HH_mm");
            //string backupFile = Path.Combine(targetFolder, $"{fileName}___{timestamp}_{index:D2}__BACKUP.fbk");
            string backupFile = Path.Combine(targetFolder, $"{fileName}___{timestamp}__BACKUP.fbk");
            string copyFile = Path.Combine(targetFolder, $"{fileName}_copy.fdb");
            string version = checkFb5.IsOn ? "5" : "2.5";

            if (File.Exists(backupFile))
            {
                if (checkRewriteFile.Checked)
                {
                    AppendCommandLog($"[UYARI] Var olan yedek dosyasının üzerine yazılıyor: {backupFile}", LogLevelCommand.Warning);
                    File.Delete(backupFile); // gbak overwrite edemez
                }
                else
                {
                    AppendCommandLog($"[ATLANDI] Aynı isimde yedek dosyası zaten mevcut: {backupFile}", LogLevelCommand.Warning);
                    return;
                }
            }


            try
            {
                if (chkCreateBackupCopy.IsOn)
                {
                    AppendCommandLog($"Kopyalanıyor: {sourceFile} → {copyFile}");
                    File.Copy(sourceFile, copyFile, true);
                    RunCommand($@"gbak -b -v -user sysdba -password masterkey ""{copyFile}"" ""{backupFile}""", version, null);
                    File.Delete(copyFile);
                }
                else
                {
                    if (IsDatabaseInUse(sourceFile))
                    {
                        AppendOutput($"[Yedekleme HATA] {sourceFile}");
                        AppendOutput("Detay: Dosya kullanımda, erişilemiyor.");
                        return;
                    }

                    RunCommand($@"gbak -b -v -user sysdba -password masterkey ""{sourceFile}"" ""{backupFile}""", version, null);
                }

                AppendOutput($"[Yedekleme BAŞARILI] {backupFile}");
            }
            catch (Exception ex)
            {
                AppendOutput($"[Yedekleme HATA] {sourceFile}");
                AppendOutput($"Detay: {ex.Message}");
            }
            finally
            {
                AppendOutput("----------------------------------------");
            }
        }

        private void RestoreDatabase(string backupFile)
        {
            string targetFolder = Path.GetDirectoryName(backupFile);
            string fileName = Path.GetFileNameWithoutExtension(backupFile);
            string timestamp = DateTime.Now.ToString("dd_MM_yyyy__HH_mm");
            string restoredFile = Path.Combine(targetFolder, $"{fileName}___{timestamp}.fdb");
            string version = checkFb5.IsOn ? "5" : "2.5";

            if (File.Exists(restoredFile))
            {
                if (checkRewriteFile.Checked)
                {
                    AppendCommandLog($"[UYARI] Var olan geri yükleme dosyasının üzerine yazılıyor: {restoredFile}", LogLevelCommand.Warning);
                    File.Delete(restoredFile); // çakışmayı önle
                }
                else
                {
                    AppendCommandLog($"[ATLANDI] Aynı isimde .fdb dosyası zaten mevcut: {restoredFile}", LogLevelCommand.Warning);
                    return; // işlem iptal
                }
            }
            // Dosya yoksa → devam edilir, log verilmez (normal durum)

            try
            {
                RunCommand($@"gbak -r -v -page_size 16384 -user sysdba -password masterkey ""{backupFile}"" ""{restoredFile}""", version, null);

                if (chkMend.IsOn)
                    RunCommand($@"gfix -mend -user sysdba -password masterkey ""{restoredFile}""", version, null);

                RunCommand($@"gfix -sweep -user sysdba -password masterkey ""{restoredFile}""", version, null);

                if (chkFullValidation.IsOn)
                    RunCommand($@"gfix -v -user sysdba -password masterkey -full ""{restoredFile}""", version, null);
                else
                    RunCommand($@"gfix -v -user sysdba -password masterkey ""{restoredFile}""", version, null);

                AppendOutput($"[Geri Yükleme BAŞARILI] {restoredFile}");
            }
            catch (Exception ex)
            {
                AppendOutput($"[Geri Yükleme HATA] {backupFile}");
                AppendOutput($"Detay: {ex.Message}");
            }
            finally
            {
                AppendOutput("----------------------------------------");
            }
        }




        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
                AppendOutput("İşlem iptal ediliyor...");
            }

            if (backgroundWorker2.IsBusy)
            {
                backgroundWorker2.CancelAsync();
                AppendOutput("İşlem iptal ediliyor...");
            }
        }

        private void RunCommand(string command, string fbVer, BackgroundWorker worker)
        {
            if (worker != null && worker.CancellationPending)
                throw new OperationCanceledException();

            try
            {
                string fbBinPath = (fbVer == "5") ? fb5path : fb25path;
                string[] commandParts = command.Trim().Split(new[] { ' ' }, 2);
                string exeName = commandParts[0];
                string arguments = commandParts.Length > 1 ? commandParts[1] : "";

                string exeFullPath = Path.Combine(fbBinPath, exeName + ".exe");
                AppendCommandLog($"\"{exeFullPath}\" {arguments}");

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = exeFullPath,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = new Process { StartInfo = psi })
                {
                    process.OutputDataReceived += (s, e) => { if (!string.IsNullOrEmpty(e.Data)) AppendOutput(e.Data); };
                    process.ErrorDataReceived += (s, e) => { if (!string.IsNullOrEmpty(e.Data)) AppendOutput("Hata: " + e.Data); AppendCommandLog("Hata: " + e.Data); };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                AppendOutput("Komut çalıştırma hatası: " + ex.Message);

                AppendCommandLog($" ");
            }


            AppendCommandLog($" ");
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            try
            {
                string sourceFile = sourceFilePath;
                string targetFolder = Path.GetDirectoryName(sourceFile);
                string sourceFileName = Path.GetFileNameWithoutExtension(sourceFile);
                string timestamp = DateTime.Now.ToString("dd_MM_yyyy__HH_mm");
                string restoredFile = Path.Combine(targetFolder, $"{sourceFileName}___{timestamp}.fdb");
                string backupFile = Path.Combine(targetFolder, $"{sourceFileName}___{timestamp}__BACKUP.fbk");
                int step = 1;
                string version = checkFb5.IsOn ? "5" : "2.5";

                if (chkCreateBackupCopy.IsOn)
                {
                    worker.ReportProgress(10, $"{step++}. Adım: Dosya kopyalanıyor...");
                    AppendCommandLog("Kopyalama işlemi başladı...");

                    string copyFile = Path.Combine(targetFolder, $"{sourceFileName}_copy.fdb");
                    FileInfo fileInfo = new FileInfo(sourceFile);
                    long fileLength = fileInfo.Length;

                    using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
                    using (FileStream destStream = new FileStream(copyFile, FileMode.Create, FileAccess.Write))
                    {
                        byte[] buffer = new byte[1024 * 1024];
                        int bytesRead;
                        long totalBytesRead = 0;

                        while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            destStream.Write(buffer, 0, bytesRead);
                            totalBytesRead += bytesRead;
                            int progress = (int)((totalBytesRead * 100) / fileLength);
                            worker.ReportProgress(20 + progress, $"Kopyalama: {progress}%");

                            if (worker.CancellationPending)
                            {
                                e.Cancel = true;
                                return;
                            }
                        }
                    }

                    worker.ReportProgress(50, "Kopyalama işlemi tamamlandı.");
                    AppendCommandLog("Kopyalama işlemi tamamlandı.");

                    if (File.Exists(backupFile)) File.Delete(backupFile);
                    worker.ReportProgress(60, $"{step++}. Adım: Yedekleme başlatılıyor...");
                    RunCommand($@"gbak -b -v -user sysdba -password masterkey {copyFile} {backupFile}", version, worker);

                    if (File.Exists(copyFile)) File.Delete(copyFile);
                    worker.ReportProgress(70, "Kopyalanan kaynak dosya silindi.");
                    AppendCommandLog("Kopyalanan kaynak dosya silindi.");
                }
                else
                {
                    if (IsDatabaseInUse(sourceFile))
                    {
                        worker.ReportProgress(100, "Veritabanı kullanımda, işlem yapılamaz!");
                        e.Cancel = true;
                        return;
                    }

                    worker.ReportProgress(10, $"{step++}. Adım: Kaynak dosyadan yedekleme alınıyor...");
                    if (File.Exists(backupFile)) File.Delete(backupFile);
                    RunCommand($@"gbak -b -v -user sysdba -password masterkey {sourceFile} {backupFile}", version, worker);
                }

                worker.ReportProgress(100, "İşlem tamamlandı!");
                e.Result = $"İşlem tamamlandı! Yedek dosyası: {backupFile}";
            }
            catch (OperationCanceledException)
            {
                e.Cancel = true;
            }
            catch (Exception ex)
            {
                e.Result = "Hata: " + ex.Message;
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = Math.Max(0, Math.Min(100, e.ProgressPercentage));
            progressBar1.EditValue = progress;
            progressBar1.Text = e.UserState?.ToString();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                AppendOutput("İşlem iptal edildi.");
            else if (e.Error != null)
                AppendOutput("Hata: " + e.Error.Message);
            else
                AppendOutput(e.Result?.ToString());

            isWorking(false);
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            try
            {
                string sourceFile = sourceFilePath;
                string targetFolder = Path.GetDirectoryName(sourceFile);
                string sourceFileName = Path.GetFileNameWithoutExtension(sourceFile);
                string timestamp = DateTime.Now.ToString("dd_MM_yyyy__HH_mm");
                string restoredFile = Path.Combine(targetFolder, $"{sourceFileName}___{timestamp}.fdb");
                int step = 1;
                string version = checkFb5.IsOn ? "5" : "2.5";

                worker.ReportProgress(80, $"{step++}. Adım: Veritabanı geri yükleniyor...");
                if (File.Exists(restoredFile)) File.Delete(restoredFile);
                RunCommand($@"gbak -r -v -page_size 16384 -user sysdba -password masterkey {sourceFile} {restoredFile}", version, worker);

                if (chkMend.IsOn)
                {
                    worker.ReportProgress(90, $"{step++}. Adım: Gfix -mend işlemi başlatılıyor...");
                    RunCommand($@"gfix -mend -user sysdba -password masterkey {restoredFile}", version, worker);
                }

                worker.ReportProgress(95, $"{step++}. Adım: Sweep işlemi başlatılıyor...");
                RunCommand($@"gfix -sweep -user sysdba -password masterkey {restoredFile}", version, worker);

                if (chkFullValidation.IsOn)
                {
                    worker.ReportProgress(100, $"{step++}. Adım: Gfix -v -full doğrulama işlemi başlatılıyor...");
                    RunCommand($@"gfix -v -user sysdba -password masterkey -full {restoredFile}", version, worker);
                }
                else
                {
                    worker.ReportProgress(100, $"{step++}. Adım: Gfix -v doğrulama işlemi başlatılıyor...");
                    RunCommand($@"gfix -v -user sysdba -password masterkey {restoredFile}", version, worker);
                }

                worker.ReportProgress(100, "İşlem tamamlandı!");
                e.Result = $"İşlem tamamlandı! Geri yüklenen dosya: {restoredFile}";
            }
            catch (OperationCanceledException)
            {
                e.Cancel = true;
            }
            catch (Exception ex)
            {
                e.Result = "Hata: " + ex.Message;
            }
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = Math.Max(0, Math.Min(100, e.ProgressPercentage));
            progressBar1.EditValue = progress;
            progressBar1.Text = e.UserState?.ToString();
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                AppendOutput("İşlem iptal edildi.");
                AppendCommandLog("İşlem iptal edildi.");
            }
            else if (e.Error != null)
            {
                AppendOutput("Hata: " + e.Error.Message);
                AppendCommandLog("Hata: " + e.Error.Message);
            }
            else
            {
                AppendOutput(e.Result?.ToString());
                AppendCommandLog(e.Result?.ToString());
            }

            isWorking(false);
        }
        private void AppendOutput(string message, LogLevel level = LogLevel.Info)
        {
            if (txtOutput.InvokeRequired)
            {
                txtOutput.Invoke(new Action(() => AppendOutput(message, level)));
            }
            else
            {
                txtOutput.SelectionStart = txtOutput.TextLength;
                txtOutput.SelectionLength = 0;

                txtOutput.SelectionColor = GetColorForLogLevel(level);
                txtOutput.AppendText($"{DateTime.Now:HH:mm:ss} - {message}{Environment.NewLine}");
                txtOutput.SelectionColor = txtOutput.ForeColor;
                txtOutput.ScrollToCaret();
            }
        }

        private Color GetColorForLogLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Info:
                    return Color.Black;
                case LogLevel.Warning:
                    return Color.Orange;
                case LogLevel.Error:
                    return Color.Red;
                default:
                    return Color.Black;
            }
        }


        private void AppendCommandLog(string message, LogLevelCommand level = LogLevelCommand.Info)
        {
            string timestamp = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

            string prefix;
            switch (level)
            {
                case LogLevelCommand.Success:
                    prefix = "[BAŞARILI]";
                    break;
                case LogLevelCommand.Error:
                    prefix = "[HATA]";
                    break;
                default:
                    prefix = "[BİLGİ]";
                    break;
            }

            string logMessage = $"[{timestamp}] {prefix} {message}{Environment.NewLine}";

            if (txtCommandsLog.InvokeRequired)
            {
                txtCommandsLog.Invoke(new Action(() => AppendLogToTextbox(txtCommandsLog, logMessage)));
            }
            else
            {
                AppendLogToTextbox(txtCommandsLog, logMessage);
            }
        }

        private void AppendLogToTextbox(RichTextBox textbox, string message)
        {
            textbox.AppendText(message);
            textbox.SelectionStart = textbox.Text.Length;
            textbox.ScrollToCaret();
        }



        private bool IsDatabaseInUse(string databasePath)
        {
            try
            {
                using (FileStream fs = new FileStream(databasePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return false;
                }
            }
            catch (IOException)
            {
                return true;
            }
        }

        private void isWorking(bool working)
        {
            btnBackup.Enabled = !working;
            btnCancel.Enabled = working;
            chkFullValidation.Enabled = !working;
            chkMend.Enabled = !working;
            chkCreateBackupCopy.Enabled = !working;
            checkFb5.Enabled = !working;
        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {

            fileList.Clear(); // GridControl'ü temizler
        }

        
        BindingList<FileEntry> fileList = new BindingList<FileEntry>();

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string path = e.Node.Tag?.ToString();
            if (path != null && File.Exists(path)) // Yani bu bir dosya
            {
                FileInfo fi = new FileInfo(path);
                var entry = new FileEntry
                {
                    FileName = fi.Name,
                    FullPath = fi.FullName,
                    LastModified = fi.LastWriteTime
                };

                if (!fileList.Any(f => f.FullPath == entry.FullPath))
                {
                    fileList.Add(entry);
                }
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            var view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view == null) return;

            // Hangi satırda tıklanıldığını belirle
            var hitInfo = view.CalcHitInfo(view.GridControl.PointToClient(Control.MousePosition));
            if (!hitInfo.InRow) return;

            int rowHandle = hitInfo.RowHandle;
            if (rowHandle < 0) return;

            var row = view.GetRow(rowHandle) as FileEntry;
            if (row != null && fileList.Contains(row))
            {
                fileList.Remove(row);
            }
        }

        private void chkCreateBackupCopy_Toggled(object sender, EventArgs e)
        {
            Properties.Settings.Default.CreateBackupCopy = chkCreateBackupCopy.IsOn;
            Properties.Settings.Default.Save();
        }

        private void checkFb5_Toggled(object sender, EventArgs e)
        {
            Properties.Settings.Default.UseFirebird5 = checkFb5.IsOn;
            Properties.Settings.Default.Save();
        }

        private void chkFullValidation_Toggled(object sender, EventArgs e)
        {
            Properties.Settings.Default.FullValidation = chkFullValidation.IsOn;
            Properties.Settings.Default.Save();
        }

        private void chkMend_Toggled(object sender, EventArgs e)
        {
            Properties.Settings.Default.Mend = chkMend.IsOn;
            Properties.Settings.Default.Save();
        }
        private void checkRewriteFile_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.CheckRewriteFile = checkRewriteFile.Checked;
            Properties.Settings.Default.Save();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset(); // Varsayılanlara döner
            Properties.Settings.Default.Save();
            LoadSettings(); // UI’ya uygula
        }




        //private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        //{
        //    string path = e.Node.Tag.ToString();
        //    listView1.Items.Clear();
        //    try
        //    {
        //        foreach (string file in Directory.GetFiles(path))
        //        {
        //            listView1.Items.Add(Path.GetFileName(file));
        //        }
        //    }
        //    catch { }
        //}

    }
    public class FileEntry
    {
        public string FileName { get; set; }
        public string FullPath { get; set; }
        public DateTime LastModified { get; set; }
    }


    enum LogLevel
    {
        Info,
        Warning,
        Error
    }

    enum LogLevelCommand
    {
        Info,
        Success,
        Error,
        Warning
    }

}
