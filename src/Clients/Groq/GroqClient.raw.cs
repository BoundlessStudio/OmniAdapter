using Boundless.OmniAdapter.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Groq;

public partial class GroqClient : IDisposable
{
  private readonly JsonSerializerOptions serializerOptions;
  private readonly HttpClient httpClient;

  public GroqClient(IHttpClientFactory factory, IOptions<GroqSettings> options)
  {
    serializerOptions = new JsonSerializerOptions()
    {
      DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
      Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) },
      ReferenceHandler = ReferenceHandler.IgnoreCycles,
    };

    var key = options.Value.GroqApiKey;

    httpClient = factory.CreateClient("Groq");
    httpClient.BaseAddress = new Uri("https://api.groq.com/openai/v1/");
    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {key}");
    httpClient.DefaultRequestHeaders.Add("User-Agent", "BoundlessAi");
  }

  public void Dispose()
  {
    httpClient.Dispose();
  }

  private static RateLimits ExtractRateLimits(HttpResponseMessage response)
  {
    var limits = new Models.RateLimits();

    if (response.Headers.TryGetValues("x-ratelimit-limit-requests", out IEnumerable<string> headerLimitRequests))
    {
      if (int.TryParse(headerLimitRequests.FirstOrDefault(), out int limitRequests))
      {
        limits.LimitRequests = limitRequests;
      }
    }
    if (response.Headers.TryGetValues("x-ratelimit-limit-tokens", out IEnumerable<string> headerLimitTokens))
    {
      if (int.TryParse(headerLimitTokens.FirstOrDefault(), out int limitTokens))
      {
        limits.LimitTokens = limitTokens;
      }
    }
    if (response.Headers.TryGetValues("x-ratelimit-remaining-requests", out IEnumerable<string> headerRemainingRequests))
    {
      if (int.TryParse(headerRemainingRequests.FirstOrDefault(), out int remainingRequests))
      {
        limits.RemainingRequests = remainingRequests;
      }
    }
    if (response.Headers.TryGetValues("x-ratelimit-remaining-tokens", out IEnumerable<string> headerRemainingTokens))
    {
      if (int.TryParse(headerRemainingTokens.FirstOrDefault(), out int remainingTokens))
      {
        limits.RemainingTokens = remainingTokens;
      }
    }
    if (response.Headers.TryGetValues("x-ratelimit-reset-requests", out IEnumerable<string> headerResetRequests))
    {
      if (DateTimeOffset.TryParse(headerResetRequests.FirstOrDefault(), out DateTimeOffset resetRequests))
      {
        limits.ResetRequests = resetRequests - DateTimeOffset.UtcNow;
      }
    }
    if (response.Headers.TryGetValues("x-ratelimit-reset-tokens", out IEnumerable<string> headerResetTokens))
    {
      if (DateTimeOffset.TryParse(headerResetTokens.FirstOrDefault(), out DateTimeOffset resetTokens))
      {
        limits.ResetTokens = resetTokens - DateTimeOffset.UtcNow;
      }
    }
    return limits;
  }

  public async Task<CompletionResponse?> GetChatAsync(CompletionRequest request, CancellationToken cancellationToken = default)
  {
    if (request is null)
      throw new ArgumentNullException(nameof(request));

    request.Stream = false;

    using var content = JsonContent.Create(request, options: serializerOptions);
    using var response = await httpClient.PostAsync("chat/completions", content, cancellationToken);
    var limits = ExtractRateLimits(response);
    
    response.EnsureSuccessStatusCode();
    var result = await response.Content.ReadFromJsonAsync<CompletionResponse>(serializerOptions, cancellationToken);
    if (result is not null) result.RateLimits = limits;

    return result;
  }

  public async IAsyncEnumerable<CompletionResponse> StreamChatAsync(CompletionRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
  {
    if (request is null)
      throw new ArgumentNullException(nameof(request));

    request.Stream = true;

    using var content = JsonContent.Create(request, options: serializerOptions);
    using var response = await httpClient.PostAsync("chat/completions", content, cancellationToken);
    response.EnsureSuccessStatusCode();

    using var stream = await response.Content.ReadAsStreamAsync();
    using var reader = new StreamReader(stream);
    while (!reader.EndOfStream)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var streamData = await reader.ReadLineAsync();
      if (string.IsNullOrWhiteSpace(streamData))
        continue;

      var eventData = streamData.GetEventStreamData();
      if (string.IsNullOrWhiteSpace(eventData))
        continue;

      if (eventData == "[DONE]")
        break;

      var partial = JsonSerializer.Deserialize<CompletionResponse>(eventData, serializerOptions);
      if (partial is null)
        continue;

      yield return partial;
    }
  }

 
}
