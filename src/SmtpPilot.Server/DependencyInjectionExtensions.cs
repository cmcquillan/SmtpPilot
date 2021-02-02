using Microsoft.Extensions.DependencyInjection;
using SmtpPilot.Server.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmtpPilot.Server
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddSmtpPilotCore(IServiceCollection services)
        {
            return services;
        }
    }
}
