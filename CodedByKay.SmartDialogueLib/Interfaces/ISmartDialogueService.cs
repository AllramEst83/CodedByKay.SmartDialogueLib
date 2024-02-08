using CodedByKay.SmartDialogueLib.Models;

namespace CodedByKay.SmartDialogueLib.Interfaces
{
    internal interface ISmartDialogueService
    {
        Task<string> SendChatMessageAsync(Guid chatId, string message, MessageType messageType);

    }
}
