using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xwcs.core.db.binding;
using xwcs.core.evt;
using xwcs.core.manager;

namespace app.testing
{
	public partial class Form5_DataTableTest : Form
	{
		private lib.db.doc.niterdoc.NiterDocEntities ctx;
		private DataTable _dataTable;
		//private BindingSource _bs = new BindingSource();
		private GridBindingSource _gbs = new GridBindingSource();
		public Form5_DataTableTest()
		{
			SEventProxy.InvokeDelegate = this;

			xwcs.core.user.SecurityContext.getInstance().setUserProvider(new lib.core.user.BackOfficeUserProvider());

			InitializeComponent();
			

			initDataTable();
		}

		protected override void Dispose(bool disposing)
		{


			if (disposing)
			{
				// kill loggers
				SLogManager.getInstance().Dispose();

				if (components != null) components.Dispose();

			}
			base.Dispose(disposing);
		}

		private void initDataTable()
		{
			ctx = new lib.db.doc.niterdoc.NiterDocEntities();

			
			ctx.iter.Take(100).ToList();
			_gbs.DataSource = ctx.iter.Local.ToBindingList();
			_gbs.AttachToGrid(gridControl1);
			_gbs.ListChanged += _gbs_ListChanged;
			

			/*
			List<lib.db.doc.niterdoc.iter> tmp = ctx.iter.Take(100).ToList();

			_dataTable = new DataTable("Table1");
			_dataTable.Columns.Add("id", typeof(int));
			_dataTable.Columns.Add("OTA_edit", typeof(string));
			_dataTable.Columns.Add("oggetto_uff_edit", typeof(string));			

			foreach (lib.db.doc.niterdoc.iter iter in tmp)
			{
				DataRow row = _dataTable.NewRow();
				row["id"] = iter.id;
				row["OTA_edit"] = iter.OTA;
				row["oggetto_uff_edit"] = iter.oggetto_uff_edit;
				_dataTable.Rows.Add(row);
			}
			_gbs.DataType = typeof(lib.db.doc.niterdoc.iter);
			_gbs.DataSource = _dataTable;
			_gbs.AttachToGrid(gridControl1);	
			_gbs.ListChanged += _gbs_ListChanged;
			_gbs.CurrentItemChanged += _gbs_CurrentItemChanged;
			*/
		}

		private void _gbs_CurrentItemChanged(object sender, EventArgs e)
		{
			Console.WriteLine("_gbs_CurrentItemChanged");
		}

		private void _gbs_ListChanged(object sender, ListChangedEventArgs e)
		{
			Console.WriteLine("_gbs_ListChanged");
			PropertyDescriptor pd = e.PropertyDescriptor;
			if (pd != null)
			{
				object obj = pd.GetValue(_gbs.Current);
				Console.WriteLine("Value : " + obj.ToString());
				//(_gbs.Current as DataRowView).Row[2] = "AAA";
			}
		}

	}
}
