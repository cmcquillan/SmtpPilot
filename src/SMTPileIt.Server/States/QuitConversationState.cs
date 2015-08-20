using System;
using SMTPileIt.Server.Conversation;

namespace SMTPileIt.Server.States
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

        public void EnterState(ISmtpStateContext context)
        {
            context.Reply(SmtpReply.ServerClosing);
            context.Client.Disconnect();
        }

        public void LeaveState(ISmtpStateContext context)
        {
        }

        public IConversationState ProcessData(ISmtpStateContext context, string line)
        {
            return this;
        }

        public IConversationState ProcessNewCommand(ISmtpStateContext context, SmtpCmd cmd, string line)
        {
            return this;
        }
    }
}