using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SMTPileIt.Server.IO
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
            _listener = new TcpListener(_ipAddress, ipPort);
            _listener.Start();
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
            var client = _listener.AcceptTcpClient();
            int clientId = client.Client.Handle.ToInt32();

            return new TcpMailClient(client, clientId);
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
