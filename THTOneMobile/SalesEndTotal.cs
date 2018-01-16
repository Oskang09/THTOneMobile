using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using THTOneMobile.Classes;

namespace THTOneMobile
{
    public partial class SalesEndTotal : Form
    {
        public SalesEndTotal(string a)
        {
            InitializeComponent();
            total.Text = a;
        }

        private void endBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
