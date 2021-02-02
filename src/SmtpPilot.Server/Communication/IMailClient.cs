using SmtpPilot.Server.Conversation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Server.Communication
{
    public interface IMailClient
    {
        Guid ClientId { get; }

        void Write(string message);

        void Disconnect();

        int SecondsClientHasBeenSilent { get; }

        int ReadLine(Span<char> buffer);
    }
}
