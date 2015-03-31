using SMTPileIt.Server.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMTPileIt.Server.States
{
    public class QuitState : IConversationState
    {
        public void EnterState(IMailClient client)
        {
            throw new NotImplementedException();
        }

        public IConversationState Process(ISmtpStateContext context)
        {
            throw new NotImplementedException();
        }

        public void LeaveState(ISmtpStateContext context)
        {
            throw new NotImplementedException();
        }

        public Conversation.SmtpCommand AllowedCommands
        {
            get { throw new NotImplementedException(); }
        }
    }
}
