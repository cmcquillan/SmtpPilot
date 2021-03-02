using SmtpPilot.Server.Conversation;

namespace SmtpPilot.WebHooks.Models
{
    public class EmailAddressModel
    {
        public string LocalPart { get; set; }
        public string Domain { get; set; }

        internal static EmailAddressModel Create(Mailbox fromAddress)
        {
            return new EmailAddressModel
            {
                LocalPart = fromAddress.LocalPart,
                Domain = fromAddress.Domain,
            };
        }
    }
}