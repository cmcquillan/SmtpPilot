using SMTPileIt.Server.IO;
using System;

namespace SMTPileIt.Server
{
    public class MailClientConnectedEventArgs : MailClientEventArgs
    {
        public MailClientConnectedEventArgs(IMailClient client)
            : base(client)
        {
        }
    }
}