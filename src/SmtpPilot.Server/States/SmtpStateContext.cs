using SmtpPilot.Server.Communication;
using SmtpPilot.Server.Conversation;
using System;
using System.Buffers;

namespace SmtpPilot.Server.States
{
    public class SmtpStateContext
    {
        public SmtpStateContext(
            IServiceProvider serviceProvider,
            SmtpPilotConfiguration configuration,
            IMailClient client,
            EmailStatistics emailStats,
            SmtpServerEvents events)
        {
            ServiceProvider = serviceProvider;
            Configuration = configuration;
            Client = client;
            EmailStats = emailStats;
            Events = events;
            ContextBuilder = new EmailMessageBuilder();
        }

        public EmailStatistics EmailStats { get; }

        internal EmailMessageBuilder ContextBuilder { get; }

        public IMailClient Client { get; }

        public SmtpServerEvents Events { get; }

        public SmtpPilotConfiguration Configuration { get; }

        public IServiceProvider ServiceProvider { get; }

        public void Reply(SmtpReply reply)
        {
            Client.Write(reply.FullText);
        }
    }
}
