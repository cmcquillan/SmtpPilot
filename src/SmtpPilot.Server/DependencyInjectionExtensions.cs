using Microsoft.Extensions.DependencyInjection;
using SmtpPilot.Server.Communication;
using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmtpPilot.Server
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddSmtpPilotCore(this IServiceCollection services)
        {
            services.AddSingleton<IMailClientFactory, KestrelMailClientFactory>();
            services.AddSingleton<IEmailMessageFactory, EmailMessageFactory>();

            return services;
        }
    }
}
