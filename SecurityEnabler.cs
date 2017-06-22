using DevExpress.Utils.Controls;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace app.testing
{
	public class SecurityEnabler
	{
		private List<System.ComponentModel.Component> _controls = null;
		private List<System.Windows.Forms.Control> _parentControls = null;
		private const int WM_SETREDRAW = 0x000B;

		public void registerControl(System.ComponentModel.Component bs)
		{
			if (_controls == null) _controls = new List<System.ComponentModel.Component>();
			if (_parentControls == null) _parentControls = new List<System.Windows.Forms.Control>();

			_controls.Add(bs);			

			PropertyInfo pi = bs.GetType().GetProperty("ParentContainerControl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
			if (pi != null)
			{
				System.Windows.Forms.Control parentC = pi.GetValue(bs) as System.Windows.Forms.Control;
				if (!_parentControls.Contains(parentC)) 
				{
					_parentControls.Add(parentC);
				}				
			}
		}

		public void enableAllControls(bool bEnable)
		{

			foreach (System.Windows.Forms.Control c in _parentControls)
			{				
				//c.SuspendLayout();
				//DrawingControl.SuspendDrawing(c);
			}

			foreach (System.ComponentModel.Component component in _controls)
			{
				System.Windows.Forms.Control control = component as System.Windows.Forms.Control;
				if (control != null)
				{
					control.Enabled = bEnable;
				}
				else
				{
					DevExpress.XtraBars.BarItem barItem = component as DevExpress.XtraBars.BarItem;
					if (barItem != null)
					{
						barItem.Enabled = bEnable;
					}
				}
			}

			foreach (System.Windows.Forms.Control c in _parentControls)
			{
				//c.ResumeLayout();
				//DrawingControl.ResumeDrawing(c);
			}
		}
	}
}
