namespace app.testing
{
	partial class Form_DragDrop
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.gridControl1 = new DevExpress.XtraGrid.GridControl();
			this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.gridControl2 = new DevExpress.XtraGrid.GridControl();
			this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
			this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
			this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
			this.behaviorManager1 = new DevExpress.Utils.Behaviors.BehaviorManager(this.components);
			((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).BeginInit();
			this.SuspendLayout();
			// 
			// gridControl1
			// 
			this.gridControl1.AllowDrop = true;
			this.gridControl1.Location = new System.Drawing.Point(13, 13);
			this.gridControl1.MainView = this.gridView1;
			this.gridControl1.Name = "gridControl1";
			this.gridControl1.Size = new System.Drawing.Size(1178, 205);
			this.gridControl1.TabIndex = 0;
			this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
			// 
			// gridView1
			// 
			this.gridView1.GridControl = this.gridControl1;
			this.gridView1.Name = "gridView1";
			this.gridView1.OptionsBehavior.Editable = false;
			this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridView1.OptionsSelection.MultiSelect = true;
			this.gridView1.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridView1_CustomDrawCell);
			this.gridView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridView1_MouseDown);
			this.gridView1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gridView1_MouseMove);
			// 
			// gridControl2
			// 
			this.gridControl2.AllowDrop = true;
			this.gridControl2.Location = new System.Drawing.Point(13, 224);
			this.gridControl2.MainView = this.gridView2;
			this.gridControl2.Name = "gridControl2";
			this.gridControl2.Size = new System.Drawing.Size(1178, 205);
			this.gridControl2.TabIndex = 1;
			this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
			this.gridControl2.DragDrop += new System.Windows.Forms.DragEventHandler(this.gridControl2_DragDrop);
			this.gridControl2.DragOver += new System.Windows.Forms.DragEventHandler(this.gridControl2_DragOver);
			// 
			// gridView2
			// 
			this.gridView2.GridControl = this.gridControl2;
			this.gridView2.Name = "gridView2";
			this.gridView2.OptionsBehavior.Editable = false;
			this.gridView2.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridView2_CustomDrawCell);
			// 
			// simpleButton1
			// 
			this.simpleButton1.Location = new System.Drawing.Point(13, 446);
			this.simpleButton1.Name = "simpleButton1";
			this.simpleButton1.Size = new System.Drawing.Size(75, 23);
			this.simpleButton1.TabIndex = 2;
			this.simpleButton1.Text = "Open";
			this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
			// 
			// simpleButton2
			// 
			this.simpleButton2.Location = new System.Drawing.Point(113, 446);
			this.simpleButton2.Name = "simpleButton2";
			this.simpleButton2.Size = new System.Drawing.Size(75, 23);
			this.simpleButton2.TabIndex = 3;
			this.simpleButton2.Text = "simpleButton2";
			this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
			// 
			// simpleButton3
			// 
			this.simpleButton3.Location = new System.Drawing.Point(216, 446);
			this.simpleButton3.Name = "simpleButton3";
			this.simpleButton3.Size = new System.Drawing.Size(75, 23);
			this.simpleButton3.TabIndex = 4;
			this.simpleButton3.Text = "simpleButton3";
			this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
			// 
			// Form_DragDrop
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1203, 481);
			this.Controls.Add(this.simpleButton3);
			this.Controls.Add(this.simpleButton2);
			this.Controls.Add(this.simpleButton1);
			this.Controls.Add(this.gridControl2);
			this.Controls.Add(this.gridControl1);
			this.Name = "Form_DragDrop";
			this.Text = "Form_DragDrop";
			((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraGrid.GridControl gridControl1;
		private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
		private DevExpress.XtraGrid.GridControl gridControl2;
		private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
		private DevExpress.XtraEditors.SimpleButton simpleButton1;
		private DevExpress.XtraEditors.SimpleButton simpleButton2;
		private DevExpress.XtraEditors.SimpleButton simpleButton3;
		private DevExpress.Utils.Behaviors.BehaviorManager behaviorManager1;
	}
}