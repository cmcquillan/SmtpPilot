using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SmtpPilot.Server.Conversation
{
    public class SmtpConversation
    {
        private readonly List<ConversationElement> _elements = new List<ConversationElement>();
        private Stack<IMessage> _messages = new Stack<IMessage>();


        public SmtpConversation()
        {
        }

        public void Reset()
        {
            if (_messages.Count > 0)
                _messages.Pop();
        }

        public IMessage CurrentMessage
        {
            get
            {
                return _messages.Peek();
            }
        }

        public void AddElement(ConversationElement element)
        {
            _elements.Add(element);
        }

        public void NewMessage(IMessage message)
        {
            _messages.Push(message);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var e in _elements)
            {
                sb.Append(e.ToString());
            }

            return sb.ToString();
        }
    }
}
