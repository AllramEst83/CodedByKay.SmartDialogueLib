using CodedByKay.SmartDialogueLib.Models;

namespace CodedByKay.SmartDialogueLib.Helpers
{
    /// <summary>
    /// Provides utility methods for validating configuration options.
    /// </summary>
    internal static class ValidationHelpers
    {
        /// <summary>
        /// Validates the provided <see cref="SmartDialogueLibOptions"/> to ensure they are correctly configured.
        /// </summary>
        /// <param name="options">The options to validate.</param>
        /// <exception cref="ArgumentException">Thrown if any configuration option is invalid.</exception>
        internal static void ValidateOptions(SmartDialogueLibOptions options)
        {
            // Ensure the OpenAI API key is not null, empty, or consists only of white-space characters.
            if (string.IsNullOrWhiteSpace(options.OpenAIApiKey))
            {
                throw new ArgumentException("OpenAI API key cannot be null or empty.", nameof(options.OpenAIApiKey));
            }

            // Attempt to create a URI from the OpenAIApiUrl to verify its format.
            try
            {
                var uri = new Uri(options.OpenAIApiUrl);

                // Ensure the scheme of the URI is either HTTP or HTTPS.
                if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
                {
                    throw new ArgumentException("The OpenAIApiUrl must be an HTTP or HTTPS URL.", nameof(options.OpenAIApiUrl));
                }
            }
            catch (UriFormatException)
            {
                // If a UriFormatException is caught, the OpenAIApiUrl is not a valid URL format.
                throw new ArgumentException("The OpenAIApiUrl is not a valid URL.", nameof(options.OpenAIApiUrl));
            }
        }
    }

}
