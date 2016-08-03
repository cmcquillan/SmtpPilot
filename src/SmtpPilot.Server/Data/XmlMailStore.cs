using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmtpPilot.Server.Conversation;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace SmtpPilot.Server.Data
{
    public class XmlMailStore : IMailStore
    {
        private readonly string _storagePath;
        private static readonly DataContractSerializer _serializer;

        static XmlMailStore()
        {
            _serializer = new DataContractSerializer(typeof(XmlMailMessage), new Type[] { typeof(XmlMailHeader) });
        }

        public XmlMailStore(string path = ".")
        {
            if(Path.IsPathRooted(path))
            {
                _storagePath = path;
            } 
            else
            {
                _storagePath = Path.Combine(Path.DirectorySeparatorChar.ToString(), path);
            }
        }

        public void SaveMessage(IMessage message)
        {
            string fileName = String.Format("{0}-{1}.xml", DateTime.Now.ToString("yyyy-dd-MM"), Guid.NewGuid());
            var msg = XmlMailMessage.FromMessage(message);

            using (var stream = File.OpenWrite(fileName))
            {
                _serializer.WriteObject(stream, msg);
            }
        }

        [DataContract]
        internal class XmlMailMessage
        {
            [DataMember]
            internal string[] To { get; set; }

            [DataMember]
            internal string[] Cc { get; set; }

            [DataMember]
            internal string[] Bcc { get; set; }

            [DataMember]
            internal string From { get; set; }

            [DataMember]
            internal XmlMailHeader[] Headers { get; set; }

            [DataMember]
            internal string Message { get; set; }

            internal static XmlMailMessage FromMessage(IMessage message)
            {
                return new XmlMailMessage()
                {
                    From = message.FromAddress.ToString(),
                    To = message.ToAddresses.Where(p => p. Type == AddressType.To).Select(p => p.ToString()).ToArray(),
                    Cc = message.ToAddresses.Where(p => p.Type == AddressType.Cc).Select(p => p.ToString()).ToArray(),
                    Bcc = message.ToAddresses.Where(p => p.Type == AddressType.Bcc).Select(p => p.ToString()).ToArray(),
                    Headers = message.Headers.Select(p => new XmlMailHeader() { Name = p.Name, Value = p.Value }).ToArray(),
                    Message = message.Data,
                };
            }
        }

        [DataContract]
        internal class XmlMailHeader
        {
            [DataMember]
            internal string Name { get; set; }

            [DataMember]
            internal string Value { get; set; }
        }
    }
}
