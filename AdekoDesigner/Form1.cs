using DevExpress.LookAndFeel;
using System;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using DevExpress.XtraEditors;
using AutoUpdaterDotNET;
using AdekoDesigner.Properties;

namespace AdekoDesigner
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
        private AdekoLib adekoLib;

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


            LoadLibs();

            CheckForUpdate();
        }

        private void CheckForUpdate()
        {
            //if (string.IsNullOrEmpty(settings.UpdateURL)) settings.UpdateURL = "file://192.168.1.202/TeknikOfis/Vitem%20TopSolid%20Ortak/AdekoDesigner/UpdateInfo.xml";
            //AutoUpdater.ReportErrors = true;
            AutoUpdater.Start(settings.UpdateURL);
        }

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
            /// AdekoLib formunu oluştur ve göster
            
            if (adekoLib != null) adekoLib.Close();
            adekoLib = new AdekoLib
            {
                MdiParent = this,
                WindowState = FormWindowState.Maximized,
                settings = settings
            };

            adekoLib.Show();
            adekoLib.RefreshData();

            //// SettingsForm'u göster ve veri yenileme aksiyonunu ayarla
            //using (SettingsForm settingsForm = new SettingsForm(settings))
            //{
            //    if (settingsForm.ShowDialog() == DialogResult.OK)
            //    {
            //        // Kullanıcı ayarları onayladıysa, AdekoLib formundaki ayarları güncelle
            //        adekoLib.UpdateSettings(settingsForm.CurrentSettings);

            //        // Ayrıca, verileri de yenileyin
            //        adekoLib.RefreshData();
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
                    UpdateURL = "file://192.168.1.202/TeknikOfis/Vitem%20TopSolid%20Ortak/AdekoDesigner/UpdateInfo.xml",
                    MainDir = "C:\\Adeko 14",
                    FbDir = "C:\\Fenix\\VITEM2023.FDB",
                    FbUser = "SYSDBA",
                    FbPass = "masterkey",
                    IgnoredFolders = new List<string>
                    {
                        "kapak", "kulp", "cera", "adeko_render_viewer32",
                        "adeko_render_viewer64", "dclImages", "iconengines",
                        "imageformats", "lights", "materials", "platforms",
                        "ADEData", "adeko_render_viewer", "Agrx", "btoolsets",
                        "Fonts", "Help", "Imalat", "lang", "language",
                        "lng", "logs", "Patterns", "Shaders", "tefris",
                        "xmf_tr", ".git", "0FİYAT LİSTELERİ"
                    }
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

                    try { adekoLib.UpdateSettings(settings); } catch (Exception) { }

                    // Kullanıcıya bilgi ver
                    // XtraXtraMessageBox.Show("Ayarlar başarıyla kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

    }


    public class Settings
    {
        public string UpdateURL { get; set; }
        public string MainDir { get; set; }
        public string FbDir { get; set; }
        public string FbUser { get; set; }
        public string FbPass { get; set; }
        public List<string> IgnoredFolders { get; set; }
    }
}
