using System;
using SMTPileIt.Server.Conversation;
using SMTPileIt.Server.Internal;

namespace SMTPileIt.Server.States
{
    public class DataConversationState : IConversationState
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
            context.Conversation.AddElement(new SmtpData());
        }

        public void LeaveState(ISmtpStateContext context)
        {
            context.Reply(new SmtpReply(SmtpReplyCode.Code250, "Awesome"));
        }

        public IConversationState ProcessData(ISmtpStateContext context, string line)
        { 
            if(line.Equals(Constants.EndOfDataElement))
            {
                return new EndDataConversationState();
            }
            return this;
        }

        public IConversationState ProcessNewCommand(ISmtpStateContext context, SmtpCmd cmd, string line)
        {
            return this;
        }
    }
}