namespace AdekoDesigner
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.richTextBox_IgnoredFolderList = new System.Windows.Forms.RichTextBox();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.textFolderPath = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.textFbDir = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.textFbUser = new DevExpress.XtraEditors.TextEdit();
            this.textFbPass = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.textFolderPath.Properties)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textFbDir.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textFbUser.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textFbPass.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControl1.Location = new System.Drawing.Point(3, 3);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(90, 30);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Adeko 14 Dizini";
            // 
            // labelControl2
            // 
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl2.Location = new System.Drawing.Point(5, 5);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(93, 42);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Kütüphaneye Eklenmeyecek Klasörler Listesi";
            // 
            // richTextBox_IgnoredFolderList
            // 
            this.richTextBox_IgnoredFolderList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_IgnoredFolderList.Location = new System.Drawing.Point(99, 39);
            this.richTextBox_IgnoredFolderList.Name = "richTextBox_IgnoredFolderList";
            this.richTextBox_IgnoredFolderList.Size = new System.Drawing.Size(225, 471);
            this.richTextBox_IgnoredFolderList.TabIndex = 3;
            this.richTextBox_IgnoredFolderList.Text = "";
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnSave.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnSave.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnSave.ImageOptions.SvgImage")));
            this.btnSave.Location = new System.Drawing.Point(469, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 38);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Kaydet";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnCancel.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnCancel.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnCancel.ImageOptions.SvgImage")));
            this.btnCancel.Location = new System.Drawing.Point(557, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 38);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "İptal";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // textFolderPath
            // 
            this.textFolderPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textFolderPath.Location = new System.Drawing.Point(99, 2);
            this.textFolderPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textFolderPath.Name = "textFolderPath";
            this.textFolderPath.Size = new System.Drawing.Size(225, 28);
            this.textFolderPath.TabIndex = 5;
            this.textFolderPath.ToolTipTitle = "Lütfen Dosya Seçiniz";
            this.textFolderPath.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.textFolderPath_ButtonClick);
            // 
            // labelControl3
            // 
            this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl3.Location = new System.Drawing.Point(5, 51);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(72, 32);
            this.labelControl3.TabIndex = 1;
            this.labelControl3.Text = "*(Her Klasör için 1 Satır)";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 320F));
            this.tableLayoutPanel1.Controls.Add(this.textFolderPath, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.richTextBox_IgnoredFolderList, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelControl1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(647, 513);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 121F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.labelControl6, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.labelControl5, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.textFbDir, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelControl4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.textFbUser, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.textFbPass, 1, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(316, 467);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // labelControl6
            // 
            this.labelControl6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControl6.Location = new System.Drawing.Point(3, 75);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelControl6.Size = new System.Drawing.Size(115, 30);
            this.labelControl6.TabIndex = 13;
            this.labelControl6.Text = "FireBird Kullanıcı Şifresi";
            // 
            // labelControl5
            // 
            this.labelControl5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControl5.Location = new System.Drawing.Point(3, 39);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelControl5.Size = new System.Drawing.Size(115, 30);
            this.labelControl5.TabIndex = 11;
            this.labelControl5.Text = "Firebird Kullanıcı Adı";
            // 
            // textFbDir
            // 
            this.textFbDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textFbDir.Location = new System.Drawing.Point(124, 2);
            this.textFbDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textFbDir.Name = "textFbDir";
            this.textFbDir.Size = new System.Drawing.Size(189, 28);
            this.textFbDir.TabIndex = 10;
            this.textFbDir.ToolTipTitle = "Lütfen Dosya Seçiniz";
            // 
            // labelControl4
            // 
            this.labelControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControl4.Location = new System.Drawing.Point(3, 3);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelControl4.Size = new System.Drawing.Size(115, 30);
            this.labelControl4.TabIndex = 9;
            this.labelControl4.Text = "UR-GE FireBird Dizini";
            // 
            // textFbUser
            // 
            this.textFbUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textFbUser.Location = new System.Drawing.Point(124, 39);
            this.textFbUser.Name = "textFbUser";
            this.textFbUser.Size = new System.Drawing.Size(189, 28);
            this.textFbUser.TabIndex = 12;
            // 
            // textFbPass
            // 
            this.textFbPass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textFbPass.Location = new System.Drawing.Point(124, 75);
            this.textFbPass.Name = "textFbPass";
            this.textFbPass.Size = new System.Drawing.Size(189, 28);
            this.textFbPass.TabIndex = 12;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 513);
            this.Controls.Add(this.tableLayoutPanel1);
            this.IconOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("SettingsForm.IconOptions.SvgImage")));
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ayarlar";
            this.Load += new System.EventHandler(this.Settings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textFolderPath.Properties)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textFbDir.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textFbUser.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textFbPass.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private System.Windows.Forms.RichTextBox richTextBox_IgnoredFolderList;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.ButtonEdit textFolderPath;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.ButtonEdit textFbDir;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit textFbUser;
        private DevExpress.XtraEditors.TextEdit textFbPass;
    }
}