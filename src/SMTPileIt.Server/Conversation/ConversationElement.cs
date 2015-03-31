using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPileIt.Server.Conversation
{
    public class ConversationElement
    {
        public const char ASCIISpace = (char)0x20;
        private readonly StringBuilder _fullMessage = new StringBuilder();


        public ConversationElement()
        {
            Reply = new SmtpReply(SmtpReplyCode.Code250);
        }

        public string FullMessage { get { return _fullMessage.ToString(); } }
        public SmtpCommand Command { get; private set; }
        public string ArgText { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public virtual bool Terminated { get { return true; } protected set { } }
        public SmtpReply Reply { get; protected set; }

        public virtual string SendReply()
        {
            return Reply.GetReply();
        }

        protected void Append(string line)
        {
            _fullMessage.Append(line);
        }

        public static ConversationElement Parse(SmtpCommand command, string message)
        {
            ConversationElement retValue = new ConversationElement();
            retValue.ArgText = message.Substring(5, message.Length - 5);
            retValue.Command = command;
            retValue.TimeStamp = DateTime.Now;

            retValue.Append(message);

            return retValue;
        }
    }
}
