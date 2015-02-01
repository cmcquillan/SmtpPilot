using SMTPileIt.Server.Conversation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SMTPileIt.Server
{
    public class SMTPileIt
    {
        public const int DefaultSmtpPort = 25;

        private readonly TcpListener _listener;
        private readonly List<TcpClient> _clients;
        private readonly Dictionary<int, SmtpConversation> _conversations;


        public SMTPileIt(string ipString, int ipPort)
        {
            IPAddress addr = IPAddress.Parse(ipString);
            _listener = new TcpListener(addr, ipPort);
            _clients = new List<TcpClient>();
            _conversations = new Dictionary<int, SmtpConversation>();
        }

        public void Run()
        {
            _listener.Start();

            while(true)
            {
                if (_listener.Pending())
                {
                    var c = _listener.AcceptTcpClient();
                    _clients.Add(c);
                    _conversations[c.Client.Handle.ToInt32()] = new SmtpConversation();
                    Write(c.GetStream(), "220 ");
                }

                foreach(var client in _clients)
                {
                    if(client.Available > 0)
                    {
                        var stream = client.GetStream();

                        string line = Read(stream);

                        var element = ConversationElement.Parse(line);

                        _conversations[client.Client.Handle.ToInt32()].AddElement(element);

                        Console.WriteLine(element.Command);

                        Write(stream, "250 OK");
                    }
                }
            }
        }

        public string Read(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.ASCII, false, 2048, true))
            {
                string line = reader.ReadLine();
                Console.WriteLine(line);
                return line;
            }
        }

        public void Write(Stream s, string message)
        {
            using(var writer = new StreamWriter(s, Encoding.ASCII, 2048, true))
            {
                writer.WriteLine(message);
                writer.Flush();
            }
        }
    }
}
