namespace SmtpPilot.Server.Conversation
{
    public interface IAddress
    {
        string DisplayName { get; }
        string Address { get; }
        string Host { get; }
        string User { get; }
        AddressType Type { get; }
    }
}
