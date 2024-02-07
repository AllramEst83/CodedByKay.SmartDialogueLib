using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodedByKay.SmartDialogueLib.Models
{
    internal class OpenAiRequestModel
    {
        public string[] Messages { get; set; } = [];
        public int MaxTokens { get; set; } = 0;
        public double Temperature { get; set; } = double.MinValue;
        public double TopP { get; set; } = double.MinValue;
        public string Model { get; set; } = string.Empty;
    }
}
