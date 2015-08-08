using SMTPileIt.Server.Conversation;
using SMTPileIt.Server.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SMTPileIt.Server.States
{
    public class AcceptMailConversationState : IConversationState
    {
        public IConversationState ProcessData(ISmtpStateContext context, string line)
        {
            return this;
        }

        public void LeaveState(ISmtpStateContext context)
        {
            if (!context.HasError)
                context.Reply(new SmtpReply(SmtpReplyCode.Code250, String.Empty));
        }

        public SmtpCommand AllowedCommands
        {
            get { return SmtpCommand.MAIL; }
        }


        public void EnterState(ISmtpStateContext context)
        {
            
        }

        public IConversationState ProcessNewCommand(ISmtpStateContext context, SmtpCmd cmd, string line)
        {
            switch(cmd.Command)
            {
                case SmtpCommand.MAIL:
                    string[] matches = IO.IOHelper.ParseEmails(cmd.Args);
                    if (matches.Length != 1)
                        return new ErrorConversationState();

                    string from = matches[0];
                    context.SetFrom(from);
                    return new RecipientConversationState();
                default:
                    throw new NotImplementedException();
            }
            
        }
    }
}
