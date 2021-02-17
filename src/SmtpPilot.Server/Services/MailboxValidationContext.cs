using SmtpPilot.Server.Communication;
using SmtpPilot.Server.Conversation;
using System;

namespace SmtpPilot.Server.Services
{
    public class MailboxValidationContext
    {
        private readonly IMailClient _mailClient;

        public MailboxValidationContext(IServiceProvider serviceProvider, EmailAddress emailAddress, IMailClient mailClient = null)
        {
            ServiceProvider = serviceProvider;
            EmailAddress = emailAddress;
            _mailClient = mailClient;
        }

        public bool CanRespond => _mailClient is not null;

        public void Respond(string response)
        {
            _mailClient.Write(response);
        }

        public IServiceProvider ServiceProvider { get; }

        public EmailAddress EmailAddress { get; }
    }
}
