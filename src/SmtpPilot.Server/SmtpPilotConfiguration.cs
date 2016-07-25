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

        public SmtpPilotConfiguration()
            : this(Localhost, DefaultSmtpPort)
        {

        }

        public SmtpPilotConfiguration(string listenUri, int portNumber)
        {
            Listeners.Add(new TcpClientListener(listenUri, portNumber));
        }

        public IList<IMailClientListener> Listeners { get; } = new List<IMailClientListener>();
    }
}
