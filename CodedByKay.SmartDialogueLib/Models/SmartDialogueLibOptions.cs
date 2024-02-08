namespace CodedByKay.SmartDialogueLib.Models
{
    /// <summary>
    /// Configuration options for SmartDialogueLib, including API settings and default values for interacting with OpenAI.
    /// </summary>
    public class SmartDialogueLibOptions
    {
        /// <summary>
        /// Gets or sets the OpenAI API URL. Default is "https://api.openai.com/v1/chat/completions".
        /// </summary>
        public string OpenAIApiUrl { get; set; } = "https://api.openai.com/v1/chat/completions";

        /// <summary>
        /// Gets or sets the model to be used for the API requests. Default is "gpt-3.5-turbo".
        /// </summary>
        public string Model { get; set; } = "gpt-3.5-turbo";

        /// <summary>
        /// Gets or sets the OpenAI API key. Replace "open_ai_api_key" with your actual API key.
        /// </summary>
        public string OpenAIApiKey { get; set; } = "open_ai_api_key";

        /// <summary>
        /// Gets or sets the maximum number of tokens to generate in the completion. Default is 2000.
        /// </summary>
        public int MaxTokens { get; set; } = 2000;

        /// <summary>
        /// Gets or sets the temperature for randomizing the output. Default is 1.
        /// </summary>
        public double Temperature { get; set; } = 1;

        /// <summary>
        /// Gets or sets the top p for nucleus sampling. Default is 1.
        /// </summary>
        public double TopP { get; set; } = 1;

        /// <summary>
        /// Gets or sets the average token length used for estimating token count. Default is 2.85.
        /// </summary>
        /// <remarks>
        /// This value is used to estimate the number of tokens in the input text based on character count.
        /// </remarks>
        public double AverageTokeLenght { get; set; } = 2.85;
    }

}
