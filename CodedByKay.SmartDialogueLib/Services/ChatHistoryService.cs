using CodedByKay.SmartDialogueLib.Interfaces;
using CodedByKay.SmartDialogueLib.Models;
using System.Collections.Concurrent;

namespace CodedByKay.SmartDialogueLib.Services
{
    /// <summary>
    /// Manages chat history, including adding, retrieving, and deleting chat messages.
    /// </summary>
    internal class ChatHistoryService : IChatHistoryService
    {
        private readonly ConcurrentDictionary<Guid, List<ChatMessage>> _chatHistories = new();
        private readonly SmartDialogueLibOptions _options;

        /// <summary>
        /// Initializes a new instance of the ChatHistoryService with configuration options.
        /// </summary>
        /// <param name="options">Configuration options for the chat history service.</param>
        public ChatHistoryService(SmartDialogueLibOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Adds a new chat message to the specified chat.
        /// </summary>
        /// <param name="message">The content of the chat message.</param>
        /// <param name="chatId">The unique identifier for the chat.</param>
        /// <param name="messageType">The type of the chat message.</param>
        /// <returns>True if the message was added successfully; otherwise, false.</returns>
        public bool AddChatMessage(string message, Guid chatId, MessageType messageType)
        {
            // Validate input parameters
            if (chatId == Guid.Empty) throw new ArgumentNullException(nameof(chatId), "Please provide a valid chatId.");
            if (messageType == MessageType.Unknown) throw new ArgumentException("Please provide a valid MessageType.", nameof(messageType));

            // Create a new chat message object
            var newMessage = new ChatMessage
            {
                Message = message,
                MessageType = messageType,
                Timestamp = DateTime.UtcNow
            };

            // Add the new message to the chat history dictionary
            _chatHistories.AddOrUpdate(chatId,
                // If chatId doesn't exist, initialize and add the new message
                _ => new List<ChatMessage> { newMessage },
                // If chatId exists, add the new message thread-safely
                (_, existingMessages) =>
                {
                    lock (existingMessages) // Lock the list during update to ensure thread safety
                    {
                        existingMessages.Add(newMessage);
                    }
                    return existingMessages;
                });

            return true;
        }

        /// <summary>
        /// Retrieves ordered chat messages for a given session ID.
        /// </summary>
        /// <param name="sessionId">The unique identifier for the session.</param>
        /// <returns>A list of <see cref="ChatMessage"/> objects ordered by timestamp.</returns>
        public List<ChatMessage> GetChatMessages(Guid sessionId)
        {
            if (_chatHistories.TryGetValue(sessionId, out var chatHistory))
            {
                List<ChatMessage> orderedMessages;
                lock (chatHistory) // Ensure thread-safe read and order
                {
                    orderedMessages = chatHistory.OrderBy(x => x.Timestamp).ToList();
                }
                return orderedMessages;
            }
            else
            {
                return new List<ChatMessage>();
            }
        }

        /// <summary>
        /// Deletes the chat history for a specified chat session.
        /// </summary>
        /// <param name="chatId">The unique identifier of the chat session.</param>
        /// <returns>True if the chat history was successfully deleted, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the chatId is empty.</exception>
        public bool DeleteChatHistoryById(Guid chatId)
        {
            if (chatId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(chatId), "Please provide a valid ChatId.");
            }

            // Attempts to remove the chat history for the specified chatId
            return _chatHistories.TryRemove(chatId, out _);
        }

        /// <summary>
        /// Deletes all chat histories managed by the service.
        /// </summary>
        public void DeleteAllChatHistories()
        {
            _chatHistories.Clear();
        }

        /// <summary>
        /// Adjusts chat history length to meet the maximum token count limit.
        /// </summary>
        /// <param name="chatId">Identifier for the chat session.</param>
        /// <param name="maxTokenCount">Maximum token count allowed in the chat history.</param>
        public void ReCalculateHistoryLength(Guid chatId, int maxTokenCount)
        {
            // Attempt to get the chat history for the given chatId
            if (_chatHistories.TryGetValue(chatId, out var chatHistory))
            {
                // Synchronize access to the chatHistory
                lock (chatHistory)
                {
                    // Calculate the total estimated token count for all messages
                    int totalTokenCount = chatHistory.Sum(message => EstimateTokenCount(message.Message, _options.AverageTokenLength));

                    // Check if the total estimated token count exceeds the maximum allowed
                    if (totalTokenCount > _options.MaxTokens)
                    {
                        // Calculate the excess token count
                        int excessTokens = totalTokenCount - _options.MaxTokens;

                        // Temporarily store messages for removal
                        var messagesForRemoval = new List<ChatMessage>();

                        // Iterate through messages, oldest first, to determine which to remove
                        foreach (var message in chatHistory.OrderBy(x => x.Timestamp))
                        {
                            int messageTokenCount = EstimateTokenCount(message.Message, _options.AverageTokenLength);
                            excessTokens -= messageTokenCount;

                            messagesForRemoval.Add(message);

                            // If the removal of this message brings us within the limit, stop
                            if (excessTokens <= 0)
                            {
                                break;
                            }
                        }

                        // Remove the calculated messages from chat history
                        foreach (var message in messagesForRemoval)
                        {
                            chatHistory.Remove(message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Estimates the token count for a given message based on the average token length.
        /// </summary>
        /// <param name="message">The message for which to estimate the token count.</param>
        /// <param name="averageTokenLenght">The average length of a token, used to calculate the estimated token count for the message.</param>
        /// <returns>The estimated number of tokens in the message.</returns>

        private static int EstimateTokenCount(string message, double averageTokenLenght)
        {
            return (int)Math.Ceiling(message.Length / averageTokenLenght);
        }
    }

}
