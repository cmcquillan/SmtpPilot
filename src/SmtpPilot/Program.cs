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
using System.Threading;
using System.Threading.Tasks;

namespace SmtpPilot
{
    class Program
    {
        public static SMTPServer Server = null;
        public static KestrelClientListenerAdapter AdapterInstance = new KestrelClientListenerAdapter();

        static async Task Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            var options = ConsoleParse.GetOptions(args);
            List<IMailClientListener> listeners = new List<IMailClientListener>();

            //foreach(var ipAddr in options.ListenIPAddress)
            //{
            //    listeners.Add(new TcpClientListener(ipAddr, options.ListenPort));
            //}
            listeners.Add(new KestrelClientListener(AdapterInstance));

            var config = new SmtpPilotConfiguration(listeners, options.HostName)
            {
                ClientTimeoutSeconds = 1000,
            };

            if (options.WriteMailToFolder)
            {
                config.MailStore = new JsonMailStore(options.WriteMailToFolderPath);
            }

            if (options.WriteMailToMemory)
            {
                config.MailStore = new InMemoryMailStore();
            }

            if (options.WebHookUri != null)
            {
                config.AddWebHooks(options.WebHookUri, 15, 1);
            }

            config.ServerEvents.ClientConnected += ConsoleHooks.Server_ClientConnected;
            config.ServerEvents.ClientDisconnected += ConsoleHooks.Server_ClientDisconnected;
            config.ServerEvents.EmailProcessed += ConsoleHooks.Server_EmailProcessed;
            config.ServerEvents.ServerStarted += ConsoleHooks.Server_Started;
            config.ServerEvents.ServerStopped += ConsoleHooks.Server_Stopped;

            Server = new SMTPServer(config);

            ConsoleHooks.LogInfo("Starting mock SMTP server.");

            await CreateHostBuilder(args).Build().RunAsync();

            ConsoleHooks.LogInfo("Press 'q' to stop server.");

            while (true)
            {
                if (!options.Headless)
                {
                    if (Console.ReadKey().KeyChar == 'q')
                        break;
                }

                Thread.Sleep(100);
            }

            ConsoleHooks.LogInfo("Shutting down mock SMTP server.");
            Server.Stop();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            ConsoleHooks.LogInfo($"Received shutdown signal.");
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options =>
                {
                    // TCP 25
                    options.ListenLocalhost(25, builder =>
                    {
                        builder.UseConnectionHandler<SmtpPilotConnectionHandler>();
                    });

                    // HTTP 5000
                    options.ListenLocalhost(5000);

                    // HTTPS 5001
                    options.ListenLocalhost(5001, builder =>
                    {
                        builder.UseHttps();
                    });
                })
                .UseStartup<Startup>();
    }
}
