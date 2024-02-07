using CodedByKay.SmartDialogueLib.Interfaces;
using CodedByKay.SmartDialogueLib.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodedByKay.SmartDialogueLib.Services
{
    internal class ChatHistoryService : IChatHistoryService
    {
        private readonly ConcurrentDictionary<Guid, List<ChatMessage>> _chatHistories = new();

        public bool AddChatMessage(string message, Guid chatId, MessageType messageType)
        {
            if (chatId == Guid.Empty)
            {
                throw new ArgumentNullException("Please provide a valid chatId.");
            }

            if (messageType == MessageType.Unknown)
            {
                throw new ArgumentException("Please provide a valid MessageType.");
            }

            var messages = _chatHistories.GetOrAdd(chatId, _ => new List<ChatMessage>());
            lock (messages) // Ensure thread safety for list modification
            {
                messages.Add(new ChatMessage()
                {
                    Message = message,
                    MessageType = messageType
                });
            }

            _chatHistories.AddOrUpdate(chatId, messages, (key, oldValue) => messages);

            return true;
        }

        public List<ChatMessage> GetChatMessages(Guid sessionId)
        {
            if (_chatHistories.TryGetValue(sessionId, out var chatHistory))
            {
                return chatHistory;
            }
            else
            {
                return [];
            }
        }
        public bool DeleteChatHistoryById(Guid chatId)
        {
            if (chatId == Guid.Empty)
            {
                throw new ArgumentNullException("Please provide a valid chatId.");
            }

            bool isRemoved = _chatHistories.TryRemove(chatId, out _);

            return isRemoved;
        }

        public void DeleteAllChatHistory()
        {
            _chatHistories.Clear();
        }

        public void ReCalculateHistoryLength(Guid chatId, int maxTokenCount)
        {
            // Check if the specified chat history exists
            if (_chatHistories.TryGetValue(chatId, out var chatHistory))
            {
                // Get the current total token count
                int totalTokenCount = chatHistory.Sum(message => EstimateTokenCount(message.Message));

                // Calculate how many tokens we need to remove
                int tokensToRemove = totalTokenCount - maxTokenCount;

                // Ensure that the chatHistory is sorted from oldest to newest
                chatHistory = [.. chatHistory.OrderBy(x => x.Timestamp)];

                while (tokensToRemove > 0 && chatHistory.Count > 0)
                {
                    // The first message is the oldest due to the ordering above
                    var messageToRemove = chatHistory[0];
                    int messageTokenCount = EstimateTokenCount(messageToRemove.Message);

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

        private static int EstimateTokenCount(string message)
        {
            // Estimate the token count based on the length of the message
            // assuming that on average, one token is 2.85 characters long in your specific use case
            return (int)Math.Ceiling(message.Length / 2.85);
        }
    }
}
