using SmtpPilot.Server.Conversation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmtpPilot.WebHooks.Models
{
    public class EmailHeaderModel
    {
        public EmailHeaderModel() { }

        public string Name { get; set; }

        public string Value { get; set; }

        public static EmailHeaderModel Create(SmtpHeader head)
        {
            return new EmailHeaderModel
            {
                Name = head.Name,
                Value = head.Value,
            };
        }
    }
}
