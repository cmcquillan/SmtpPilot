using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.Internal;
using SmtpPilot.Server.IO;

namespace SmtpPilot.Server.States
{
    internal class OpenConnectionState : MinimalConversationState
    {
        public override ConversationStateKey ThisKey => ConversationStates.OpenConnection;

        public override void EnterState(SmtpStateContext context)
        {
            context.Reply(new SmtpReply(SmtpReplyCode.Code220, $"{context.Configuration.HostName} Mock SMTP Server Ready"));
        }

        internal override string HandleHelp()
        {
            return Constants.HelpTextOpenState;
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
                try
                {
                    if (command == SmtpCommand.HELO)
                    {
                        context.Events.OnClientConnected(this, new MailClientConnectedEventArgs(context.Client));
                        context.Reply(new SmtpReply(SmtpReplyCode.Code250, context.Configuration.HostName));

                        return ConversationStates.Accept;
                    }
                    else
                    {
                        return ProcessBaseCommands(command, context);
                    }
                }
                finally
                {
                    // Discard the rest
                    buffer.Slice(0, count).Clear();
                }
            }

            return ThisKey;
        }
    }
}
