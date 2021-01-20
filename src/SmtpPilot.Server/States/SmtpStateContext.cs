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
        internal SmtpStateContext(IMailClient client, SmtpConversation conversation, SmtpCommand command, EmailStatistics stats, SmtpPilotConfiguration configuration)
        {
            Configuration = configuration;
            Statistics = stats;
            Client = client;
            Conversation = conversation;
            Command = command;
            Items = new Dictionary<object, object>();
        }

        public EmailStatistics Statistics { get; }

        public IMailClient Client { get; }

        public SmtpConversation Conversation { get; }

        public SmtpCommand Command { get;

            /* Has a setter, which is more than the Interface contains.
             * This is done to hide the setter from the IConversationState
             * objects and make clear the read-only intent when operating
             * in that context.
             */
            set; }

        public bool HasError => Conversation.HasError;

        public ISmtpPilotConfiguration Configuration { get; }

        public IDictionary<object, object> Items { get; }

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
            Configuration.ServerEvents.OnEmailProcessed(Client, new EmailProcessedEventArgs(Client, Conversation.CurrentMessage, Statistics));
        }

        public void NewMessage()
        {
            Conversation.NewMessage();
        }
    }
}
