using SMTPileIt.Server.Conversation;
using SMTPileIt.Server.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPileIt.Server.States
{
    public interface IConversationState
    {
        void EnterState(IMailClient client);

        IConversationState Process(ISmtpStateContext context);

        void LeaveState(ISmtpStateContext context);

        SmtpCommand AllowedCommands { get; }
    }
}
