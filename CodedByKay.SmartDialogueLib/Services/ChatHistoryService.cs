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
        /// Recalculates and trims the chat history for a specified chat session to ensure it does not exceed a specified token count.
        /// </summary>
        /// <param name="chatId">The unique identifier of the chat session.</param>
        /// <param name="maxTokenCount">The maximum allowed token count for the chat history.</param>
        public void ReCalculateHistoryLength(Guid chatId, int maxTokenCount)
        {
            // Check if the specified chat history exists
            if (_chatHistories.TryGetValue(chatId, out var chatHistory))
            {
                // Get the current total token count
                int totalTokenCount = chatHistory.Sum(message => EstimateTokenCount(message.Message, _options.AverageTokeLenght));

                // Calculate how many tokens we need to remove
                int tokensToRemove = totalTokenCount - _options.MaxTokens;

                // Ensure that the chatHistory is sorted from oldest to newest
                chatHistory = [.. chatHistory.OrderBy(x => x.Timestamp)];

                while (tokensToRemove > 0 && chatHistory.Count > 0)
                {
                    // The first message is the oldest due to the ordering above
                    var messageToRemove = chatHistory[0];
                    int messageTokenCount = EstimateTokenCount(messageToRemove.Message, _options.AverageTokeLenght);

                    // Only remove the message if doing so won't make tokensToRemove negative
                    if (tokensToRemove - messageTokenCount >= 0)
                    {
                        tokensToRemove -= messageTokenCount;
                        chatHistory.RemoveAt(0); // Removes the oldest message
                    }
                    else
                    {
                        // If removing the message would make tokensToRemove negative,
                        // break out of the loop as we can't remove any more messages without going below zero.
                        break;
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
