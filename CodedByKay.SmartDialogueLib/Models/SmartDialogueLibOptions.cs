using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodedByKay.SmartDialogueLib.Models
{
    public class SmartDialogueLibOptions
    {
        public string OpenAIApiUrl { get; set; } = "https://api.openai.com/v1/";
        public string Model { get; set; } = "text-davinci-003";
        public string OpenAiApiKey { get; set; } = "open_ai_api_key";
        public int MaxTokens { get; set; } = 0;
        public double Temperature { get; set; } = 0;
        public double TopP { get; set; } = 0;
    }
}
