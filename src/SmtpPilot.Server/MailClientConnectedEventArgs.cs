using SmtpPilot.Server.Communication;
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