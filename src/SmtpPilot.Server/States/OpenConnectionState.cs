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
            var line = context.GetBufferSegment(1024);
            if (context.Client.ReadUntil(Markers.CarriageReturnLineFeed, line.Span, 0, out var count))
            {
                context.AdvanceBuffer(count);
                var command = IOHelper.GetCommand(line.Slice(0,4).ToArray());
                context.Conversation.AddElement(new SmtpCmd(command, line.ToString()));
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

            return this;
        }
    }
}
