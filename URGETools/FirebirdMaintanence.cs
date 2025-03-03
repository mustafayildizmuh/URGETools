using DevExpress.XtraEditors;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace URGETools
{
    public partial class FirebirdMaintanence : XtraForm
    {
        // Seçilen kaynak dosya yolunu tutan üye değişkeni.
        private string sourceFilePath;

        public FirebirdMaintanence()
        {
            InitializeComponent();
        }

        private void btnRepair_Click(object sender, EventArgs e)
        {
            // OpenFileDialog kullanarak kaynak dosya seçimi yapılıyor.
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Veritabanı Dosyası (*.fdb)|*.fdb|Tüm Dosyalar (*.*)|*.*";
                openFileDialog.Title = "Kaynak veritabanı dosyası seçin";
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("Kaynak veritabanı dosyası seçilmedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                sourceFilePath = openFileDialog.FileName;
            }

            // Seçilen dosyanın varlığı kontrol ediliyor.
            if (!File.Exists(sourceFilePath))
            {
                MessageBox.Show("Seçilen dosya mevcut değil!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hedef klasör, seçilen dosyanın bulunduğu klasör olarak alınıyor.
            // (txtTargetFolder kaldırıldığı için, dosyanın klasörü kullanılıyor.)
            string targetFolder = Path.GetDirectoryName(sourceFilePath);

            // DevExpress ProgressBarControl (progressBar1) sıfırlanıyor ve durum metni ayarlanıyor.
            progressBar1.EditValue = 0;
            progressBar1.Text = "Başlangıç...";

            // Çıktı kontrolü temizleniyor.
            txtOutput.Clear();

            // İşlem başladığı sürece gerekli butonlar ve kontroller devre dışı bırakılıyor.
            btnRepair.Enabled = false;
            btnCancel.Enabled = true;
            chkFullValidation.Enabled = false;
            chkMend.Enabled = false;
            chkCreateBackupCopy.Enabled = false;

            // Arka plan iş parçacığı başlatılıyor.
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
                AppendOutput("İşlem iptal ediliyor...");
            }
        }

        private void RunCommand(string command)
        {
            if (backgroundWorker1.CancellationPending)
                throw new OperationCanceledException();

            try
            {
                AppendCommandLog(command); // Komut loglanıyor.

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/C " + command,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = new Process { StartInfo = psi })
                {
                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                            AppendOutput(e.Data); // Çıktı loglanıyor.
                    };
                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                            AppendOutput("Hata: " + e.Data); // Hata loglanıyor.
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                AppendOutput("Komut çalıştırma hatası: " + ex.Message);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Seçilen kaynak dosya ve onun klasörü kullanılıyor.
                string sourceFile = sourceFilePath;
                string targetFolder = Path.GetDirectoryName(sourceFile);
                string backupFile = Path.Combine(targetFolder, "backup.fbk");

                string sourceFileName = Path.GetFileNameWithoutExtension(sourceFile);
                string timestamp = DateTime.Now.ToString("dd_MM_yyyy__HH_mm");
                string restoredFile = Path.Combine(targetFolder, $"{sourceFileName}___{timestamp}.fdb");

                int step = 1;

                // Eğer "yedek kopya oluştur" seçeneği işaretliyse, dosya kopyalanıyor.
                if (chkCreateBackupCopy.IsOn)
                {
                    backgroundWorker1.ReportProgress(10, $"{step++}. Adım: Dosya kopyalanıyor...");
                    AppendCommandLog("Kopyalama işlemi başladı...");

                    string copyFile = Path.Combine(targetFolder, $"{sourceFileName}_copy.fdb");
                    FileInfo fileInfo = new FileInfo(sourceFile);
                    long fileLength = fileInfo.Length;

                    using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
                    using (FileStream destStream = new FileStream(copyFile, FileMode.Create, FileAccess.Write))
                    {
                        byte[] buffer = new byte[1024 * 1024]; // 1MB buffer
                        int bytesRead;
                        long totalBytesRead = 0;

                        while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            destStream.Write(buffer, 0, bytesRead);
                            totalBytesRead += bytesRead;
                            int progress = (int)((totalBytesRead * 100) / fileLength);
                            backgroundWorker1.ReportProgress(20 + progress, $"Kopyalama: {progress}%");
                        }
                    }

                    backgroundWorker1.ReportProgress(50, "Kopyalama işlemi tamamlandı.");
                    AppendCommandLog("Kopyalama işlemi tamamlandı.");

                    backgroundWorker1.ReportProgress(60, $"{step++}. Adım: Yedekleme başlatılıyor...");
                    if (File.Exists(backupFile)) File.Delete(backupFile);
                    RunCommand($@"gbak -b -v -user sysdba -password masterkey {copyFile} {backupFile}");

                    if (backgroundWorker1.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    File.Delete(copyFile);
                    backgroundWorker1.ReportProgress(70, "Kopyalanan kaynak dosya silindi.");
                    AppendCommandLog("Kopyalanan kaynak dosya silindi.");
                }
                else
                {
                    // Veritabanı kullanımda mı kontrol ediliyor.
                    if (IsDatabaseInUse(sourceFile))
                    {
                        backgroundWorker1.ReportProgress(100, "Veritabanı kullanımda, işlem yapılamaz!");
                        e.Cancel = true;
                        return;
                    }

                    backgroundWorker1.ReportProgress(10, $"{step++}. Adım: Kaynak dosyadan yedekleme alınıyor...");
                    if (File.Exists(backupFile)) File.Delete(backupFile);
                    RunCommand($@"gbak -b -v -user sysdba -password masterkey {sourceFile} {backupFile}");

                    if (backgroundWorker1.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                backgroundWorker1.ReportProgress(80, $"{step++}. Adım: Veritabanı geri yükleniyor...");
                if (File.Exists(restoredFile)) File.Delete(restoredFile);
                RunCommand($@"gbak -r -v -page_size 16384 -user sysdba -password masterkey {backupFile} {restoredFile}");

                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                if (chkMend.IsOn)
                {
                    backgroundWorker1.ReportProgress(90, $"{step++}. Adım: Gfix -mend işlemi başlatılıyor...");
                    RunCommand($@"gfix -mend -user sysdba -password masterkey {restoredFile}");
                }

                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                backgroundWorker1.ReportProgress(95, $"{step++}. Adım: Sweep işlemi başlatılıyor...");
                RunCommand($@"gfix -sweep -user sysdba -password masterkey {restoredFile}");

                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                if (chkFullValidation.IsOn)
                {
                    backgroundWorker1.ReportProgress(100, $"{step++}. Adım: Gfix -v -full doğrulama işlemi başlatılıyor...");
                    RunCommand($@"gfix -v -user sysdba -password masterkey -full {restoredFile}");
                }
                else
                {
                    backgroundWorker1.ReportProgress(100, $"{step++}. Adım: Gfix -v doğrulama işlemi başlatılıyor...");
                    RunCommand($@"gfix -v -user sysdba -password masterkey {restoredFile}");
                }

                backgroundWorker1.ReportProgress(100, "İşlem tamamlandı!");
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

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;
            if (progress < 0) progress = 0;
            if (progress > 100) progress = 100;

            // DevExpress ProgressBarControl güncelleniyor.
            progressBar1.EditValue = progress;
            progressBar1.Text = e.UserState.ToString();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                AppendOutput("İşlem iptal edildi.");
            }
            else if (e.Error != null)
            {
                AppendOutput("Hata: " + e.Error.Message);
            }
            else
            {
                AppendOutput(e.Result.ToString());
            }

            btnRepair.Enabled = true;
            btnCancel.Enabled = false;
            chkFullValidation.Enabled = true;
            chkMend.Enabled = true;
            chkCreateBackupCopy.Enabled = true;
        }

        private void AppendOutput(string message)
        {
            if (txtOutput.InvokeRequired)
            {
                txtOutput.Invoke(new Action(() => AppendOutput(message)));
            }
            else
            {
                txtOutput.AppendText(message + Environment.NewLine);
                txtOutput.SelectionStart = txtOutput.Text.Length;
                txtOutput.ScrollToCaret();
            }
        }

        private bool IsDatabaseInUse(string databasePath)
        {
            try
            {
                using (FileStream fs = new FileStream(databasePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return false; // Veritabanı kullanımda değil.
                }
            }
            catch (IOException)
            {
                return true; // Veritabanı kullanımda.
            }
        }

        // Çalıştırılan komutu loglayan fonksiyon.
        private void AppendCommandLog(string message)
        {
            string timestamp = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            string logMessage = $"[{timestamp}] {message}";

            if (txtCommandsLog.InvokeRequired)
            {
                txtCommandsLog.Invoke(new Action(() => AppendCommandLog(message)));
            }
            else
            {
                txtCommandsLog.AppendText(logMessage + Environment.NewLine);
                txtCommandsLog.SelectionStart = txtCommandsLog.Text.Length;
                txtCommandsLog.ScrollToCaret();
            }
        }
    }
}
