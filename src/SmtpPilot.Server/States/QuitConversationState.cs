using SmtpPilot.Server.Conversation;
using System;

namespace SmtpPilot.Server.States
{
    public class QuitConversationState : IConversationState
    {
        public bool ShouldDisconnect => true;

        public IConversationState Advance(SmtpStateContext context)
        {
            throw new NotImplementedException();
        }

        public void EnterState(SmtpStateContext context)
        {
            context.Reply(SmtpReply.ServerClosing);
            context.Configuration.ServerEvents.OnClientDisconnected(this, new MailClientDisconnectedEventArgs(context.Client, DisconnectReason.TransactionCompleted));
        }
    }
}