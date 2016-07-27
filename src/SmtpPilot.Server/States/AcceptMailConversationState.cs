using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SmtpPilot.Server.States
{
    public class AcceptMailConversationState : MinimalConversationState
    {
        public override IConversationState ProcessData(ISmtpStateContext context, string line)
        {
            return new ErrorConversationState("Did not recognize command.");
        }

        public override void LeaveState(ISmtpStateContext context)
        {
            if (!context.HasError)
                context.Reply(SmtpReply.OK);
        }

        public override SmtpCommand AllowedCommands
        {
            get { return base.AllowedCommands | SmtpCommand.MAIL; }
        }


        public override void EnterState(ISmtpStateContext context)
        {
            
        }

        public override IConversationState ProcessNewCommand(ISmtpStateContext context, SmtpCmd cmd, string line)
        {
            switch(cmd.Command)
            {
                case SmtpCommand.MAIL:
                    string[] matches = IO.IOHelper.ParseEmails(cmd.Args);
                    if (matches.Length != 1)
                        return new ErrorConversationState();

                    string from = matches[0];
                    context.NewMessage();
                    context.SetFrom(from);
                    
                    return new RecipientConversationState();
                default:
                    return base.ProcessNewCommand(context, cmd, line);
            }
            
        }

        internal override string HandleHelp()
        {
            return Constants.HelpTextAcceptState;
        }
    }
}
