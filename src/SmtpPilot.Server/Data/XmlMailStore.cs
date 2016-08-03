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
    }
}
