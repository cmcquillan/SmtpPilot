using System;
using System.Collections.ObjectModel;
using System.IO;

namespace SmtpPilot.Server.Conversation
{
    public interface IMessage
    {
        Stream MessageBody { get; }
        Mailbox FromAddress { get; }
        ReadOnlyCollection<SmtpHeader> Headers { get; }
        ReadOnlyCollection<Mailbox> Recipients { get; }
    }
}