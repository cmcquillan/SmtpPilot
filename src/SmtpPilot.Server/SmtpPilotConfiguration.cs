using SmtpPilot.Server.Data;
using SmtpPilot.Server.IO;
using System.Collections.Generic;
using System.Net;

namespace SmtpPilot.Server
{
    public class SmtpPilotConfiguration : ISmtpPilotConfiguration
    {
        public const string Localhost = "127.0.0.1";
        public const string DefaultHostName = "smtp-pilot.local";
        public const int DefaultSmtpPort = 25;
        public const int DefaultTimeoutSeconds = 60;

        public SmtpPilotConfiguration()
            : this(Localhost, DefaultSmtpPort)
        {
        }

        public SmtpPilotConfiguration(string listenIp, ushort portNumber)
            : this(listenIp, portNumber, DefaultHostName)
        {
        }

        public SmtpPilotConfiguration(string listenIp, ushort portNumber, string hostname)
            : this(new[] { new TcpListenerParameters(IPAddress.Parse(listenIp), portNumber) }, hostname)
        {
        }

        public SmtpPilotConfiguration(IEnumerable<TcpListenerParameters> listenParameters, string hostname)
        {
            ServerEvents = new SmtpServerEvents();
            ClientTimeoutSeconds = DefaultTimeoutSeconds;
            HostName = hostname ?? DefaultHostName;
            ListenParameters = new List<TcpListenerParameters>(listenParameters);
        }

        public string HostName { get; set; }

        public IReadOnlyList<TcpListenerParameters> ListenParameters { get; }

        public int ClientTimeoutSeconds { get; set; }

        public IMailStore MailStore { get; set; }

        public SmtpServerEvents ServerEvents { get; set; }
    }
}
