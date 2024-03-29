﻿using CodedByKay.SmartDialogueLib.Models;

namespace CodedByKay.SmartDialogueLib.Interfaces
{
    public interface ISmartDialogueService
    {
        Task<string> SendChatMessageAsync(Guid chatId, string message);

    }
}
