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
using xwcs.core.db;
using xwcs.core.evt;

namespace app.testing
{
	public partial class Form2_Clipboard : Form
	{
		private lib.db.doc.niterdoc.NiterDocEntities ctx;
		//private lib.db.doc.niterdoc.NiterDocEntities ctx2;
		BindingSource _bs;
		//RichEditControl a;
		public Form2_Clipboard()
		{
            xwcs.core.user.SecurityContext.getInstance().setUserProvider(new lib.core.user.BackOfficeUserProvider());

            SEventProxy.InvokeDelegate = this;

            InitializeComponent();
			ctx = new lib.db.doc.niterdoc.NiterDocEntities();
			ctx.Database.Log = Console.WriteLine;
            ctx.SetAsCurrentDbContext(); // eed for lazy load in entities

            /*
			ctx2 = new lib.db.doc.niterdoc.NiterDocEntities();
			ctx2.Database.Log = Console.WriteLine;
            */

			_bs = new BindingSource();

            gridView1.OptionsEditForm.CustomEditFormLayout = new NotesEditForm();
		}

		private void simpleButton1_Click(object sender, EventArgs e)
		{			
			
			refreshGrid();
		}
		
		private void refreshGrid()
		{
            _bs.PositionChanged += _bs_PositionChanged;
            //_bs.DataSource = ctx.iter.Take(100).ToList();
            _bs.DataSource = ctx.iter_in_xwbo_note.Take(100).ToList();
            gridControl1.DataSource = _bs;
		}

        private void _bs_PositionChanged(object sender, EventArgs e)
        {
            //handle current xw notes creation
            lib.db.doc.niterdoc.iter_in_xwbo_note tt = _bs.Current as lib.db.doc.niterdoc.iter_in_xwbo_note;

            // load note
            if (!ctx.Entry(tt).Reference("xwbo_note").IsLoaded)
            {
                ctx.Entry(tt).Reference("xwbo_note").Load();
            }

            //if not existent create it
            if(ReferenceEquals(null, tt.xwbo_note))
            {
                tt.xwbo_note = new lib.db.doc.niterdoc.xwbo_note();
                ctx.xwbo_note.Add(tt.xwbo_note);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
		{
			lib.db.doc.niterdoc.iter currentIter = _bs.Current as lib.db.doc.niterdoc.iter;
			var originalEntity = ctx.iter.Include("xwbo_iter").AsNoTracking().FirstOrDefault(o => o.id == currentIter.id);

            
            ctx.xwbo_iter.Add(originalEntity.xwbo_iter);
            ctx.iter.Add(originalEntity);
            originalEntity.ncopia += 1;
            originalEntity.xwbo_iter.ncopia += 1;
			//originalEntity.ndoc_xwbo_iter = xwbonew.ndoc;
			//ctx.iter.Add(originalEntity);
			ctx.SaveChanges();
		}

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            lib.db.doc.niterdoc.iter_in_xwbo_note tt = _bs.Current as lib.db.doc.niterdoc.iter_in_xwbo_note;

            ctx.SaveChanges();
        }
    }
}
