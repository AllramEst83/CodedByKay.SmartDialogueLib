using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Text;

namespace CodedByKay.SmartDialogueLib.Services
{
    public class SmartDialogueService : ISmartDialogueService
    {
        private readonly ConcurrentDictionary<string, List<string>> _chatHistories = new();
        private readonly HttpClient _httpClient;
        private readonly string _model;

        public SmartDialogueService(HttpClient httpClient, string model)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _model = model;
        }

        public async Task<string> SendMessageAsync(string sessionId, string message)
        {
            // Add the message to chat history
            var messages = _chatHistories.GetOrAdd(sessionId, _ => new List<string>());
            lock (messages) // Ensure thread safety for list modification
            {
                messages.Add(message);
            }

            // Prepare the request body
            var content = new StringContent(JsonConvert.SerializeObject(new { model = _model, prompt = message }), Encoding.UTF8, "application/json");

            // Send the message to OpenAI
            var response = await _httpClient.PostAsync("messages", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            // Assume responseContent is the response from OpenAI; process as needed

            // Optionally, add the response to the chat history
            lock (messages)
            {
                messages.Add(responseContent);
            }

            return responseContent;
        }

    }
}
