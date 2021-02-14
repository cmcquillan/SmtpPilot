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

        }

        public override IConversationState Advance(SmtpStateContext context)
        {
            var buffer = context.GetBufferSegment(1024);
            if (context.Client.ReadUntil(Markers.CarriageReturnLineFeed, buffer.Span, 0, out var count))
            {
                var command = IOHelper.GetCommand(buffer.Span[0..4].ToArray());
                var line = buffer[5..].ToString();
                context.AdvanceBuffer(count);

                switch (command)
                {
                    case SmtpCommand.MAIL:
                        string[] matches = IOHelper.ParseEmails(line);
                        if (matches.Length != 1)
                            return ConversationStates.Error;

                        string from = matches[0];

                        var messageFactory = context.ServiceProvider.GetRequiredService<IEmailMessageFactory>();
                        context.Conversation.NewMessage(messageFactory.CreateNewMessage());

                        context.Conversation.CurrentMessage.FromAddress = new EmailAddress(from, AddressType.From);
                        context.Reply(SmtpReply.OK);
                        return ConversationStates.Recipient;
                    case SmtpCommand.VRFY:
                        context.Reply(new SmtpReply(SmtpReplyCode.Code250, String.Format("{0} <{0}@{1}>", line, context.Configuration.HostName)));
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
