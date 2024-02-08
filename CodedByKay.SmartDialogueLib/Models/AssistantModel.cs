using Newtonsoft.Json;

namespace CodedByKay.SmartDialogueLib.Models
{
    internal class AssistantModel
    {
        [JsonProperty("role")]
        public string Role { get; set; } = "";
        [JsonProperty("content")]
        public string Content { get; set; } = "";
    }
}
