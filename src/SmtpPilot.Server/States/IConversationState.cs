using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Server.States
{
    public interface IConversationState
    {
        void EnterState(ISmtpStateContext context);
        bool ShouldDisconnect { get; }
        IConversationState Advance(SmtpStateContext2 context);
    }
}
