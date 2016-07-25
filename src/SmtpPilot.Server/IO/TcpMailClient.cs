using SmtpPilot.Server.Conversation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SmtpPilot.Server.IO
{
    public class TcpMailClient : IMailClient, IDisposable
    {
        private const int START_BUFFER_SIZE = 2048;
        private readonly TcpClient _tcpClient;
        private readonly int _clientId;
        private readonly NetworkStream _inputStream;
        private readonly StreamReader _reader;
        private char[] _buffer = new char[START_BUFFER_SIZE];
        private int _bufferMultiple = 1;
        private int _bufferReadPosition = 0;
        private int _bufferDataPosition = 0;

        public TcpMailClient(TcpClient tcpClient)
            : this(tcpClient, tcpClient.Client.Handle.ToInt32())
        {

        }

        public TcpMailClient(TcpClient client, int clientId)
        {
            _tcpClient = client;
            _clientId = clientId;
            _inputStream = _tcpClient.GetStream();
            _reader = new StreamReader(_tcpClient.GetStream());
        }

        public int ClientId
        {
            get { return _clientId; }
        }

        public void Write(string message)
        {
            var s = _tcpClient.GetStream();
            using (var writer = new StreamWriter(s, Encoding.ASCII, START_BUFFER_SIZE, true))
            {
                writer.WriteLine(message);
                writer.Flush();
            }
        }

        public string ReadLine()
        {
            if (_inputStream.DataAvailable)
                ReadToBuffer();

            if (_bufferDataPosition > _bufferReadPosition)
            {
                string s = IOHelper.GetLineFromBuffer(_buffer, _bufferReadPosition, _bufferDataPosition - _bufferReadPosition);
                _bufferReadPosition += s.Length;
                return s;
            }

            return null;
        }

        private void ReadToBuffer()
        {
            while (_inputStream.DataAvailable)
            {
                int spaceLeftInBuffer = _buffer.Length - _bufferDataPosition;

                int dataRead = _reader.Read(_buffer, _bufferDataPosition, spaceLeftInBuffer);
                _bufferDataPosition += dataRead;

                if (dataRead == spaceLeftInBuffer)
                {
                    AdjustBuffer();
                }
            }
        }

        private void AdjustBuffer()
        {
            if (_bufferReadPosition > 0)
            {
                ReallocateBuffer();
                return;
            }

            _bufferMultiple += 1;
            ReallocateBuffer();
        }

        private void ReallocateBuffer()
        {
            byte[] newBuf = new byte[START_BUFFER_SIZE * _bufferMultiple];
            Array.Copy(_buffer, _bufferReadPosition, newBuf, 0, _bufferDataPosition - _bufferReadPosition);
            _bufferDataPosition = _bufferDataPosition - _bufferReadPosition;
            _bufferReadPosition = 0;
        }

        public void Disconnect()
        {
            _inputStream.Close();
            _tcpClient.Close();
        }


        public bool Disconnected
        {
            get { return !_tcpClient.Connected; }
        }


        public SmtpCommand PeekCommand()
        {
            ReadToBuffer();

            if ((_bufferDataPosition - _bufferReadPosition) < 4)
                return SmtpCommand.NonCommand;

            char[] cmdText = new char[4];

            for (int i = 0; i < cmdText.Length; i++)
            {
                cmdText[i] = _buffer[_bufferReadPosition + i];
            }

            return IOHelper.GetCommand(cmdText);
        }

        public bool HasData
        {
            get
            {
                ReadToBuffer();
                return BufferHasNewLine();
            }
        }

        private bool BufferHasNewLine()
        {
            char[] newLine = Environment.NewLine.ToCharArray();
            int lengthEqual = 0;

            for(int i = _bufferReadPosition; i < _bufferDataPosition; i++)
            {
                if (_buffer[i] == newLine[lengthEqual])
                    lengthEqual++;
                else
                    lengthEqual = 0;

                if (lengthEqual == newLine.Length)
                    return true;
            }

            return false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                _inputStream.Dispose();
                _reader.Dispose();
                _tcpClient.Close();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }

        ~TcpMailClient()
        {
            Dispose(false);
        }
    }
}
