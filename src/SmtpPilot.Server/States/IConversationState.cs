namespace SmtpPilot.Server.States
{
    public interface IConversationState
    {
        void EnterState(SmtpStateContext context);
        bool ShouldDisconnect { get; }
        IConversationState Advance(SmtpStateContext context);
    }
}
