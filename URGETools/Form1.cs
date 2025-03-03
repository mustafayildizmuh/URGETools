using AutoUpdaterDotNET;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace URGETools
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Form1()
        {
            InitializeComponent();

            LoadUserSkinAndPalette(); // Apply saved skin and palette

            this.IsMdiContainer = true;

            filePath = "Settings.json";
            settings = GetOrCreateSettings();
        }

        private Settings settings;
        private FirebirdMaintanence maintanenceForm;
        private string version;

        string filePath;

        private void Form1_Load(object sender, EventArgs e)
        {

            // Ayarları yükle veya varsayılan değerlere ayarla
            if (Properties.Settings.Default.WindowLocation == Point.Empty)
            {
                Properties.Settings.Default.WindowLocation = new Point(100, 100); // Varsayılan değer
            }

            if (Properties.Settings.Default.WindowSize == Size.Empty)
            {
                Properties.Settings.Default.WindowSize = new Size(800, 600); // Varsayılan değer
            }

            this.Location = Properties.Settings.Default.WindowLocation;
            this.Size = Properties.Settings.Default.WindowSize;
            this.WindowState = Properties.Settings.Default.WindowState;

            version = getVersion();

            this.Text = this.Text + " - v" + version;

            LoadLibs();

            CheckForUpdate();
        }

        private void CheckForUpdate()
        {
            //if (string.IsNullOrEmpty(settings.UpdateURL)) settings.UpdateURL = "file://192.168.1.202/TeknikOfis/Vitem%20TopSolid%20Ortak/URGETools/UpdateInfo.xml";
            //AutoUpdater.ReportErrors = true;

            //// Güncelleme öncesi
            //string settingsFile = Path.Combine(Application.StartupPath, "Settings.json");
            //string backupSettingsFile = Path.Combine(Path.GetTempPath(), "myApp_settings_backup.json");

            //string designsFolder = Path.Combine(Application.StartupPath, "Designs");
            //string backupDesignsFolder = Path.Combine(Path.GetTempPath(), "MyAppSettingsBackup");

            //// Yedekle
            //File.Copy(settingsFile, backupSettingsFile, overwrite: true);
            //DirectoryBackupHelper.BackupDirectory(designsFolder, backupDesignsFolder);

            // AutoUpdater.NET ile güncelleme süreci
            AutoUpdater.Start(settings.UpdateURL);

            //// Güncelleme sonrası, uygulama yeniden başlayınca:
            //File.Copy(backupSettingsFile, settingsFile, overwrite: true);
            //DirectoryBackupHelper.RestoreDirectory(backupDesignsFolder, designsFolder);
        }


        //public static class DirectoryBackupHelper
        //{
        //    /// <summary>
        //    /// Belirtilen klasörü (ve alt klasörlerini) hedef klasöre kopyalar.
        //    /// Eğer hedef klasör mevcut değilse, otomatik olarak oluşturur.
        //    /// </summary>
        //    /// <param name="sourceDir">Kaynak klasörün tam yolu</param>
        //    /// <param name="destinationDir">Hedef klasörün tam yolu</param>
        //    /// <param name="recursive">Alt klasörleri de kopyalamak için true</param>
        //    public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        //    {
        //        // Kaynak klasörü kontrol et
        //        DirectoryInfo dir = new DirectoryInfo(sourceDir);
        //        if (!dir.Exists)
        //            throw new DirectoryNotFoundException($"Kaynak klasör bulunamadı: {dir.FullName}");

        //        // Hedef klasörü oluştur (varsa hiçbir işlem yapmaz)
        //        Directory.CreateDirectory(destinationDir);

        //        // Kaynak klasördeki tüm dosyaları kopyala
        //        foreach (FileInfo file in dir.GetFiles())
        //        {
        //            string targetFilePath = Path.Combine(destinationDir, file.Name);
        //            file.CopyTo(targetFilePath, overwrite: true);
        //        }

        //        // Eğer recursive true ise, alt klasörleri de kopyala
        //        if (recursive)
        //        {
        //            foreach (DirectoryInfo subDir in dir.GetDirectories())
        //            {
        //                string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
        //                CopyDirectory(subDir.FullName, newDestinationDir, recursive);
        //            }
        //        }
        //    }

        //    /// <summary>
        //    /// Kaynak klasörü yedekler (backupFolder içine kopyalar).
        //    /// </summary>
        //    /// <param name="sourceFolder">Yedeklenecek klasör</param>
        //    /// <param name="backupFolder">Yedek klasörünün yolu</param>
        //    public static void BackupDirectory(string sourceFolder, string backupFolder)
        //    {
        //        CopyDirectory(sourceFolder, backupFolder, recursive: true);
        //        Console.WriteLine($"'{sourceFolder}' klasörü '{backupFolder}' konumuna yedeklendi.");
        //    }

        //    /// <summary>
        //    /// Yedek klasörü orijinal konuma geri yükler.
        //    /// </summary>
        //    /// <param name="backupFolder">Yedek klasörünün yolu</param>
        //    /// <param name="destinationFolder">Geri yüklenecek (orijinal) klasör</param>
        //    public static void RestoreDirectory(string backupFolder, string destinationFolder)
        //    {
        //        // Restore işlemi yaparken hedef klasörü oluşturur ve yedeği kopyalar
        //        CopyDirectory(backupFolder, destinationFolder, recursive: true);
        //        Console.WriteLine($"'{backupFolder}' klasöründeki dosyalar '{destinationFolder}' konumuna geri yüklendi.");
        //    }
        //}

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Ayarları kaydet
            if (this.WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.WindowLocation = this.Location;
                Properties.Settings.Default.WindowSize = this.Size;
            }

            Properties.Settings.Default.WindowState = this.WindowState;
            Properties.Settings.Default.Save();

            SaveUserSkinAndPalette(); // Save skin and palette before closing
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadLibs();
        }


        private void LoadLibs()
        {
            /// maintanenceForm formunu oluştur ve göster

            if (maintanenceForm != null) maintanenceForm.Close();
            maintanenceForm = new FirebirdMaintanence
            {
                MdiParent = this,
                WindowState = FormWindowState.Maximized
                //,
                //settings = settings
            };

            maintanenceForm.Show();
            //maintanenceForm.RefreshData();

            //// SettingsForm'u göster ve veri yenileme aksiyonunu ayarla
            //using (SettingsForm settingsForm = new SettingsForm(settings))
            //{
            //    if (settingsForm.ShowDialog() == DialogResult.OK)
            //    {
            //        // Kullanıcı ayarları onayladıysa, maintanenceForm formundaki ayarları güncelle
            //        maintanenceForm.UpdateSettings(settingsForm.CurrentSettings);

            //        // Ayrıca, verileri de yenileyin
            //        maintanenceForm.RefreshData();
            //    }
            //}
        }

        private void SaveUserSkinAndPalette()
        {
            string skinName = UserLookAndFeel.Default.SkinName;
            string paletteName = UserLookAndFeel.Default.ActiveSvgPaletteName; // Example: save the first palette name

            // Save skin and palette to settings
            Properties.Settings.Default.SelectedSkin = skinName;
            Properties.Settings.Default.SelectedPalette = paletteName;
            Properties.Settings.Default.Save(); // Save to application settings
        }


        private void LoadUserSkinAndPalette()
        {
            string skinName = Properties.Settings.Default.SelectedSkin;
            string paletteName = Properties.Settings.Default.SelectedPalette;

            // Apply the saved skin
            if (!string.IsNullOrEmpty(skinName) && !string.IsNullOrEmpty(paletteName))
            {
                UserLookAndFeel.Default.SetSkinStyle(skinName, paletteName);
            }
        }

        public Settings GetOrCreateSettings()
        {
            // JSON dosyasını yüklemeye çalış
            var settings = LoadSettingsFromJson();

            // Dosya yoksa varsayılan değerlerle yeni ayarlar oluştur
            if (settings == null)
            {
                settings = new Settings
                {
                    UpdateURL = "file://192.168.1.202/TeknikOfis/Vitem%20TopSolid%20Ortak/URGETools/UpdateInfo.xml"
                };

                // Varsayılan değerlerle JSON dosyasını kaydet
                SaveSettingsToJson(settings);
            }

            return settings;
        }


        public void SaveSettingsToJson(Settings settings)
        {
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented); // Okunaklı format
            File.WriteAllText(filePath, json);
        }

        public Settings LoadSettingsFromJson()
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Settings>(json);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var settings = GetOrCreateSettings(); // Ayarları yükle

            // SettingsForm'u göster
            using (SettingsForm form = new SettingsForm(settings))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Kullanıcı değişiklikleri onayladıysa ayarları kaydet
                    SaveSettingsToJson(settings);

                    //try { maintanenceForm.UpdateSettings(settings); } catch (Exception) { }

                    // Kullanıcıya bilgi ver
                    // XtraXtraMessageBox.Show("Ayarlar başarıyla kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // Update
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AutoUpdater.ReportErrors = true;
            CheckForUpdate();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            InfoForm iForm = new InfoForm();
            iForm.version = version;
            iForm.ShowDialog(this);
        }

        private string getVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            // AssemblyFileVersion'ı almak:
            object[] fileVersionAttributes = assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
            if (fileVersionAttributes.Length > 0)
            {
                var fileVersionAttribute = fileVersionAttributes[0] as AssemblyFileVersionAttribute;
                string fileVersion = fileVersionAttribute.Version;
                return fileVersion;
            }

            return "1.0.0.0";
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ResetUserSettings();
        }

        void ResetUserSettings()
        {
            try
            {
                // Settings.json dosyasını sil
                string settingsFile = Path.Combine(Application.StartupPath, "Settings.json");
                if (File.Exists(settingsFile))
                {
                    File.Delete(settingsFile);
                }

                // Designs klasörünün içeriğini temizle ama klasörü silme
                string designsFolder = Path.Combine(Application.StartupPath, "Designs");
                if (Directory.Exists(designsFolder))
                {
                    foreach (string file in Directory.GetFiles(designsFolder))
                    {
                        File.Delete(file);
                    }

                    foreach (string dir in Directory.GetDirectories(designsFolder))
                    {
                        Directory.Delete(dir, true); // true -> İçindeki tüm alt klasörleri de siler
                    }
                }

                XtraMessageBox.Show(this, "Ayarlar başarıyla sıfırlandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(this, $"Sıfırlama sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }


    public class Settings
    {
        public string UpdateURL { get; set; }
    }
}
