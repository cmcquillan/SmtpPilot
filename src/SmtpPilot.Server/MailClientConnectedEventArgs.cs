using SmtpPilot.Server.IO;
using System;

namespace SmtpPilot.Server
{
    public class MailClientConnectedEventArgs : MailClientEventArgs
    {
        public MailClientConnectedEventArgs(IMailClient client)
            : base(client)
        {
        }
    }
}