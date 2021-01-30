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

        public KestrelConnectionHandler(SmtpPilotConfiguration configuration, EmailStatistics statistics)
        {
            _configuration = configuration;
            _statistics = statistics;
        }

        public override async Task OnConnectedAsync(ConnectionContext connection)
        {
            var conversation = new SmtpConversation();
            var mailClient = new KestrelMailClient(connection);
            var machine = new SmtpStateMachine(mailClient, conversation, _statistics, _configuration);

            while (!mailClient.Disconnected)
            {
                await machine.ProcessData();

                if (mailClient.SecondsClientHasBeenSilent > _configuration.ClientTimeoutSeconds)
                {
                    break;
                }

                await Task.Delay(1);
            }

            _configuration.ServerEvents.OnClientDisconnected(this, new MailClientDisconnectedEventArgs(mailClient, DisconnectReason.ClientDisconnect));
        }
    }
}
