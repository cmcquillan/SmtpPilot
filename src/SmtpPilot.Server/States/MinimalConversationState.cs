using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmtpPilot.Server.Conversation;

namespace SmtpPilot.Server.States
{
    public abstract class MinimalConversationState : IConversationState
    {
        public virtual SmtpCommand AllowedCommands
        {
            get
            {
                return SmtpCommand.NOOP | SmtpCommand.RSET | SmtpCommand.QUIT | SmtpCommand.HELP;
            }
        }

        public virtual bool AcceptingCommands => true;

        public virtual void EnterState(ISmtpStateContext context)
        {

        }

        public virtual void LeaveState(ISmtpStateContext context)
        {
            
        }

        public virtual IConversationState ProcessData(ISmtpStateContext context, SmtpCmd cmd, ReadOnlySpan<char> line)
        {
            switch(cmd.Command)
            {
                case SmtpCommand.NOOP:
                    context.Reply(SmtpReply.OK);
                    return this;
                case SmtpCommand.RSET:
                    context.Conversation.Reset();
                    context.Reply(SmtpReply.OK);
                    return ConversationStates.Accept;
                case SmtpCommand.QUIT:
                    return ConversationStates.Quit;
                case SmtpCommand.HELP:
                    context.Reply(new SmtpReply(SmtpReplyCode.Code250, HandleHelp()));
                    return this;
                default:
                    return ConversationStates.Error;
            }
        }

        internal abstract string HandleHelp();
    }
}
