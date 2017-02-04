using SmtpPilot.Server.Data;
using SmtpPilot.Server.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Server
{
    public class SmtpPilotConfiguration
    {
        public const string Localhost = "127.0.0.1";
        public const int DefaultSmtpPort = 25;
        public const int DefaultTimeoutSeconds = 60;

        public SmtpPilotConfiguration()
            : this(Localhost, DefaultSmtpPort)
        {
        }

        public SmtpPilotConfiguration(string listenIp, int portNumber)
            : this(listenIp, portNumber, Environment.MachineName)
        {
        }

        public SmtpPilotConfiguration(string listenIp, int portNumber, string hostname)
            : this(new[] { new TcpClientListener(listenIp, portNumber) }, hostname)
        {
        }

        public SmtpPilotConfiguration(IEnumerable<IMailClientListener> listeners, string hostname)
        {
            ClientTimeoutSeconds = DefaultTimeoutSeconds;
            HostName = hostname;

            foreach (var l in listeners)
                Listeners.Add(l);
        }

        public string HostName { get; set; }

        public IList<IMailClientListener> Listeners { get; } = new List<IMailClientListener>();

        public int ClientTimeoutSeconds { get; set; }

        public IMailStore MailStore { get; set; }
    }
}
