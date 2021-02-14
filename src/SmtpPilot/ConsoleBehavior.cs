using System;

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
