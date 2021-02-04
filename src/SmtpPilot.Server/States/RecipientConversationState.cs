using System;
using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.IO;
using SmtpPilot.Server.Internal;
using System.Linq;

namespace SmtpPilot.Server.States
{
    public class RecipientConversationState : MinimalConversationState
    {
        public override IConversationState Advance(SmtpStateContext2 context)
        {
            var buffer = context.GetBufferSegment(1024);

            if (context.Client.ReadUntil(Markers.CarriageReturnLineFeed, buffer.Span, 0, out var count))
            {
                var command = IOHelper.GetCommand(buffer.Span[0..4].ToArray());
                var line = buffer[5..].ToString();

                switch (command)
                {
                    case SmtpCommand.RCPT:
                        string[] emails = IOHelper.ParseEmails(line);
                        context.Conversation.CurrentMessage.AddAddresses(emails.Select(p => new EmailAddress(p, AddressType.To)).Cast<IAddress>().ToArray());
                        context.Reply(SmtpReply.OK);
                        return this;
                    case SmtpCommand.DATA:
                        return ConversationStates.DataRead;
                    default:
                        return ProcessBaseCommands(command, context);
                }
            }

            return this;
        }

        public override void EnterState(ISmtpStateContext context)
        {
        }

        internal override string HandleHelp()
        {
            return Constants.HelpTextRecipientState;
        }
    }
}