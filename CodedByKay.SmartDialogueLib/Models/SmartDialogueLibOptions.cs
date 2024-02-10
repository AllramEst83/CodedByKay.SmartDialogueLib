namespace CodedByKay.SmartDialogueLib.Models
{
    /// <summary>
    /// Configuration options for SmartDialogueLib, including API settings and default values for interacting with OpenAI.
    /// </summary>
    public class SmartDialogueLibOptions
    {
        /// <summary>
        /// Gets or sets the OpenAI API URL. Default is "https://api.openai.com/v1/".
        /// </summary>
        public string OpenAIApiUrl { get; set; } = "https://api.openai.com/v1/";

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
        public double AverageTokenLength { get; set; } = 2.85;

        /// <summary>
        /// Holds the refined instruction set for the SmartDialogue model, outlining its objectives,
        /// methodological approach, and principles for user interaction.
        /// </summary>
        /// <remarks>
        /// The <see cref="ModelInstruction"/> property encapsulates a streamlined guide crafted
        /// to steer the SmartDialogue model in processing user queries and generating
        /// responses that are not only precise and relevant but also easily understandable and engaging. 
        /// This guide accentuates the model's engagement in distilling complex information into 
        /// accessible responses, ensuring users can grasp and apply the information effectively.
        /// 
        /// Revised aspects include:
        /// - Context-aware analysis of conversation threads for nuanced understanding.
        /// - Delivery of direct and relevant responses to the latest user query.
        /// - Solicitation of further details for queries lacking specificity.
        /// - Formulation of succinct, direct answers supplemented with clear, rational explanations.
        /// - Utilization of examples or analogies to elucidate complex concepts.
        /// - Adherence to clarity and accessibility in communication, simplifying technical jargon.
        /// - Emphasis on conciseness, retaining essential detail without unnecessary elaboration.
        /// - Encouragement of interactive dialogue, inviting users to probe deeper into topics.
        /// - Commitment to respectful, professional, and ethical user interactions.
        /// - Incorporation of a feedback mechanism for ongoing refinement and user satisfaction.
        /// 
        /// Updated to reflect a more focused and user-friendly approach, this documentation acts 
        /// as both an operational framework for the model and a benchmark for developing AI interactions
        /// that prioritize user comprehension, engagement, and ethical standards.
        /// </remarks>
        public string ModelInstruction { get; set; } =
        @"
            SmartDialogue Model Instruction Overview

            Objective:
            - Deliver accurate, relevant, and comprehensible responses.
            - Simplify complex information for easy user comprehension.

            Understanding the Query:
            - Context Awareness: Review entire conversation for nuanced understanding.
            - Direct Responses: Address the latest query with focused clarity.
            - Seek Clarification: Politely request more details for vague queries.

            Generating the Answer:
            - Direct and Succinct: Provide concise answers, omitting unnecessary elaboration.

            Explaining the Answer:
            - Simplify and Rationalize: Break down complex concepts, explaining the rationale clearly.
            - Use Examples/Analogies: Employ examples to enhance understanding.

            Precision and Clarity:
            - Accessible Language: Use clear language, simplifying technical terms.

            Conciseness:
            - Brevity with Clarity: Eliminate extraneous info, maintaining essential detail.

            Engaging the User:
            - Encourage Interaction: Invite follow-up questions to deepen understanding.

            Ethical and Respectful Interaction Guidelines:
            - Maintain a respectful, professional tone, avoiding offensive content.

            Feedback Mechanism:
            - Implement a feedback option for continuous model improvement.

            This guideline aims to enhance SmartDialogue's effectiveness in delivering user-friendly, informative, and engaging interactions.

        ";
    }

}
