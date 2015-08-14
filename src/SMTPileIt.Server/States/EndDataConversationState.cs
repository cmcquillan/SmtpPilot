using System;
using SMTPileIt.Server.Conversation;

namespace SMTPileIt.Server.States
{
    public class EndDataConversationState : IConversationState
    {
        public SmtpCommand AllowedCommands
        {
            get
            {
                return SmtpCommand.QUIT;
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
            switch(cmd.Command)
            {
                case SmtpCommand.RSET:
                    context.Reply(new SmtpReply(SmtpReplyCode.Code250, "OK"));
                    return new AcceptMailConversationState();
                case SmtpCommand.QUIT:
                    return new QuitConversationState();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}