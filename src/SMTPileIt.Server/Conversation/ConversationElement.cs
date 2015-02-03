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
        { }

        public string FullMessage { get { return _fullMessage.ToString(); } }
        public SmtpCommand Command { get; private set; }
        public string ArgText { get; private set; }
        public DateTime TimeStamp { get; private set; }


        public void Append(string line)
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

            var retValue = new ConversationElement()
            {
                ArgText = message.Substring(verbIndex+1, message.Length - (verbIndex+1)),
                Command = command,
                TimeStamp = DateTime.Now,
            };

            retValue.Append(message);

            return retValue;
        }
    }
}
