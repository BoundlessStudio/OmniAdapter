# OmniAdapter

## Overview
OmniAdapter is a versatile set of interfaces that supports seamless integration with various AI providers across multiple modalities, including text, audio, and images to normalize their differences.

## Table of Contents
1. [Overview](#overview)
2. [Features](#features)
3. [Installation](#installation)
4. [Configuration](#configuration)
5. [Usage](#usage)
6. [Dependencies](#dependencies)
7. [Resilience](#resilience)
8. [FAQ](#faq)
9. [License](#license)
10. [Contributing](#contributing)
11. [ToDo](#ToDo)

## Features
The main features of this package are the clients for the various providers and normalization to a common set of interfaces.

### Interfaces
- IChatCompletion
- IChatStream
- IAudioCompletion
- IImageCompletion

### Clients
- *AmazonQ*
  - [Text Generation](https://aws.amazon.com/q/)
- **Anthropic**
  - [Text Generation](https://docs.anthropic.com/en/docs/text-generation)
- **AzureAI**
  - [Text Generation](https://azure.microsoft.com/en-ca/products/ai-studio#product-overview)
- **Gemini**
  - [Text Generation](https://ai.google.dev/gemini-api/docs/get-started/tutorial?lang=rest)
- **Groq**
  - [Text Generation](https://console.groq.com/docs/quickstart)
- **OpenAI**
  - [Text Generation](https://platform.openai.com/docs/guides/text-generation/chat-completions-api)
  - [Text to Speech](https://platform.openai.com/docs/guides/text-to-speech)
  - [Speech to Text](https://platform.openai.com/docs/guides/speech-to-text)
- **Perplexity**
  - [Text Generation](https://docs.perplexity.ai/reference/post_chat_completions)
- *ElevenLabs*
  - [Text to Speech](https://elevenlabs.io/docs/api-reference/text-to-speech)
- *Leonardo*
  - [Generate Images](https://docs.leonardo.ai/docs/generate-images-using-alchemy)

> :warning: AmazonQ, ElevenLabs, Leonardo are still in active devlopement.

## Installation

To install OmniAdapter, you need to add the package to your .NET project. You can do this using the .NET CLI or by adding it directly to your project file.

```sh
dotnet add package Boundless.OmniAdapter
```

## Configuration
OmniAdapter supports configuration for various AI provider clients. You can configure these settings in your application's configuration files (e.g., `appsettings.json`) or through environment variables.

### Example Configuration in `appsettings.json`

```json
{
  "AnthropicSettings": {
    "AnthropicApiKey": "your-anthropic-api-key"
  },
  "AzureAiSettings": {
    "AzureAiApiKey": "your-azure-ai-api-key",
    "AzureAiEndpoint": "your-azure-ai-endpoint",
    "AzureAiDeployment": "your-azure-ai-deployment"
  },
  "GeminiSettings": {
    "GeminiApiKey": "your-gemini-api-key"
  },
  "GroqSettings": {
    "GroqApiKey": "your-groq-api-key"
  },
  "OpenAiSettings": {
    "OpenAiApiKey": "your-openai-api-key",
    "OpenAiOrganization": "your-openai-organization",
    "OpenAiProject": "your-openai-project"
  },
  "PerplexitySettings": {
    "PerplexityApiKey": "your-perplexity-api-key"
  }
}
```

## Usage

### Setup

To use OmniAdapter in your .NET project, follow these steps to set up and configure the various clients and interfaces.

1. **Create a new .NET project** or open an existing one.
2. **Install the OmniAdapter package** as described in the Installation section.
3. **Configure the services** in your `Program.cs`

```csharp
public class Program
{
    static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                var configuration = hostContext.Configuration;
                services.Configure<OpenAiSettings>(configuration.GetSection("OpenAiSettings"));
                services.AddOpenAiClient();
            });
}
```

These extention methods will Validate the correct settings are aviable on startup, add a keyed singleton for the client and its implemented interfaces, and a keyed http client.

> :exclamation: Each key is unique to the provider's name.


### Functions

There is a build in method within the function class to create a Function object from a .Net Delegate using refelection.

> :rotating_light: The Delegate meet the following condtions:
> 1. The name is lifted from the method name but must match the regex `^[a-zA-Z0-9_-]{1,64}$`. 
> 2. The the functions description comes from the [Description] Attribute on the method.
> 3. must single paramter. That paramter is automaticly converted into a schema for the .Net type useing the JsonSchema.Net. 

```csharp
Function.CreateFrom<T>(T action) where T : Delegate
Function.CreateFrom<T>(string name, string description, T action) where T : Delegate
```

> :new: The [OmniKernel](https://github.com/BoundlessStudio/OmniKernel) is desinged to handle function binding, upon other considerations.


## Dependencies
A list of dependencies and prerequisites for running OmniAdapter.

**JsonSchema.Net**
Tools for parsing, generating, and validating JSON Schemas within .NET applications.

**System.Net.Http.Json**
Extension methods for HttpClient and HttpContent that enable automatic serialization and deserialization using System.Text.Json.

**Microsoft.Extensions.Http**
Features for working with HTTP services in a structured way. Integrates with the Dependency Injection (DI) framework in ASP.NET Core for easier configuration and usage of HttpClient instances.

**Microsoft.Extensions.Options.DataAnnotations**
Validates options using data annotations within the options pattern in ASP.NET Core. Supports attributes like [Required], [Range], [StringLength], etc., for validating configuration properties.

## Resilience

Resilience mechanisms for HttpClient built on the Polly framework via the `Microsoft.Extensions.Http.Resilience` package.

When configuring an HttpClient through the HTTP client factory, the following extensions can add pre-configured hedging or resilience behaviors. These pipelines combine multiple resilience strategies with default settings.

- Total Request Timeout Pipeline: Ensures the request, including hedging attempts, does not exceed the configured time limit.
- Retry Pipeline: Retries the request if the dependency is slow or returns a transient error.
- Rate Limiter Pipeline: Limits the maximum number of requests sent to the dependency.
- Circuit Breaker: Blocks execution if too many direct failures or timeouts are detected.
- Attempt Timeout Pipeline: Limits each request attempt duration and throws an exception if it exceeds the limit.

```csharp
.ConfigureServices((hostContext, services) =>
{
    services.AddOpenAiClient().AddStandardResilienceHandler();
});
```


## FAQ
1. What is the purpose of the IChatCompletion, IChatStream, IAudioCompletion, and IImageCompletion interfaces?
These interfaces provide a standardized way to interact with different AI providers across various modalities (text, audio, images). They ensure that you can switch between providers without changing the core logic of your application.

2. How does OmniAdapter handle API rate limits and errors from different AI providers?
OmniAdapter expose the http client for you to add any resilience strategies such as retry mechanisms, rate limiting, circuit breakers, and total request timeouts to handle API rate limits and errors. These strategies ensure that your application remains stable and responsive even when facing issues with the AI providers.

3. Can I use OmniAdapter in a non-.NET project?
OmniAdapter is designed specifically for .NET projects. If you are using a different technology stack, you would need to look for or develop a similar adapter that integrates with the AI providers you need.

4. How do I add a new AI provider client to OmniAdapter?
To add a new AI provider client, you would need to create a new client class that implements the relevant interfaces (e.g., IChatCompletion, IAudioCompletion). You would also need to update the configuration and registration process to include this new client.

5. What kind of logging and monitoring does OmniAdapter support?
OmniAdapter can integrate with standard .NET logging and monitoring frameworks. You can configure logging and monitoring in your application to track the performance and usage of the AI clients.

6. Is there a way to mock or simulate AI provider responses for testing purposes?
Yes, you can create mock implementations of the interfaces provided by OmniAdapter (e.g., IChatCompletion, IAudioCompletion) to simulate AI provider responses. This allows you to test your application's logic without making actual API calls.

7. How do I handle authentication for different AI providers in OmniAdapter?
Authentication for different AI providers is handled through configuration settings. Each provider requires specific API keys or credentials, which you can set in your configuration files or environment variables.

8. Can I use OmniAdapter for real-time applications?
Yes, OmniAdapter supports real-time applications, especially with the IChatStream interface for streaming text responses. Ensure that you configure the resilience strategies properly to handle the demands of real-time processing.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributing
Follow the [Contributing](CONTRIBUTING.md) for guidelines for contributing to the project, including code style and submission process.
Looking for a place to start? Check the list below or any [issues](issues) labled with `good first issue`

## ToDo
1. Merge the Azure ContentFilter into the FinishReason.ContentFilter
2. Logging
3. Add Client for AmazonQ
4. Add Client for Elevenlabs
5. Add Client for Leonardo
