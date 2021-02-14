using SmtpPilot.Server.Conversation;
using System.Linq;

namespace SmtpPilot.WebHooks.Models
{
    public class EmailMessageModel
    {
        public EmailMessageModel() { }

        public EmailHeaderModel[] Headers { get; set; }
        public EmailAddressModel From { get; set; }
        public EmailAddressModel[] To { get; set; }
        public string Message { get; set; }

        public static EmailMessageModel Create(IMessage msg)
        {
            return new EmailMessageModel
            {
                Headers = msg?.Headers?.Select(p => EmailHeaderModel.Create(p))?.ToArray(),
                From = EmailAddressModel.Create(msg?.FromAddress),
                To = msg?.ToAddresses?.Select(p => EmailAddressModel.Create(p))?.ToArray(),
                Message = msg?.Data,
            };
        }
    }
}
