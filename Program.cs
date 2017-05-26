using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app.testing
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new Form1());
			//Application.Run(new lib.db.states.iter.IterSMTestForm());
			//Application.Run(new lib.db.states.bt.BtSMTestForm());
			//Application.Run(new Form2_Clipboard());
			Application.Run(new MemoInGridForm());
		}
	}
}
