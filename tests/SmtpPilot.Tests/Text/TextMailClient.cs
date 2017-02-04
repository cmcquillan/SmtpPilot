using System;
using System.IO;
using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.IO;

namespace SmtpPilot.Tests.Text
{
    internal class TextMailClient : IMailClient, IDisposable
    {
        private StreamReader _stream;
        private bool _streamRead;

        public TextMailClient(Stream stream)
        {
            _stream = new StreamReader(stream);
        }

        public int ClientId => 1;

        public bool Disconnected => _stream.EndOfStream;

        public bool HasData => !(_stream.EndOfStream);

        public int SecondsClientHasBeenSilent => 0;

        public void Disconnect()
        {
            // Intentionally Empty
        }

        public SmtpCommand PeekCommand()
        {
            throw new NotImplementedException();
        }

        public string ReadLine()
        {
            return _stream.ReadLine() + "\r\n";
        }

        public void Write(string message)
        {
            // Intentionally Empty
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _stream?.Dispose();
            }
        }
    }
}