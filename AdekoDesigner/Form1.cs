using System;
using System.Drawing;
using System.Windows.Forms;

namespace AdekoDesigner
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Form1()
        {
            InitializeComponent();
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
    }
}
