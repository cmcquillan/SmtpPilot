using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.Internal;
using SmtpPilot.Server.IO;
using SmtpPilot.Server.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmtpPilot.Server
{
    public class SMTPServer
    {
        private readonly IList<IMailClientListener> _listeners;
        private readonly List<IMailClient> _clients = new List<IMailClient>();
        private readonly Dictionary<Guid, SmtpStateMachine> _conversations = new Dictionary<Guid, SmtpStateMachine>();
        private readonly SmtpPilotConfiguration _configuration;
        private readonly List<IMailClient> _clientsToRemove = new List<IMailClient>();
        private readonly EmailStatistics _emailStats = new EmailStatistics();

        public SMTPServer(string ipString, int ipPort)
            : this(new SmtpPilotConfiguration(ipString, ipPort))
        {
        }

        public SMTPServer(SmtpPilotConfiguration configuration)
        {
            _configuration = configuration;
            _listeners = new List<IMailClientListener>(configuration.Listeners);
            Events.EmailProcessed += TrackEmailStatistics;

            if(_configuration.MailStore != null)
            {
                Events.EmailProcessed += (o, eva) => _configuration.MailStore.SaveMessage(eva.Message);
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

        public SmtpServerEvents Events
        {
            get
            {
                return _configuration.ServerEvents;
            }
        }

        public EmailStatistics Statistics { get { return _emailStats; } }

        public async Task Run(CancellationToken cancellationToken = default)
        {
            _emailStats.SetStart();

            Events.OnServerStart(this, new ServerEventArgs(this, ServerEventType.Started));

            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var listener in _listeners)
                {
                    if (listener.ClientPending)
                    {
                        var c = listener.AcceptClient();

                        Events.OnClientConnected(this, new MailClientConnectedEventArgs(c));

                        _clients.Add(c);
                        _emailStats.AddClient(1);
                        var conversation = new SmtpConversation();
                        var stateMachine = new SmtpStateMachine(c, conversation, _emailStats, _configuration);
                        _conversations[c.ClientId] = stateMachine;
                    }
                }

                foreach (var client in _clients)
                {
                    if (client.Disconnected)
                    {
                        _clientsToRemove.Add(client);
                        Events.OnClientDisconnected(this, new MailClientDisconnectedEventArgs(client, DisconnectReason.ClientDisconnect));
                        continue;
                    }

                    if (_conversations[client.ClientId].IsInQuitState)
                    {
                        _clientsToRemove.Add(client);
                        client.Disconnect();
                        Events.OnClientDisconnected(this, new MailClientDisconnectedEventArgs(client, DisconnectReason.TransactionCompleted));
                        continue;
                    }

                    await _conversations[client.ClientId].ProcessData();

                    if(client.SecondsClientHasBeenSilent > _configuration.ClientTimeoutSeconds)
                    {
                        _clientsToRemove.Add(client);
                        client.Disconnect();
                        Events.OnClientDisconnected(this, new MailClientDisconnectedEventArgs(client, DisconnectReason.ClientTimeout));
                    }
                }

                await Task.Delay(5);

                _emailStats.RemoveClient(_clientsToRemove.Count);

                foreach(var client in _clientsToRemove)
                {
                    _clients.Remove(client);
                }

                _clientsToRemove.Clear();
            }

            /*
             * Check each listener for an IDisposable and... well... dispose of them.
             */
            foreach (var listener in _listeners)
                (listener as IDisposable)?.Dispose();

            Events.OnServerStop(this, new ServerEventArgs(this, ServerEventType.Stopped));
        }
    }
}