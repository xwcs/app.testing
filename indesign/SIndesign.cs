using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
//using InDesign;

namespace xwcs.indesign
{

    [xwcs.core.cfg.attr.Config("MainAppConfig")]
    public class SIndesign : core.cfg.Configurable
    {
        private static readonly object _lock = new object();


        // indesign
        //private InDesign._Application _app = null;

        //socket server
        private AsyncSocketService _server = null;

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

        /// <summary>
        /// Reset all data and reinit InDesign
        /// </summary>
        private void ResetApp()
        {
            // reset chached data
            

            // recreate
            Type type = Type.GetTypeFromProgID("InDesign.Application.CS6", true);
            _app = (_Application) Activator.CreateInstance(type, true);

            // reset server
            if(!ReferenceEquals(_server, null))
            {
                if (_server.Running)
                {
                    _server.Stop();
                    _server.Dispose();                    
                }
            }
            // make server
            int port = int.Parse(getCfgParam("Indesign/CshpServerPort", "0"));
            _server = new AsyncSocketService(port);
            _server.Run();
        }       
           
    }
}
