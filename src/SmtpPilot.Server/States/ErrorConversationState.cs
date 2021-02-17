namespace SmtpPilot.Server.States
{
    internal class ErrorConversationState : IConversationState
    {
        private readonly string _errorMessage;

        public ErrorConversationState()
            : this("You have performed an illegal operation.")
        {

        }

        public ErrorConversationState(string errorMessage)
        {
            _errorMessage = errorMessage;
        }

        public void EnterState(SmtpStateContext context)
        {
            context.EmailStats.AddErrorGenerated();
            context.Reply(new Conversation.SmtpReply(Conversation.SmtpReplyCode.Code503, _errorMessage));
        }

        public ConversationStateKey Advance(SmtpStateContext context)
        {
            return ConversationStates.Error;
        }

        public bool ShouldDisconnect { get; }
    }
}
