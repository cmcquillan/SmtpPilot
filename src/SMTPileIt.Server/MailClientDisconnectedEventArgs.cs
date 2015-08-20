using SMTPileIt.Server.IO;

namespace SMTPileIt.Server
{
    public class MailClientDisconnectedEventArgs : MailClientEventArgs
    {
        public MailClientDisconnectedEventArgs(IMailClient client, DisconnectReason reason)
            : base(client)
        {
            Reason = reason;
        }

        public DisconnectReason Reason { get; }
    }

    public enum DisconnectReason : int
    {
        TransactionCompleted = 0,
        ClientTimeout = 1,
        ClientError = 2,
        ServerError = 3,
    }
}