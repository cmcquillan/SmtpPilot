using SmtpPilot.Server.Conversation;
using System.IO;
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
            using var rdr = new StreamReader(msg.MessageBody);
            return new EmailMessageModel
            {
                Headers = msg?.Headers?.Select(p => EmailHeaderModel.Create(p))?.ToArray(),
                From = EmailAddressModel.Create(msg.FromAddress),
                To = msg?.Recipients?.Select(p => EmailAddressModel.Create(p))?.ToArray(),
                Message = rdr.ReadToEnd(),
            };
        }
    }
}
