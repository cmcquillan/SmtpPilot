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
        private SmtpCommand _command;
        private readonly SmtpConversation _conversation;
        private readonly IMailClient _client;
        private readonly EmailStatistics _stats;

        internal SmtpStateContext(IMailClient client, SmtpConversation conversation, SmtpCommand command, EmailStatistics stats)
        {
            _stats = stats;
            _client = client;
            _conversation = conversation;
            _command = command;
        }

        protected virtual void OnEmailProcessed(EmailProcessedEventArgs eventArgs)
        {
            EmailProcessedEventHandler handler = EmailProcessed;

            if (handler != null)
            {
                foreach (EmailProcessedEventHandler sub in handler.GetInvocationList())
                {
                    try
                    {
                        sub(this, eventArgs);
                    }
                    catch (Exception ex)
                    { }
                }
            }
        }

        public IMailClient Client
        {
            get { return _client; }
        }

        public SmtpConversation Conversation
        {
            get { return _conversation; }
        }

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

        public bool HasError
        {
            get
            {
                return _conversation.HasError;
            }
        }

        public EmailProcessedEventHandler EmailProcessed { get; set; }

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
            OnEmailProcessed(new EmailProcessedEventArgs(_client, _conversation.CurrentMessage, _stats));
        }

        public void NewMessage()
        {
            Conversation.NewMessage();
        }
    }
}
