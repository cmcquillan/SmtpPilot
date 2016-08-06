using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot
{
    [Flags]
    internal enum ExitCode : int
    {
        Success = 0,
        InvalidArguments = 1,
    }
}
