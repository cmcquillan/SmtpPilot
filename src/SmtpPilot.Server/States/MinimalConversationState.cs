using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.IO;
using System;

namespace SmtpPilot.Server.States
{
    internal abstract class MinimalConversationState : IConversationState
    {
        public bool ShouldDisconnect => true;

        public abstract ConversationStateKey ThisKey { get; }

        public abstract ConversationStateKey Advance(SmtpStateContext context);

        public ConversationStateKey ProcessBaseCommands(SmtpCommand smtpCommand, ReadOnlySpan<char> buffer, SmtpStateContext context)
        {
            switch (smtpCommand)
            {
                case SmtpCommand.NOOP:
                    context.Reply(SmtpReply.OK);
                    return ThisKey;
                case SmtpCommand.RSET:
                    if (buffer.IsEmptyOrWhitespace())
                    {
                        context.ContextBuilder.ResetState();
                        context.Reply(SmtpReply.OK);
                        return ConversationStates.Accept;
                    }
                    else
                    {
                        context.Reply(SmtpReply.SyntaxError);
                        return ThisKey;
                    }
                case SmtpCommand.QUIT:
                    if (buffer.IsEmptyOrWhitespace())
                    {
                        return ConversationStates.Quit;
                    }
                    else
                    {
                        context.Reply(SmtpReply.SyntaxError);
                        return ThisKey;
                    }
                case SmtpCommand.HELP:
                    context.Reply(new SmtpReply(SmtpReplyCode.Code250, HandleHelp()));
                    return ThisKey;
                default:
                    return ConversationStates.Error;
            }
        }

        public virtual void EnterState(SmtpStateContext context)
        {

        }

        internal abstract string HandleHelp();
    }
}
