using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.States;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SmtpPilot.Server.IO
{
    public class TcpMailClient : IMailClient, IDisposable
    {
        private const byte _cr = 0x0D;
        private const byte _lf = 0x0A;

        private const int BUFFER_SIZE = 2048;
        private DateTimeOffset _lastDataAvailable;
        private TcpClient _tcpClient;
        private NetworkStream _inputStream;
        private readonly int _clientId;
        private byte[] _buffer;
        private int _bufferPosition = 0;
        private int _scanPosition = 0;
        private int _readPosition = 0;

        public TcpMailClient(TcpClient tcpClient)
            : this(tcpClient, tcpClient.Client.Handle.ToInt32())
        {

        }

        public TcpMailClient(TcpClient client, int clientId)
        {
            _tcpClient = client;
            _clientId = clientId;
            _inputStream = _tcpClient.GetStream();
            _lastDataAvailable = DateTimeOffset.UtcNow;
            _buffer = new byte[BUFFER_SIZE];
        }

        public int ClientId => _clientId;

        public bool Disconnected => !(_tcpClient.Connected);

        public bool HasData => _inputStream.DataAvailable;

        public int SecondsClientHasBeenSilent => (int)(DateTimeOffset.UtcNow - _lastDataAvailable).TotalSeconds;

        public void Write(string message)
        {
            Debug.WriteLine($"Writing to transmission channel: {message}");
            if (_tcpClient.Connected)
            {
                try
                {
                    byte[] messageBytes = Encoding.ASCII.GetBytes(message);
                    _inputStream.Write(messageBytes, 0, messageBytes.Length);
                    _inputStream.Write(new[] { _cr, _lf }, 0, 2);
                    _inputStream.Flush();
                }
                catch (IOException)
                {
                    Debug.WriteLine("Client preemptively closed transmission channel.", TraceConstants.IO);
                }
            }
        }

        public string ReadLine()
        {
            bool foundLine = false;

            while (_inputStream.DataAvailable)
            {
                // Check remaining space and enlarge buffer if necessary.
                int remainingSpace = _buffer.Length - _bufferPosition;

                if (remainingSpace == 0)
                {
                    Debug.WriteLine("Performing buffer resize.", TraceConstants.IO);
                    Array.Resize(ref _buffer, _buffer.Length * 2);
                    continue;
                }

                // Fill the buffer.
                _bufferPosition += _inputStream.Read(_buffer, _bufferPosition, remainingSpace);
            }

            // Scan for a newline
            for (; _scanPosition < _buffer.Length; _scanPosition += 1)
            {
                if (_buffer.Length >= _scanPosition + 1
                    && _buffer[_scanPosition] == _cr
                    && _buffer[_scanPosition + 1] == _lf)
                {
                    foundLine = true;
                    _scanPosition += 1;
                    break;
                }
            }

            if (foundLine)
            {
                int characterCount = _scanPosition - _readPosition + 1;

                var str = Encoding.ASCII.GetString(_buffer, _readPosition, characterCount);
                _readPosition += characterCount;

                Debug.WriteLine($"Receiving from transmission channel: {str}");
                return str;
            }

            return null;
        }

        public SmtpCommand PeekCommand()
        {
            SmtpCommand cmd = SmtpCommand.NonCommand;
            Enum.TryParse(Encoding.ASCII.GetString(_buffer, _bufferPosition, 4), out cmd);
            return cmd;
        }

        public void Disconnect()
        {
            _tcpClient?.Close();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Disconnect();
                _inputStream?.Dispose();
                _inputStream = null;
                _tcpClient = null;
            }
        }
    }
}
