using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot
{
    internal class SmtpPilotOptions
    {
        public string WebHookUri { get; set; }

        internal bool Headless { get; set; }

        internal string HostName { get; set; }

        internal bool WriteMailToFolder { get; set; }

        internal string WriteMailToFolderPath { get; set; }

        internal bool WriteMailToMemory { get; set; }

        internal List<string> ListenIPAddress { get; set; } = new List<string>();

        internal int ListenPort { get; set; }
    }
}
