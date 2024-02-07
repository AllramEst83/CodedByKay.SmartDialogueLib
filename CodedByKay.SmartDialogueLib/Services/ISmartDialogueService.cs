using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodedByKay.SmartDialogueLib.Services
{
    internal interface ISmartDialogueService
    {
        Task<string> SendMessageAsync(string sessionId, string message);
    }
}
