using SmtpPilot.Server;
using SmtpPilot.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmtpPilot
{
    class Program
    {
        private static SMTPServer _server;

        static void Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            var options = ConsoleParse.GetOptions(args);
            var config = new SmtpPilotConfiguration(options.ListenIPAddress, options.ListenPort, options.HostName)
            {
                ClientTimeoutSeconds = 1000,
            };

            if(options.WriteMailToFolder)
            {
                config.MailStore = new JsonMailStore(options.WriteMailToFolderPath);
            }

            if(options.WriteMailToMemory)
            {
                config.MailStore = new InMemoryMailStore();
            }

            _server = new SMTPServer(config);

            ConsoleHooks.LogInfo("Starting mock SMTP server.");

            _server.ClientConnected += ConsoleHooks.Server_ClientConnected;
            _server.ClientDisconnected += ConsoleHooks.Server_ClientDisconnected;
            _server.EmailProcessed += ConsoleHooks.Server_EmailProcessed;
            _server.ServerStarted += ConsoleHooks.Server_Started;
            _server.ServerStopped += ConsoleHooks.Server_Stopped;
            _server.Start();

            ConsoleHooks.LogInfo("Press 'q' to stop server.");

            while (true)
            {
                if(!options.Headless)
                {
                    if (Console.ReadKey().KeyChar == 'q')
                        break;
                }

                Thread.Sleep(100);
            }

            ConsoleHooks.LogInfo("Shutting down mock SMTP server.");
            _server.Stop();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            ConsoleHooks.LogInfo($"Received shutdown signal.");
            _server.Stop();
        }
    }
}
