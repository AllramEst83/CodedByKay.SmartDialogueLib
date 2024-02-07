using CodedByKay.SmartDialogueLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodedByKay.SmartDialogueLib.Services
{
    internal interface ISmartDialogueService
    {
        Task<string> SendMessageAsync(Guid chatId, string message, MessageType messageType);

    }
}
