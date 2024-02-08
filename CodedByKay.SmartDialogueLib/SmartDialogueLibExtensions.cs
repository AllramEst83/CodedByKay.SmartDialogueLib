using CodedByKay.SmartDialogueLib.Helpers;
using CodedByKay.SmartDialogueLib.Interfaces;
using CodedByKay.SmartDialogueLib.Models;
using CodedByKay.SmartDialogueLib.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CodedByKay.SmartDialogueLib
{
    /// <summary>
    /// Extension methods for setting up SmartDialogueLib in an IServiceCollection.
    /// </summary>
    public static class SmartDialogueLibExtensions
    {
        /// <summary>
        /// Adds SmartDialogueLib services to the specified IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <param name="configureOptions">An action to configure the SmartDialogueLibOptions.</param>
        /// <returns>The original IServiceCollection, for chaining further add or configuration method calls.</returns>
        /// <remarks>
        /// This method configures services required for SmartDialogueLib functionality, including
        /// options configuration, HTTP client setup for OpenAI API communication, and singleton
        /// registration for chat history and dialogue services.
        /// </remarks>
        public static IServiceCollection AddSmartDialogue(this IServiceCollection services, Action<SmartDialogueLibOptions> configureOptions)
        {
            // Create a new instance of SmartDialogueLibOptions and configure it using the provided action.
            var options = new SmartDialogueLibOptions();
            configureOptions(options);

            // Validate the configured options.
            ValidationHelpers.ValidateOptions(options);

            // Register the configured options as a singleton in the services collection.
            services.AddSingleton(options);

            // Setup and register an HttpClient configured for accessing the OpenAI API.
            services.AddHttpClient("OpenAiHttpClient", client =>
            {
                client.BaseAddress = new Uri(options.OpenAIApiUrl); // Set the API base address.
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", options.OpenAIApiKey); // Set the authorization header.
            });

            // Register the ChatHistoryService as a singleton, using the configured options.
            services.AddSingleton<IChatHistoryService, ChatHistoryService>(serviceProvider =>
            {
                return new ChatHistoryService(options);
            });

            // Register the SmartDialogueService as a transient service.
            // This ensures a new instance is created for each request, allowing for scoped usage patterns such as per-request configuration.
            services.AddTransient<ISmartDialogueService, SmartDialogueService>(serviceProvider =>
            {
                // Resolve the HttpClientFactory and create a named HttpClient for OpenAI communication.
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("OpenAiHttpClient");

                // Resolve the IChatHistoryService singleton instance.
                var chatHistoryService = serviceProvider.GetRequiredService<IChatHistoryService>();

                // Create and return a new instance of SmartDialogueService with dependencies injected.
                return new SmartDialogueService(httpClient, options, chatHistoryService);
            });

            // Return the IServiceCollection to support method chaining.
            return services;
        }
    }
}
