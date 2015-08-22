using SMTPileIt.Server.Conversation;
using SMTPileIt.Server.IO;
using System;

namespace SMTPileIt.Server
{
    public class EmailProcessedEventArgs : MailClientEventArgs
    {
        public EmailProcessedEventArgs(IMailClient client, IMessage message)
            : base(client)
        {
            Message = message;
        }

        public IMessage Message { get; }
    }
}