using Boundless.OmniAdapter.Anthropic;
using Boundless.OmniAdapter.AzureAi;
using Boundless.OmniAdapter.Groq;
using Boundless.OmniAdapter.Interfaces;
using Boundless.OmniAdapter.Kernel;
using Boundless.OmniAdapter.OpenAi;
using Boundless.OmniAdapter.Perplexity;
using Microsoft.Extensions.DependencyInjection;

namespace Boundless.OmniAdapter;

public static class ServiceCollectionExtensions
{
  public static void AddOpenAiClient(this IServiceCollection services)
  {
    const string key = "OpenAi";
    services.AddKeyedSingleton<IChatCompletion, OpenAiClient>(key);
    services.AddKeyedSingleton<IChatStream, OpenAiClient>(key);
    services.AddKeyedSingleton<IAudioCompletion, OpenAiClient>(key);
    services.AddKeyedSingleton<IImageCompletion, OpenAiClient>(key);
    services.AddSingleton<OpenAiClient>();
  }
  public static void AddPerplexityClient(this IServiceCollection services)
  {
    const string key = "Perplexity";
    services.AddKeyedSingleton<IChatCompletion, PerplexityClient>(key);
    services.AddKeyedSingleton<IChatStream, PerplexityClient>(key);
    services.AddSingleton<PerplexityClient>();
  }
  public static void AddAzureAiClient(this IServiceCollection services)
  {
    const string key = "AzureAi";
    services.AddKeyedSingleton<IChatCompletion, AzureAiClient>(key);
    services.AddKeyedSingleton<IChatStream, AzureAiClient>(key);
    services.AddSingleton<AzureAiClient>();

  }
  public static void AddAnthropicClient(this IServiceCollection services)
  {
    const string key = "Anthropic";
    services.AddKeyedSingleton<IChatCompletion, AnthropicClient>(key);
    services.AddKeyedSingleton<IChatStream, AnthropicClient>(key);
    services.AddSingleton<AnthropicClient>();
  }
  public static void AddGroqClient(this IServiceCollection services)
  {
    const string key = "Groq";
    services.AddKeyedSingleton<IChatCompletion, GroqClient>(key);
    services.AddKeyedSingleton<IChatStream, GroqClient>(key);
    services.AddSingleton<GroqClient>();
  }

  public static void AddKernel(this IServiceCollection services, Action<IServiceProvider, KernelBuilder> fn)
  {
    services.AddSingleton<IKernel>(sp =>
    {
      var builder = new KernelBuilder();
      fn(sp, builder);
      return builder.Build();
    });
  }

  public static void AddKernel(this IServiceCollection services, string key, Action<IServiceProvider, KernelBuilder> fn)
  {
    services.AddKeyedSingleton<IKernel>(key, (sp, _) =>
    {
      var builder = new KernelBuilder();
      fn(sp, builder);
      return builder.Build();
    });
  }
}
