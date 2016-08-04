using SmtpPilot.Server;
using SmtpPilot.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpPilot
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new SmtpPilotConfiguration("127.0.0.1", 25)
            {
                ClientTimeoutSeconds = 1000,
                MailStore = new XmlMailStore(),
            };

            var server = new SMTPServer(config);

            ConsoleHooks.LogInfo("Starting mock SMTP server.");

            server.ClientConnected += ConsoleHooks.Server_ClientConnected;
            server.ClientDisconnected += ConsoleHooks.Server_ClientDisconnected;
            server.EmailProcessed += ConsoleHooks.Server_EmailProcessed;
            server.ServerStarted += ConsoleHooks.Server_Started;
            server.ServerStopped += ConsoleHooks.Server_Stopped;
            server.Start();

            ConsoleHooks.LogInfo("Press 'q' to stop server.");

            while (Console.ReadKey(true).KeyChar != 'q') { }

            ConsoleHooks.LogInfo("Shutting down mock SMTP server.");
            server.Stop();
        }
    }
}
