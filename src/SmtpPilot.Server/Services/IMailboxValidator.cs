using System;
using System.Collections.Generic;
using System.Text;

namespace SmtpPilot.Server.Services
{
    public interface IMailboxValidator
    {
        bool Validate(MailboxValidationContext context);
    }
}
