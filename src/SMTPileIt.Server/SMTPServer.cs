using SMTPileIt.Server.Conversation;
using SMTPileIt.Server.IO;
using SMTPileIt.Server.States;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SMTPileIt.Server
{
    public class SMTPServer
    {
        private readonly IMailClientListener _listener;
        private readonly List<IMailClient> _clients = new List<IMailClient>();
        private readonly Dictionary<int, SmtpStateMachine> _conversations = new Dictionary<int, SmtpStateMachine>();
        private volatile bool _running;
        private Thread _runThread;

        public SMTPServer(IMailClientListener clientListener)
        {
            _listener = clientListener;
        }

        public SMTPServer(string ipString, int ipPort)
        {
            _listener = new TcpClientListener(ipString, ipPort);
        }

        public void Start()
        {
            _runThread = new Thread(new ThreadStart(Run));
            _runThread.Start();
        }

        public void Stop()
        {
            _running = false;
            _runThread.Join();
        }

        public void Run()
        {
            _running = true;

            while(_running)
            {
                if (_listener.ClientPending)
                {
                    var c = _listener.AcceptClient();
                    _clients.Add(c);
                    _conversations[c.ClientId] = new SmtpStateMachine(c, new SmtpConversation());
                }

                foreach (var client in _clients)
                {
                    if (_conversations[client.ClientId].IsInQuitState)
                        continue;

                    _conversations[client.ClientId].ProcessLine();
                    Thread.Sleep(5);
                }

                _clients.RemoveAll(p => p.Disconnected);
            }

            (_listener as IDisposable).Dispose();
        }
    }
}
