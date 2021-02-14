using Microsoft.Extensions.Logging;
using System.IO.Pipelines;

namespace SmtpPilot.Server.Communication
{
    public interface IMailClientFactory
    {
        IMailClient CreateClient(IDuplexPipe pipe, ILoggerFactory loggerFactory);
    }
}
