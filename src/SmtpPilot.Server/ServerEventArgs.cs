using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Server
{
    public class ServerEventArgs : EventArgs
    {
        public ServerEventArgs(ServerEventType type)
        {
            EventType = type;
            EventTime = DateTimeOffset.Now;
        }

        public ServerEventType EventType { get; }

        public DateTimeOffset EventTime { get; }
    }
}
