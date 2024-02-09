using CodedByKay.SmartDialogueLib.Interfaces;
using CodedByKay.SmartDialogueLib.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace CodedByKay.SmartDialogueLib.Services
{
    /// <summary>
    /// Service to handle sending messages to OpenAI and managing chat history.
    /// </summary>
    public class SmartDialogueService : ISmartDialogueService
    {
        private readonly SmartDialogueLibOptions _options;
        private readonly IChatHistoryService _chatHistoryService;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the SmartDialogueService with necessary dependencies.
        /// </summary>
        /// <param name="httpClient">HttpClient used for making API requests.</param>
        /// <param name="options">Configuration options for the service.</param>
        /// <param name="chatHistoryService">Service for managing chat history.</param>
        /// <exception cref="ArgumentNullException">Thrown if httpClient is null.</exception>
        public SmartDialogueService(HttpClient httpClient, SmartDialogueLibOptions options, IChatHistoryService chatHistoryService)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options;
            _chatHistoryService = chatHistoryService;
        }

        /// <summary>
        /// Sends a message to the OpenAI API and processes the response.
        /// </summary>
        /// <param name="chatId">The unique identifier for the chat session.</param>
        /// <param name="message">The message to send to OpenAI.</param>
        /// <param name="messageType">The type of the message (User or System).</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the OpenAI response.</returns>
        /// <exception cref="Exception">Thrown if the response from OpenAI does not contain a valid answer or if the request fails.</exception>
        public async Task<string> SendChatMessageAsync(Guid chatId, string message, MessageType messageType)
        {
            // Log the user's message to chat history
            _chatHistoryService.AddChatMessage(message, chatId, MessageType.User);

            // Prepare the list of messages for the OpenAI request, starting with a system prompt
            var messagesList = new List<AssistantModel>()
            {
                new(){ Role = "system", Content = _options.ModelInstruction}
            };

            // Retrieve and add the chat history to the request payload
            var chatHistory = _chatHistoryService.GetChatMessages(chatId);
            foreach (var historyMessage in chatHistory)
            {
                var role = historyMessage.MessageType == MessageType.User ? "user" : "assistant";
                messagesList.Add(new AssistantModel()
                {
                    Role = role,
                    Content = historyMessage.Message
                });
            }

            // Construct the request payload
            OpenAiRequestModel requestPayload = new()
            {
                Messages = [.. messagesList],
                MaxTokens = _options.MaxTokens,
                Temperature = _options.Temperature,
                TopP = _options.TopP,
                Model = _options.Model
            };

            // Serialize the request payload to JSON and send it to OpenAI
            var content = new StringContent(JsonConvert.SerializeObject(requestPayload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/v1/chat/completions", content);

            try
            {
                response.EnsureSuccessStatusCode();

                // Process the response from OpenAI
                string jsonResponse = await response.Content.ReadAsStringAsync();
                JObject parsedResponse = JObject.Parse(jsonResponse);
                var modelAnswerToken = parsedResponse.SelectToken("choices[0].message.content");
                string? modelAnswer = modelAnswerToken?.ToString();

                if (!string.IsNullOrEmpty(modelAnswer))
                {
                    _chatHistoryService.AddChatMessage(modelAnswer, chatId, MessageType.Model);
                    _chatHistoryService.ReCalculateHistoryLength(chatId, _options.MaxTokens);
                    return modelAnswer;
                }
                else
                {
                    throw new Exception("The response from OpenAI did not contain a valid 'modelAnswer'.");
                }
            }
            catch (HttpRequestException ex)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to retrieve response: {response.ReasonPhrase}. Response content: {errorContent}", ex);
            }
        }

        /// <summary>
        /// Deletes the chat history associated with a specific chat ID.
        /// </summary>
        /// <param name="chatId">The unique identifier of the chat to be deleted.</param>
        /// <returns>True if the chat history was successfully removed; otherwise, false.</returns>
        /// <remarks>This method attempts to remove the chat history for the specified chatId. If the chatId does not exist or has already been deleted, the method will return false.</remarks>
        public bool DeleteChatById(Guid chatId)
        {
            // Attempt to delete the chat history by the given chatId.
            var isRemoved = _chatHistoryService.DeleteChatHistoryById(chatId);

            // Return the result of the deletion attempt.
            return isRemoved;
        }

        /// <summary>
        /// Deletes all chat histories managed by the service.
        /// </summary>
        /// <returns>Always returns true to indicate the operation was requested.</returns>
        /// <remarks>This method invokes the chat history service to clear all stored chat histories. It always returns true as an indication that the operation was performed, without specifying whether any chat histories were actually present and deleted.</remarks>
        public bool DeleteAllChats()
        {
            // Request the chat history service to delete all chat histories.
            _chatHistoryService.DeleteAllChatHistories();

            // Return true to indicate the delete operation was executed.
            return true;
        }

    }
}
