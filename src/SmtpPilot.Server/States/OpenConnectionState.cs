using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Server.States
{
    public class OpenConnectionState : IConversationState
    {
        public void EnterState(ISmtpStateContext context)
        {
            context.Reply(new SmtpReply(SmtpReplyCode.Code220, "Ord-Mantell SMTP Server Ready"));
        }

        public IConversationState ProcessData(ISmtpStateContext context, string line)
        {
            return new ErrorConversationState("Command not recognized.");
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
