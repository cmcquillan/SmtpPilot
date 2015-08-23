using SMTPileIt.Server.Conversation;
using SMTPileIt.Server.Internal;
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

        public event MailClientConnectedEventHandler ClientConnected;

        public event MailClientDisconnectedEventHandler ClientDisconnected;

        private event EmailProcessedEventHandler _internalEmailProcessed;

        public event EmailProcessedEventHandler EmailProcessed
        {
            /*
             * CONSIDER:  This could be an expensive operation is lots of 
             * concurrent connections are taking place.  There might be reason 
             * to "drop" hooking into existing conversations.  If that is not an
             * option, we could potentially parallelize this, since event adds/removes
             * are atomic operations behind the scenes.
             */
            add
            {
                _internalEmailProcessed += value;
                foreach(var c in _conversations)
                {
                    c.Value.Context.EmailProcessed += value;
                }
            }
            remove
            {
                _internalEmailProcessed -= value;
                foreach(var c in _conversations)
                {
                    c.Value.Context.EmailProcessed -= value;
                }
            }
        }

        protected virtual void OnClientConnected(MailClientConnectedEventArgs eventArgs)
        {
            MailClientConnectedEventHandler handler = ClientConnected;

            if (handler != null)
            {
                foreach(MailClientConnectedEventHandler sub in handler.GetInvocationList())
                {
                    try
                    {
                        sub(this, eventArgs);
                    }
                    catch (Exception ex)
                    { }
                }
            }
        }

        protected virtual void OnClientDisconnected(MailClientDisconnectedEventArgs eventArgs)
        {
            MailClientDisconnectedEventHandler handler = ClientDisconnected;

            if(handler != null)
            {
                foreach(MailClientDisconnectedEventHandler sub in handler.GetInvocationList())
                {
                    try
                    {
                        sub(this, eventArgs);
                    }
                    catch (Exception ex)
                    { }
                }
            }
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

                    OnClientConnected(new MailClientConnectedEventArgs(c));

                    _clients.Add(c);
                    var conversation = new SmtpConversation();
                    var stateMachine = new SmtpStateMachine(c, conversation);
                    stateMachine.Context.EmailProcessed += _internalEmailProcessed;
                    _conversations[c.ClientId] = stateMachine;
                }

                foreach (var client in _clients)
                {
                    if (_conversations[client.ClientId].IsInQuitState)
                    {
                        client.Disconnect();
                        OnClientDisconnected(new MailClientDisconnectedEventArgs(client, DisconnectReason.TransactionCompleted));
                        continue;
                    }

                    _conversations[client.ClientId].ProcessLine();
                    Thread.Sleep(5);
                }

                _clients.RemoveAll(p => p.Disconnected);
            }

            (_listener as IDisposable).Dispose();
        }
    }
}
