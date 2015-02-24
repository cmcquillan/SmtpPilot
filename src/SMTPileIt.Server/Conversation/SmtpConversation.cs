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

        public SmtpConversation() { }

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
                return _elements.Last();
            }
        }

        public bool IsInDataState 
        {
            get
            {
                if (_elements.Count == 0)
                    return false;

                return LastElement.Command == SmtpCommand.DATA;
            }
        }

        public void AddElement(ConversationElement element)
        {
            _elements.Add(element);
        }
    }
}
