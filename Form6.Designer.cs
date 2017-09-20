namespace app.testing
{
	partial class Form6
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
			this.buttonEdit1 = new DevExpress.XtraEditors.ButtonEdit();
			this.behaviorManager1 = new DevExpress.Utils.Behaviors.BehaviorManager(this.components);
			this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.buttonEdit1.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonEdit1
			// 
			this.behaviorManager1.SetBehaviors(this.buttonEdit1, new DevExpress.Utils.Behaviors.Behavior[] {
            ((DevExpress.Utils.Behaviors.Behavior)(DevExpress.Utils.Behaviors.Common.OpenFileBehavior.Create(typeof(DevExpress.XtraEditors.Behaviors.OpenFileBehaviorSourceForButtonEdit), true, DevExpress.Utils.Behaviors.Common.FileIconSize.Small, null, null, DevExpress.Utils.Behaviors.Common.CompletionMode.FilesAndDirectories, "c:\\Windows")))});
			this.buttonEdit1.Location = new System.Drawing.Point(13, 23);
			this.buttonEdit1.Name = "buttonEdit1";
			this.buttonEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.buttonEdit1.Size = new System.Drawing.Size(259, 20);
			this.buttonEdit1.TabIndex = 0;
			// 
			// simpleButton1
			// 
			this.simpleButton1.Location = new System.Drawing.Point(13, 76);
			this.simpleButton1.Name = "simpleButton1";
			this.simpleButton1.Size = new System.Drawing.Size(75, 23);
			this.simpleButton1.TabIndex = 1;
			this.simpleButton1.Text = "simpleButton1";
			this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// Form6
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.simpleButton1);
			this.Controls.Add(this.buttonEdit1);
			this.Name = "Form6";
			this.Text = "Form6";
			((System.ComponentModel.ISupportInitialize)(this.buttonEdit1.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.ButtonEdit buttonEdit1;
		private DevExpress.Utils.Behaviors.BehaviorManager behaviorManager1;
		private DevExpress.XtraEditors.SimpleButton simpleButton1;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
	}
}