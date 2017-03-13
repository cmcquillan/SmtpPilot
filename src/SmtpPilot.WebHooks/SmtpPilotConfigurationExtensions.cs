using System;
using System.Threading;
using SmtpPilot.WebHooks;

namespace SmtpPilot.Server
{
    public static class SmtpPilotConfigurationExtensions
    {
        public static SmtpPilotConfiguration AddWebHooks(this SmtpPilotConfiguration config, string webHookUrl)
        {
            var extension = new SmtpPilotWebHookExtension(config, webHookUrl);
            return config;
        }
    }
}
