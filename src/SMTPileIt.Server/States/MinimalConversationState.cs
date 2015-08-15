using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMTPileIt.Server.Conversation;

namespace SMTPileIt.Server.States
{
    public abstract class MinimalConversationState : IConversationState
    {
        public virtual SmtpCommand AllowedCommands
        {
            get
            {
                return SmtpCommand.NOOP | SmtpCommand.RSET | SmtpCommand.QUIT;
            }
        }

        public virtual void EnterState(ISmtpStateContext context)
        {
            
        }

        public virtual void LeaveState(ISmtpStateContext context)
        {
            
        }

        public virtual IConversationState ProcessData(ISmtpStateContext context, string line)
        {
            throw new NotImplementedException();
        }

        public virtual IConversationState ProcessNewCommand(ISmtpStateContext context, SmtpCmd cmd, string line)
        {
            switch(cmd.Command)
            {
                case SmtpCommand.NOOP:
                    context.Reply(SmtpReply.OK);
                    return this;
                case SmtpCommand.RSET:
                    context.Conversation.Reset();
                    context.Reply(SmtpReply.OK);
                    return new AcceptMailConversationState();
                case SmtpCommand.QUIT:
                    return new QuitConversationState();
                default:
                    return new ErrorConversationState();
            }
        }
    }
}
