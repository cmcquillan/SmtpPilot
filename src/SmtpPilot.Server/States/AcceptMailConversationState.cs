using Microsoft.Extensions.DependencyInjection;
using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.Internal;
using SmtpPilot.Server.IO;
using System;

namespace SmtpPilot.Server.States
{
    public class AcceptMailConversationState : MinimalConversationState
    {
        public override void EnterState(SmtpStateContext context)
        {
            context.ContextBuilder.ResetState();
        }

        public override IConversationState Advance(SmtpStateContext context)
        {
            var temp = context.ContextBuilder.GetTemporaryBuffer().Slice(0, 4);

            if (!context.Client.Read(4, temp))
            {
                return this;
            }

            var command = IOHelper.GetCommand(temp);
            var buffer = context.ContextBuilder.GetBuffer(1024);

            if (context.Client.ReadUntil(Markers.CarriageReturnLineFeed, buffer, 0, out var count))
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
                        return this;
                    default:
                        return ProcessBaseCommands(command, context);
                }

            }

            return this;
        }

        internal override string HandleHelp()
        {
            return Constants.HelpTextAcceptState;
        }
    }
}
