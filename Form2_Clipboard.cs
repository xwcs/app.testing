using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.XtraRichEdit;

namespace app.testing
{
	public partial class Form2_Clipboard : Form
	{
		private lib.db.doc.niterdoc.NiterDocEntities ctx;
		private lib.db.doc.niterdoc.NiterDocEntities ctx2;
		BindingSource _bs;
		RichEditControl a;
		public Form2_Clipboard()
		{
			InitializeComponent();
			ctx = new lib.db.doc.niterdoc.NiterDocEntities();
			ctx.Database.Log = Console.WriteLine;

			ctx2 = new lib.db.doc.niterdoc.NiterDocEntities();
			ctx2.Database.Log = Console.WriteLine;

			_bs = new BindingSource();
		}

		private void simpleButton1_Click(object sender, EventArgs e)
		{
			ctx.iter.Load();
			
			refreshGrid();
		}
		
		private void refreshGrid()
		{
			_bs.DataSource = ctx.iter.ToList();
			gridControl1.DataSource = _bs.DataSource;
		}
		private void simpleButton2_Click(object sender, EventArgs e)
		{
			lib.db.doc.niterdoc.iter currentIter = _bs.Current as lib.db.doc.niterdoc.iter;
			var originalEntity = ctx.iter.AsNoTracking().FirstOrDefault(o => o.id == currentIter.id);
			
			lib.db.doc.niterdoc.xwbo_iter xwbo = ctx2.xwbo_iter.AsNoTracking().FirstOrDefault(o => o.ndoc == originalEntity.ndoc_xwbo_iter);
			xwbo.ncopia += 1;
			ctx2.xwbo_iter.Add(xwbo);
			ctx2.SaveChanges();


			lib.db.doc.niterdoc.xwbo_iter xwbonew = ctx.xwbo_iter.AsNoTracking().FirstOrDefault(o => o.nrec == xwbo.nrec && o.ncopia == xwbo.ncopia);


			ctx.iter.Add(originalEntity);	

			originalEntity.ncopia += 1;
			originalEntity.ndoc_xwbo_iter = xwbonew.ndoc;
			ctx.iter.Add(originalEntity);
			ctx.SaveChanges();
		}
	}
}
