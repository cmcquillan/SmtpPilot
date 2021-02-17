using Microsoft.Extensions.DependencyInjection;
using SmtpPilot.Server.Communication;
using SmtpPilot.Server.Conversation;
using SmtpPilot.Server.States;

namespace SmtpPilot.Server
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddSmtpPilotCore(this IServiceCollection services)
        {
            services.AddSingleton<IMailClientFactory, KestrelMailClientFactory>();

            services.AddSingleton<ConversationStateCollection>();
            services.AddSingleton<IConversationState, AcceptMailConversationState>();
            services.AddSingleton<IConversationState, DataConversationState>();
            services.AddSingleton<IConversationState, ErrorConversationState>();
            services.AddSingleton<IConversationState, OpenConnectionState>();
            services.AddSingleton<IConversationState, QuitConversationState>();
            services.AddSingleton<IConversationState, RecipientConversationState>();

            return services;
        }
    }
}
