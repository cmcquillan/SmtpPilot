using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPileIt.Server.Conversation
{
    public class SmtpCmd : ConversationElement
    {
        private readonly SmtpCommand _cmd;
        private readonly StringBuilder _lines;

        public SmtpCmd(SmtpCommand cmd, string text)
        {
            _cmd = cmd;
            _lines = new StringBuilder();

            if (!String.IsNullOrEmpty(text))
                _lines.AppendLine(text);
        }

        public override string Preamble
        {
            get { return _cmd.ToString(); }
        }

        public override string FullText
        {
            get { return this.ToString(); }
        }

        public void AppendLine(string line)
        {
            _lines.AppendLine(line);
        }

        public override string ToString()
        {
            return String.Format("{1}", _lines.ToString());
        }
    }
}
