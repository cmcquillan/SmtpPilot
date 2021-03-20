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
            var temp = context.ContextBuilder.GetTemporaryBuffer();
            int spaceIx;
            SmtpCommand command;

            if (context.Client.PeekUntil(Markers.DataCommand, temp, out _))
            {
                command = SmtpCommand.DATA;
                spaceIx = 4;
            }
            else if (!context.Client.PeekUntil(Markers.Space, temp, out spaceIx))
            {
                return ThisKey;
            }
            else
            {
                command = IOHelper.GetCommand(temp.Slice(0, spaceIx));
            }

            var buffer = context.ContextBuilder.GetBuffer(1024);

            if (context.Client.ReadUntil(Markers.CarriageReturnLineFeed, buffer, spaceIx, out var count))
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