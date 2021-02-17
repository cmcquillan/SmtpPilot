using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.Internal;
using SmtpPilot.Server.IO;

namespace SmtpPilot.Server.States
{
    public class OpenConnectionState : MinimalConversationState
    {
        public override void EnterState(SmtpStateContext context)
        {
            context.Reply(new SmtpReply(SmtpReplyCode.Code220, $"{context.Configuration.HostName} Mock SMTP Server Ready"));
        }

        internal override string HandleHelp()
        {
            return Constants.HelpTextOpenState;
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

            return this;
        }
    }
}
