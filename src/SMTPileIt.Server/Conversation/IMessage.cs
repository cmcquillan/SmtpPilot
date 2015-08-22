using System.Collections.Generic;

namespace SMTPileIt.Server.Conversation
{
    public interface IMessage
    {
        bool IsComplete { get; }
        string Data { get; }
        string DataString { get; }
        IAddress FromAddress { get; set; }
        IReadOnlyCollection<SmtpHeader> Headers { get; }
        IReadOnlyCollection<IAddress> ToAddresses { get; }

        void AddAddresses(IAddress[] email);
        void AddHeader(SmtpHeader header);
        void AppendLine(string line);
        void Complete();
    }
}