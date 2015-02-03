using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace SMTPileIt.Server.IO
{
    public class TcpMailClient : IMailClient
    {
        private readonly TcpClient _tcpClient;
        private readonly int _clientId;

        public TcpMailClient(System.Net.Sockets.TcpClient tcpClient)
            : this(tcpClient, tcpClient.Client.Handle.ToInt32())
        {

        }

        public TcpMailClient(System.Net.Sockets.TcpClient client, int clientId)
        {
            _tcpClient = client;
            _clientId = clientId;
        }

        public int ClientId
        {
            get { return _clientId; }
        }


        public void Write(string message)
        {
            var s = _tcpClient.GetStream();
            using (var writer = new StreamWriter(s, Encoding.ASCII, 2048, true))
            {
                writer.WriteLine(message);
                writer.Flush();
            }
        }


        public string Read()
        {
            var s = _tcpClient.GetStream();
            using (var reader = new StreamReader(s, Encoding.ASCII, false, 2048, true))
            {
                string line = reader.ReadLine();
                Console.WriteLine(line);
                return line;
            }
        }
    }
}
