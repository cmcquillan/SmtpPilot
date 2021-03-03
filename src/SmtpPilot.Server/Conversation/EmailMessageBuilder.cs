using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace SmtpPilot.Server.Conversation
{
    internal class EmailMessageBuilder
    {
        private const int BufferSize = 4096;
        private readonly MemoryPool<char> _memoryPool = MemoryPool<char>.Shared;
        private readonly Dictionary<SegmentType, List<Memory<char>>> _segments;
        private readonly List<IMemoryOwner<char>> _segmentList = new List<IMemoryOwner<char>>();
        private IMemoryOwner<char> _currentSegment;
        private readonly Memory<char> _temporaryBuffer;
        private int _currentSegmentWritten;

        internal EmailMessageBuilder()
        {
            _segments = new Dictionary<SegmentType, List<Memory<char>>>()
            {
                [SegmentType.From] = new List<Memory<char>>(),
                [SegmentType.Data] = new List<Memory<char>>(),
                [SegmentType.Rcpt] = new List<Memory<char>>(),
            };
            EnsureBuffer();
            _temporaryBuffer = new Memory<char>(new char[128]);
        }

        internal Span<char> GetTemporaryBuffer()
        {
            return _temporaryBuffer.Span;
        }

        internal Span<char> GetBuffer(int size)
        {
            EnsureBuffer(size);

            return _currentSegment.Memory.Span.Slice(_currentSegmentWritten, size);
        }

        private void EnsureBuffer(int size = BufferSize)
        {
            if (_currentSegment is not null && _currentSegment.Memory.Length - _currentSegmentWritten < size)
            {
                _currentSegment = null;
                _currentSegmentWritten = 0;
            }

            if (_currentSegment is null)
            {
                _currentSegment = _memoryPool.Rent(BufferSize);
                _currentSegment.Memory.Span.Clear();
                _segmentList.Add(_currentSegment);
            }
        }

        internal void StartMessage(int start, int count)
        {
            AddSegment(SegmentType.From, start, count);
        }

        private void MarkWritten(int length)
        {
            _currentSegmentWritten += length;
        }

        private void AddSegment(SegmentType type, int start, int count)
        {
            _segments[type].Add(_currentSegment.Memory.Slice(_currentSegmentWritten + start, count));
            MarkWritten(start + count);
        }

        internal IMessage BuildMessage()
        {
            try
            {
                return new LazilyMaterializedEmailMessage(
                    _segmentList.AsEnumerable(),
                    _segments[SegmentType.From].FirstOrDefault(),
                    _segments[SegmentType.Rcpt].AsEnumerable(),
                    _segments[SegmentType.Data].AsEnumerable());
            }
            finally
            {
                ClearMemoryOwners();
            }
        }

        internal void ResetState()
        {
            foreach (var chunk in _segmentList)
            {
                chunk.Dispose();
            }

            ClearMemoryOwners();

            EnsureBuffer();
        }

        private void ClearMemoryOwners()
        {
            _currentSegment = null;
            _currentSegmentWritten = 0;
            _segmentList.Clear();

            foreach (var segmentType in _segments)
            {
                segmentType.Value.Clear();
            }
        }

        internal void AddAddressSegment(int start, int count)
        {
            AddSegment(SegmentType.Rcpt, start, count);
        }

        internal void AddDataSegment(int start, int count)
        {
            AddSegment(SegmentType.Data, start, count);
        }
    }
}
