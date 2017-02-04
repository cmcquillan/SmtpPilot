using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmtpPilot.Server.Conversation;
using System.Diagnostics;

namespace SmtpPilot.Server.Data
{
    public class InMemoryMailStore : IMailStore
    {
        public const int DefaultRetentionAmount = 100;
        private readonly int _retentionAmount;
        private readonly LinkedList<IMessage> _messages;

        public InMemoryMailStore()
            : this(DefaultRetentionAmount)
        {
        }

        public InMemoryMailStore(int retentionAmount)
        {
            _retentionAmount = retentionAmount;
            _messages = new LinkedList<IMessage>();
        }

        public void SaveMessage(IMessage message)
        {
            if (_messages.Count == _retentionAmount)
            {
                Debug.WriteLine("Message Eviction", TraceConstants.InMemoryStore);
                _messages.RemoveFirst();
            }

            Debug.WriteLine("Message Insertion", TraceConstants.InMemoryStore);
            _messages.AddLast(message);
        }
    }
}
