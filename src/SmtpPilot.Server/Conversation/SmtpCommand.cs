using System;

namespace SmtpPilot.Server.Conversation
{
    [Flags]
    public enum SmtpCommand : int
    {
        None = 0,
        EHLO = 1,
        HELO = 1 << 1,
        MAIL = 1 << 2,
        RCPT = 1 << 3,
        DATA = 1 << 4,
        SIZE = 1 << 5,
        VRFY = 1 << 6,
        EXPN = 1 << 7,
        SEND = 1 << 8,
        SOML = 1 << 9,
        SAML = 1 << 10,
        RSET = 1 << 11,
        HELP = 1 << 12,
        NOOP = 1 << 13,
        TURN = 1 << 14,
        QUIT = 1 << 15,
    }


}
