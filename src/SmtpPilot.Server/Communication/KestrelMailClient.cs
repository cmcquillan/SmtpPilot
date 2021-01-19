using Microsoft.AspNetCore.Connections;
using SmtpPilot.Server.IO;
using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Server.Communication
{
    public class KestrelMailClient : IMailClient
    {
        private readonly StringBuilder _builder = new StringBuilder();
        private readonly ConnectionContext _context;
        private bool _closed;
        private readonly PipeReader _reader;
        private readonly PipeWriter _writer;

        public KestrelMailClient(ConnectionContext context)
        {
            _context = context;
            _context.ConnectionClosed.Register(OnClosed);
            _reader = _context.Transport.Input;
            _writer = _context.Transport.Output;
        }

        private void OnClosed()
        {
            _closed = true;
        }

        public Guid ClientId { get; } = Guid.NewGuid();
        public bool Disconnected => _closed;
        public int SecondsClientHasBeenSilent => 1;

        public void Disconnect()
        {
            // Maybe?
            _writer.Complete();
        }

        public async Task<string> ReadLine()
        {
            var read = await _reader.ReadAsync();
            var buffer = read.Buffer;

            if (TryReadLine(ref buffer, out ReadOnlySequence<byte> line))
            {
                var ret = GetLine(line);
                _reader.AdvanceTo(buffer.Start);
                return ret;
            }

            return null;
        }

        private string GetLine(ReadOnlySequence<byte> line)
        {
            var decoder = Encoding.ASCII.GetDecoder();
            var builder = new StringBuilder();
            var length = line.Length;
            var processed = 0;

            foreach (var bytes in line)
            {
                processed += bytes.Length;
                var last = processed == length;
                var span = bytes.Span;
                var count = decoder.GetCharCount(span, last);
                Span<char> buffer = stackalloc char[count];
                decoder.GetChars(span, buffer, last);
                builder.Append(buffer);
            }

            return builder.ToString();
        }

        private bool TryReadLine(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> line)
        {
            var position = buffer.PositionOf((byte)'\n');

            if (position == null)
            {
                line = default;
                return false;
            }

            var slicePosition = buffer.GetPosition(1, position.Value);

            line = buffer.Slice(0, slicePosition);
            buffer = buffer.Slice(slicePosition);
            return true;
        }

        public void Write(string message)
        {
            var newlines = Encoding.ASCII.GetBytes(Environment.NewLine);

            var flush = _writer.WriteAsync(Encoding.ASCII.GetBytes(message)).GetAwaiter().GetResult();
            _writer.WriteAsync(newlines).GetAwaiter().GetResult();
        }
    }
}
