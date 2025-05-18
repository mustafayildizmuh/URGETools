namespace URGETools
{
    partial class MultiLang_ResGen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultiLang_ResGen));
            this.btnSelectTargetFolder = new DevExpress.XtraEditors.SimpleButton();
            this.btnSelectFormsFolder = new DevExpress.XtraEditors.SimpleButton();
            this.btnRun = new DevExpress.XtraEditors.SimpleButton();
            this.textBoxFormsFolder = new DevExpress.XtraEditors.TextEdit();
            this.textBoxTargetFolder = new DevExpress.XtraEditors.TextEdit();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtOutput = new System.Windows.Forms.RichTextBox();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btnSelectDllFolder = new DevExpress.XtraEditors.SimpleButton();
            this.textBoxDllFolder = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxFormsFolder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxTargetFolder.Properties)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxDllFolder.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSelectTargetFolder
            // 
            this.btnSelectTargetFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelectTargetFolder.Location = new System.Drawing.Point(3, 3);
            this.btnSelectTargetFolder.Name = "btnSelectTargetFolder";
            this.btnSelectTargetFolder.Size = new System.Drawing.Size(220, 34);
            this.btnSelectTargetFolder.TabIndex = 2;
            this.btnSelectTargetFolder.Text = "Hedef Resx Dosyaları Klasörü Seç";
            this.btnSelectTargetFolder.Click += new System.EventHandler(this.btnSelectTargetFolder_Click);
            // 
            // btnSelectFormsFolder
            // 
            this.btnSelectFormsFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelectFormsFolder.Location = new System.Drawing.Point(3, 43);
            this.btnSelectFormsFolder.Name = "btnSelectFormsFolder";
            this.btnSelectFormsFolder.Size = new System.Drawing.Size(220, 34);
            this.btnSelectFormsFolder.TabIndex = 2;
            this.btnSelectFormsFolder.Text = "Formların Bulunduğu Klasörü Seç";
            this.btnSelectFormsFolder.Click += new System.EventHandler(this.btnSelectFormsFolder_Click);
            // 
            // btnRun
            // 
            this.btnRun.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRun.Location = new System.Drawing.Point(810, 123);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(138, 34);
            this.btnRun.TabIndex = 2;
            this.btnRun.Text = "İşleme Başla";
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // textBoxFormsFolder
            // 
            this.textBoxFormsFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxFormsFolder.Location = new System.Drawing.Point(229, 43);
            this.textBoxFormsFolder.Name = "textBoxFormsFolder";
            this.textBoxFormsFolder.Properties.AutoHeight = false;
            this.textBoxFormsFolder.Size = new System.Drawing.Size(719, 34);
            this.textBoxFormsFolder.TabIndex = 0;
            // 
            // textBoxTargetFolder
            // 
            this.textBoxTargetFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxTargetFolder.Location = new System.Drawing.Point(229, 3);
            this.textBoxTargetFolder.Name = "textBoxTargetFolder";
            this.textBoxTargetFolder.Properties.AutoHeight = false;
            this.textBoxTargetFolder.Size = new System.Drawing.Size(719, 34);
            this.textBoxTargetFolder.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 226F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.textBoxDllFolder, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSelectDllFolder, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupControl1, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.textBoxTargetFolder, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSelectTargetFolder, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSelectFormsFolder, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxFormsFolder, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnRun, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(951, 608);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // txtOutput
            // 
            this.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOutput.Location = new System.Drawing.Point(2, 23);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(941, 417);
            this.txtOutput.TabIndex = 0;
            this.txtOutput.Text = "";
            // 
            // groupControl1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupControl1, 2);
            this.groupControl1.Controls.Add(this.txtOutput);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(3, 163);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(945, 442);
            this.groupControl1.TabIndex = 3;
            this.groupControl1.Text = "İşlemler";
            // 
            // btnSelectDllFolder
            // 
            this.btnSelectDllFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelectDllFolder.Location = new System.Drawing.Point(3, 83);
            this.btnSelectDllFolder.Name = "btnSelectDllFolder";
            this.btnSelectDllFolder.Size = new System.Drawing.Size(220, 34);
            this.btnSelectDllFolder.TabIndex = 4;
            this.btnSelectDllFolder.Text = "DLL seç";
            this.btnSelectDllFolder.Click += new System.EventHandler(this.btnSelectDllFolder_Click);
            // 
            // textBoxDllFolder
            // 
            this.textBoxDllFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDllFolder.Location = new System.Drawing.Point(229, 83);
            this.textBoxDllFolder.Name = "textBoxDllFolder";
            this.textBoxDllFolder.Properties.AutoHeight = false;
            this.textBoxDllFolder.Size = new System.Drawing.Size(719, 34);
            this.textBoxDllFolder.TabIndex = 5;
            // 
            // MultiLang_ResGen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(951, 608);
            this.Controls.Add(this.tableLayoutPanel1);
            this.IconOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("MultiLang_ResGen.IconOptions.SvgImage")));
            this.Name = "MultiLang_ResGen";
            this.Text = "Çoklu Dil Resx Generator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MultiLang_ResGen_FormClosing);
            this.Load += new System.EventHandler(this.MultiLang_ResGen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textBoxFormsFolder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxTargetFolder.Properties)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textBoxDllFolder.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit textBoxTargetFolder;
        private DevExpress.XtraEditors.SimpleButton btnSelectTargetFolder;
        private DevExpress.XtraEditors.TextEdit textBoxFormsFolder;
        private DevExpress.XtraEditors.SimpleButton btnSelectFormsFolder;
        private DevExpress.XtraEditors.SimpleButton btnRun;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.TextEdit textBoxDllFolder;
        private DevExpress.XtraEditors.SimpleButton btnSelectDllFolder;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private System.Windows.Forms.RichTextBox txtOutput;
    }
}