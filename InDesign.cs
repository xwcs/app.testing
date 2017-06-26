using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using xwcs.core.evt;
using xwcs.indesign;

// Cancellare il file C:\ProgramData\Adobe\InDesign\Version 8.0\it_IT\Scripting Support\8.0\Resources for Visual Basic.tlb
// Avviare InDesign come amministratore ricrea il file Resources for Visual Basic.tlb
// Aggiungere un riferimento COM a "Adobe Indesign CS6 Type Library"
//
// https://forums.adobe.com/thread/834780
// http://stackoverflow.com/questions/2483659/interop-type-cannot-be-embedded
// https://forums.adobe.com/thread/759540
// http://indesignsecrets.com/javascript-for-the-absolute-beginner.php#
// http://www.indiscripts.com/post/2010/02/how-to-create-your-own-indesign-menus
// http://www.indiscripts.com/post/2011/01/how-to-implement-a-basic-action-listener-in-indesign
// http://stackoverflow.com/questions/24433479/where-is-the-event-object-for-external-file-event-handlers
// https://msdn.microsoft.com/en-us/library/66ahbe6y%28v=vs.85%29.aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-1
// http://www.indiscripts.com/post/2013/05/indesign-scripting-forum-roundup-4#hd4sb3
// http://stackoverflow.com/questions/36845211/is-there-an-indesign-event-handler-for-when-new-pages-are-created

namespace app.testing
{
   



    public partial class InDesign : Form
    {
        
        public InDesign()
        {
            InitializeComponent();

            // invocation target for events
            SEventProxy.InvokeDelegate = this;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing )
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowDefaultWaitForm("Atendere prego", "Chiusura ...");
                // kill loggers
                xwcs.core.manager.SLogManager.getInstance().Dispose();
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseDefaultWaitForm();
                if(components != null)
                {
                    components.Dispose();
                }                
            }
            base.Dispose(disposing);
        }



        public void handler(object evt)
        {
            MessageBox.Show("Here!");
        }

        delegate void handlerDelegate(object evt);

        private void button1_Click(object sender, EventArgs e)
        {

            // got events
            SIndesign.getInstance().AfterInit += InDesign_AfterInit;

            // start all
            SIndesign.Start();


            /*
            var doc = SIndesign.App.Documents.Add();

            for (var i = 0; i < 5; i++)
                doc.Pages.Add();
                */
        }

        private void InDesign_AfterInit(object sender, EventArgs e)
        {
            JsEventBindable jeb = SIndesign.GetJsBindable(SIndesign.App.MenuActions[4]);

            jeb.AddEventHandler("beforeInvoke", indesign_beforeSave);

            // openFile
            SIndesign.ExecScript("FileManager.open(arguments[0]);", "C:\\tmp\\Egafback\\Rtf\\432A3.rtf");
        }

        private void indesign_beforeSave(object sender, OnMessageEventArgs e)
        {
            Console.WriteLine("JsEvent");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            object evt = SIndesign.App.MenuActions.ItemByID(260);
        }
    }
    
}
