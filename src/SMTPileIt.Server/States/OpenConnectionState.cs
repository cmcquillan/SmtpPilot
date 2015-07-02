using SMTPileIt.Server.Conversation;
using SMTPileIt.Server.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPileIt.Server.States
{
    public class OpenConnectionState : IConversationState
    {
        public void EnterState(IMailClient client)
        {
            client.Write(new SmtpReply(SmtpReplyCode.Code220).GetReply());
        }

        public IConversationState Process(ISmtpStateContext context)
        {
            throw new NotImplementedException();
        }

        public void LeaveState(ISmtpStateContext context)
        {
            
        }

        public SmtpCommand AllowedCommands
        {
            get { return Conversation.SmtpCommand.EHLO | Conversation.SmtpCommand.HELO; }
        }
    }
}
