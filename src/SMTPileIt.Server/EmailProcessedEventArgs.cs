using SMTPileIt.Server.Conversation;
using SMTPileIt.Server.IO;
using System;

namespace SMTPileIt.Server
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