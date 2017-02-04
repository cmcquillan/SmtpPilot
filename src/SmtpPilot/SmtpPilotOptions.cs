using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot
{
    internal class SmtpPilotOptions
    {
        internal string HostName { get; set; }

        internal bool WriteMailToFolder { get; set; }

        internal string WriteMailToFolderPath { get; set; }

        internal bool WriteMailToMemory { get; set; }

        internal string ListenIPAddress { get; set; }

        internal int ListenPort { get; set; }
    }
}
