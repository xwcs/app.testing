using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xwcs.core.evt;

namespace app.testing
{
	/*
	insert into niter.iter(numero, nrecord, ncopia, versione, build, id_tipologie, stati_stato)
	SELECT numero, nrecord, (ncopia + 150) as nrecord, versione, build, id_tipologie, stati_stato FROM niter.iter;

  int noOfRowUpdated = ctx.Database.ExecuteSqlCommand("Update student 
            set studentname ='changed student by command' where studentid=1");\

			progressBarControl1
	*/
	public partial class Form3_MassiveReplace : Form
	{
		private lib.db.doc.niterdoc.NiterDocEntities ctx;
		private BindingSource _bs;

		public Form3_MassiveReplace()
		{
			InitializeComponent();

			xwcs.core.user.SecurityContext.getInstance().setUserProvider(new lib.core.user.BackOfficeUserProvider());
			SEventProxy.InvokeDelegate = this;

			ctx = new lib.db.doc.niterdoc.NiterDocEntities();
			//ctx.Database.Log = Console.WriteLine;
			_bs = new BindingSource();
		}

		private void simpleButton1_Click(object sender, EventArgs e)
		{
			_bs.DataSource = ctx.iter.Take(50000).ToList();
			gridControl1.DataSource = _bs;
		}

		private void hideColumns()
		{
			foreach (DevExpress.XtraGrid.Columns.GridColumn c in gridView1.Columns)
			{
				if (c.FieldName == "oggetto_uff_edit") continue;
				if (c.FieldName == "numero") continue;
				c.Visible = false;
			}
		}

		private void simpleButton2_Click(object sender, EventArgs e)
		{
			hideColumns();
		}

		private void searchAndReplaceEF()
		{
			string fieldToReplace = textBox1.Text;

			DevExpress.XtraGrid.Columns.GridColumn column = gridView1.Columns[fieldToReplace];
			string searchValue = textBox2.Text;
			string newValue = textBox3.Text;

			int iCount = 0;
			int iTotalCount = 0;
			gridView1.BeginUpdate();
			for (int i = 0; i < gridView1.DataRowCount; i++)
			{
				object value = gridView1.GetRowCellDisplayText(i, column);
				if (value.ToString().Contains(searchValue))
				{
					gridView1.SetRowCellValue(i, column, newValue);
					iCount++;
					iTotalCount++;
					if (iCount > 1000)
					{
						ctx.SaveChanges();
						iCount = 0;
						Console.WriteLine("1000 items saved");
					}					
				}
			}
			ctx.SaveChanges();
			gridView1.EndUpdate();
			MessageBox.Show("Changed items : " + iTotalCount.ToString());
		}

		private void simpleButton3_Click(object sender, EventArgs e)
		{
			searchAndReplaceEF();
		}

		private void simpleButton4_Click(object sender, EventArgs e)
		{
			searchAndReplaceDB();
		}

		private void searchAndReplaceDB()
		{
			string fieldToReplace = textBox1.Text;
			DevExpress.XtraGrid.Columns.GridColumn column = gridView1.Columns[fieldToReplace];
			DevExpress.XtraGrid.Columns.GridColumn columnID = gridView1.Columns["id"];
			string searchValue = textBox2.Text;
			string newValue = textBox3.Text;
			StringBuilder sql = new StringBuilder(1000);
			sql.Append("update iter set " + fieldToReplace + " = '" + newValue + "' where id in (");
			//string sql = "update iter set " + fieldToReplace + " = '" + newValue + "' where id in (";
			int iCount = 0;

			for (int i = 0; i < gridView1.DataRowCount; i++)
			{
				object value = gridView1.GetRowCellDisplayText(i, column);
				if (value.ToString().Contains(searchValue))
				{
					if (iCount > 0)
					{
						//sql += ",";
						sql.Append(",");
					}

					//sql = string.Concat(sql, gridView1.GetRowCellDisplayText(i, columnID).ToString());
					sql.Append(gridView1.GetRowCellDisplayText(i, columnID).ToString());
					iCount++;
				}
			}
			//sql += ")";
			sql.Append(")");

			string tmp = sql.ToString();
			Console.WriteLine("String size : " + tmp.Length.ToString());
			int noOfRowUpdated = ctx.Database.ExecuteSqlCommand(tmp);
			Console.WriteLine("Records updated : " + noOfRowUpdated.ToString());
		}
	}
}
