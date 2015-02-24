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

        public static ConversationElement Parse(string message)
        {
            int verbIndex = message.IndexOf(ASCIISpace);

            if (verbIndex == -1)
                verbIndex = message.Length - 1;

            string word = message.Substring(0, verbIndex + 1);

            SmtpCommand command = (SmtpCommand)Enum.Parse(typeof(SmtpCommand), word);

            ConversationElement retValue;

            if (command != SmtpCommand.DATA)
                retValue = new ConversationElement();
            else
                retValue = new DataConversationElement();

            retValue.ArgText = message.Substring(verbIndex + 1, message.Length - (verbIndex + 1));
            retValue.Command = command;
            retValue.TimeStamp = DateTime.Now;

            retValue.Append(message);

            return retValue;
        }
    }
}
