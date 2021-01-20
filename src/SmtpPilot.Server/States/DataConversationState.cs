using System;
using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.Internal;

namespace SmtpPilot.Server.States
{
    public class DataConversationState : IConversationState
    {
        private bool _headersAreOver;

        public SmtpCommand AllowedCommands
        {
            get
            {
                return SmtpCommand.NonCommand;
            }
        }

        public bool AcceptingCommands => false;

        public void EnterState(ISmtpStateContext context)
        {
            context.Reply(SmtpReply.BeginData);
            context.Conversation.AddElement(new SmtpData());
            _headersAreOver = false;
        }

        public void LeaveState(ISmtpStateContext context)
        {
            context.Reply(SmtpReply.OK);
        }

        public IConversationState ProcessData(ISmtpStateContext context, SmtpCmd cmd, ReadOnlySpan<char> line)
        {
            if (line.SequenceEqual(Constants.CarriageReturnLineFeed.AsSpan()))
                return this;

            var index = line.LastIndexOf(Environment.NewLine.AsSpan());
            var lengthWithoutNewLine = index != -1 ? index : line.Length;
            ReadOnlySpan<char> choppedLine = line.Slice(0, lengthWithoutNewLine);

            if (!_headersAreOver)
            {
                if (IO.IOHelper.LooksLikeHeader(line))
                {
                    context.AddHeader(SmtpHeader.Parse(choppedLine));
                }
                else
                {
                    _headersAreOver = true;
                }
            }
            else
            {
                context.Conversation.CurrentMessage.AppendLine(choppedLine);
            }

            if (_headersAreOver && line.SequenceEqual(Constants.EndOfDataElement.AsSpan()))
            {
                context.CompleteMessage();
                return new AcceptMailConversationState();
            }

            return this;
        }
    }
}