using SmtpPilot.Server.IO;
using System;

namespace SmtpPilot.Server
{
    public class MailClientEventArgs : EventArgs
    {
        public MailClientEventArgs(IMailClient client)
        {
            Client = client;
            ClientId = Client.ClientId;
        }

        public int ClientId { get; }

        public IMailClient Client { get; }
    }
}