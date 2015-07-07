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

        //public string FullMessage { get { return _fullMessage.ToString(); } }
        //public SmtpCommand Command { get; private set; }
        //public string ArgText { get; private set; }
        //public DateTime TimeStamp { get; private set; }
        //public virtual bool Terminated { get { return true; } protected set { } }

        //protected void Append(string line)
        //{
        //    _fullMessage.Append(line);
        //}
    }
}
