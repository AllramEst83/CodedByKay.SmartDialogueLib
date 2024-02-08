using CodedByKay.SmartDialogueLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodedByKay.SmartDialogueLib.Interfaces
{
    public interface IChatHistoryService
    {
        bool AddChatMessage(string message, Guid chatId, MessageType messageType);
        List<ChatMessage> GetChatMessages(Guid chatId);
        bool DeleteChatHistoryById(Guid chatId);
        void DeleteAllChatHistories();
        void ReCalculateHistoryLength(Guid chatId, int maxTokenCount);
    }
}
