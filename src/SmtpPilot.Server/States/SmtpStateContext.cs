using SmtpPilot.Server.Communication;
using SmtpPilot.Server.Conversation;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace SmtpPilot.Server.States
{
    public class SmtpStateContext2 : IDisposable
    {
        private readonly SmtpPilotConfiguration _configuration;
        private readonly IMailClient _client;
        private readonly SmtpConversation _conversation;
        private readonly EmailStatistics _emailStats;
        private readonly SmtpServerEvents _events;
        private readonly char[] _buffer = new char[1024 * 32];
        private readonly ArrayPool<char> _arrayPool = ArrayPool<char>.Shared;
        private int _bufferPosition = 0;

        public SmtpStateContext2(
            SmtpPilotConfiguration configuration,
            IMailClient client, 
            SmtpConversation conversation, 
            EmailStatistics emailStats, 
            SmtpServerEvents events)
        {
            _configuration = configuration;
            _client = client;
            _conversation = conversation;
            _emailStats = emailStats;
            _events = events;
        }

        public EmailStatistics EmailStats => _emailStats;

        public SmtpConversation Conversation => _conversation;

        public IMailClient Client => _client;

        public SmtpServerEvents Events => _events;

        public SmtpPilotConfiguration Configuration => _configuration;

        public void Reply(SmtpReply reply)
        {
            Conversation.AddElement(reply);
            Client.Write(reply.FullText);
        }

        public Memory<char> GetBufferSegment(int size)
        {
            return _buffer.AsMemory().Slice(_bufferPosition, size);
        }

        public void AdvanceBuffer(int amount)
        {
            _bufferPosition += amount;
        }

        public void Dispose()
        {
            _arrayPool.Return(_buffer);
        }
    }

    public class SmtpStateContext : ISmtpStateContext
    {
        internal SmtpStateContext(
            IMailClient client,
            SmtpConversation conversation,
            SmtpCommand command,
            EmailStatistics stats,
            SmtpPilotConfiguration configuration)
        {
            Configuration = configuration;
            Statistics = stats;
            Client = client;
            Conversation = conversation;
            Command = command;
            Items = new Dictionary<object, object>();
        }

        public EmailStatistics Statistics { get; }

        public IMailClient Client { get; }

        public SmtpConversation Conversation { get; }

        public SmtpCommand Command
        {
            get;

            /* Has a setter, which is more than the Interface contains.
             * This is done to hide the setter from the IConversationState
             * objects and make clear the read-only intent when operating
             * in that context.
             */
            set;
        }

        public bool HasError => Conversation.HasError;

        public ISmtpPilotConfiguration Configuration { get; }

        public IDictionary<object, object> Items { get; }

        public void AddHeader(SmtpHeader header)
        {
            Conversation.CurrentMessage.AddHeader(header);
        }

        public void Reply(SmtpReply reply)
        {
            Conversation.AddElement(reply);
            Client.Write(reply.FullText);
        }

        public void SetFrom(string from)
        {
            Conversation.CurrentMessage.FromAddress = new EmailAddress(from, AddressType.From);
        }

        public void AddTo(string[] emails)
        {
            InternalAddAddresses(emails, AddressType.To);
        }

        public void AddCc(string[] emails)
        {
            InternalAddAddresses(emails, AddressType.Cc);
        }

        public void AddBcc(string[] emails)
        {
            InternalAddAddresses(emails, AddressType.Bcc);
        }

        private void InternalAddAddresses(string[] emails, AddressType type)
        {
            IAddress[] addresses = new IAddress[emails.Length];

            for (int i = 0; i < emails.Length; i++)
                addresses[i] = new EmailAddress(emails[i], type);

            Conversation.CurrentMessage.AddAddresses(addresses);
        }

        public void CompleteMessage()
        {
            Conversation.CurrentMessage.Complete();
            Configuration.ServerEvents.OnEmailProcessed(Client, new EmailProcessedEventArgs(Client, Conversation.CurrentMessage, Statistics));
            Statistics.AddEmailReceived();
        }

        public void NewMessage()
        {
            Conversation.NewMessage();
        }
    }
}
