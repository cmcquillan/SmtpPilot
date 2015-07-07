using SMTPileIt.Server.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPileIt.Server.Conversation
{
    public class SmtpConversation
    {
        private readonly List<ConversationElement> _elements = new List<ConversationElement>();
        
        public SmtpConversation() {  }

        public IReadOnlyList<ConversationElement> Elements
        {
            get
            {
                return _elements.AsReadOnly();
            }
        }

        public ConversationElement LastElement
        {
            get 
            {
                if (_elements.Count == 0)
                    return null;

                return _elements.Last();
            }
        }

        public SmtpCmd LastCommand
        {
            get
            {
                var q = _elements.Where(p => p is SmtpCmd);
                if (!q.Any())
                    return null;
                return q.Last() as SmtpCmd;
            }
        }

        public SmtpReply LastReply
        {
            get
            {
                var q = _elements.Where(p => p is SmtpReply);
                if (q.Any())
                    return null;
                return q.Last() as SmtpReply;
            }
        }

        public void AddElement(ConversationElement element)
        {
            _elements.Add(element);
        }
    }
}
