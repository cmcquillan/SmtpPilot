using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
using SmtpPilot.Server.Communication;
using System.Threading.Tasks;

namespace SmtpPilot
{
    internal class SmtpPilotConnectionHandler : ConnectionHandler
    {
        private readonly KestrelClientListenerAdapter _adapter;
        private readonly ILogger<SmtpPilotConnectionHandler> _logger;

        public SmtpPilotConnectionHandler(KestrelClientListenerAdapter adapter, ILogger<SmtpPilotConnectionHandler> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public override Task OnConnectedAsync(ConnectionContext connection)
        {
            return _adapter.ExecuteNewConnection(connection);
        }
    }
}
