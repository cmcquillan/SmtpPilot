using SMTPileIt.Server.Conversation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SMTPileIt.Server.IO;

namespace SMTPileIt.Server
{
    public class SMTPileIt
    {
        public const int DefaultSmtpPort = 25;

        private readonly IMailClientListener _listener;
        private readonly List<IMailClient> _clients;
        private readonly Dictionary<int, SmtpConversation> _conversations;


        public SMTPileIt(string ipString, int ipPort)
        {
            _listener = new TcpClientListener(ipString, ipPort);

            _clients = new List<IMailClient>();
            _conversations = new Dictionary<int, SmtpConversation>();
        }

        public void Run()
        {
            while(true)
            {
                if (_listener.ClientPending)
                {
                    var c = _listener.AcceptClient();
                    _clients.Add(c);
                    _conversations[c.ClientId] = new SmtpConversation();
                    c.Write("220 ");
                }

                foreach(var client in _clients)
                {
                    string input = client.Read();
                    if(!String.IsNullOrEmpty(input))
                    {
                        var element = ConversationElement.Parse(input);

                        _conversations[client.ClientId].AddElement(element);

                        Console.WriteLine(element.Command);

                        client.Write("250 OK");
                    }
                }
            }
        }
    }
}
