﻿using System;
using System.Windows.Forms;

namespace URGETools
{
    public partial class InfoForm : DevExpress.XtraEditors.XtraForm
    {
        public InfoForm()
        {
            InitializeComponent();
        }

        public string version;

        private void InfoForm_Load(object sender, EventArgs e)
        {
            labelVersion.Text = version;
        }

        private void hyperlinkLabelControl1_Click(object sender, EventArgs e)
        {

            string mailtoLink = "mailto:" + hyperlinkLabelControl1.Text + "?subject=Adeko Designer Destek Talebi &body=Telep içeriğinizi buraya yazabilirsiniz..";

            try
            {
                System.Diagnostics.Process.Start(mailtoLink);
            }
            catch (Exception ex)
            {
                MessageBox.Show("E-posta istemcisi açılırken bir hata oluştu: " + ex.Message);
            }
        }
    }
}