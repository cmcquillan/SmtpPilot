using SmtpPilot.Server.States;

namespace SmtpPilot.Server
{
    internal static class ConversationStates
    {
        internal static ConversationStateKey Accept { get; } = new ConversationStateKey(typeof(AcceptMailConversationState));

        internal static ConversationStateKey DataRead { get; } = new ConversationStateKey(typeof(DataConversationState));

        internal static ConversationStateKey Error { get; } = new ConversationStateKey(typeof(ErrorConversationState));

        internal static ConversationStateKey OpenConnection { get; } = new ConversationStateKey(typeof(OpenConnectionState));

        internal static ConversationStateKey Quit { get; } = new ConversationStateKey(typeof(QuitConversationState));

        internal static ConversationStateKey Recipient { get; } = new ConversationStateKey(typeof(RecipientConversationState));
    }
}
