namespace SmtpPilot.Server.Conversation
{
    public interface IEmailMessageFactory
    {
        IMessage CreateNewMessage();
    }
}
