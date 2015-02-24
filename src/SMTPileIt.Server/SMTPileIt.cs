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
using System.Threading;

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
                    c.Write(new SmtpReply(SmtpReplyCode.Code220).ToString());
                }

                foreach(var client in _clients)
                {
                    string input = client.Read();

                    if (!String.IsNullOrEmpty(input))
                    {
                        if(_conversations[client.ClientId].IsInDataState)
                        {
                            if (input.Equals(@"."))
                                client.Write(new SmtpReply(SmtpReplyCode.Code250).ToString());

                            _conversations[client.ClientId].LastElement.Append(Environment.NewLine + input);
                            
                            continue;
                        }

                        var element = ConversationElement.Parse(input);

                        _conversations[client.ClientId].AddElement(element);
                        Console.WriteLine(element.Command);

                        if (element.Command == SmtpCommand.DATA)
                        {
                            client.Write(new SmtpReply(SmtpReplyCode.Code354).ToString());
                        }
                        else
                        {
                            client.Write(new SmtpReply(SmtpReplyCode.Code250).ToString());
                        }
                    }

                    Thread.Sleep(5);
                }
            }
        }
    }
}
