using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPileIt.Server.Conversation
{
    public abstract class ConversationElement 
    {
        public const char ASCIISpace = (char)0x20;

        protected ConversationElement()
        {
            UtcTimestamp = DateTime.UtcNow;
            Timestamp = UtcTimestamp.ToLocalTime();
        }

        public DateTime UtcTimestamp { get; private set; }

        public DateTime Timestamp { get; private set; }

        public abstract string Preamble { get; }

        public abstract string FullText { get; }
    }
}
