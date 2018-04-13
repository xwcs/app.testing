using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xwcs.core;

namespace app.testing
{
    public partial class Intervals : Form
    {
        public Intervals()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int[] a = new int[] { 10, 11, 12, 14, 15, 1, 3, 4, 5, 7, 9, 16, 20 }; //-> 1,  (3,5), 7, (9,12), (14,16), 20

            foreach(var i in a.OrderBy(el => el).Intervals())
            {
                memoEdit1.Text += string.Join(",", i) + Environment.NewLine;
            }
        }
    }
}
