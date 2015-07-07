using SMTPileIt.Server.Conversation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMTPileIt.Server.States
{
    public class AcceptMailConversationState : IConversationState
    {

        public IConversationState ProcessData(ISmtpStateContext context, string line)
        {
            return this;
        }

        public void LeaveState(ISmtpStateContext context)
        {
            
        }

        public Conversation.SmtpCommand AllowedCommands
        {
            get { return Conversation.SmtpCommand.MAIL; }
        }


        public void EnterState(ISmtpStateContext context)
        {
            
        }

        public IConversationState ProcessNewCommand(ISmtpStateContext context, SmtpCmd cmd, string line)
        {
            throw new NotImplementedException();
        }
    }
}
