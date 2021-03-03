using SmtpPilot.Server.Conversation;
using System;
using System.IO;
using System.Runtime.Serialization.Json;

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
                _path = Path.Combine(Directory.GetCurrentDirectory(), path);
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
            if (message is null)
                return;

            string fileName = Path.Combine(_path, String.Format("{0}-{1}.json", DateTime.Now.ToString("yyyy-dd-MM"), Guid.NewGuid()));
            var msg = XmlMailMessage.FromMessage(message);

            using var stream = File.OpenWrite(fileName);
            _serializer.WriteObject(stream, msg);
        }
    }
}
