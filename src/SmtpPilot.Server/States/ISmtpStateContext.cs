using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmtpPilot.Server.States
{
    public interface ISmtpStateContext
    {
        SmtpPilotConfiguration Configuration { get; }
        EmailStatistics Statistics { get; }
        IMailClient Client { get; }
        SmtpConversation Conversation { get; }
        SmtpCommand Command { get; }
        bool HasError { get; }

        void NewMessage();
        void AddHeader(SmtpHeader header);
        void Reply(SmtpReply reply);
        void SetFrom(string from);
        void AddTo(string[] emails);
        void AddCc(string[] emails);
        void AddBcc(string[] emails);
        void CompleteMessage();
    }
}
