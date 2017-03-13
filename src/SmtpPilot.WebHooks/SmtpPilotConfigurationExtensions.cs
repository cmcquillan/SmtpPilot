using System;
using System.Threading;
using SmtpPilot.WebHooks;

namespace SmtpPilot.Server
{
    public static class SmtpPilotConfigurationExtensions
    {
        public static SmtpPilotConfiguration AddWebHooks(
            this SmtpPilotConfiguration config,
            string webHookUrl,
            int maxWebHookAttempts = 3,
            int webHookRetryFactor = 60)
        {
            var extension = new SmtpPilotWebHookExtension(config, webHookUrl)
            {
                MaxWebHookAttempts = maxWebHookAttempts,
                WebHookRetryTimeFactor = webHookRetryFactor,
            };

            return config;
        }
    }
}
