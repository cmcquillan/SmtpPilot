using SmtpPilot.Server.Data;
using SmtpPilot.Server.IO;
using System.Collections.Generic;

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

        public SmtpPilotConfiguration(string listenIp, int portNumber)
            : this(listenIp, portNumber, DefaultHostName)
        {
        }

        public SmtpPilotConfiguration(string listenIp, int portNumber, string hostname)
            : this(new[] { new TcpClientListener(listenIp, portNumber) }, hostname)
        {
        }

        public SmtpPilotConfiguration(IEnumerable<IMailClientListener> listeners, string hostname)
        {
            ServerEvents = new SmtpServerEvents();
            ClientTimeoutSeconds = DefaultTimeoutSeconds;
            HostName = hostname ?? DefaultHostName;
            Listeners = new List<IMailClientListener>(listeners);
        }

        public string HostName { get; set; }

        public IReadOnlyList<IMailClientListener> Listeners { get; }

        public int ClientTimeoutSeconds { get; set; }

        public IMailStore MailStore { get; set; }

        public SmtpServerEvents ServerEvents { get; set; }
    }
}
