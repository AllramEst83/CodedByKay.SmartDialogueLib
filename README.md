# CodedByKay.SmartDialogueLib Setup Guide

## Overview

`CodedByKay.SmartDialogueLib` is a .NET class library designed to facilitate easy communication with the OpenAI Assistant API, integrating seamlessly into .NET web applications. It supports sending messages, receiving responses, and maintaining a chat history.

## Features

### Core Features

- **Middleware Integration**: Easily integrates into the middleware pipeline of a .NET web application.
- **Asynchronous Communication**: Communicates asynchronously with the OpenAI Assistant API.
- **Secure API Key Management**: Offers a secure way to manage and use your OpenAI API key.
- **Chat History Management**: Utilizes a `ConcurrentDictionary` for efficient and thread-safe chat history management.

### OpenAI API Communication

- **Send Messages**: Allows sending messages to the OpenAI Assistant API.
- **Receive and Forward Responses**: Receives responses from OpenAI and forwards them to the library user.
- **Maintain Chat History**: Keeps a record of the chat history that can be utilized in subsequent API requests for context.

## Setup and Configuration

### Prerequisites

- .NET Core 3.1 SDK or later.
- An OpenAI API key.

### Installation

To install `CodedByKay.SmartDialogueLib`, add it to your project via NuGet Package Manager or using the .NET CLI:

```shell
dotnet add package CodedByKay.SmartDialogueLib
```

### Configuration in Startup.cs

Incorporate CodedByKay.SmartDialogueLib into your project's Startup.cs file to begin using its functionalities.


Register SmartDialogueLib Services
Then, add `CodedByKay.SmartDialogueLib` to your service collection:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddSmartDialogueLib(options =>
    {
        options.OpenAiApiKey = "your_openai_api_key_here";
        options.Model = "gpt-3.5-turbo";
        options.OpenAIApiUrl = "https://api.openai.com/v1/";
        options.MaxTokens = 2000;
        options.Temperature = 1;
        options.TopP = 1;
    });
}
```

### Usage
Inject ISmartDialogueService into your controllers or services to utilize the library:

```csharp
using Microsoft.AspNetCore.Mvc;

public class ChatController : ControllerBase
{
    private readonly ISmartDialogueService _dialogueService;

    public ChatController(ISmartDialogueService dialogueService)
    {
        _dialogueService = dialogueService;
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
            var response = await _dialogueService.SendMessageAsync(request.SessionId, request.Message);
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

### Conclusion
With CodedByKay.SmartDialogueLib, you can enhance your .NET web applications by integrating sophisticated chat functionalities powered by the OpenAI Assistant API. Follow this guide to set up and start leveraging this powerful library in your projects.
