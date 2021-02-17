using SmtpPilot.Server.Conversation;

namespace SmtpPilot.Server.States
{
    public abstract class MinimalConversationState : IConversationState
    {
        public bool ShouldDisconnect => true;

        public abstract IConversationState Advance(SmtpStateContext context);

        public IConversationState ProcessBaseCommands(SmtpCommand smtpCommand, SmtpStateContext context)
        {
            switch (smtpCommand)
            {
                case SmtpCommand.NOOP:
                    context.Reply(SmtpReply.OK);
                    return this;
                case SmtpCommand.RSET:
                    context.ContextBuilder.ResetState();
                    context.Reply(SmtpReply.OK);
                    return ConversationStates.Accept;
                case SmtpCommand.QUIT:
                    return ConversationStates.Quit;
                case SmtpCommand.HELP:
                    context.Reply(new SmtpReply(SmtpReplyCode.Code250, HandleHelp()));
                    return this;
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
