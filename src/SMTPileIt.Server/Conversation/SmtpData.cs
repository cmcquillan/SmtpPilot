using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPileIt.Server.Conversation
{
    public class SmtpData : ConversationElement, IAppendable
    {
        private readonly StringBuilder _fullText;

        public SmtpData()
        {
            _fullText = new StringBuilder();
        }

        public override string FullText
        {
            get
            {
                return _fullText.ToString();
            }
        }

        public override string Preamble
        {
            get
            {
                return String.Empty;
            }
        }

        public void Append(string l)
        {
            _fullText.Append(l);
        }

        public void AppendLine(string l)
        {
            _fullText.AppendLine(l);
        }

        public override string ToString()
        {
            return FullText;
        }
    }
}
