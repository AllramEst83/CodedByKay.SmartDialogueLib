using CodedByKay.SmartDialogueLib.Interfaces;
using CodedByKay.SmartDialogueLib.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Text;

namespace CodedByKay.SmartDialogueLib.Services
{
    public class SmartDialogueService(HttpClient httpClient, SmartDialogueLibOptions options, IChatHistoryService chatHistoryService) : ISmartDialogueService
    {
        private readonly SmartDialogueLibOptions _options = options;
        private readonly IChatHistoryService _chatHistoryService = chatHistoryService;
        private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        public async Task<string> SendMessageAsync(Guid chatId, string message, MessageType messageType)
        {
            // Add the message to chat history
            _chatHistoryService.AddChatMessage(message, chatId, MessageType.User);

            var requestPayload = new
            {
                messages = message.ToArray(),
                max_tokens = _options.MaxTokens,
                temperature = _options.Temperature,
                top_p = _options.TopP,
                model = _options.Model
            };

            // Prepare the request body
            var content = new StringContent(JsonConvert.SerializeObject(requestPayload), Encoding.UTF8, "application/json");

            // Send the message to OpenAI
            var response = await _httpClient.PostAsync("messages", content);
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                JObject parsedResponse = JObject.Parse(jsonResponse);

                string modelAnswer = parsedResponse["choices"][0]["message"]["content"].ToString();

                _chatHistoryService.AddChatMessage(modelAnswer, chatId, MessageType.System);

                return modelAnswer;
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to retrieve response: {response.ReasonPhrase}. Response content: {errorContent}");
            }
        }
    }
}
