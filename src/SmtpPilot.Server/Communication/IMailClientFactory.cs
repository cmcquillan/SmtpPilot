using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Text;

namespace SmtpPilot.Server.Communication
{
    public interface IMailClientFactory
    {
        IMailClient CreateClient(IDuplexPipe pipe, ILoggerFactory loggerFactory);
    }
}
