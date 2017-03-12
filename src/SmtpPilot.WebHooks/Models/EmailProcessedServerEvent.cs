using SmtpPilot.Server.Conversation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmtpPilot.WebHooks.Models
{
    public class EmailProcessedServerEvent
    {
        public string Type { get; set; }

        public EmailMessageModel EventData { get; set; }

        public long Timestamp { get; set; }

        public static EmailProcessedServerEvent CreateMessageProcessed(IMessage message)
        {
            return new EmailProcessedServerEvent
            {
                Type = EmailServerEventType.MessageProcessed,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                EventData = EmailMessageModel.Create(message),
            };
        }
    }
}
