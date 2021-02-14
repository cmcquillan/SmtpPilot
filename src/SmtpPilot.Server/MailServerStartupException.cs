using System;

namespace SmtpPilot.Server
{
    public class MailServerStartupException : Exception
    {
        public MailServerStartupException() { }

        public MailServerStartupException(string message) : base(message) { }

        public MailServerStartupException(string message, Exception inner) : base(message, inner) { }
    }
}
