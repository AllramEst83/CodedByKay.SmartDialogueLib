# CodedByKay.SmartDialogue Setup Guide

> **Understanding `ConcurrentDictionary` and Memory Management in .NET**

In .NET, the `ConcurrentDictionary` is a thread-safe collection that allows for concurrent modifications from multiple threads without locking the entire collection. Being a reference type, instances of `ConcurrentDictionary` and their contents are stored on the **heap**, managed by the .NET Garbage Collector (GC), which automates memory allocation and deallocation to optimize application performance.

- **Stack vs. Heap**:
  - **Stack**: Used for static memory allocation, storing method frames and variables. Reference variables (like those pointing to a `ConcurrentDictionary`) are stored here, but they point to objects located on the heap.
  - **Heap**: Used for dynamic memory allocation, where instances of reference types (e.g., `ConcurrentDictionary`) reside. The heap's memory is managed by the GC, ensuring efficient use of resources and freeing memory when objects are no longer in use.


## Overview

`CodedByKay.SmartDialogue` is a .NET class library designed to facilitate easy communication with the OpenAI API, integrating seamlessly into .NET web applications. It supports sending messages, receiving responses, and maintaining a chat history.

## Features

### Core Features

- **Middleware Integration**: Easily integrates into the middleware pipeline of a .NET web application.
- **Asynchronous Communication**: Communicates asynchronously with the OpenAI API.
- **Secure API Key Management**: Offers a secure way to manage and use your OpenAI API key.
- **Chat History Management**: Utilizes a `ConcurrentDictionary` for efficient and thread-safe chat history management.

### OpenAI API Communication

- **Send Messages**: Allows sending messages to the OpenAI API.
- **Receive and Forward Responses**: Receives responses from OpenAI and forwards them to the library user.
- **Maintain Chat History**: Keeps a record of the chat history that can be utilized in subsequent API requests for context.

## Setup and Configuration

### Prerequisites

- .NET Core 3.1 SDK or later.
- An OpenAI API key.

### Installation

To install `CodedByKay.SmartDialogue`, add it to your project via NuGet Package Manager or using the .NET CLI:

```shell
dotnet add package CodedByKay.SmartDialogue
```

### Configuration in Startup.cs

Incorporate CodedByKay.SmartDialogue into your project's Startup.cs file to begin using its functionalities.


Register SmartDialogue Services
Then, add `CodedByKay.SmartDialogue` to your service collection:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddSmartDialogue(options =>
    {
        // All values are default for the AddSmartDialogue library
        options.OpenAiApiKey = "your_openai_api_key_here";
        options.Model = "gpt-3.5-turbo";
        options.OpenAIApiUrl = "https://api.openai.com/v1/";
        options.MaxTokens = 2000;
        options.Temperature = 1;
        options.TopP = 1;
        options.AverageTokeLenght = 2.85;
    });
}
```

### Usage
Inject ISmartDialogueService into your controllers or services to utilize the library:

```csharp
using Microsoft.AspNetCore.Mvc;

public class ChatController : ControllerBase
{
    private readonly ISmartDialogueService _smartDialogueService;

    public ChatController(ISmartDialogueServiceFactory smartDialogueServiceFactory)
    {
        _smartDialogueService = smartDialogueServiceFactory.Create();
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest("Message is required.");
        }

        try
        {
            var response = await _smartDialogueService.SendMessageAsync(request.SessionId, request.Message);
            return Ok(new { Response = response });
        }
        catch (Exception ex)
        {
            // Log the exception details
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}

public class ChatRequest
{
    public string SessionId { get; set; }
    public string Message { get; set; }
}

```

<details>
  <summary>SmartDialogue Model Instruction Overview</summary>

  **Objective:**
  - Deliver accurate, relevant, and comprehensible responses.
  - Simplify complex information for easy user comprehension.

  **Understanding the Query:**
  - Context Awareness: Review entire conversation for nuanced understanding.
  - Direct Responses: Address the latest query with focused clarity.
  - Seek Clarification: Politely request more details for vague queries.

  **Generating the Answer:**
  - Direct and Succinct: Provide concise answers, omitting unnecessary elaboration.

  **Explaining the Answer:**
  - Simplify and Rationalize: Break down complex concepts, explaining the rationale clearly.
  - Use Examples/Analogies: Employ examples to enhance understanding.

  **Precision and Clarity:**
  - Accessible Language: Use clear language, simplifying technical terms.

  **Conciseness:**
  - Brevity with Clarity: Eliminate extraneous info, maintaining essential detail.

  **Engaging the User:**
  - Encourage Interaction: Invite follow-up questions to deepen understanding.

  **Ethical and Respectful Interaction Guidelines:**
  - Maintain a respectful, professional tone, avoiding offensive content.

  **Feedback Mechanism:**
  - Implement a feedback option for continuous model improvement.

  This guideline aims to enhance SmartDialogue's effectiveness in delivering user-friendly, informative, and engaging interactions.
</details>


### Conclusion
With CodedByKay.SmartDialogue, you can enhance your .NET web applications by integrating sophisticated chat functionalities powered by the OpenAI API. Follow this guide to set up and start leveraging this powerful library in your projects.
