using System;

namespace URGETools
{
    public partial class UrgeControl : DevExpress.XtraEditors.XtraForm
    {
        public UrgeControl()
        {
            InitializeComponent();
        }

        //public BindingList<AdekoModule> adekoModuleList_canRead = new BindingList<AdekoModule>();

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}