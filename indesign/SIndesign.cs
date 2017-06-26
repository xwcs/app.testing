using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using InDesign;
using xwcs.core.evt;
using System.IO;
using xwcs.core;

namespace xwcs.indesign
{

    [xwcs.core.cfg.attr.Config("MainAppConfig")]
    public class SIndesign : core.cfg.Configurable
    {
        private static xwcs.core.manager.ILogger _logger = core.manager.SLogManager.getInstance().getClassLogger(typeof(SIndesign));

        private static readonly object _lock = new object();

        // map of JsEventBindables
        private Dictionary<int, JsEventBindable> _bindables = new Dictionary<int, JsEventBindable>();

        // indesign
        private InDesign._Application _app = null;

        //socket server
        private AsyncSocketService _server = null;

        // some later execution
        protected CmdQueue _commandsQueue = new CmdQueue();

        #region singleton
        private static SIndesign instance;

        //singleton need private ctor
        private SIndesign()
        {
            
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static SIndesign getInstance()
        {
            if (instance == null)
            {
                instance = new SIndesign();
            }
            return instance;
        }

        public static void Start()
        {
            instance.ResetApp();
        }

        public static _Application App
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new SIndesign();
                    }
                }               

                if (ReferenceEquals(null, instance._app))
                {
                    instance.ResetApp();
                }

                // check if app is on
                try
                {
                    // if indesign is down this go in exception
                    string tmp = instance._app.FullName;
                }
                catch (Exception e)
                {
                    if (e is System.Runtime.InteropServices.COMException && e.HResult == unchecked((int)0x800706ba))
                    {
                        instance.ResetApp();
                    }
                    else
                    {
                        // not managed exception type , forward it
                        throw;
                    }
                }

                return instance._app;
            }
        }
        #endregion

        #region events
        private WeakEventSource<EventArgs> _wes_AfterInit = null;
        public event EventHandler<EventArgs> AfterInit
        {
            add
            {
                if (_wes_AfterInit == null)
                {
                    _wes_AfterInit = new WeakEventSource<EventArgs>();
                }
                _wes_AfterInit.Subscribe(value);
            }
            remove
            {
                _wes_AfterInit?.Unsubscribe(value);
            }
        }

        private WeakEventSource<OnMessageEventArgs> _wes_OnJsAction = null;
        public event EventHandler<OnMessageEventArgs> OnJsAction
        {
            add
            {
                if (_wes_OnJsAction == null)
                {
                    _wes_OnJsAction = new WeakEventSource<OnMessageEventArgs>();
                }
                _wes_OnJsAction.Subscribe(value);
            }
            remove
            {
                _wes_OnJsAction?.Unsubscribe(value);
            }
        }
        #endregion


        public static JsEventBindable GetJsBindable(object target)
        {
            JsEventBindable jeb = new JsEventBindable(target);
            // check if we have it
            JsEventBindable trg;
            if (instance._bindables.TryGetValue(jeb.TargetId,  out trg))
            {
                // found
                return trg;
            }else
            {
                instance._bindables[jeb.TargetId] = jeb;
                return jeb;
            }
        }

        private object ExecScriptInternal(_Application a, string script, params object[] pms)
        {
            try
            {
                return a.DoScript(
                    string.Format(@"
                        #target 'indesign';
                        #targetengine 'session_CsBridge';
                        {0}", script),
                    global::InDesign.idScriptLanguage.idJavascript,
                    pms
                );
            }catch(Exception e)
            {
                throw new ApplicationException(string.Format("Javascript error : {0}", e.Message));    
            }            
        }

        public static object ExecScript(string script, params object[] pms)
        {
            return instance.ExecScriptInternal(App, script, pms);
        }

        /// <summary>
        /// Reset all data and reinit InDesign
        /// </summary>
        private void ResetApp()
        {
            // reset server
            if (ReferenceEquals(_server, null))
            {
                // make server
                int port = int.Parse(getCfgParam("Indesign/CshpServerPort", "0"));
                _server = new AsyncSocketService(port);

                // register to message
                _server.OnMessage += _server_OnMessage;

                _server.RunAsync();
            }

            // reset chached data
            _bindables.Clear();


            // recreate
            Type type = Type.GetTypeFromProgID("InDesign.Application.CS6", true);
            _app = (_Application) Activator.CreateInstance(type, true);

            //_app.DoScript("alert('here');", global::InDesign.idScriptLanguage.idJavascript, new object[] { });

            // load script
            /*
            string ver = (string)(_app.DoScript(@"
                                #target 'indesign';
                                #targetengine 'session_CsBridge';
                                try {
                                    alert('Here' + CsBridge.version()); 
                                    ver = CsBridge.version();
                                }catch(e){
                                    alert( e.Message );
                                }",
                                global::InDesign.idScriptLanguage.idJavascript,
                                new object[] { }) ?? "");
            */
            string ver = (string)(ExecScriptInternal(_app, @"
                                try {
                                    ver = CsBridge.version();
                                }catch(e){
                                    error = e.message;
                                }",
                                new object[] { }) ?? "");
            if (ver != "0.0.2")
            {
                // load script
                string scr = File.ReadAllText("C:\\Projekty\\Egaf\\app.main\\app.testing\\JS\\id.js");
                _app.DoScript(
                    scr,
                    global::InDesign.idScriptLanguage.idJavascript,
                    new object[] { }
                );
            }


            // connect to c# server
            if ((bool)(ExecScriptInternal(_app, "CsBridge.open({url:arguments[0]});", new object[] { "10.17.61.98:13000" }) ?? false))
            {
                _PingCounter = 100;
                _commandsQueue.ExecuteLater(DoPing);
            }else
            {
                throw new ApplicationException("Connection from Indesign failed!");
            }
        }

        private int _PingCounter = 100;
        private void DoPing()
        {
            _logger.Debug("Ping: {0}", _PingCounter);

            if ((bool)(ExecScriptInternal(_app, "CsBridge.ping();", new object[] {}) ?? false))
            {
                _commandsQueue.ExecuteLater(RaiseAfterInit);
            }
            else
            {
                if(_PingCounter-- > 0)
                {
                    _commandsQueue.ExecuteLater(DoPing);
                }else
                {
                    throw new ApplicationException("Connection from Indesign failed!");
                }                
            }
        }

        private void RaiseAfterInit()
        {
            _wes_AfterInit?.Raise(this, new EventArgs());
        }

        private void _server_OnMessage(object sender, OnMessageEventArgs e)
        {
            if(e.Message.data is json.JsAction)
            {
                // do action
                _wes_OnJsAction.Raise(this, e);
            }else if(e.Message.data is json.JsEvent)
            {
                // find Target
                JsEventBindable trg;
                if(_bindables.TryGetValue((e.Message.data as json.JsEvent).currentTargetID, out trg))
                {
                    trg.RaiseEvent((e.Message.data as json.JsEvent).eventKind, e);
                }else
                {
                    throw new ApplicationException(string.Format("Missing event target with id = {0}", (e.Message.data as json.JsEvent).currentTargetID));
                }
            }
        }
    }
}
