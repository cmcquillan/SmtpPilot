using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot
{
    internal static class ConsoleParse
    {
        internal const int DefaultPort = 25;
        internal const string DefaultIp = "127.0.0.1";
        internal const string AnyIpToken = "any";
        internal const string AnyIp = "0.0.0.0";
        internal const string DefaultPath = ".";

        internal static SmtpPilotOptions GetOptions(string[] args)
        {
            var options = new SmtpPilotOptions()
            {
                ListenPort = DefaultPort,
                WriteMailToFolder = false,
                WriteMailToFolderPath = DefaultPath,
            };

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-m":
                    case "--memory-store":
                        options.WriteMailToMemory = true;
                        break;
                    case "-i":
                    case "--address":
                        string ipArg = args[++i];
                        var ipList = ipArg.Split(new[] { ',' }, StringSplitOptions.None);

                        foreach (var ipAddr in ipList)
                        {
                            string ipToAdd = ipAddr;
                            if (String.Equals(ipToAdd, AnyIpToken))
                                ipToAdd = AnyIp;

                            options.ListenIPAddress.Add(ipToAdd);
                        }
                        break;
                    case "-p":
                    case "--port":
                        int port = 0;
                        if (!Int32.TryParse(args[++i], out port))
                        {
                            ConsoleBehavior.ExitWithError("Invalid port number.", ExitCode.InvalidArguments);
                        }

                        options.ListenPort = port;
                        break;
                    case "-s":
                    case "--save":
                        options.WriteMailToFolder = true;
                        if (args.Length > i + 1 && !args[i + 1].StartsWith("-"))
                        {
                            options.WriteMailToFolderPath = args[++i];
                        }
                        break;
                    case "-h":
                    case "--host":
                        options.HostName = args[++i];
                        break;
                    case "-l":
                    case "--headless":
                        options.Headless = true;
                        break;
                    default:
                        ConsoleBehavior.ExitWithError("Unrecognized argument.", ExitCode.InvalidArguments);
                        break;
                }
            }

            if (options.ListenIPAddress.Count == 0)
                options.ListenIPAddress.Add(DefaultIp);

            return options;
        }
    }
}
