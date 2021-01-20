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
        void LeaveState(ISmtpStateContext context);
        IConversationState ProcessData(ISmtpStateContext context, SmtpCmd cmd, ReadOnlySpan<char> line);
        SmtpCommand AllowedCommands { get; }
        bool AcceptingCommands { get; }
    }
}
