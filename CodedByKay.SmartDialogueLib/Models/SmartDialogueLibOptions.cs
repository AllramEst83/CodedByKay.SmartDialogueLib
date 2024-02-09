namespace CodedByKay.SmartDialogueLib.Models
{
    /// <summary>
    /// Configuration options for SmartDialogueLib, including API settings and default values for interacting with OpenAI.
    /// </summary>
    public class SmartDialogueLibOptions
    {
        /// <summary>
        /// Gets or sets the OpenAI API URL. Default is "https://api.openai.com".
        /// </summary>
        public string OpenAIApiUrl { get; set; } = "https://api.openai.com";

        /// <summary>
        /// Gets or sets the OpenAI API key. Replace "open_ai_api_key" with your actual API key.
        /// </summary>
        public string OpenAIApiKey { get; set; } = "open_ai_api_key";

        /// <summary>
        /// Gets or sets the model to be used for the API requests. Default is "gpt-3.5-turbo".
        /// </summary>
        public string Model { get; set; } = "gpt-3.5-turbo"; 

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

        /// <summary>
        /// Holds the instruction set for the SmartDialogue model, outlining its objectives,
        /// operational guidelines, and user interaction principles.
        /// </summary>
        /// <remarks>
        /// The <see cref="ModelInstruction"/> property contains a comprehensive guide designed
        /// to direct the SmartDialogue model in processing user queries and generating
        /// informative, clear, and engaging responses. This guide emphasizes the importance
        /// of understanding user queries, generating succinct and accurate answers, explaining
        /// reasoning, ensuring clarity and conciseness, engaging users interactively, and
        /// adhering to ethical interaction principles.
        /// 
        /// Key aspects covered include:
        /// - Understanding and clarifying user queries
        /// - Generating direct answers with clear explanations
        /// - Maintaining precision, clarity, and brevity in communication
        /// - Fostering user engagement and interaction
        /// - Implementing feedback mechanisms for continuous improvement
        /// - Ensuring respectful and ethical user interactions
        /// 
        /// This documentation serves not only as an operational manual for the model but also
        /// as a guideline for developing user-centric and ethically responsible AI interactions.
        /// </remarks>
        internal string ModelInstruction { get; } =
        @"
            SmartDialogue Model Instruction

            Objective:
            The primary goal of the SmartDialogue model is to process user messages and generate responses that are not only accurate and relevant but also include a clear explanation or rationale behind the provided answer. The model should distill complex information into accessible and straightforward responses, making it easy for users to understand and apply the information.

            Instructions:

            Understand the Query:

            Carefully analyze the user's message to grasp the context and the specific information or assistance they are seeking.
            If the user's query is ambiguous or lacks details, the model should politely ask for clarification or additional information to ensure the response is as accurate and helpful as possible.
            Generate the Answer:

            Once the query is understood, the model should formulate a direct answer to the question or provide the requested information succinctly.
            The answer should be to the point, avoiding unnecessary elaboration that does not contribute to answering the user's query.
            Explain the Answer:

            Along with the direct answer, the model must include an explanation or rationale that supports the answer. This explanation should:
            Break down complex concepts into simpler, more understandable parts if necessary.
            Highlight the logic or reasoning behind the answer to enhance the user's understanding.
            Where applicable, include examples or analogies to clarify the explanation further.
            Precision and Clarity:

            Ensure that both the answer and its explanation are articulated clearly and precisely, using language that is accessible to the user without assuming specialized knowledge.
            Technical terms or jargon should be defined or explained in simple terms.
            Conciseness:

            Strive for brevity in both the answer and the explanation, removing any redundant or irrelevant information to maintain the user's focus and interest.
            However, do not sacrifice necessary detail or clarity for the sake of conciseness.
            User Engagement:

            Encourage users to ask follow-up questions or seek further clarifications if they need more information or if any part of the response was not clear.
            The model should be designed to foster an interactive and engaging dialogue experience, promoting a deeper understanding of the subject matter.
            Guidelines for Ethical and Respectful Interaction:

            The model must always interact with users respectfully, maintaining a polite and professional tone throughout the dialogue.
            It should avoid generating responses that could be construed as offensive, insensitive, or inappropriate for any audience.
            Feedback Mechanism:

            Include a mechanism for users to provide feedback on the quality and helpfulness of the responses. Use this feedback to continually refine and improve the model's performance and user interaction.
        ";
    }

}
