using CodedByKay.SmartDialogueLib.Helpers;
using CodedByKay.SmartDialogueLib.Interfaces;
using CodedByKay.SmartDialogueLib.Models;
using CodedByKay.SmartDialogueLib.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CodedByKay.SmartDialogueLib
{
    public static class SmartDialogueLibExtensions
    {
        public static IServiceCollection AddSmartDialogueLib(this IServiceCollection services, Action<SmartDialogueLibOptions> configureOptions)
        {
            var options = new SmartDialogueLibOptions();
            configureOptions(options);

            ValidationHelpers.ValidateOptions(options);

            services.AddSingleton(options);

            services.AddHttpClient("OpenAiHttpClient", client =>
            {
                client.BaseAddress = new Uri(options.OpenAiApiKey);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", options.OpenAiApiKey);
            });

            services
            .AddSingleton<IChatHistoryService, ChatHistoryService>()
            .AddTransient<ISmartDialogueService, SmartDialogueService>(serviceProvider =>
            {
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("OpenAiHttpClient");

                var chatHistoryService = serviceProvider.GetRequiredService<IChatHistoryService>();

                return new SmartDialogueService(httpClient, options, chatHistoryService);
            });
            
            return services;
        }
    }
}
