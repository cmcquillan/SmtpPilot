using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.Internal;
using SmtpPilot.Server.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Server.States
{
    public class OpenConnectionState : MinimalConversationState
    {
        public override void EnterState(ISmtpStateContext context)
        {
            context.Reply(new SmtpReply(SmtpReplyCode.Code220, "Ord-Mantell SMTP Server Ready"));
        }

        public override IConversationState ProcessData(ISmtpStateContext context, string line)
        {
            return new ErrorConversationState("Command not recognized.");
        }

        public override void LeaveState(ISmtpStateContext context)
        {
            
        }

        public override SmtpCommand AllowedCommands
        {
            get { return base.AllowedCommands | SmtpCommand.EHLO | SmtpCommand.HELO; }
        }

        public override IConversationState ProcessNewCommand(ISmtpStateContext context, SmtpCmd cmd, string line)
        {
            switch(cmd.Command)
            {
                case SmtpCommand.HELO:
                    context.Reply(new SmtpReply(SmtpReplyCode.Code250, Environment.MachineName));
                    return new AcceptMailConversationState();
                default:
                    return base.ProcessNewCommand(context, cmd, line);   
            }   
        }

        internal override string HandleHelp()
        {
            return Constants.HelpTextOpenState;
        }
    }
}
