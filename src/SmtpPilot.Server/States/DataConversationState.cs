using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.Internal;

namespace SmtpPilot.Server.States
{
    public class DataConversationState : IConversationState
    {
        public bool ShouldDisconnect => false;

        public void EnterState(SmtpStateContext context)
        {
            context.Reply(SmtpReply.BeginData);
            context.Conversation.AddElement(new SmtpData());
        }

        public IConversationState Advance(SmtpStateContext context)
        {
            var buffer = context.GetBufferSegment(1024);

            // First attempt to read until <CR><LF>.<CR><LR>
            if (context.Client.ReadUntil(Markers.EndOfDataSegment, buffer.Span, 0, out var count))
            {
                context.AdvanceBuffer(count);

                context.Conversation.CurrentMessage.Append(buffer.Span.Slice(0, count));
                context.Conversation.CurrentMessage.Complete();

                context.Events.OnEmailProcessed(this, new EmailProcessedEventArgs(context.Client, context.Conversation.CurrentMessage, context.EmailStats));
                context.EmailStats.AddEmailReceived();

                context.Reply(SmtpReply.OK);

                return ConversationStates.Accept;
            }
            // Otherwise just fill the buffer and append. We'll cycle back through this method.
            else if (context.Client.Read(1024, buffer.Span))
            {
                context.AdvanceBuffer(1024);
                context.Conversation.CurrentMessage.Append(buffer.Span);
            }

            return this;
        }
    }
}