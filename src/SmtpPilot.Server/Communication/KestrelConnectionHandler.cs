using Microsoft.AspNetCore.Connections;
using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.IO;
using SmtpPilot.Server.States;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Server.Communication
{
    public class KestrelConnectionHandler : ConnectionHandler
    {
        private readonly SmtpPilotConfiguration _configuration;
        private readonly EmailStatistics _statistics;
        private readonly SmtpConversation _conversation;
        private IMailClient _mailClient;
        private SmtpStateMachine _machine;

        public KestrelConnectionHandler(SmtpPilotConfiguration configuration, EmailStatistics statistics)
        {
            _configuration = configuration;
            _statistics = statistics;
            _conversation = new SmtpConversation();
        }

        public override async Task OnConnectedAsync(ConnectionContext connection)
        {
            _mailClient = new KestrelMailClient(connection);
            _machine = new SmtpStateMachine(_mailClient, _conversation, _statistics, _configuration);

            while (!_mailClient.Disconnected)
            {
                await _machine.ProcessData();

                if (_mailClient.SecondsClientHasBeenSilent > _configuration.ClientTimeoutSeconds)
                {
                    break;
                }

                await Task.Delay(1);
            }

            _configuration.ServerEvents.OnClientDisconnected(this, new MailClientDisconnectedEventArgs(_mailClient, DisconnectReason.ClientDisconnect));
        }
    }
}
