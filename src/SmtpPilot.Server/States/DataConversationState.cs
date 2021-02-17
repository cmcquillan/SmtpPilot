using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.Internal;
using System;
using System.Buffers;

namespace SmtpPilot.Server.States
{
    internal class DataConversationState : IConversationState
    {
        public bool ShouldDisconnect => false;

        public void EnterState(SmtpStateContext context)
        {
            context.Reply(SmtpReply.BeginData);
        }

        public ConversationStateKey Advance(SmtpStateContext context)
        {
            bool endOfData = false;
            bool dataHasPeriod = false;
            var buffer = context.ContextBuilder.GetBuffer(2048);

            if (!context.Client.ReadUntil(Markers.CarriageReturnLineFeed, buffer, 0, out var count))
            {
                return ConversationStates.DataRead;
            }

            if (buffer[0] == '.')
            {
                if (count > 1)
                {
                    dataHasPeriod = true;
                }
                else
                {
                    endOfData = true;
                }
            }

            context.ContextBuilder.AddDataSegment(dataHasPeriod ? 1 : 0, count);

            if (endOfData)
            {
                context.Events.OnEmailProcessed(this,
                    new EmailProcessedEventArgs(
                        context.Client,
                        context.ContextBuilder.BuildMessage(),
                        context.EmailStats));

                context.EmailStats.AddEmailReceived();

                context.Reply(SmtpReply.OK);

                return ConversationStates.Accept;
            }

            return ConversationStates.DataRead;
        }
    }
}