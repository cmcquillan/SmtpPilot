using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmtpPilot.Server.Conversation;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;

namespace SmtpPilot.Server.Data
{
    public class JsonMailStore : IMailStore
    {
        private readonly string _path;
        private readonly DataContractJsonSerializer _serializer;

        public JsonMailStore(string path = ".")
        {
            if (Path.IsPathRooted(path))
            {
                _path = path;
            }
            else
            {
                _path = Path.Combine(Environment.CurrentDirectory, path);
            }

            _serializer = new DataContractJsonSerializer(typeof(XmlMailMessage), new DataContractJsonSerializerSettings
            {
                KnownTypes = new[]
                {
                    typeof(XmlMailHeader),
                    typeof(XmlAddressList)
                },
            });
        }

        public void SaveMessage(IMessage message)
        {
            string fileName = Path.Combine(_path, String.Format("{0}-{1}.json", DateTime.Now.ToString("yyyy-dd-MM"), Guid.NewGuid()));
            var msg = XmlMailMessage.FromMessage(message);

            using (var stream = File.OpenWrite(fileName))
            {
                _serializer.WriteObject(stream, msg);
            }
        }
    }
}
