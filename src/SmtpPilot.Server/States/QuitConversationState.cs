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
        }

        public void LeaveState(ISmtpStateContext context)
        {
        }

        public IConversationState ProcessData(ISmtpStateContext context, SmtpCmd cmd, string line)
        {
            return this;
        }
    }
}