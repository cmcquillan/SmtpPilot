using SmtpPilot.Server.Communication;
using SmtpPilot.Server.Conversation;
using System;

namespace SmtpPilot.Server
{
    public class EmailProcessedEventArgs : MailClientEventArgs
    {
        public EmailProcessedEventArgs(IMailClient client, IMessage message, EmailStatistics stats)
            : base(client)
        {
            Message = message;
            Statistics = stats;
        }

        public IMessage Message { get; }
        public EmailStatistics Statistics { get; }
    }
}