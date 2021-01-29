using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using SmtpPilot.Server;
using SmtpPilot.Server.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
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
                .ConfigureServices(services =>
                {
                    services.AddSingleton(configuration);
                    services.AddSingleton<EmailStatistics>();
                })
                .UseKestrel(options =>
                {
                    // TCP 25
                    options.ListenLocalhost(25, builder =>
                    {
                        builder.UseConnectionHandler<KestrelConnectionHandler>();
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
