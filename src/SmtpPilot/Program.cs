using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using SmtpPilot.Server;
using SmtpPilot.Server.Communication;
using SmtpPilot.Server.Data;
using SmtpPilot.Server.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmtpPilot
{
    class Program
    {
        private static SMTPServer _server;
        public static KestrelClientListenerAdapter AdapterInstance = new KestrelClientListenerAdapter();

        static void Main(string[] args)
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

            if(options.WriteMailToFolder)
            {
                config.MailStore = new JsonMailStore(options.WriteMailToFolderPath);
            }

            if(options.WriteMailToMemory)
            {
                config.MailStore = new InMemoryMailStore();
            }

            if(options.WebHookUri != null)
            {
                config.AddWebHooks(options.WebHookUri, 15, 1);
            }

            config.ServerEvents.ClientConnected += ConsoleHooks.Server_ClientConnected;
            config.ServerEvents.ClientDisconnected += ConsoleHooks.Server_ClientDisconnected;
            config.ServerEvents.EmailProcessed += ConsoleHooks.Server_EmailProcessed;
            config.ServerEvents.ServerStarted += ConsoleHooks.Server_Started;
            config.ServerEvents.ServerStopped += ConsoleHooks.Server_Stopped;

            _server = new SMTPServer(config);

            ConsoleHooks.LogInfo("Starting mock SMTP server.");

            _server.Start();

            CreateHostBuilder(args).Build().Run();

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

    internal class SmtpPilotConnectionHandler : ConnectionHandler
    {
        private readonly KestrelClientListenerAdapter _adapter;
        private readonly ILogger<SmtpPilotConnectionHandler> _logger;

        public SmtpPilotConnectionHandler(KestrelClientListenerAdapter adapter, ILogger<SmtpPilotConnectionHandler> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public override Task OnConnectedAsync(ConnectionContext connection)
        {
            return _adapter.ExecuteNewConnection(connection);
        }
    }
}
