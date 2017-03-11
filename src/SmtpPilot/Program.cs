using SmtpPilot.Server;
using SmtpPilot.Server.Data;
using SmtpPilot.Server.IO;
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
            List<IMailClientListener> listeners = new List<IMailClientListener>();

            foreach(var ipAddr in options.ListenIPAddress)
            {
                listeners.Add(new TcpClientListener(ipAddr, options.ListenPort));
            }

            var config = new SmtpPilotConfiguration(listeners, options.HostName)
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

            config.ServerEvents.ClientConnected += ConsoleHooks.Server_ClientConnected;
            config.ServerEvents.ClientDisconnected += ConsoleHooks.Server_ClientDisconnected;
            config.ServerEvents.EmailProcessed += ConsoleHooks.Server_EmailProcessed;
            config.ServerEvents.ServerStarted += ConsoleHooks.Server_Started;
            config.ServerEvents.ServerStopped += ConsoleHooks.Server_Stopped;

            _server = new SMTPServer(config);

            ConsoleHooks.LogInfo("Starting mock SMTP server.");

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
