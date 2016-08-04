using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Server
{
    public class ServerEventArgs : EventArgs
    {
        public ServerEventArgs(SMTPServer server, ServerEventType type)
        {
            Server = server;
            EventType = type;
            EventTime = DateTimeOffset.Now;
        }

        public SMTPServer Server { get; }

        public ServerEventType EventType { get; }

        public DateTimeOffset EventTime { get; }
    }
}
