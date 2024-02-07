using CodedByKay.SmartDialogueLib.Models;
using CodedByKay.SmartDialogueLib.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace CodedByKay.SmartDialogueLib
{
    public static class SmartDialogueLibExtensions
    {
        public static IServiceCollection AddSmartDialogueLib(this IServiceCollection services, Action<SmartDialogueLibOptions> configureOptions)
        {
            var options = new SmartDialogueLibOptions();
            configureOptions(options);

            services.AddHttpClient("OpenAiHttpClient", client =>
            {
                client.BaseAddress = new Uri(options.OpenAiApiKey);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", options.OpenAiApiKey);
            });

            services.AddSingleton<ISmartDialogueService, SmartDialogueService>(serviceProvider =>
            {
                var httpClientFactory = serviceProvider.GetRequiredService<System.Net.Http.IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("OpenAiHttpClient");
                return new SmartDialogueService(httpClient, options.Model);
            });

            return services;
        }
    }
}
