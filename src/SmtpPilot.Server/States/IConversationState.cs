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
        void EnterState(SmtpStateContext context);
        bool ShouldDisconnect { get; }
        IConversationState Advance(SmtpStateContext context);
    }
}
