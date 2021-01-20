using System;
using System.IO;
using System.Threading.Tasks;
using SmtpPilot.Server.IO;

namespace SmtpPilot.Tests.Text
{
    internal class TextMailClient : IMailClient, IDisposable
    {
        private readonly StreamReader _stream;

        public TextMailClient(Stream stream)
        {
            _stream = new StreamReader(stream);
        }

        public Guid ClientId => Guid.Empty;

        public bool Disconnected => _stream.EndOfStream;

        public int SecondsClientHasBeenSilent => 0;

        public void Disconnect()
        {
            // Intentionally Empty
        }

        public async Task<string> ReadLine()
        {
            return await Task.FromResult(_stream.ReadLine() + "\r\n");
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

        public int ReadLine(Span<char> buffer)
        {
            var line = _stream.ReadLine() + "\r\n";

            for(int i = 0; i < line.Length; i++)
            {
                buffer[i] = line[i];
            }

            return line.Length;
        }
    }
}