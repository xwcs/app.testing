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
        TcpThr t = null;
        AsyncService srv = null;

        global::InDesign._Application app;

        public InDesign()
        {
            InitializeComponent();

            // invocation target for events
            SEventProxy.InvokeDelegate = this;
        }

        protected override void Dispose(bool disposing)
        {
            //t.stop();
            if (disposing && (components != null))
            {
                
                components.Dispose();
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
            Type type = Type.GetTypeFromProgID("InDesign.Application.CS6", true);
            app = (global::InDesign._Application)System.Activator.CreateInstance(type, true);

            /*System.Diagnostics.Process.Start("C:\\Program Files (x86)\\Adobe\\Adobe InDesign CS6\\InDesign.exe");
            InDesign.Application app = new InDesign.Application();*/
            //app.ScriptPreferences.UserInteractionLevel = InDesign.idUserInteractionLevels.idNeverInteract;

            //handlerDelegate dlg = handler;


            

            var doc = app.Documents.Add();

            for (var i = 0; i < 5; i++)
                //doc.Pages.Add(idLocationOptions.idAtBeginning);
                doc.Pages.Add();

            /*
            t = new TcpThr();
            //t.app = app;

            t.start();
            */

            srv = new AsyncService(13000);
            srv.OnMessage += Srv_OnMessage;
            srv.Run();
        }

        private void Srv_OnMessage(object sender, EventArgs e)
        {
            //object menu = app.MenuActions[4];

            //js.evt.propagationStopped = true;
            srv.Evt.defaultPrevented = true;


            /*
            try
            {
                // take data
                object evt = app.MenuActions.ItemByID(srv.Evt.targetID);

                //object aaa = app.ScriptPreferences.Events.ItemByID((int)srv.Evt.evt.id);
            }
            catch (Exception)
            {

            }

            */
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //InDesign.MenuAction salva = (InDesign.MenuAction)app.MenuActions[4];
            //salva.AddEventListener("beforeInvoke", "myEventHandler");

            //app.DoScript(File.ReadAllText("id.js"), InDesign.idScriptLanguage.idJavascript, new object[] { });
            

            this.app.DoScript(@"#target 'indesign';
                #targetengine 'session_CsBridge';
                CsBridge.open({url:arguments[0]});
            ",
            global::InDesign.idScriptLanguage.idJavascript,
            new object[] { "10.17.61.98:13000" });

            
            this.app.DoScript(@"#target 'indesign';
                #targetengine 'session_CsBridge';
                CsBridge.addEventHandler(arguments[0], 'beforeInvoke', 'handler1');
            ",
            global::InDesign.idScriptLanguage.idJavascript,
            new object[] { app.MenuActions[4] });

            object evt = app.MenuActions.ItemByID(260);

            //object aaa = app.ScriptPreferences.Events.ItemByID(1);

            /*        app.DoScript(@"#target 'indesign';
                        #targetengine 'session_CsBridge';
                        addEventHandler('beforeInvoke', arguments[0]);
                    ",
                    InDesign.idScriptLanguage.idJavascript,
                    new object[] { app.MenuActions[4]  });
              */

            // app.DoScript("alert(\"First argument: \" + arguments[0] + \"\\rSecond argument: \" + arguments[1]);", InDesign.idScriptLanguage.idJavascript, new object[] { "PIPPO", "PLUTO" });



        }
    }

    public class AsyncService
    {

        public readonly object Gate = new object();

        private xwcs.indesign.json.JsEvent _evt;
        public xwcs.indesign.json.JsEvent Evt
        {
            get
            {
                return _evt;
            }
        }

        private WeakEventSource<EventArgs> _wes_OnMessage = null;
        public event EventHandler<EventArgs> OnMessage
        {
            add
            {
                if (_wes_OnMessage == null)
                {
                    _wes_OnMessage = new WeakEventSource<EventArgs>();
                }
                _wes_OnMessage.Subscribe(value);
            }
            remove
            {
                _wes_OnMessage?.Unsubscribe(value);
            }
        }



        private IPAddress ipAddress;
        private int port;
        public AsyncService(int port) {
            this.port = port;
            string hostName = Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.GetHostEntry(hostName);
            this.ipAddress = null;
            for (int i = 0; i < ipHostInfo.AddressList.Length; ++i)
            {
                if (ipHostInfo.AddressList[i].AddressFamily ==
                  AddressFamily.InterNetwork)
                {
                    this.ipAddress = ipHostInfo.AddressList[i];
                    break;
                }
            }
            if (this.ipAddress == null)
                throw new Exception("No IPv4 address for server");
        }
        public async void Run() {
            TcpListener listener = new TcpListener(this.ipAddress, this.port);
            listener.Start();
            Console.Write("Service is now running");          
            Console.WriteLine(" on port " + this.port);
            while (true)
            {
                try
                {
                    TcpClient tcpClient = await listener.AcceptTcpClientAsync();
                    Task t = Process(tcpClient);
                    await t;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private async Task Process(TcpClient tcpClient) {
            string clientEndPoint =  tcpClient.Client.RemoteEndPoint.ToString();
            Console.WriteLine("Received connection request from "
              + clientEndPoint);
            try
            {
                NetworkStream networkStream = tcpClient.GetStream();


                // Buffer for reading data
                Byte[] bytes = new Byte[10000];
                String data = null;

                
                int i;

                // Loop to receive all the data sent by the client.
                while ((i = await networkStream.ReadAsync(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received: {0}", data);

                    // Process the data sent by the client.
                    //data = data.ToUpper();

                    _evt = (xwcs.indesign.json.JsEvent)JsonConvert.DeserializeObject<xwcs.indesign.json.Message>(data).data;
                    
                    _wes_OnMessage.Raise(this, new EventArgs());
                   

                    data = JsonConvert.SerializeObject(_evt);

                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                    // Send back a response.
                    await networkStream.WriteAsync(msg, 0, msg.Length);
                    Console.WriteLine("Sent: {0}", data);
                }




                tcpClient.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (tcpClient.Connected)
                    tcpClient.Close();
            }
        }
        
    }


    public class TcpThr {

        

        private System.Threading.Thread  t = null;

        public bool terminated { get; set; } = false;


        public void start()
        {
            

            t = new System.Threading.Thread(Main);
            t.Start();        
        }

        public void stop()
        {
            terminated = true;

            t.Join();
        }

        private void Main()
        {

            global::InDesign._Application app;
        Type type = Type.GetTypeFromProgID("InDesign.Application.CS6", true);
        app = (global::InDesign._Application)System.Activator.CreateInstance(type, true);

            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[10000];
                String data = null;

                // Enter the listening loop.
                while (!terminated)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        // Process the data sent by the client.
                        //data = data.ToUpper();

                        dynamic js = JsonConvert.DeserializeObject(data);
                        /*evt : {
                            bubbles : evt.bubbles,
                            cancelable : evt.cancelable,
                            defaultPrevented: evt.defaultPrevented,
                            eventType: evt.eventType,
                            id: evt.id,
                            index: evt.index,
                            isValid: evt.isValid,
                            propagationStopped: evt.propagationStopped,
                            timeStamp: evt.timeStamp
                        }
                        */
                        js.evt.propagationStopped = true;
                        js.evt.defaultPrevented = true;

                        object evt = app.MenuActions.ItemByID(js.evt.targetID);

                        object aaa = app.ScriptPreferences.Events.ItemByID(1);

                        data = JsonConvert.SerializeObject(js);

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", data);
                    }

                    // Shutdown and end connection
                    client.Close();


                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }


            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

    }
}
