using SmtpPilot.Server.Data;
using SmtpPilot.Server.IO;
using System.Collections.Generic;

namespace SmtpPilot.Server
{
    public interface ISmtpPilotConfiguration
    {
        int ClientTimeoutSeconds { get; }
        string HostName { get; }
        IReadOnlyList<TcpListenerParameters> ListenParameters { get; }
        IMailStore MailStore { get; }
        SmtpServerEvents ServerEvents { get; }
    }
}