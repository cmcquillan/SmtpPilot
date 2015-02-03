using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMTPileIt.Server.Conversation
{
    public enum SmtpCommand
    {
        EHLO = 0,
        HELO = EHLO,
        MAIL = 1,
        RCPT = 2,
        DATA = 3,
        SIZE = 4,
        VRFY = 5,
        EXPN = 6,
        SEND = 7,
        SOML = 8,
        SAML = 9,
        RSET = 10,
        HELP = 11,
        NOOP = 12,
        TURN = 13,
        QUIT = 100,
    }
}
