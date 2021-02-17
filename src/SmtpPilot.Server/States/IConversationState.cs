namespace SmtpPilot.Server.States
{
    internal interface IConversationState
    {
        void EnterState(SmtpStateContext context);
        bool ShouldDisconnect { get; }
        ConversationStateKey Advance(SmtpStateContext context);
    }
}
