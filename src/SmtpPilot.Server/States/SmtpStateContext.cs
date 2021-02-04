using SmtpPilot.Server.Communication;
using SmtpPilot.Server.Conversation;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace SmtpPilot.Server.States
{
    public class SmtpStateContext : IDisposable
    {
        private readonly char[] _buffer = new char[1024 * 32];
        private readonly ArrayPool<char> _arrayPool = ArrayPool<char>.Shared;
        private int _bufferPosition = 0;

        public SmtpStateContext(
            IServiceProvider serviceProvider,
            SmtpPilotConfiguration configuration,
            IMailClient client, 
            SmtpConversation conversation, 
            EmailStatistics emailStats, 
            SmtpServerEvents events)
        {
            ServiceProvider = serviceProvider;
            Configuration = configuration;
            Client = client;
            Conversation = conversation;
            EmailStats = emailStats;
            Events = events;
        }

        public EmailStatistics EmailStats { get; }

        public SmtpConversation Conversation { get; }

        public IMailClient Client { get; }

        public SmtpServerEvents Events { get; }

        public SmtpPilotConfiguration Configuration { get; }

        public IServiceProvider ServiceProvider { get; }

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
}
