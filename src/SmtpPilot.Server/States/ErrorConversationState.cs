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
            : this("You have performed an illegal operation.")
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

        public IConversationState Advance(SmtpStateContext2 context)
        {
            return this;
        }

        public bool ShouldDisconnect { get; }
    }
}
