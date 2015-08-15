using System;
using SMTPileIt.Server.Conversation;
using SMTPileIt.Server.IO;

namespace SMTPileIt.Server.States
{
    public class RecipientConversationState : MinimalConversationState
    {
        public override SmtpCommand AllowedCommands
        {
            get
            {
                return base.AllowedCommands | SmtpCommand.RCPT | SmtpCommand.DATA;
            }
        }

        public override void EnterState(ISmtpStateContext context)
        {
        }

        public override void LeaveState(ISmtpStateContext context)
        {
        }

        public override IConversationState ProcessData(ISmtpStateContext context, string line)
        {
            return this;
        }

        public override IConversationState ProcessNewCommand(ISmtpStateContext context, SmtpCmd cmd, string line)
        {
            switch(cmd.Command)
            {
                case SmtpCommand.RCPT:
                    string[] emails = IOHelper.ParseEmails(cmd.Args);
                    context.AddTo(emails);
                    context.Reply(SmtpReply.OK);
                    return this;
                case SmtpCommand.DATA:
                    return new DataConversationState();
                default:
                    return base.ProcessNewCommand(context, cmd, line);
            }
        }
    }
}