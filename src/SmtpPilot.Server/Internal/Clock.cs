using System;
using System.Collections.Generic;
using System.Text;

namespace SmtpPilot.Server.Internal
{
    internal class Clock : IClock
    {
        public DateTimeOffset Now => DateTimeOffset.Now;

        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
