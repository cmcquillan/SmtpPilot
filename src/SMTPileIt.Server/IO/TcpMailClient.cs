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
        private const int BUFFER_SIZE = 2048;
        private readonly TcpClient _tcpClient;
        private readonly int _clientId;
        private readonly StreamReader _reader;

        public TcpMailClient(System.Net.Sockets.TcpClient tcpClient)
            : this(tcpClient, tcpClient.Client.Handle.ToInt32())
        {

        }

        public TcpMailClient(System.Net.Sockets.TcpClient client, int clientId)
        {
            _tcpClient = client;
            _clientId = clientId;
            _reader = new StreamReader(_tcpClient.GetStream());
        }

        public int ClientId
        {
            get { return _clientId; }
        }

        public void Write(string message)
        {
            var s = _tcpClient.GetStream();
            using (var writer = new StreamWriter(s, Encoding.ASCII, BUFFER_SIZE, true))
            {
                writer.WriteLine(message);
                writer.Flush();
            }
        }

        public string Read()
        {
            if (_reader.EndOfStream)
                return null;

            string line = _reader.ReadLine();
            Console.WriteLine(line);
            return line;
        }


        public void Disconnect()
        {
            _reader.Dispose();
            _tcpClient.Close();
        }


        public bool Disconnected
        {
            get { return !_tcpClient.Connected; }
        }
    }
}
