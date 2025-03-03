namespace URGETools
{
    partial class FirebirdMaintanence
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FirebirdMaintanence));
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.txtOutput = new System.Windows.Forms.RichTextBox();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.txtCommandsLog = new System.Windows.Forms.RichTextBox();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.chkFullValidation = new DevExpress.XtraEditors.ToggleSwitch();
            this.chkMend = new DevExpress.XtraEditors.ToggleSwitch();
            this.chkCreateBackupCopy = new DevExpress.XtraEditors.ToggleSwitch();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnRepair = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressBar1 = new DevExpress.XtraEditors.ProgressBarControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel1)).BeginInit();
            this.splitContainerControl1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel2)).BeginInit();
            this.splitContainerControl1.Panel2.SuspendLayout();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkFullValidation.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMend.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCreateBackupCopy.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            // 
            // splitContainerControl1.Panel1
            // 
            this.splitContainerControl1.Panel1.Controls.Add(this.groupControl1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            // 
            // splitContainerControl1.Panel2
            // 
            this.splitContainerControl1.Panel2.Controls.Add(this.groupControl2);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(910, 451);
            this.splitContainerControl1.SplitterPosition = 315;
            this.splitContainerControl1.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.txtOutput);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(910, 315);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "İşlemler";
            // 
            // txtOutput
            // 
            this.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOutput.Location = new System.Drawing.Point(2, 23);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(906, 290);
            this.txtOutput.TabIndex = 0;
            this.txtOutput.Text = "";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.txtCommandsLog);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl2.Location = new System.Drawing.Point(0, 0);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(910, 120);
            this.groupControl2.TabIndex = 0;
            this.groupControl2.Text = "Komutlar";
            // 
            // txtCommandsLog
            // 
            this.txtCommandsLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCommandsLog.Location = new System.Drawing.Point(2, 23);
            this.txtCommandsLog.Name = "txtCommandsLog";
            this.txtCommandsLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtCommandsLog.Size = new System.Drawing.Size(906, 95);
            this.txtCommandsLog.TabIndex = 0;
            this.txtCommandsLog.Text = "";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.tableLayoutPanel1);
            this.panelControl1.Controls.Add(this.panelControl2);
            this.panelControl1.Controls.Add(this.btnRepair);
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 485);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(910, 57);
            this.panelControl1.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.34417F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.65583F));
            this.tableLayoutPanel1.Controls.Add(this.chkFullValidation, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkMend, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkCreateBackupCopy, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(457, 53);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // chkFullValidation
            // 
            this.chkFullValidation.Location = new System.Drawing.Point(187, 3);
            this.chkFullValidation.Name = "chkFullValidation";
            this.chkFullValidation.Properties.AutoHeight = false;
            this.chkFullValidation.Properties.OffText = "(gfix -v -full) Kullanma";
            this.chkFullValidation.Properties.OnText = "(gfix -v -full) Kullan";
            this.chkFullValidation.Size = new System.Drawing.Size(177, 20);
            this.chkFullValidation.TabIndex = 11;
            this.chkFullValidation.ToolTip = "Veritabanını tam doğrulama (gfix -v -full) ile kontrol eder. \r\nEğer veritabanı bo" +
    "zuk ve açılmıyorsa, (gfix -v -full) ile kontrol edip, raporlama sağlar.";
            // 
            // chkMend
            // 
            this.chkMend.Location = new System.Drawing.Point(187, 29);
            this.chkMend.Name = "chkMend";
            this.chkMend.Properties.AutoHeight = false;
            this.chkMend.Properties.OffText = "(gfix -mend) Veritabanı Onarımı Uygulama";
            this.chkMend.Properties.OnText = "(gfix -mend) Veritabanı Onarımı Uygula";
            this.chkMend.Size = new System.Drawing.Size(254, 21);
            this.chkMend.TabIndex = 11;
            this.chkMend.ToolTip = resources.GetString("chkMend.ToolTip");
            // 
            // chkCreateBackupCopy
            // 
            this.chkCreateBackupCopy.EditValue = true;
            this.chkCreateBackupCopy.Location = new System.Drawing.Point(3, 3);
            this.chkCreateBackupCopy.Name = "chkCreateBackupCopy";
            this.chkCreateBackupCopy.Properties.AutoHeight = false;
            this.chkCreateBackupCopy.Properties.OffText = "Kopya ile Yedek Kullanma";
            this.chkCreateBackupCopy.Properties.OnText = "Kopya ile Yedek Kullan";
            this.chkCreateBackupCopy.Size = new System.Drawing.Size(177, 20);
            this.chkCreateBackupCopy.TabIndex = 11;
            this.chkCreateBackupCopy.ToolTip = "Veritabanınında aktif bağlantı/kullanıcılar var ise aktifleştirin";
            // 
            // panelControl2
            // 
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl2.Location = new System.Drawing.Point(2, 2);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(10, 53);
            this.panelControl2.TabIndex = 6;
            // 
            // btnRepair
            // 
            this.btnRepair.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRepair.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnRepair.ImageOptions.Image")));
            this.btnRepair.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnRepair.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnRepair.Location = new System.Drawing.Point(703, 2);
            this.btnRepair.Name = "btnRepair";
            this.btnRepair.Size = new System.Drawing.Size(117, 53);
            this.btnRepair.TabIndex = 0;
            this.btnRepair.Text = "Veritabanı Seç";
            this.btnRepair.Click += new System.EventHandler(this.btnRepair_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnCancel.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnCancel.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnCancel.ImageOptions.SvgImage")));
            this.btnCancel.Location = new System.Drawing.Point(820, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 53);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "İptal";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 451);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(910, 34);
            this.progressBar1.TabIndex = 2;
            // 
            // FirebirdMaintanence
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 542);
            this.Controls.Add(this.splitContainerControl1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.panelControl1);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("FirebirdMaintanence.IconOptions.Image")));
            this.Name = "FirebirdMaintanence";
            this.Text = "Firebird Databse Bakım";
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel1)).EndInit();
            this.splitContainerControl1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel2)).EndInit();
            this.splitContainerControl1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkFullValidation.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMend.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCreateBackupCopy.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnRepair;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.ToggleSwitch chkFullValidation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.ToggleSwitch chkMend;
        private DevExpress.XtraEditors.ToggleSwitch chkCreateBackupCopy;
        private System.Windows.Forms.RichTextBox txtOutput;
        private System.Windows.Forms.RichTextBox txtCommandsLog;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private DevExpress.XtraEditors.ProgressBarControl progressBar1;
    }
}