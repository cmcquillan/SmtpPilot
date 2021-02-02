using System;
using SmtpPilot.Server.Conversation;

namespace SmtpPilot.Server.States
{
    public class QuitConversationState : IConversationState
    {
        public SmtpCommand AllowedCommands
        {
            get
            {
                return SmtpCommand.NonCommand;
            }
        }

        public bool AcceptingCommands => false;

        public void EnterState(ISmtpStateContext context)
        {
            context.Reply(SmtpReply.ServerClosing);
            context.Configuration.ServerEvents.OnClientDisconnected(this, new MailClientDisconnectedEventArgs(context.Client, DisconnectReason.TransactionCompleted));
        }

        public void LeaveState(ISmtpStateContext context)
        {
        }

        public IConversationState ProcessData(ISmtpStateContext context, SmtpCmd cmd, ReadOnlySpan<char> line)
        {
            return this;
        }
    }
}