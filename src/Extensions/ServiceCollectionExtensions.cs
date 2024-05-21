using Boundless.OmniAdapter.Anthropic;
using Boundless.OmniAdapter.AzureAi;
using Boundless.OmniAdapter.Gemini;
using Boundless.OmniAdapter.Groq;
using Boundless.OmniAdapter.Interfaces;
using Boundless.OmniAdapter.OpenAi;
using Boundless.OmniAdapter.Perplexity;
using Microsoft.Extensions.DependencyInjection;

namespace Boundless.OmniAdapter;

public static class ServiceCollectionExtensions
{
  public static IHttpClientBuilder AddOpenAiClient(this IServiceCollection services)
  {
    const string key = "OpenAi";
    services.AddOptionsWithValidateOnStart<OpenAiSettings>().ValidateDataAnnotations();
    services.AddKeyedSingleton<IChatCompletion, OpenAiClient>(key);
    services.AddKeyedSingleton<IChatStream, OpenAiClient>(key);
    services.AddKeyedSingleton<IAudioCompletion, OpenAiClient>(key);
    services.AddKeyedSingleton<IImageCompletion, OpenAiClient>(key);
    services.AddSingleton<OpenAiClient>();
    return services.AddHttpClient(key);
  }
  public static IHttpClientBuilder AddPerplexityClient(this IServiceCollection services)
  {
    const string key = "Perplexity";
    services.AddOptionsWithValidateOnStart<PerplexitySettings>().ValidateDataAnnotations();
    services.AddKeyedSingleton<IChatCompletion, PerplexityClient>(key);
    services.AddKeyedSingleton<IChatStream, PerplexityClient>(key);
    services.AddSingleton<PerplexityClient>();
    return services.AddHttpClient(key);
  }
  public static IHttpClientBuilder AddAzureAiClient(this IServiceCollection services)
  {
    const string key = "AzureAi";
    services.AddOptionsWithValidateOnStart<AzureAiSettings>().ValidateDataAnnotations();
    services.AddKeyedSingleton<IChatCompletion, AzureAiClient>(key);
    services.AddKeyedSingleton<IChatStream, AzureAiClient>(key);
    services.AddSingleton<AzureAiClient>();
    return services.AddHttpClient(key);

  }
  public static IHttpClientBuilder AddAnthropicClient(this IServiceCollection services)
  {
    const string key = "Anthropic";
    services.AddOptionsWithValidateOnStart<AnthropicSettings>().ValidateDataAnnotations();
    services.AddKeyedSingleton<IChatCompletion, AnthropicClient>(key);
    services.AddKeyedSingleton<IChatStream, AnthropicClient>(key);
    services.AddSingleton<AnthropicClient>();
    return services.AddHttpClient(key);
  }
  public static IHttpClientBuilder AddGroqClient(this IServiceCollection services)
  {
    const string key = "Groq";
    services.AddOptionsWithValidateOnStart<GroqSettings>().ValidateDataAnnotations();
    services.AddKeyedSingleton<IChatCompletion, GroqClient>(key);
    services.AddKeyedSingleton<IChatStream, GroqClient>(key);
    services.AddSingleton<GroqClient>();
    return services.AddHttpClient(key);
  }

  public static IHttpClientBuilder AddGeminiClient(this IServiceCollection services)
  {
    const string key = "Gemini";
    services.AddOptionsWithValidateOnStart<GeminiSettings>().ValidateDataAnnotations();
    services.AddKeyedSingleton<IChatCompletion, GeminiClient>(key);
    services.AddSingleton<GeminiClient>();
    return services.AddHttpClient(key);
  }
}
