using DevExpress.Utils.DragDrop;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using lib.db.doc.niterdoc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xwcs.core.db.binding;
using xwcs.core.evt;

namespace app.testing
{
	public partial class Form_DragDrop : Form
	{
		private lib.db.doc.niterdoc.NiterDocEntities ctx;
		private GridBindingSource _bs1;
		private GridBindingSource _bs2;
		GridHitInfo downHitInfo = null;

		private PictureBox c;
		private Bitmap bmp;
		private Point pointDown;
		private int iTmpY;

//		DragManager mm;

		public Form_DragDrop()
		{
			InitializeComponent();
			xwcs.core.user.SecurityContext.getInstance().setUserProvider(new lib.core.user.BackOfficeUserProvider());
			SEventProxy.InvokeDelegate = this;

			ctx = new lib.db.doc.niterdoc.NiterDocEntities();
			_bs1 = new GridBindingSource();
			_bs2 = new GridBindingSource();
			gridControl1.DataSource = _bs1;
			gridControl2.DataSource = _bs2;

			c = new PictureBox();
			this.Controls.Add(c);
			c.BringToFront();
			c.Hide();
		}

		private void simpleButton1_Click(object sender, EventArgs e)
		{
			_bs1.DataSource = ctx.iter.Take(100).ToList();
			_bs2.DataSource = ctx.iter.Take(0).ToList();
		}

		private void gridView1_MouseDown(object sender, MouseEventArgs e)
		{
			GridView view = sender as GridView;
			downHitInfo = null;
			GridHitInfo hitInfo = view.CalcHitInfo(new Point(e.X, e.Y));
			if (Control.ModifierKeys != Keys.None) return;

			//if (e.Button == MouseButtons.Left && hitInfo.RowHandle >= 0)
			if (e.Button == MouseButtons.Left && hitInfo.InRow && hitInfo.HitTest != GridHitTest.RowIndicator)
			{ 
				downHitInfo = hitInfo;
				pointDown = gridControl1.PointToClient(MousePosition);
				Console.WriteLine(pointDown.X);
				Console.WriteLine(pointDown.Y);
			}	
		}

		private void gridView1_MouseMove(object sender, MouseEventArgs e)
		{
			GridView view = sender as GridView;
			if (e.Button == MouseButtons.Left && downHitInfo != null)
			{
				Size dragSize = SystemInformation.DragSize;
				Rectangle dragRect = new Rectangle(new Point(downHitInfo.HitPoint.X - dragSize.Width / 2,
					downHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);

				if (!dragRect.Contains(new Point(e.X, e.Y)))
				{
					//Multi select
					List<iter> selectedRows = new List<iter>();
					int[] selection = view.GetSelectedRows();
					for (int i = 0; i < selection.Length; i++)
					{
						iter row = view.GetRow(selection[i]) as iter;
						selectedRows.Add(row);
					}

					view.GridControl.DoDragDrop(selectedRows, DragDropEffects.Move);
					downHitInfo = null;
					DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = true;
					//Cursor tmpC = new Cursor("c:\\Temp\\400x700.jpg");
					//view.GridControl.Cursor = tmpC;


					//Single select
					/*					
					iter row = view.GetRow(downHitInfo.RowHandle) as iter;
					if (row != null)
					{					
						view.GridControl.DoDragDrop(row, DragDropEffects.Move);
						downHitInfo = null;
						DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = true;
					}
					*/
				}
			}
		}

		private void CopyRegionIntoImage(Bitmap srcBitmap, Rectangle srcRegion, ref Bitmap destBitmap, Rectangle destRegion)
		{
			using (Graphics grD = Graphics.FromImage(destBitmap))
			{
				grD.DrawImage(srcBitmap, destRegion, srcRegion, GraphicsUnit.Pixel);
			}
		}

		Rectangle rect;
		private void gridControl2_DragOver(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(List<iter>)))
			{
				//e.Effect = DragDropEffects.Move;
				if (c.Visible == false)
				{
					rect.Width = gridView1.GridControl.ClientRectangle.Width;
					rect.Height = gridView1.GridControl.ClientRectangle.Height;
					bmp = new Bitmap(rect.Width, rect.Height);

					
					gridView1.GridControl.DrawToBitmap(bmp, new Rectangle(0, 0, rect.Width, rect.Height));
					Bitmap bmp2 = new Bitmap(rect.Width, 20);
					CopyRegionIntoImage(bmp, new Rectangle(0, /*pointDown.Y - 15*/iTmpY, rect.Width, 20), ref bmp2, new Rectangle(0, 0, rect.Width, 20));

					c.Image = bmp2;
					c.Width = rect.Width;
					c.Height = 20;
				}

				c.Show();
				e.Effect = DragDropEffects.Move;
				Point p = PointToClient(MousePosition);
				p.X += 2;
				p.Y += 2;
				c.Location = p;				
			}
			else
			{
				//e.Effect = DragDropEffects.None;
				e.Effect = DragDropEffects.None;
				c.Hide();
			}
		}

		private void gridControl2_DragDrop(object sender, DragEventArgs e)
		{
			//Multi select
			GridControl grid = sender as GridControl;
			List<iter> rows = e.Data.GetData(typeof(List<iter>)) as List<iter>;
			foreach(iter obj in rows)
			{
				_bs2.Add(obj);
				_bs1.Remove(obj);			
			}
			c.Hide();


			//Single select
			/*
			GridControl grid = sender as GridControl;
			iter row = e.Data.GetData(typeof(iter)) as iter;
			_bs2.Add(row);
			_bs1.Remove(row);
			*/
		}

		private void simpleButton2_Click(object sender, EventArgs e)
		{
			gridView1.SaveLayoutToXml("c:\\Temp\\tmpL.xml");
		}

		private void simpleButton3_Click(object sender, EventArgs e)
		{
			gridView1.RestoreLayoutFromXml("c:\\Temp\\tmpL.xml");
		}

		private void gridView2_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
		{

		}

		private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
		{
			
			if (e.RowHandle == (sender as GridView).FocusedRowHandle)
			{
				iTmpY = e.Bounds.Y;
				//rect = e.Bounds;
				//rect.Width = 300;
				//rect.Height = 300;
			}
		}
	}
}
