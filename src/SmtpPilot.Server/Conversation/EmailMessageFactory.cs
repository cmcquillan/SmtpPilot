namespace SmtpPilot.Server.Conversation
{
    public class EmailMessageFactory : IEmailMessageFactory
    {
        public IMessage CreateNewMessage()
        {
            return new EmailMessage();
        }
    }
}
