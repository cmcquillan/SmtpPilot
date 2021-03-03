using System;

namespace SmtpPilot.Server.Internal
{
    internal interface IClock
    {
        DateTimeOffset Now { get; }

        DateTimeOffset UtcNow { get; }
    }
}
