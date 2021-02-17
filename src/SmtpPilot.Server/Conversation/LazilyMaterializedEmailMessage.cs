using SmtpPilot.Server.IO;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Pipelines;
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
        private MaterializedAddress? _fromAddress;
        private ReadOnlyCollection<IAddress> _addresses;

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

        public IAddress FromAddress => MaterializeFromAddress();

        private IAddress MaterializeFromAddress()
        {
            if (_fromAddress is null)
            {
                _fromAddress = BuildMaterializedAddress(_fromSegment, AddressType.From);
            }

            return _fromAddress.Value.GetAddress();
        }

        private MaterializedAddress BuildMaterializedAddress(Memory<char> segment, AddressType type)
        {
            var addressString = segment.Span.ToString();
            var email = IOHelper.ParseEmails(addressString).FirstOrDefault();
            var addr = new MaterializedAddress(email, type);
            return addr;
        }

        public ReadOnlyCollection<SmtpHeader> Headers => MaterializeHeaders();

        private ReadOnlyCollection<SmtpHeader> MaterializeHeaders()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IAddress> ToAddresses => MaterializeToAddresses();

        private ReadOnlyCollection<IAddress> MaterializeToAddresses()
        {
            if (_addresses is null)
            {
                var list = new List<IAddress>();
                foreach (var item in _recipientSegments)
                {
                    list.Add(BuildMaterializedAddress(item, AddressType.To).GetAddress());
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

        private struct MaterializedAddress
        {
            private readonly string _addressString;
            private readonly IAddress _address;

            internal MaterializedAddress(string address, AddressType type)
            {
                _addressString = address;
                _address = new EmailAddress(_addressString, type);
            }

            internal IAddress GetAddress() => _address;
        }
    }
}
