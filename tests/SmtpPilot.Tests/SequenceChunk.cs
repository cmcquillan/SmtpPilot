using System;
using System.Buffers;

namespace SmtpPilot.Tests
{
    internal class SequenceChunk<T> : ReadOnlySequenceSegment<T>
    {
        public SequenceChunk(ReadOnlyMemory<T> memory)
        {
            Memory = memory;
        }

        public SequenceChunk<T> Append(ReadOnlyMemory<T> memory)
        {
            var chunk = new SequenceChunk<T>(memory)
            {
                RunningIndex = RunningIndex + Memory.Length
            };

            Next = chunk;

            return chunk;
        }
    }
}
