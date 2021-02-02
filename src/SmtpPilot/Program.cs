using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmtpPilot.Server;
using SmtpPilot.Server.Communication;
using SmtpPilot.Server.Data;
using SmtpPilot.Server.IO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SmtpPilot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;

            var options = ConsoleParse.GetOptions(args);
            var config = options.ToConfiguration(GetListenParameters(options));
            config.ClientTimeoutSeconds = 1000;
            config.ServerEvents.ClientConnected += ConsoleHooks.Server_ClientConnected;
            config.ServerEvents.ClientDisconnected += ConsoleHooks.Server_ClientDisconnected;
            config.ServerEvents.EmailProcessed += ConsoleHooks.Server_EmailProcessed;
            config.ServerEvents.ServerStarted += ConsoleHooks.Server_Started;
            config.ServerEvents.ServerStopped += ConsoleHooks.Server_Stopped;

            ConsoleHooks.LogInfo("Starting mock SMTP server.");

            await SMTPServer.CreateSmtpHost(args, config).Run();

            ConsoleHooks.LogInfo("Press 'q' to stop server.");
            ConsoleHooks.LogInfo("Shutting down mock SMTP server.");
        }

        private static IEnumerable<TcpListenerParameters> GetListenParameters(SmtpPilotOptions options)
        {
            foreach(var item in options.ListenIPAddress)
            {
                yield return new TcpListenerParameters(IPAddress.Parse(item), options.ListenPort);
            }
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            ConsoleHooks.LogInfo($"Received shutdown signal.");
        }
    }
}
