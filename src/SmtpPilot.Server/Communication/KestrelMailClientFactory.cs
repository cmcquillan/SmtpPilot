using Microsoft.Extensions.Logging;
using System.IO.Pipelines;

namespace SmtpPilot.Server.Communication
{
    public class KestrelMailClientFactory : IMailClientFactory
    {
        public IMailClient CreateClient(IDuplexPipe pipe, ILoggerFactory loggerFactory)
        {
            return new KestrelMailClient(pipe, loggerFactory.CreateLogger<KestrelMailClient>());
        }
    }
}
