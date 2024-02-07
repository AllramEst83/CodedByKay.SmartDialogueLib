using CodedByKay.SmartDialogueLib.Models;

namespace CodedByKay.SmartDialogueLib.Helpers
{
    internal static class ValidationHelpers
    {
        internal static void ValidateOptions(SmartDialogueLibOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.OpenAiApiKey))
            {
                throw new ArgumentException("OpenAI API key cannot be null or empty.", nameof(options.OpenAiApiKey));
            }

            if (string.IsNullOrWhiteSpace(options.Model))
            {
                throw new ArgumentException($"The specified model '{options.Model}' is not supported. Please use one of the following: {string.Join(", ", allowedModels)}", nameof(options.Model));
            }

            try
            {
                var uri = new Uri(options.OpenAIApiUrl);
                if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
                {
                    throw new ArgumentException("The OpenAIApiUrl must be an HTTP or HTTPS URL.", nameof(options.OpenAIApiUrl));
                }
            }
            catch (UriFormatException)
            {
                throw new ArgumentException("The OpenAIApiUrl is not a valid URL.", nameof(options.OpenAIApiUrl));
            }
        }
    }
}
