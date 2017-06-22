using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app.testing
{
	class DrawingControl
	{ 
		
		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

		private const int WM_SETREDRAW = 11;

		public static void SuspendDrawing(Control cc)
		{
			SendMessage(cc.Handle, WM_SETREDRAW, false, 0);
		}

		public static void ResumeDrawing(Control cc)
		{
			SendMessage(cc.Handle, WM_SETREDRAW, true, 0);
			cc.Refresh();
		}
	}
}
