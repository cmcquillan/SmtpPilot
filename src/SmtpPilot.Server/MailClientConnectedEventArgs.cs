using SmtpPilot.Server.Communication;

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