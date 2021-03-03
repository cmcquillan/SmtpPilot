using SmtpPilot.Server.Conversation;
using System;
using System.IO;
using System.Runtime.Serialization;

namespace SmtpPilot.Server.Data
{
    public class XmlMailStore : IMailStore
    {
        private readonly string _storagePath;
        private static readonly DataContractSerializer _serializer;

        static XmlMailStore()
        {
            _serializer = new DataContractSerializer(typeof(XmlMailMessage), new Type[] { typeof(XmlMailHeader), typeof(XmlAddressList) });
        }

        public XmlMailStore(string path = ".")
        {
            if (Path.IsPathRooted(path))
            {
                _storagePath = path;
            }
            else
            {
                _storagePath = Path.Combine(Directory.GetCurrentDirectory(), path);
            }
        }

        public void SaveMessage(IMessage message)
        {
            if (message is null)
                return;

            string fileName = Path.Combine(_storagePath, String.Format("{0}-{1}.xml", DateTime.Now.ToString("yyyy-dd-MM"), Guid.NewGuid()));
            var msg = XmlMailMessage.FromMessage(message);

            using var stream = File.OpenWrite(fileName);
            _serializer.WriteObject(stream, msg);
        }
    }
}
