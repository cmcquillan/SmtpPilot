using NUnit.Framework;
using System.Buffers;
using System.Linq;

namespace SmtpPilot.Tests
{
    public static class SequenceHelper
    {
        internal static SequenceChunk<byte> Chunk(byte[] bytes)
            => new SequenceChunk<byte>(bytes);

        internal static byte[] C(params char[] chars) => chars.Select(p => (byte)p).ToArray();

        public static ReadOnlySequence<byte> Seq(params byte[][] theBits)
        {
            var first = Chunk(theBits[0]);
            SequenceChunk<byte> last = first;
            foreach (var bytes in theBits.Skip(1))
            {
                last = last.Append(bytes);
            }

            return new ReadOnlySequence<byte>(first, 0, last, last.Memory.Length);
        }
    }
}
