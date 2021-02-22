using Microsoft.Extensions.DependencyInjection;
using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.Internal;
using SmtpPilot.Server.IO;
using System;

namespace SmtpPilot.Server.States
{
    internal class AcceptMailConversationState : MinimalConversationState
    {
        public override ConversationStateKey ThisKey => ConversationStates.Accept;

        public override void EnterState(SmtpStateContext context)
        {
            context.ContextBuilder.ResetState();
        }

        public override ConversationStateKey Advance(SmtpStateContext context)
        {
            var temp = context.ContextBuilder.GetTemporaryBuffer().Slice(0, 4);

            if (!context.Client.Peek(4, temp))
            {
                return ConversationStates.Accept;
            }

            var command = IOHelper.GetCommand(temp);
            var buffer = context.ContextBuilder.GetBuffer(1024);

            if (context.Client.ReadUntil(Markers.CarriageReturnLineFeed, buffer, 4, out var count))
            {
                switch (command)
                {
                    case SmtpCommand.MAIL:

                        // Look for the ':' in MAIL FROM:
                        var start = buffer.IndexOf(':') + 1;
                        context.ContextBuilder.StartMessage(start, count - start);
                        context.Reply(SmtpReply.OK);

                        return ConversationStates.Recipient;
                    case SmtpCommand.VRFY:

                        context.Reply(new SmtpReply(SmtpReplyCode.Code250, String.Format("{0} <{0}@{1}>", "sample", context.Configuration.HostName)));

                        return ConversationStates.Accept;
                    default:
                        return ProcessBaseCommands(command, context);
                }

            }

            return ConversationStates.Accept;
        }

        internal override string HandleHelp()
        {
            return Constants.HelpTextAcceptState;
        }
    }
}
