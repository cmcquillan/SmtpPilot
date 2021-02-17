using Microsoft.Extensions.DependencyInjection;
using SmtpPilot.Server.Communication;
using SmtpPilot.Server.Conversation;

namespace SmtpPilot.Server
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddSmtpPilotCore(this IServiceCollection services)
        {
            services.AddSingleton<IMailClientFactory, KestrelMailClientFactory>();

            return services;
        }
    }
}
