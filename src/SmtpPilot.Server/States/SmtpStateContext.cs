using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot.Server.States
{
    public class SmtpStateContext : ISmtpStateContext
    {
        private SmtpCommand _command;
        private readonly SmtpConversation _conversation;
        private readonly IMailClient _client;
        private readonly EmailStatistics _stats;
        private readonly SmtpPilotConfiguration _configuration;

        internal SmtpStateContext(IMailClient client, SmtpConversation conversation, SmtpCommand command, EmailStatistics stats, SmtpPilotConfiguration configuration)
        {
            _configuration = configuration;
            _stats = stats;
            _client = client;
            _conversation = conversation;
            _command = command;
        }

        public EmailStatistics Statistics => _stats;

        public IMailClient Client => _client;

        public SmtpConversation Conversation => _conversation;

        public SmtpCommand Command
        {
            get { return _command; }

            /* Has a setter, which is more than the Interface contains.
             * This is done to hide the setter from the IConversationState
             * objects and make clear the read-only intent when operating
             * in that context.
             */
            set { _command = value; }
        }

        public bool HasError => _conversation.HasError;

        public SmtpPilotConfiguration Configuration => _configuration;

        public void AddHeader(SmtpHeader header)
        {
            Conversation.CurrentMessage.AddHeader(header);
        }

        public void Reply(SmtpReply reply)
        {
            Conversation.AddElement(reply);
            Client.Write(reply.FullText);
        }

        public void SetFrom(string from)
        {
            Conversation.CurrentMessage.FromAddress = new EmailAddress(from, AddressType.From);
        }

        public void AddTo(string[] emails)
        {
            InternalAddAddresses(emails, AddressType.To);
        }

        public void AddCc(string[] emails)
        {
            InternalAddAddresses(emails, AddressType.Cc);
        }

        public void AddBcc(string[] emails)
        {
            InternalAddAddresses(emails, AddressType.Bcc);
        }

        private void InternalAddAddresses(string[] emails, AddressType type)
        {
            IAddress[] addresses = new IAddress[emails.Length];

            for (int i = 0; i < emails.Length; i++)
                addresses[i] = new EmailAddress(emails[i], type);

            Conversation.CurrentMessage.AddAddresses(addresses);
        }

        public void CompleteMessage()
        {
            Conversation.CurrentMessage.Complete();
            Configuration.ServerEvents.OnEmailProcessed(_client, new EmailProcessedEventArgs(_client, _conversation.CurrentMessage, _stats));
        }

        public void NewMessage()
        {
            Conversation.NewMessage();
        }
    }
}
