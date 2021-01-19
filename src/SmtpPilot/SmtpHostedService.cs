using Microsoft.Extensions.Hosting;
using SmtpPilot.Server;
using System.Threading;
using System.Threading.Tasks;

namespace SmtpPilot
{
    internal class SmtpHostedService : BackgroundService
    {
        private readonly SMTPServer _server;

        public SmtpHostedService(SMTPServer server)
        {
            _server = server;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _server.Run(stoppingToken);
        }
    }
}