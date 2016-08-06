using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot
{
    internal static class ConsoleBehavior
    {
        internal static void ExitWithError(string errorMessage, ExitCode code)
        {
            ConsoleHooks.LogInfo(errorMessage);
            Environment.Exit((int)code);
        }
    }
}
