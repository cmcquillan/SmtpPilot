using SMTPileIt.Server.Conversation;
using SMTPileIt.Server.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPileIt.Server.States
{
    public class SmtpStateContext : ISmtpStateContext
    {
        private readonly SmtpCommand _command;
        private readonly SmtpConversation _conversation;
        private readonly IMailClient _client;

        internal SmtpStateContext(IMailClient client, SmtpConversation conversation, SmtpCommand command)
        {
            _client = client;
            _conversation = conversation;
            _command = command;
        }

        public IO.IMailClient Client
        {
            get { return _client; }
        }

        public Conversation.SmtpConversation Conversation
        {
            get { return _conversation; }
        }

        public Conversation.SmtpCommand Command
        {
            get { return _command; }
        }
    }
}
