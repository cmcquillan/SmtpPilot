using SmtpPilot.Server.Conversation;

namespace SmtpPilot.Server.Data
{
    public interface IMailStore
    {
        void SaveMessage(IMessage message);
    }
}
