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
                return SmtpCommand.RCPT;
            }
        }

        public void EnterState(ISmtpStateContext context)
        {
        }

        public void LeaveState(ISmtpStateContext context)
        {
            if (!context.HasError)
                context.Reply(new SmtpReply(SmtpReplyCode.Code250, "Awesome"));
        }

        public IConversationState ProcessData(ISmtpStateContext context, string line)
        {
            return this;
        }

        public IConversationState ProcessNewCommand(ISmtpStateContext context, SmtpCmd cmd, string line)
        {
            string[] emails = IOHelper.ParseEmails(cmd.Args);

            if(emails.Length == 0)
            {
                return new ErrorConversationState("Must provide at least one TO address.");
            }

            context.SetTo(emails);
            return new EnterDataConversationState();
        }
    }
}