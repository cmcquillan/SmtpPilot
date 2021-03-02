using SmtpPilot.Server.IO;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace SmtpPilot.Server.Conversation
{
    public class LazilyMaterializedEmailMessage : IMessage, IDisposable
    {
        private bool _disposedValue;
        private readonly IEnumerable<IMemoryOwner<char>> _owners;
        private readonly Memory<char> _fromSegment;
        private readonly IEnumerable<Memory<char>> _recipientSegments;
        private readonly IEnumerable<Memory<char>> _bodySegments;
        private Mailbox? _fromAddress;
        private ReadOnlyCollection<Mailbox> _addresses;

        public LazilyMaterializedEmailMessage(
            IEnumerable<IMemoryOwner<char>> owners,
            Memory<char> fromSegment,
            IEnumerable<Memory<char>> recipientSegments,
            IEnumerable<Memory<char>> bodySegments)
        {
            _owners = owners.ToArray();
            _fromSegment = fromSegment;
            _recipientSegments = recipientSegments.ToArray();
            _bodySegments = bodySegments.ToArray();
        }

        public Stream MessageBody => MaterializeMessageBody();

        private Stream MaterializeMessageBody()
        {
            throw new NotImplementedException();
        }

        public Mailbox FromAddress => MaterializeFromAddress();

        private Mailbox MaterializeFromAddress()
        {
            if (_fromAddress is null)
            {
                _fromAddress = BuildMaterializedAddress(_fromSegment);
            }

            return _fromAddress.Value;
        }

        private Mailbox BuildMaterializedAddress(Memory<char> segment)
        {
            var addressString = segment.Span.ToString();
            var email = IOHelper.ParseEmails(addressString).FirstOrDefault();
            var addr = Mailbox.Parse(email);
            return addr;
        }

        public ReadOnlyCollection<SmtpHeader> Headers => MaterializeHeaders();

        private ReadOnlyCollection<SmtpHeader> MaterializeHeaders()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<Mailbox> Recipients => MaterializeToAddresses();

        private ReadOnlyCollection<Mailbox> MaterializeToAddresses()
        {
            if (_addresses is null)
            {
                var list = new List<Mailbox>();
                foreach (var item in _recipientSegments)
                {
                    list.Add(BuildMaterializedAddress(item));
                }

                _addresses = list.AsReadOnly();
            }

            return _addresses;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    foreach (var item in _owners)
                        item.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
