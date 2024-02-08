using CodedByKay.SmartDialogueLib.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodedByKay.SmartDialogueLib.Models
{
    internal class OpenAiRequestModel
    {
        [JsonProperty("messages")]
        public AssistantModel[] Messages { get; set; } = [];
        [JsonProperty("max_tokens")]
        public int MaxTokens { get; set; } = 0;
        [JsonProperty("temperature")]
        public double Temperature { get; set; } = double.MinValue;
        [JsonProperty("top_p")]
        public double TopP { get; set; } = double.MinValue;
        [JsonProperty("model")]
        public string Model { get; set; } = string.Empty;
    }
}
