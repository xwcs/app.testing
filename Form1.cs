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

namespace app.testing
{
	public partial class Form1 : Form
	{
		private lib.db.doc.niterdoc.NiterDocEntitiesDoc ctx;
		public Form1()
		{
			InitializeComponent();
			ctx = new lib.db.doc.niterdoc.NiterDocEntitiesDoc();
			ctx.Database.Log = Console.WriteLine;
		}

		private void simpleButton1_Click(object sender, EventArgs e)
		{
			ctx.edizione.Load();
			
			refreshGrid();
		}
		
		private void refreshGrid()
		{
			string bookmark = "";

			if (gridView1.FocusedRowHandle >= 0) bookmark = gridView1.GetRowCellDisplayText(gridView1.FocusedRowHandle, "id");

			gridControl1.DataSource = ctx.edizione.Where(x => x.nrecord == 30615).ToList().OrderBy(x => x.ordine);

			if (bookmark == "") return;

			int tmp = gridView1.LocateByDisplayText(0, gridView1.Columns["id"], bookmark);
			if (tmp >= 0)
			{
				gridView1.FocusedRowHandle = tmp;
				gridView1.MakeRowVisible(tmp, true);
			}
		}

		private void simpleButton2_Click(object sender, EventArgs e)
		{
			int iRowSelect = gridView1.FocusedRowHandle;

			if (iRowSelect >= 0)
			{
				lib.db.doc.niterdoc.edizione actualData = gridView1.GetRow(iRowSelect) as lib.db.doc.niterdoc.edizione;
				lib.db.doc.niterdoc.edizione beforeData = gridView1.GetRow(iRowSelect - 1) as lib.db.doc.niterdoc.edizione;

				if ((beforeData == null) || (actualData.ordine == beforeData.ordine)) return;

				actualData.ordine--;
				beforeData.ordine++;
				refreshGrid();
			}
		}

		private void simpleButton3_Click(object sender, EventArgs e)
		{
			int iRowSelect = gridView1.FocusedRowHandle;

			if (iRowSelect >= 0)
			{
				lib.db.doc.niterdoc.edizione actualData = gridView1.GetRow(iRowSelect) as lib.db.doc.niterdoc.edizione;
				lib.db.doc.niterdoc.edizione nextData = gridView1.GetRow(iRowSelect + 1) as lib.db.doc.niterdoc.edizione;

				if ((nextData == null) || (actualData.ordine == nextData.ordine)) return;

				actualData.ordine++;
				nextData.ordine--;
				refreshGrid();
			}
		}

		private void simpleButton4_Click(object sender, EventArgs e)
		{
			ctx.SaveChanges();
		}
	}
}
