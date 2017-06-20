using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.testing
{
	public class SecurityEnabler
	{
		private List<System.Windows.Forms.Control> _controls = null;
		public void registerControl(System.Windows.Forms.Control bs)
		{
			if (_controls == null) _controls = new List<System.Windows.Forms.Control>();
			_controls.Add(bs);
		}

		public void enableAllControls(bool bEnable)
		{
			foreach(System.Windows.Forms.Control c in _controls)
			{
				c.Enabled = bEnable;
			}
		}
	}
}
