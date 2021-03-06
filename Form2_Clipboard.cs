﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.XtraRichEdit;
using xwcs.core.db;
using xwcs.core.evt;
using System.Net;
using System.Collections.Specialized;
using System.Net.Http;
using System.IO;

using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Parameters;

namespace app.testing
{
	public partial class Form2_Clipboard : Form
	{
		private lib.db.doc.niterdoc.NiterDocEntities ctx;
		private lib.db.titles.niterdoc.NiterTitlesEntities ctx2;
		BindingSource _bs;
		//RichEditControl a;
		public Form2_Clipboard()
		{
            xwcs.core.user.SecurityContext.getInstance().setUserProvider(new lib.core.user.BackOfficeUserProvider());

            SEventProxy.InvokeDelegate = this;

            InitializeComponent();
			ctx = new lib.db.doc.niterdoc.NiterDocEntities();
			ctx.Database.Log = Console.WriteLine;

			ctx2 = new lib.db.titles.niterdoc.NiterTitlesEntities();
			ctx2.Database.Log = Console.WriteLine;



			lib.db.titles.niterdoc.v_iter tmp = new lib.db.titles.niterdoc.v_iter();

			_bs = new BindingSource();

            gridView1.OptionsEditForm.CustomEditFormLayout = new NotesEditForm();
		}

		private void simpleButton1_Click(object sender, EventArgs e)
		{			
			
			refreshGrid();
		}
		
		private void refreshGrid()
		{
            _bs.PositionChanged += _bs_PositionChanged;
            //_bs.DataSource = ctx.iter.Take(100).ToList();
            _bs.DataSource = ctx.iter_in_xwbo_note.Take(100).ToList();
            gridControl1.DataSource = _bs;
		}

        private void _bs_PositionChanged(object sender, EventArgs e)
        {
            //handle current xw notes creation
            lib.db.doc.niterdoc.iter_in_xwbo_note tt = _bs.Current as lib.db.doc.niterdoc.iter_in_xwbo_note;

            // load note
            if (!ctx.Entry(tt).Reference("xwbo_note").IsLoaded)
            {
                ctx.Entry(tt).Reference("xwbo_note").Load();
            }

            //if not existent create it
            if(ReferenceEquals(null, tt.xwbo_note))
            {
                tt.xwbo_note = new lib.db.doc.niterdoc.xwbo_note();
                ctx.xwbo_note.Add(tt.xwbo_note);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
		{
			lib.db.doc.niterdoc.iter currentIter = _bs.Current as lib.db.doc.niterdoc.iter;
			var originalEntity = ctx.iter.Include("xwbo_iter").AsNoTracking().FirstOrDefault(o => o.id == currentIter.id);

            
            ctx.xwbo_iter.Add(originalEntity.xwbo_iter);
            ctx.iter.Add(originalEntity);
            originalEntity.ncopia += 1;
            originalEntity.xwbo_iter.ncopia += 1;
			//originalEntity.ndoc_xwbo_iter = xwbonew.ndoc;
			//ctx.iter.Add(originalEntity);
			ctx.SaveChanges();
		}


        public string Upload(string url, List<KeyValuePair<string, string>> requestParameters, MemoryStream file)
        {

            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                Proxy = new WebProxy(string.Format("{0}:{1}", "127.0.0.1", 8888), false),
                PreAuthenticate = false,
                UseDefaultCredentials = false,
            };

            var client = new HttpClient(httpClientHandler);
            client.DefaultRequestHeaders.Add("Accept-Language", " en-US");
            client.DefaultRequestHeaders.Add("Accept", " text/html, application/xhtml+xml, */*");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)");

            var content = new MultipartFormDataContent();

            content.Add(new StreamContent(file));
            var addMe = new FormUrlEncodedContent(requestParameters);

            content.Add(addMe);
            var result = client.PostAsync(url, content);
            return result.Result.ToString();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //lib.db.doc.niterdoc.iter_in_xwbo_note tt = _bs.Current as lib.db.doc.niterdoc.iter_in_xwbo_note;

            //ctx.SaveChanges();





            
            WebClient client = new WebClient();
            NameValueCollection parameters = new NameValueCollection();
            //List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            //parameters.Add(new KeyValuePair<string, string>("MAX_FILE_SIZE", "100000000"));
            parameters.Add("MAX_FILE_SIZE", "100000000");
            //parameters.Add("db", "niter");
            client.Proxy = new WebProxy("127.0.0.1", 8888);
            client.QueryString = parameters;



            try
            {
                Uri uri = new Uri("http://localhost:4854/test.html");
/*
                MemoryStream inMemoryCopy = new MemoryStream();
                using (FileStream fs = File.OpenRead(@"c:\tmp\niter.sql"))
                {
                    fs.CopyTo(inMemoryCopy);
                }

*/
                //string response = Upload("http://localhost.fiddler:4854/attach/put?db=niter", parameters, inMemoryCopy);
                var responseBytes = client.UploadFile("http://localhost.fiddler:4854/attach/put?db=niter", "POST", @"c:\tmp\wtf.xml");
                string response = Encoding.ASCII.GetString(responseBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

		private void simpleButton4_Click(object sender, EventArgs e)
		{
			/*
			ctx.tipologie.Load();
			ctx.tipologie_view.Load();
			foreach (var tmp in ctx.tipologie)
			{
				lib.db.doc.niterdoc.view.tipologie_view aaa = new lib.db.doc.niterdoc.view.tipologie_view(tmp);
				ctx.tipologie_view.Add(aaa);
			}
			*/
			/*			
			var data = ctx.tipologie
								//.Where(t => t.id == 1)
								.Select(rhs => new lib.db.doc.niterdoc.view.tipologie_view() {
									id = rhs.id,
									tipo = rhs.tipo,
									disp_abbrev = rhs.disp_abbrev,
									disposizione = rhs.disposizione,
									disposizione_breve = rhs.disposizione_breve,
									organo = rhs.organo,
									organo_breve = rhs.organo_breve,
									sezione = rhs.sezione,
									localita = rhs.localita,
									valenza = rhs.valenza,
									disabilitato = rhs.disabilitato
								})
								.ToList();
			*/		


			xwcs.core.ui.print.PrintDesignerForm form = new xwcs.core.ui.print.PrintDesignerForm();			
			form.Show();


			/*
			var data = ctx.tipologie
								//.Where(t => t.id == 1)
								.Select(t => new lib.db.doc.niterdoc.view.tipologie_view(t))
								.ToList();
			var data1 = (from t in ctx.tipologie
						where t.id == 1
						select new lib.db.doc.niterdoc.view.tipologie_view(t)).ToList();
					*/

		}

		private void simpleButton5_Click(object sender, EventArgs e)
		{
			//TODO : dat do try
			XtraReport report = XtraReport.FromFile("c:\\7\\XtraReport2.repx", true);
			if (report != null)
			{
				ReportPrintTool printTool = new ReportPrintTool(report);

				report.Tag = "FromApp";


				SEventProxy.BlockModelEvents();
				report.DataSource = ctx2.v_iter
									 .Where(s => s.id < 10)
									 //.Include("tipologie")
									 //.Include("editori")
									 .AsNoTracking()
									 .ToList();

				SEventProxy.AllowModelEvents();
				printTool.ShowPreviewDialog();
			}
		}
	}
}
