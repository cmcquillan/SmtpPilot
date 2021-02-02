using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Server.States
{
    public class ErrorConversationState : IConversationState
    {
        private readonly string _errorMessage;

        public ErrorConversationState()
            :this("You have performed an illegal operation.")
        {

        }

        public ErrorConversationState(string errorMessage)
        {
            _errorMessage = errorMessage;
        }

        public void EnterState(ISmtpStateContext context)
        {
            context.Statistics.AddErrorGenerated();
            context.Reply(new Conversation.SmtpReply(Conversation.SmtpReplyCode.Code503, _errorMessage));
        }

        public void LeaveState(ISmtpStateContext context)
        {
            throw new NotImplementedException();
        }

        public IConversationState ProcessData(ISmtpStateContext context, Conversation.SmtpCmd cmd, ReadOnlySpan<char> line)
        {
            throw new NotImplementedException();
        }

        public Conversation.SmtpCommand AllowedCommands
        {
            get { return Conversation.SmtpCommand.None; }
        }

        public bool AcceptingCommands => false;
    }
}
