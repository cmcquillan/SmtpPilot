using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.Internal;
using SmtpPilot.Server.IO;
using SmtpPilot.Server.States;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SmtpPilot.Server
{
    public class SMTPServer
    {
        private readonly IList<IMailClientListener> _listeners;
        private readonly List<IMailClient> _clients = new List<IMailClient>();
        private readonly Dictionary<int, SmtpStateMachine> _conversations = new Dictionary<int, SmtpStateMachine>();
        private readonly SmtpPilotConfiguration _configuration;

        private volatile bool _running;
        private Thread _runThread;
        private EmailStatistics _emailStats = new EmailStatistics();

        public SMTPServer(string ipString, int ipPort)
            : this(new SmtpPilotConfiguration(ipString, ipPort))
        {
        }

        public SMTPServer(SmtpPilotConfiguration configuration)
        {
            _listeners = new List<IMailClientListener>(configuration.Listeners);
            EmailProcessed += TrackEmailStatistics;
            _configuration = configuration;

            if(_configuration.MailStore != null)
            {
                EmailProcessed += (o, eva) => _configuration.MailStore.SaveMessage(eva.Message);
            }
        }

        public SMTPServer()
            : this(new SmtpPilotConfiguration())
        {

        }

        private void TrackEmailStatistics(object sender, EmailProcessedEventArgs eventArgs)
        {
            _emailStats.AddEmailReceived();
        }

        public EmailStatistics Statistics { get { return _emailStats; } }

        public event MailClientConnectedEventHandler ClientConnected;

        public event MailClientDisconnectedEventHandler ClientDisconnected;

        public event ServerStartedEventHandler ServerStarted;

        public event ServerStoppedEventHandler ServerStopped;

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
                foreach (var c in _conversations)
                {
                    c.Value.Context.EmailProcessed += value;
                }
            }
            remove
            {
                _internalEmailProcessed -= value;
                foreach (var c in _conversations)
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
                foreach (MailClientConnectedEventHandler sub in handler.GetInvocationList())
                {
                    try
                    {
                        sub(this, eventArgs);
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        protected virtual void OnClientDisconnected(MailClientDisconnectedEventArgs eventArgs)
        {
            MailClientDisconnectedEventHandler handler = ClientDisconnected;

            if (handler != null)
            {
                foreach (MailClientDisconnectedEventHandler sub in handler.GetInvocationList())
                {
                    try
                    {
                        sub(this, eventArgs);
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        protected virtual void OnServerStart(ServerEventArgs eventArgs)
        {
            ServerStartedEventHandler handler = ServerStarted;

            if(handler != null)
            {
                foreach(ServerStartedEventHandler sub in handler.GetInvocationList())
                {
                    try
                    {
                        sub(this, eventArgs);
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        protected virtual void OnServerStop(ServerEventArgs eventArgs)
        {
            ServerStoppedEventHandler handler = ServerStopped;

            if(handler != null)
            {
                foreach(ServerStoppedEventHandler sub in handler.GetInvocationList())
                {
                    try
                    {
                        sub(this, eventArgs);
                    }
                    catch (Exception)
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
            _emailStats.SetStart();

            OnServerStart(new ServerEventArgs(this, ServerEventType.Started));

            while (_running)
            {
                foreach (var listener in _listeners)
                {
                    if (listener.ClientPending)
                    {
                        var c = listener.AcceptClient();

                        OnClientConnected(new MailClientConnectedEventArgs(c));

                        _clients.Add(c);
                        _emailStats.AddClient(1);
                        var conversation = new SmtpConversation();
                        var stateMachine = new SmtpStateMachine(c, conversation, _emailStats);
                        stateMachine.Context.EmailProcessed += _internalEmailProcessed;
                        _conversations[c.ClientId] = stateMachine;
                    }
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

                    if(client.SecondsClientHasBeenSilent > _configuration.ClientTimeoutSeconds)
                    {
                        client.Disconnect();
                        OnClientDisconnected(new MailClientDisconnectedEventArgs(client, DisconnectReason.ClientTimeout));
                    }
                }

                Thread.Sleep(5);

                _emailStats.RemoveClient(_clients.RemoveAll(p => p.Disconnected));
            }

            /*
             * Check each listener for an IDisposable and... well... dispose of them.
             */
            foreach (var listener in _listeners)
                (listener as IDisposable)?.Dispose();

            OnServerStop(new ServerEventArgs(this, ServerEventType.Stopped));
        }
    }
}