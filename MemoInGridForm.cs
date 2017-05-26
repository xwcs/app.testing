using DevExpress.XtraEditors.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app.testing
{
	public partial class MemoInGridForm : Form
	{
		public MemoInGridForm()
		{
			InitializeComponent();
		}

		public class MyData
		{
			public int id { get; set; }
			public string text { get; set; }
		}

		private void simpleButton1_Click(object sender, EventArgs e)
		{
			List<MyData> list = new List<MyData>();
			list.Add(new MyData() { id = 1, text = "qwertyuiopasdfghjklzxcvbnm" });
			list.Add(new MyData() { id = 2, text = "aaaaaaaaa\r\nbbbbbbb\r\ncccccc" });

			gridControl1.DataSource = list;

			RepositoryItemMemoEdit memoEdit = new RepositoryItemMemoEdit();

			memoEdit.AutoHeight = true;
			memoEdit.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
			memoEdit.WordWrap = true;
			gridView1.GridControl.RepositoryItems.Add(memoEdit);
			gridView1.Columns["text"].ColumnEdit = memoEdit;
		}
	}
}
