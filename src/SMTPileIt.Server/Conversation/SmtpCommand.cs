using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMTPileIt.Server.Conversation
{
    [Flags]
    public enum SmtpCommand : int
    {
        NonCommand = 0,
        EHLO = 1,
        HELO = EHLO,
        MAIL = 1 << 1,
        RCPT = 1 << 2,
        DATA = 1 << 3,
        SIZE = 1 << 4,
        VRFY = 1 << 5,
        EXPN = 1 << 6,
        SEND = 1 << 7,
        SOML = 1 << 8,
        SAML = 1 << 9,
        RSET = 1 << 10,
        HELP = 1 << 11,
        NOOP = 1 << 12,
        TURN = 1 << 13,
        QUIT = 1 << 14,
    }

    
}
