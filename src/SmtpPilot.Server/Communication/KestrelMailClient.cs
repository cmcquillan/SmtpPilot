using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.Internal;
using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Text;

namespace SmtpPilot.Server.Communication
{
    public class KestrelMailClient : IMailClient
    {
        private readonly PipeReader _reader;
        private readonly PipeWriter _writer;
        private readonly ILogger<KestrelMailClient> _logger;

        public KestrelMailClient(IDuplexPipe pipe, ILogger<KestrelMailClient> logger)
        {
            _reader = pipe.Input;
            _writer = pipe.Output;
            _logger = logger;
        }

        public KestrelMailClient(TcpClient client)
        {
            var stream = client.GetStream();
            _reader = PipeReader.Create(stream);
            _writer = PipeWriter.Create(stream);
        }

        public Guid ClientId { get; } = Guid.NewGuid();

        public int SecondsClientHasBeenSilent => 1;

        public void Disconnect()
        {
            // Maybe?
            _writer.Complete();
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
            _logger.LogTrace("Read to {byteCount} from socket", line.Length);
            return true;
        }

        public void Write(string message)
        {
            var newlines = Encoding.ASCII.GetBytes(Constants.CarriageReturnLineFeed);
            _writer.WriteAsync(Encoding.ASCII.GetBytes(message)).GetAwaiter().GetResult();
            _writer.WriteAsync(newlines).GetAwaiter().GetResult();
        }

        public int ReadLine(Span<char> outBuffer)
        {
            if (_reader.TryRead(out var read))
            {
                var inBuffer = read.Buffer;
                if (TryReadLine(ref inBuffer, out var line))
                {
                    FillLine(line, outBuffer, out var processed, out var written);
                    _reader.AdvanceTo(inBuffer.Start);
                    return written;
                }
            }

            return 0;
        }

        private void FillLine(ReadOnlySequence<byte> line, Span<char> buffer, out int processed, out int written)
        {
            var decoder = Encoding.ASCII.GetDecoder();
            var length = buffer.Length;
            processed = 0;
            written = 0;

            foreach (var bytes in line)
            {
                processed += bytes.Length;
                var last = processed == length;
                var span = bytes.Span;
                var chars = decoder.GetChars(span, buffer, last);
                buffer = buffer[(chars + 1)..];
                written += chars;
            }

        }

        private void FillCount(ReadOnlySequence<byte> read, Span<char> buffer, int count)
        {
            var decoder = Encoding.ASCII.GetDecoder();
            var num = 0;

            foreach (var segment in read.Slice(0, count))
            {
                var maximum = Math.Min(buffer.Length - num, segment.Length);
                var chars = decoder.GetChars(segment.Span[0..maximum], buffer, true);
                buffer = buffer[(chars + 1)..];
                num += chars;
            }
        }

        public bool Read(int count, Span<char> buffer)
        {
            if (_reader.TryRead(out var result))
            {
                if (result.Buffer.Length >= count)
                {
                    FillCount(result.Buffer, buffer, count);
                    _reader.AdvanceTo(result.Buffer.GetPosition(count));
                    return true;
                }
            }

            return false;
        }
    }
}
