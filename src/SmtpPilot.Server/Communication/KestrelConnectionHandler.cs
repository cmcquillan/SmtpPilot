using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.IO;
using SmtpPilot.Server.States;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmtpPilot.Server.Communication
{
    public class KestrelConnectionHandler : ConnectionHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILoggerFactory _loggerFactory;
        private readonly SmtpPilotConfiguration _configuration;
        private readonly EmailStatistics _statistics;

        public KestrelConnectionHandler(IServiceProvider serviceProvider, SmtpPilotConfiguration configuration, EmailStatistics statistics)
        {
            _serviceProvider = serviceProvider;
            _loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            _configuration = configuration;
            _statistics = statistics;
            _statistics.SetStart();
        }

        public override async Task OnConnectedAsync(ConnectionContext connection)
        {
            var conversation = new SmtpConversation();
            var mailClient = new KestrelMailClient(connection, _loggerFactory.CreateLogger<KestrelMailClient>());
            var machine = new SmtpStateMachine(mailClient, conversation, _statistics, _configuration, _loggerFactory.CreateLogger<SmtpStateMachine>());

            while (!mailClient.Disconnected)
            {
                await machine.ProcessData();

                if (mailClient.Disconnected)
                    break;

                if (mailClient.SecondsClientHasBeenSilent > _configuration.ClientTimeoutSeconds)
                    break;

                await Task.Delay(1);
            }

            _configuration.ServerEvents.OnClientDisconnected(this, new MailClientDisconnectedEventArgs(mailClient, DisconnectReason.ClientDisconnect));
        }
    }
}
