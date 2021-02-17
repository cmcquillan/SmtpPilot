using System;
using System.Collections.ObjectModel;
using System.IO;

namespace SmtpPilot.Server.Conversation
{
    public interface IMessage
    {
        Stream MessageBody { get; }
        EmailAddress FromAddress { get; }
        ReadOnlyCollection<SmtpHeader> Headers { get; }
        ReadOnlyCollection<EmailAddress> ToAddresses { get; }
    }
}