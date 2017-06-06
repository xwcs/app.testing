using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Reflection;
using xwcs.core.manager;
using xwcs.core;

namespace app.testing
{
    public partial class TreeViewForm : DevExpress.XtraEditors.XtraForm
    {
        BindingSource bs;
        lib.db.doc.niterdoc.NiterDocEntities ctx;

        bool _starting = false;

        public TreeViewForm()
        {
            xwcs.core.user.SecurityContext.getInstance().setUserProvider(new lib.core.user.BackOfficeUserProvider());

            InitializeComponent();

            //DataRow myRow;


            ctx = new lib.db.doc.niterdoc.NiterDocEntities();
            ctx.Database.Log = Console.WriteLine;
            DataTable dt = ctx.edit_classificazioni.AsEnumerable().ToDataTable();

            bs = new BindingSource();

            bs.CurrentItemChanged += Bs_CurrentItemChanged;

            _starting = true;

            treeList1.ParentFieldName = "parent_id";
            treeList1.OptionsBehavior.AutoPopulateColumns = true;
            treeList1.DataSource = bs;
            bs.DataSource = dt;
            
            treeList1.PopulateColumns();

            _starting = false;
        }

        private void Bs_CurrentItemChanged(object sender, EventArgs e)
        {
            if (_starting) return;
            lib.db.doc.niterdoc.edit_classificazioni cl = ctx.edit_classificazioni.Local.Where(c => c.id == (bs.Current as DataRowView).Row.Field<int>("id")).FirstOrDefault();

            cl.SetItemFromRow((bs.Current as DataRowView).Row);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            ctx.SaveChanges();
        }
    }



    /*
     * static void Main(string[] args)
        {
            // create sequence 
            Item[] items = new Item[] { new Book{Id = 1, Price = 13.50, Genre = "Comedy", Author = "Jim Bob"}, 
                                        new Book{Id = 2, Price = 8.50, Genre = "Drama", Author = "John Fox"},  
                                        new Movie{Id = 1, Price = 22.99, Genre = "Comedy", Director = "Phil Funk"},
                                        new Movie{Id = 1, Price = 13.40, Genre = "Action", Director = "Eddie Jones"}};

                        
            var query1 = from i in items
                         where i.Price > 9.99
                         orderby i.Price
                         select i;

            // load into new DataTable
            DataTable table1 = query1.CopyToDataTable();

            // load into existing DataTable - schemas match            
            DataTable table2 = new DataTable();
            table2.Columns.Add("Price", typeof(int));
            table2.Columns.Add("Genre", typeof(string));

            var query2 = from i in items
                         where i.Price > 9.99
                         orderby i.Price
                         select new {i.Price, i.Genre};

            query2.CopyToDataTable(table2, LoadOption.PreserveChanges);


            // load into existing DataTable - expand schema + autogenerate new Id.
            DataTable table3 = new DataTable();
            DataColumn dc = table3.Columns.Add("NewId", typeof(int));
            dc.AutoIncrement = true;
            table3.Columns.Add("ExtraColumn", typeof(string));

            var query3 = from i in items
                         where i.Price > 9.99
                         orderby i.Price
                         select new { i.Price, i.Genre };

            query3.CopyToDataTable(table3, LoadOption.PreserveChanges);

            // load sequence of scalars.

            var query4 = from i in items
                         where i.Price > 9.99
                         orderby i.Price
                         select i.Price;

            var DataTable4 = query4.CopyToDataTable();
        }

        public class Item
        {
            public int Id { get; set; }
            public double Price { get; set; }
            public string Genre { get; set; }   
        }

        public class Book : Item
        {
            public string Author { get; set; }
        }

        public class Movie : Item
        {
            public string Director { get; set; }
        }
        
    }
     * 
     * 
     */




    
}