using SmtpPilot.Server.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmtpPilot.Server
{
    public static class ConversationStates
    {
        public static AcceptMailConversationState Accept { get; } = new AcceptMailConversationState();

        public static DataConversationState DataRead { get; } = new DataConversationState();

        public static ErrorConversationState Error { get; } = new ErrorConversationState();

        public static OpenConnectionState OpenConnection { get; } = new OpenConnectionState();

        public static QuitConversationState Quit { get; } = new QuitConversationState();

        public static RecipientConversationState Recipient { get; } = new RecipientConversationState();
    }
}
