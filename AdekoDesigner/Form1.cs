using DevExpress.LookAndFeel;
using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.LookAndFeel;

namespace AdekoDesigner
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Form1()
        {
            InitializeComponent();

            LoadUserSkinAndPalette(); // Apply saved skin and palette

            this.IsMdiContainer = true;
        }

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
            // AdekoLib formunu oluştur ve göster
            AdekoLib adekoLib = new AdekoLib
            {
                MdiParent = this,
                WindowState = FormWindowState.Maximized
            };
            adekoLib.Show();
            adekoLib.RefreshData();
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



    }
}
