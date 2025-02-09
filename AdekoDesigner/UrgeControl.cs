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
    public partial class UrgeControl : DevExpress.XtraEditors.XtraForm
    {
        public UrgeControl()
        {
            InitializeComponent();
        }

        public BindingList<AdekoModule> adekoModuleList_canRead = new BindingList<AdekoModule>();

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}