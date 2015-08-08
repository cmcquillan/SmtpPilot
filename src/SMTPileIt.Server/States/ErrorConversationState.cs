using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPileIt.Server.States
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
            context.Reply(new Conversation.SmtpReply(Conversation.SmtpReplyCode.Code503, _errorMessage));
        }

        public void LeaveState(ISmtpStateContext context)
        {
            throw new NotImplementedException();
        }

        public IConversationState ProcessNewCommand(ISmtpStateContext context, Conversation.SmtpCmd cmd, string line)
        {
            throw new NotImplementedException();
        }

        public IConversationState ProcessData(ISmtpStateContext context, string line)
        {
            throw new NotImplementedException();
        }

        public Conversation.SmtpCommand AllowedCommands
        {
            get { return Conversation.SmtpCommand.NonCommand; }
        }
    }
}
