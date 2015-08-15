using SMTPileIt.Server.Conversation;
using SMTPileIt.Server.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPileIt.Server.States
{
    public class OpenConnectionState : IConversationState
    {
        public void EnterState(ISmtpStateContext context)
        {
            context.Reply(new SmtpReply(SmtpReplyCode.Code220, "Ord-Mantell SMTP Server Ready"));
        }

        public IConversationState ProcessData(ISmtpStateContext context, string line)
        {
            throw new NotImplementedException();
        }

        public void LeaveState(ISmtpStateContext context)
        {
            
        }

        public SmtpCommand AllowedCommands
        {
            get { return SmtpCommand.EHLO | SmtpCommand.HELO; }
        }

        public IConversationState ProcessNewCommand(ISmtpStateContext context, SmtpCmd cmd, string line)
        {
            context.Reply(new SmtpReply(SmtpReplyCode.Code250, Environment.MachineName));
            return new AcceptMailConversationState();
        }
    }
}
