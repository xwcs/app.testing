﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xwcs.core.evt;

namespace xwcs.indesign
{
    public class OnMessageEventArgs : EventArgs
    {
        public OnMessageEventArgs(json.Message m)
        {
            Message = m;
            Result = null;
        }
        public json.Message Message { get; private set; }
        public object Result { get; set; }
    }

    public class AsyncSocketService : IDisposable
    {
        private static xwcs.core.manager.ILogger _logger = core.manager.SLogManager.getInstance().getClassLogger(typeof(AsyncSocketService));

        private CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();

        private bool _running = false;

        readonly object _gate = new object();

        private TcpListener _listener;

        private WeakEventSource<OnMessageEventArgs> _wes_OnMessage = null;
        public event EventHandler<OnMessageEventArgs> OnMessage
        {
            add
            {
                if (_wes_OnMessage == null)
                {
                    _wes_OnMessage = new WeakEventSource<OnMessageEventArgs>();
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
        public AsyncSocketService(int port)
        {
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

        public string Url
        {
            get
            {
                return string.Format("{0}:{1}", this.ipAddress, this.port);
            }
        }

       
        public async void RunAsync()
        {
            if (_running) return;
            _running = true;

            _listener = new TcpListener(this.ipAddress, this.port);
            _listener.Start();
            try
            {
                _logger.Debug("Service is now running on port :{0}", this.port);
                while (true)
                {
                    try
                    {
                        TcpClient tcpClient = await _listener.AcceptTcpClientAsync();

                        await Process(tcpClient);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message);
                    }
                }
            }
            finally
            {
                _listener.Stop();
                _logger.Debug("Service on port :{0} Stopped!", this.port);
            }
        }

        

        public void Stop()
        {
            _cancelTokenSource.Cancel();
            _listener.Stop();
        }

        private async Task Process(TcpClient tcpClient)
        {
            string clientEndPoint = tcpClient.Client.RemoteEndPoint.ToString();
            _logger.Debug("Received connection request from {0}", clientEndPoint);
            try
            {
                NetworkStream networkStream = tcpClient.GetStream();
                
                // Buffer for reading data
                byte[] bytes = new byte[10000];
                string data = null;

                int i;

                using(_cancelTokenSource.Token.Register(() => networkStream.Close()))
                {
                    // Loop to receive all the data sent by the client.
                    while ((i = await networkStream.ReadAsync(bytes, 0, bytes.Length, _cancelTokenSource.Token)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = Encoding.ASCII.GetString(bytes, 0, i);

#if DEBUG_TRACE_LOG_ON
                    _logger.Debug("Received: {0}", data);
#endif

                        OnMessageEventArgs args = new OnMessageEventArgs(JsonConvert.DeserializeObject<xwcs.indesign.json.Message>(data));

                        _wes_OnMessage.Raise(this, args);

                        dynamic resp = new System.Dynamic.ExpandoObject();
                        resp.status = "ok";
                        if (!ReferenceEquals(args.Result, null))
                        {
                            resp.data = args.Result;
                        }
                        data = JsonConvert.SerializeObject(resp);

                        byte[] msg = Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        await networkStream.WriteAsync(msg, 0, msg.Length);
#if DEBUG_TRACE_LOG_ON
                    _logger.Debug("Sent: {0}", data);
#endif
                    }
                    tcpClient.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                if (tcpClient.Connected)
                    tcpClient.Close();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                _cancelTokenSource.Cancel();
               
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AsyncSocketService() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
