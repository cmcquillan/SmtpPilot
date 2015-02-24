using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPileIt.Server.Conversation
{
    public class DataConversationElement : ConversationElement
    {
        private bool _hasReplied = false;

        public DataConversationElement()
        {
            Reply = new SmtpReply(SmtpReplyCode.Code354);
        }

        public override bool Terminated { get; protected set; }

        public override string SendReply()
        {
            if (_hasReplied && !Terminated)
                return null;

            if (_hasReplied && Terminated)
                return new SmtpReply(SmtpReplyCode.Code250).GetReply();

            _hasReplied = true;
            return base.SendReply();
        }

        public void AppendLine(string line)
        {
            if (line.Equals(@"."))
                Terminated = true;

            Append(line + Environment.NewLine);
        }
    }
}
