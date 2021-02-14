using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmtpPilot.Server;
using SmtpPilot.Server.Communication;
using System.Threading;
using System.Threading.Tasks;

namespace SmtpPilot
{
    public class SMTPServer
    {
        private readonly IWebHost _host;

        public SmtpServerEvents Events { get; }

        public EmailStatistics Statistics { get; }

        internal SMTPServer(IWebHost host, SmtpServerEvents events, EmailStatistics statistics)
        {
            _host = host;
            Events = events;
            Statistics = statistics;
        }

        public static SMTPServer CreateSmtpHost(string[] args, SmtpPilotConfiguration configuration)
        {
            var builder = WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureServices(services =>
                {
                    if (configuration.MailStore != null)
                    {
                        services.AddSingleton(configuration.MailStore);
                        configuration.ServerEvents.EmailProcessed += (o, e) =>
                        {
                            configuration.MailStore.SaveMessage(e.Message);
                        };
                    }

                    services.AddSmtpPilotCore();
                    services.AddSingleton(configuration);
                    services.AddSingleton<EmailStatistics>();
                })
                .ConfigureKestrel(kestrel =>
                {
                    kestrel.Limits.MaxConcurrentConnections = 1000;
                })
                .UseKestrel(options =>
                {
                    foreach (var item in configuration.ListenParameters)
                    {
                        options.Listen(item.Address, item.Port, builder =>
                        {
                            builder.UseConnectionHandler<KestrelConnectionHandler>();
                        });
                    }
                }).UseStartup<Startup>();

            var host = builder.Build();
            var stats = host.Services.GetRequiredService<EmailStatistics>();

            return new SMTPServer(host, configuration.ServerEvents, stats);
        }

        public async Task Run(CancellationToken cancellationToken = default)
        {
            await _host.RunAsync(cancellationToken);
        }
    }
}
