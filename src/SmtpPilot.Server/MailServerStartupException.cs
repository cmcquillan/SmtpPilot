using System;
using System.Collections.Generic;
using System.Text;

namespace SmtpPilot.Server
{
    public class MailServerStartupException : Exception
    {
        public MailServerStartupException() { }

        public MailServerStartupException(string message) : base(message) { }

        public MailServerStartupException(string message, Exception inner) : base(message, inner) { }
    }
}
