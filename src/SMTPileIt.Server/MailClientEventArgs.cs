using SMTPileIt.Server.IO;
using System;

namespace SMTPileIt.Server
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