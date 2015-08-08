using System;
using SMTPileIt.Server.Conversation;

namespace SMTPileIt.Server.States
{
    public class EnterDataConversationState : IConversationState
    {
        public SmtpCommand AllowedCommands
        {
            get
            {
                return SmtpCommand.DATA;
            }
        }

        public void EnterState(ISmtpStateContext context)
        {
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
            context.Reply(new SmtpReply(SmtpReplyCode.Code354, "Go for it."));
            return new DataConversationState();
        }
    }
}