using SmtpPilot.Server.Conversation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmtpPilot.Server.IO
{
    public interface IMailClient
    {
        Guid ClientId { get; }

        void Write(string message);

        string ReadLine();

        void Disconnect();

        bool Disconnected { get; }

        bool HasData { get; }

        int SecondsClientHasBeenSilent { get; }
    }
}
