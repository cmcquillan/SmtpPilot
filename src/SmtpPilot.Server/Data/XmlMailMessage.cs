using SmtpPilot.Server.Conversation;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace SmtpPilot.Server.Data
{
    [DataContract(Namespace = XmlMailMessage.Namespace)]
    internal class XmlMailMessage
    {
        internal const string Namespace = "https://quilltechnologies.com/Contracts/SmtpPilot/02/2017/XmlMail";

        [DataMember(Order = 0)]
        internal string From { get; set; }

        [DataMember(Order = 1)]
        internal XmlAddressList To { get; set; }

        [DataMember(Order = 2)]
        internal XmlAddressList Cc { get; set; }

        [DataMember(Order = 3)]
        internal XmlAddressList Bcc { get; set; }

        [DataMember(Order = 4)]
        internal XmlMailHeader[] Headers { get; set; }

        [DataMember(Order = 5)]
        internal string Message { get; set; }

        internal static XmlMailMessage FromMessage(IMessage message)
        {
            using var rdr = new StreamReader(message.MessageBody);
            return new XmlMailMessage()
            {
                From = message.FromAddress.ToString(),
                To = new XmlAddressList(message.ToAddresses.Where(p => p.Type == AddressType.To).Select(p => p.ToString()).ToList()),
                Cc = new XmlAddressList(message.ToAddresses.Where(p => p.Type == AddressType.Cc).Select(p => p.ToString()).ToList()),
                Bcc = new XmlAddressList(message.ToAddresses.Where(p => p.Type == AddressType.Bcc).Select(p => p.ToString()).ToList()),
                Headers = message.Headers.Select(p => new XmlMailHeader() { Name = p.Name, Value = p.Value }).ToArray(),
                Message = rdr.ReadToEnd(),
            };
        }
    }
}
