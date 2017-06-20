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
	public partial class Form4_EnableDisableControls : Form
	{
		private SecurityEnabler _se = new SecurityEnabler();
		public Form4_EnableDisableControls()
		{
			InitializeComponent();

			_se.registerControl(simpleButton1);
			_se.registerControl(simpleButton2);
			_se.registerControl(simpleButton3);
			_se.registerControl(simpleButton4);
			_se.registerControl(simpleButton5);
			_se.registerControl(simpleButton6);
			_se.registerControl(simpleButton7);
			_se.registerControl(simpleButton8);
			_se.registerControl(simpleButton9);
			_se.registerControl(simpleButton10);
			_se.registerControl(simpleButton11);
			_se.registerControl(simpleButton12);
			_se.registerControl(simpleButton13);
			_se.registerControl(simpleButton14);
			_se.registerControl(simpleButton15);
			_se.registerControl(simpleButton16);
			_se.registerControl(simpleButton17);
			_se.registerControl(simpleButton18);
			_se.registerControl(simpleButton19);
			_se.registerControl(simpleButton20);

			_se.registerControl(simpleButton23);
			_se.registerControl(simpleButton24);
			_se.registerControl(simpleButton25);
			_se.registerControl(simpleButton26);
			_se.registerControl(simpleButton27);
			_se.registerControl(simpleButton28);
			_se.registerControl(simpleButton29);
			_se.registerControl(simpleButton30);

			_se.registerControl(simpleButton31);
			_se.registerControl(checkBox1);
			_se.registerControl(checkBox2);
			_se.registerControl(textBox1);
			_se.registerControl(textEdit1);

			_se.registerControl(panel2);
		}

		private void simpleButton21_Click(object sender, EventArgs e)
		{
			_se.enableAllControls(true);
		}

		private void simpleButton22_Click(object sender, EventArgs e)
		{
			_se.enableAllControls(false);
		}
	}
}
