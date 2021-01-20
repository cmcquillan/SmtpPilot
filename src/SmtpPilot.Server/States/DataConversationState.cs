using System;
using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.Internal;

namespace SmtpPilot.Server.States
{
    public class DataConversationState : IConversationState
    {
        private readonly object _headersCompleteKey = new object();
        private readonly object _headersCompleteValue = new object();

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

            if (!HeadersAreComplete(context))
            {
                if (IO.IOHelper.LooksLikeHeader(line))
                {
                    context.AddHeader(SmtpHeader.Parse(choppedLine));
                }
                else
                {
                    SetHeadersComplete(context, true);
                }
            }
            else
            {
                context.Conversation.CurrentMessage.AppendLine(choppedLine);
            }

            if (HeadersAreComplete(context) && line.SequenceEqual(Constants.EndOfDataElement.AsSpan()))
            {
                context.CompleteMessage();
                SetHeadersComplete(context, false);
                return ConversationStates.Accept;
            }

            return this;
        }

        private void SetHeadersComplete(ISmtpStateContext context, bool value)
        {
            context.Items[_headersCompleteKey] = value ? _headersCompleteValue : null;
        }

        private bool HeadersAreComplete(ISmtpStateContext context)
        {
            return context.Items.ContainsKey(_headersCompleteKey) && context.Items[_headersCompleteKey] == _headersCompleteValue;
        }
    }
}