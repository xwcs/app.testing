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
            DataTable dt = ctx.edit_classificazioni.AsEnumerable().CopyToDataTable();

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




    public static class DataSetLinqOperators
    {
        public static DataTable CopyToDataTable<T>(this IEnumerable<T> source)
        {
            return new ObjectShredder<T>().Shred(source, null, null);
        }

        public static DataTable CopyToDataTable<T>(this IEnumerable<T> source,
                                                    DataTable table, LoadOption? options)
        {
            return new ObjectShredder<T>().Shred(source, table, options);
        }


        public static void SetItemFromRow<T>(this T item, DataRow row)
    where T : class
        {
            // go through each column
            foreach (DataColumn c in row.Table.Columns)
            {
                // find the property for the column
                PropertyInfo p = item.GetType().GetProperty(c.ColumnName);

                // if exists, set the value
                if (p != null && row[c] != DBNull.Value)
                {
                    p.SetValue(item, row[c], null);
                }
            }
        }

    }

    public class ObjectShredder<T>
    {
        private FieldInfo[] _fi;
        private PropertyInfo[] _pi;
        private Dictionary<string, int> _ordinalMap;
        private Type _type;

        public ObjectShredder()
        {
            _type = typeof(T);
            _fi = _type.GetFields();
            _pi = _type.GetProperties();
            _ordinalMap = new Dictionary<string, int>();
        }

        public DataTable Shred(IEnumerable<T> source, DataTable table, LoadOption? options)
        {
            if (typeof(T).IsPrimitive)
            {
                return ShredPrimitive(source, table, options);
            }


            if (table == null)
            {
                table = new DataTable(typeof(T).Name);
            }

            // now see if need to extend datatable base on the type T + build ordinal map
            table = ExtendTable(table, typeof(T));

            table.BeginLoadData();
            using (IEnumerator<T> e = source.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    if (options != null)
                    {
                        table.LoadDataRow(ShredObject(table, e.Current), (LoadOption)options);
                    }
                    else
                    {
                        table.LoadDataRow(ShredObject(table, e.Current), true);
                    }
                }
            }
            table.EndLoadData();
            return table;
        }

        public DataTable ShredPrimitive(IEnumerable<T> source, DataTable table, LoadOption? options)
        {
            if (table == null)
            {
                table = new DataTable(typeof(T).Name);
            }

            if (!table.Columns.Contains("Value"))
            {
                table.Columns.Add("Value", typeof(T));
            }

            table.BeginLoadData();
            using (IEnumerator<T> e = source.GetEnumerator())
            {
                Object[] values = new object[table.Columns.Count];
                while (e.MoveNext())
                {
                    values[table.Columns["Value"].Ordinal] = e.Current;

                    if (options != null)
                    {
                        table.LoadDataRow(values, (LoadOption)options);
                    }
                    else
                    {
                        table.LoadDataRow(values, true);
                    }
                }
            }
            table.EndLoadData();
            return table;
        }

        public DataTable ExtendTable(DataTable table, Type type)
        {
            // value is type derived from T, may need to extend table.
            foreach (FieldInfo f in type.GetFields())
            {
                if (!_ordinalMap.ContainsKey(f.Name))
                {
                    DataColumn dc = table.Columns.Contains(f.Name) ? table.Columns[f.Name]
                        : table.Columns.Add(f.Name, f.FieldType);
                    _ordinalMap.Add(f.Name, dc.Ordinal);
                }
            }
            foreach (PropertyInfo p in type.GetProperties())
            {
                if (!_ordinalMap.ContainsKey(p.Name))
                {
                    DataColumn dc = table.Columns.Contains(p.Name) ? table.Columns[p.Name]
                        : table.Columns.Add(p.Name, ((p.PropertyType.IsGenericType) ? p.PropertyType.GetGenericArguments()[0] : p.PropertyType));
                    _ordinalMap.Add(p.Name, dc.Ordinal);
                }
            }
            return table;
        }

        public object[] ShredObject(DataTable table, T instance)
        {

            FieldInfo[] fi = _fi;
            PropertyInfo[] pi = _pi;

            if (instance.GetType() != typeof(T))
            {
                ExtendTable(table, instance.GetType());
                fi = instance.GetType().GetFields();
                pi = instance.GetType().GetProperties();
            }

            Object[] values = new object[table.Columns.Count];
            foreach (FieldInfo f in fi)
            {
                values[_ordinalMap[f.Name]] = f.GetValue(instance);
            }

            foreach (PropertyInfo p in pi)
            {
                values[_ordinalMap[p.Name]] = p.GetValue(instance, null);
            }
            return values;
        }
    }
}