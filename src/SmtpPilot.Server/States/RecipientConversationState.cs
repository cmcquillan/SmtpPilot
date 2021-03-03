using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.Internal;
using SmtpPilot.Server.IO;
using System;
using System.Linq;

namespace SmtpPilot.Server.States
{
    internal class RecipientConversationState : MinimalConversationState
    {
        public override ConversationStateKey ThisKey => ConversationStates.Recipient;

        public override void EnterState(SmtpStateContext context)
        {
        }

        public override ConversationStateKey Advance(SmtpStateContext context)
        {
            var temp = context.ContextBuilder.GetTemporaryBuffer().Slice(0, 4);

            if (!context.Client.Peek(4, temp))
            {
                return ThisKey;
            }

            var command = IOHelper.GetCommand(temp);
            var buffer = context.ContextBuilder.GetBuffer(1024);

            if (context.Client.ReadUntil(Markers.CarriageReturnLineFeed, buffer, 4, out var count))
            {
                switch (command)
                {
                    case SmtpCommand.RCPT:
                        var start = buffer.IndexOf(':') + 1;
                        context.ContextBuilder.AddAddressSegment(start, count - start);
                        context.Reply(SmtpReply.OK);
                        return ThisKey;
                    case SmtpCommand.DATA:
                        if (((ReadOnlySpan<char>)buffer.Slice(0, count)).IsEmptyOrWhitespace())
                        {
                            return ConversationStates.DataRead;
                        }
                        else
                        {
                            context.Reply(SmtpReply.SyntaxError);
                            return ThisKey;
                        }
                    default:
                        return ProcessBaseCommands(command, buffer.Slice(0, count), context);
                }
            }

            return ThisKey;
        }

        internal override string HandleHelp()
        {
            return Constants.HelpTextRecipientState;
        }
    }
}