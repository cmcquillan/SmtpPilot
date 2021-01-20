using System;
using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.IO;
using SmtpPilot.Server.Internal;

namespace SmtpPilot.Server.States
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

        public override IConversationState ProcessData(ISmtpStateContext context, SmtpCmd cmd, ReadOnlySpan<char> line)
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
                    return base.ProcessData(context, cmd, line);
            }
        }

        internal override string HandleHelp()
        {
            return Constants.HelpTextRecipientState;
        }
    }
}