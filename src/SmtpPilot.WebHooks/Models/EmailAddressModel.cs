using SmtpPilot.Server.Conversation;

namespace SmtpPilot.WebHooks.Models
{
    public class EmailAddressModel
    {
        public string Address { get; set; }
        public string DisplayName { get; set; }
        public string Host { get; set; }
        public AddressType Type { get; set; }
        public string User { get; set; }

        internal static EmailAddressModel Create(EmailAddress fromAddress)
        {
            return new EmailAddressModel
            {
                Address = fromAddress.Address,
                DisplayName = fromAddress.DisplayName,
                Host = fromAddress.Host,
                Type = fromAddress.Type,
                User = fromAddress.User,
            };
        }
    }
}