using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMTPileIt.Server.Conversation
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

        public IReadOnlyList<ConversationElement> Elements
        {
            get
            {
                return _elements.AsReadOnly();
            }
        }

        public IMessage CurrentMessage
        {
            get
            {
                return _messages.Peek();
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

        public bool HasError
        {
            get
            {
                //CONSIDER: Linq performance vs loop performance.
                return _elements.Select(p => p as SmtpReply)
                  .Where(p => p != null && p.IsError)
                  .Any();
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

        public void NewMessage()
        {
            _messages.Push(new EmailMessage());
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach(var e in _elements)
            {
                sb.Append(e.ToString());
            }

            return sb.ToString();
        }
    }
}
