using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
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

			for(int i = 1; i <= 300; i++)
			{
				Type myType = this.GetType();
				FieldInfo fi = myType.GetField("simpleButton" + i.ToString(), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
				if (fi != null)
				{
					SimpleButton btn = fi.GetValue(this) as SimpleButton;
					if (btn != null) _se.registerControl(btn);
				}
			}

			for (int i = 1; i <= 50; i++)
			{
				Type myType = this.GetType();
				FieldInfo fi = myType.GetField("barButtonItem" + i.ToString(), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
				if (fi != null)
				{
					BarButtonItem btn = fi.GetValue(this) as BarButtonItem;
					Component cc = btn;
					if (btn != null) _se.registerControl(btn);
				}
			}


			_se.registerControl(checkBox1);
			_se.registerControl(checkBox2);
			_se.registerControl(textBox1);
			_se.registerControl(textEdit1);

			_se.registerControl(panel2);
		}

		private void simpleButton21_Click(object sender, EventArgs e)
		{
			DrawingControl.SuspendDrawing(this);
			_se.enableAllControls(true);
			DrawingControl.ResumeDrawing(this);
		}

		private void simpleButton22_Click(object sender, EventArgs e)
		{
			DrawingControl.SuspendDrawing(this);
			_se.enableAllControls(false);
			DrawingControl.ResumeDrawing(this);
		}
	}
}
