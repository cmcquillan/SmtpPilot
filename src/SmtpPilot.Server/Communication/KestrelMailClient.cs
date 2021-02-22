using Microsoft.Extensions.Logging;
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
        private readonly Decoder _decoder = Encoding.ASCII.GetDecoder();
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
                    FillLine(line, outBuffer, out _, out var written);
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
                var amountToRead = Math.Min(buffer.Length, segment.Length);
                var chars = decoder.GetChars(segment.Span[0..amountToRead], buffer, true);

                num += chars;

                if (num >= count)
                    return;

                buffer = buffer[chars..];
            }
        }

        public bool ReadUntil(byte[] marker, Span<char> buffer, int readOffset, out int count)
        {
            count = 0;
            if (_reader.TryRead(out var result))
            {
                var sr = new SequenceReader<byte>(result.Buffer);

                if (readOffset > 0)
                    sr.Advance(readOffset);

                if (sr.TryReadTo(out var newSequence, marker, true))
                {
                    if (newSequence.Length <= buffer.Length)
                    {
                        count = DecodeAndConsume(ref newSequence, buffer);
                        _reader.AdvanceTo(sr.Position);

                        return true;
                    }
                }

                _reader.AdvanceTo(result.Buffer.Start);
            }

            return false;
        }

        private int DecodeAndConsume(ref ReadOnlySequence<byte> newSequence, Span<char> buffer)
        {
            var decoder = Encoding.ASCII.GetDecoder();
            var length = newSequence.Length;
            var processed = 0;

            foreach (var item in newSequence)
            {
                processed += item.Length;
                var chars = _decoder.GetChars(item.Span, buffer, processed == length);
                buffer = buffer[(chars + 1)..];
            }

            return processed;
        }

        public bool Read(int count, Span<char> buffer)
        {
            return ReadInternal(count, buffer, true);
        }

        private bool ReadInternal(int count, Span<char> buffer, bool advance)
        {
            if (_reader.TryRead(out var result))
            {
                if (result.Buffer.Length >= count)
                {
                    FillCount(result.Buffer, buffer, count);
                    _reader.AdvanceTo(advance
                        ? result.Buffer.GetPosition(count)
                        : result.Buffer.Start);
                    return true;
                }

                _reader.AdvanceTo(result.Buffer.Start);
            }

            return false;
        }

        public bool Peek(int count, Span<char> buffer)
        {
            return ReadInternal(count, buffer, false);
        }
    }
}
