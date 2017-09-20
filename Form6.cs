using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app.testing
{
	public partial class Form6 : Form
	{
		public Form6()
		{
			InitializeComponent();
		}

		private void simpleButton1_Click(object sender, EventArgs e)
		{
			xwcs.core.print.ReportOptions model = new xwcs.core.print.ReportOptions();
			xwcs.core.ui.controls.EntityEditForm<xwcs.core.print.ReportOptions> form = new xwcs.core.ui.controls.EntityEditForm<xwcs.core.print.ReportOptions>(model);

			//lib.db.doc.niterdoc.autori a = new lib.db.doc.niterdoc.autori();
			//xwcs.core.ui.controls.EntityEditForm<lib.db.doc.niterdoc.autori> form = new xwcs.core.ui.controls.EntityEditForm<lib.db.doc.niterdoc.autori>(a);
			form.ShowDialog();

		}
	}
}
