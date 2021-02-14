using System;

namespace SmtpPilot
{
    [Flags]
    internal enum ExitCode : int
    {
        Success = 0,
        InvalidArguments = 1,
    }
}
