using System;
using SMTPileIt.Server.Conversation;
using SMTPileIt.Server.IO;

namespace SMTPileIt.Server.States
{
    public class RecipientConversationState : IConversationState
    {
        public SmtpCommand AllowedCommands
        {
            get
            {
                return SmtpCommand.RCPT|SmtpCommand.DATA;
            }
        }

        public void EnterState(ISmtpStateContext context)
        {
        }

        public void LeaveState(ISmtpStateContext context)
        {
            //if (!context.HasError)
            //    context.Reply(new SmtpReply(SmtpReplyCode.Code250, "Awesome"));
        }

        public IConversationState ProcessData(ISmtpStateContext context, string line)
        {
            return this;
        }

        public IConversationState ProcessNewCommand(ISmtpStateContext context, SmtpCmd cmd, string line)
        {
            switch(cmd.Command)
            {
                case SmtpCommand.RCPT:
                    string[] emails = IOHelper.ParseEmails(cmd.Args);
                    context.AddTo(emails);
                    context.Reply(new SmtpReply(SmtpReplyCode.Code250, "Awesome"));
                    return this;
                case SmtpCommand.DATA:
                    return new DataConversationState();
                default:
                    return new ErrorConversationState();
            }
        }
    }
}