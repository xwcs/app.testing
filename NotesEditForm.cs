using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace app.testing
{
    public partial class NotesEditForm : DevExpress.XtraGrid.Views.Grid.EditFormUserControl
    {
        public NotesEditForm()
        {
            InitializeComponent();


            SetBoundFieldName(richTextBox1, "xwnote");
            SetBoundPropertyName(richTextBox1, "Text");
            SetBoundFieldName(textEdit1, "nnota");
            SetBoundPropertyName(textEdit1, "EditValue");
        }
    }
}
