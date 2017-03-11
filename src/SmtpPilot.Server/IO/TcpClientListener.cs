using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SmtpPilot.Server.IO
{
    public class TcpClientListener : IMailClientListener, IDisposable
    {
        private readonly string _ipString;
        private readonly int _ipPort;
        private readonly IPAddress _ipAddress;
        private readonly TcpListener _listener;

        public TcpClientListener(string ipString, int ipPort)
        {
            _ipString = ipString;
            _ipPort = ipPort;
            _ipAddress = IPAddress.Parse(ipString);

            try
            {
                _listener = new TcpListener(_ipAddress, ipPort);
                _listener.Start();
            }
            catch (SocketException ex)
            {
                string msg = $"Could not open listener connection to {_ipString}: {ex.SocketErrorCode}";
                Debug.WriteLine(msg, TraceConstants.TcpConnection);
                throw new MailServerStartupException(msg, ex);
            }
        }

        public bool ClientPending
        {
            get
            {
                return _listener.Pending();
            }
        }

        public IMailClient AcceptClient()
        {
            var client = _listener.AcceptTcpClientAsync().Result;
            return new TcpMailClient(client, Guid.NewGuid());
        }

        protected void Dispose(bool disposing)
        {
            if(disposing)
            {
                _listener.Stop();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
