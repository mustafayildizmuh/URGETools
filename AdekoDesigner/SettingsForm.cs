using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdekoDesigner
{
    public partial class SettingsForm : DevExpress.XtraEditors.XtraForm
    {
        public Settings CurrentSettings { get; set; } // Ana formdan alınacak ayarlar

        public SettingsForm(Settings settings)
        {
            InitializeComponent();
            CurrentSettings = settings; // Ana formdan gelen ayarları al
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            // Ayarları forma yükle
            textFolderPath.Text = CurrentSettings.MainDir;

            // IgnoredFolders listesini richTextBox'a yükle
            richTextBox_IgnoredFolderList.Text = string.Join(Environment.NewLine, CurrentSettings.IgnoredFolders);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Kullanıcının yaptığı değişiklikleri CurrentSettings'e aktar
            CurrentSettings.MainDir = textFolderPath.Text;

            // IgnoredFolders listesini richTextBox'tan doğru şekilde al
            CurrentSettings.IgnoredFolders = richTextBox_IgnoredFolderList.Lines
                .Where(line => !string.IsNullOrWhiteSpace(line)) // Boş satırları atla
                .ToList();

            this.DialogResult = DialogResult.OK; // Değişiklikleri onayla
            this.Close();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; // Değişiklikleri iptal et
            this.Close();
        }

        private void textFolderPath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Ana klasörü seçin";
                folderBrowserDialog.SelectedPath = textFolderPath.Text;

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    textFolderPath.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }


    }
}