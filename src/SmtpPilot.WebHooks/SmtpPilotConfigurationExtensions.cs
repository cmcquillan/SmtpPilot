using Newtonsoft.Json;
using SmtpPilot.Server;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SmtpPilot.Server.Conversation;
using System.Threading;
using SmtpPilot.WebHooks.Models;
using Newtonsoft.Json.Serialization;

namespace SmtpPilot.Server
{
    public static class SmtpPilotConfigurationExtensions
    {
        private static HttpClient _client;
        private static CancellationTokenSource _cancellationTokenSource;
        private static List<Task> _pendingTasks = new List<Task>();
        private static string _webHookUrl;

        public static SmtpPilotConfiguration AddWebHooks(this SmtpPilotConfiguration config, string webHookUrl)
        {
            _webHookUrl = webHookUrl;
            _cancellationTokenSource = new CancellationTokenSource();

            config.ServerEvents.EmailProcessed += EmailProcessedWebHook;
            config.ServerEvents.ServerStarted += ServerStarted;
            config.ServerEvents.ServerStopped += ServerStopped;

            return config;
        }

        private static void ServerStopped(object sender, ServerEventArgs eventArgs)
        {
            _cancellationTokenSource.Cancel();
            _client?.Dispose();
            _client = null;
        }

        private static void ServerStarted(object sender, ServerEventArgs eventArgs)
        {
            _client = new HttpClient();
        }

        private static void EmailProcessedWebHook(object sender, EmailProcessedEventArgs eventArgs)
        {
            _pendingTasks.RemoveAll(p => p.IsCompleted);
            _pendingTasks.Add(Task.Run(async () => await SendMessageToWebHook(eventArgs.Message, _cancellationTokenSource.Token)));
        }

        private static async Task SendMessageToWebHook(IMessage message, CancellationToken token)
        {
            var evt = EmailProcessedServerEvent.CreateMessageProcessed(message);
            var data = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(evt, Formatting.None, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }), token);
            await _client.PostAsync(_webHookUrl + "/event/email", new StringContent(data, Encoding.UTF8, "application/json"), token);
        }
    }
}
