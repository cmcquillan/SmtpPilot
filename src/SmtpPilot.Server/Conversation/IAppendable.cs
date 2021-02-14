namespace SmtpPilot.Server.Conversation
{
    public interface IAppendable
    {
        void AppendLine(string l);
        void Append(string l);
    }
}
