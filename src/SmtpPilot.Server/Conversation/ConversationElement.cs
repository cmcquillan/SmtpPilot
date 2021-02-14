using System;

namespace SmtpPilot.Server.Conversation
{
    public abstract class ConversationElement
    {
        protected ConversationElement()
        {
            UtcTimestamp = DateTimeOffset.UtcNow;
            Timestamp = UtcTimestamp.ToLocalTime();
        }

        public DateTimeOffset UtcTimestamp { get; private set; }

        public DateTimeOffset Timestamp { get; private set; }

        public abstract string Preamble { get; }

        public abstract string FullText { get; }
    }
}
