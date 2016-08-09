using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SmtpPilot.Server.Conversation
{
    public interface IMessage
    {
        bool IsComplete { get; }
        string Data { get; }
        string DataString { get; }
        IAddress FromAddress { get; set; }
        ReadOnlyCollection<SmtpHeader> Headers { get; }
        ReadOnlyCollection<IAddress> ToAddresses { get; }

        void AddAddresses(IAddress[] email);
        void AddHeader(SmtpHeader header);
        void AppendLine(string line);
        void Complete();
    }
}