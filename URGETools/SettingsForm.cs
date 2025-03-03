using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace URGETools
{
    public partial class SettingsForm : DevExpress.XtraEditors.XtraForm
    {
        public Settings CurrentSettings { get; set; }
        public Action RefreshDataAction { get; set; } // AdekoLib'deki RefreshData metodunu tetikleyecek aksiyon

        public SettingsForm(Settings settings)
        {
            InitializeComponent();
            CurrentSettings = settings; // Ana formdan gelen ayarları al
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            // Ayarları forma yükle
            //textFolderPath.Text = CurrentSettings.MainDir;
            //textFbDir.Text = CurrentSettings.FbDir;
            //textFbUser.Text = CurrentSettings.FbUser;
            //textFbPass.Text = CurrentSettings.FbPass;

            //// IgnoredFolders listesini richTextBox'a yükle
            //richTextBox_IgnoredFolderList.Text = string.Join(Environment.NewLine, CurrentSettings.IgnoredFolders);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //// Kullanıcının yaptığı değişiklikleri CurrentSettings'e aktar
            //CurrentSettings.MainDir = textFolderPath.Text;
            //CurrentSettings.FbDir = textFbDir.Text;
            //CurrentSettings.FbUser = textFbUser.Text;
            //CurrentSettings.FbPass = textFbPass.Text;

            //// IgnoredFolders listesini richTextBox'tan doğru şekilde al
            //CurrentSettings.IgnoredFolders = richTextBox_IgnoredFolderList.Lines
            //    .Where(line => !string.IsNullOrWhiteSpace(line)) // Boş satırları atla
            //    .ToList();

            //// Ayarları kaydet
            //this.DialogResult = DialogResult.OK; // Değişiklikleri onayla
            //this.Close();

            //// Verileri yenilemek için RefreshDataAction'ı çağır
            //RefreshDataAction?.Invoke();
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