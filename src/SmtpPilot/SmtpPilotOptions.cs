using SmtpPilot.Server;
using SmtpPilot.Server.Data;
using SmtpPilot.Server.IO;
using System.Collections.Generic;

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

        internal ushort ListenPort { get; set; }

        internal SmtpPilotConfiguration ToConfiguration(IEnumerable<TcpListenerParameters> listeners)
        {
            var config = new SmtpPilotConfiguration(listeners, HostName);
            if (WriteMailToFolder)
            {
                config.MailStore = new JsonMailStore(WriteMailToFolderPath);
            }

            if (WriteMailToMemory)
            {
                config.MailStore = new InMemoryMailStore();
            }

            if (WebHookUri != null)
            {
                config.AddWebHooks(WebHookUri, 15, 1);
            }

            return config;
        }
    }
}
