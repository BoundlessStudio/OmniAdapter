using Boundless.OmniAdapter.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Anthropic;

public partial class AnthropicClient
{
  private readonly JsonSerializerOptions serializerOptions;
  private readonly HttpClient httpClient;

  public AnthropicClient(IHttpClientFactory factory, IOptions<AnthropicSettings> options)
  {
    serializerOptions = new JsonSerializerOptions()
    {
      DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
      Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) },
      ReferenceHandler = ReferenceHandler.IgnoreCycles,
    };

    var key = options.Value.AnthropicApiKey;

    httpClient = factory.CreateClient("Anthropic");
    httpClient.BaseAddress = new Uri("https://api.anthropic.com/v1/");
    httpClient.DefaultRequestHeaders.Add("x-api-key", key);
    httpClient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
    httpClient.DefaultRequestHeaders.Add("anthropic-beta", "tools-2024-04-04");
    // httpClient.DefaultRequestHeaders.Add("User-Agent", "BoundlessAi");
  }

  public void Dispose()
  {
    httpClient.Dispose();
  }

  private static RateLimits ExtractRateLimits(HttpResponseMessage response)
  {
    var limits = new Models.RateLimits();

    if(response.Headers.TryGetValues("anthropic-ratelimit-requests-limit", out IEnumerable<string> headerLimitRequests))
    {
      if (int.TryParse(headerLimitRequests.FirstOrDefault(), out int limitRequests))
      {
        limits.LimitRequests = limitRequests;
      }
    }
    if (response.Headers.TryGetValues("anthropic-ratelimit-tokens-limit", out IEnumerable<string> headerLimitTokens))
    {
      if (int.TryParse(headerLimitTokens.FirstOrDefault(), out int limitTokens))
      {
        limits.LimitTokens = limitTokens;
      }
    }
    if (response.Headers.TryGetValues("anthropic-ratelimit-requests-remaining", out IEnumerable<string> headerRemainingRequests))
    {
      if (int.TryParse(headerRemainingRequests.FirstOrDefault(), out int remainingRequests))
      {
        limits.RemainingRequests = remainingRequests;
      }
    }
    if (response.Headers.TryGetValues("anthropic-ratelimit-tokens-remaining", out IEnumerable<string> headerRemainingTokens))
    {
      if (int.TryParse(headerRemainingTokens.FirstOrDefault(), out int remainingTokens))
      {
        limits.RemainingTokens = remainingTokens;
      }
    }
    if (response.Headers.TryGetValues("anthropic-ratelimit-requests-reset", out IEnumerable<string> headerResetRequests))
    {
      if (DateTimeOffset.TryParse(headerResetRequests.FirstOrDefault(), out DateTimeOffset resetRequests))
      {
        limits.ResetRequests = resetRequests - DateTimeOffset.UtcNow;
      }
    }
    if (response.Headers.TryGetValues("anthropic-ratelimit-tokens-reset", out IEnumerable<string> headerResetTokens))
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
    using var response = await httpClient.PostAsync("messages", content, cancellationToken);
    var limits = ExtractRateLimits(response);
    response.EnsureSuccessStatusCode();
    var result = await response.Content.ReadFromJsonAsync<CompletionResponse>(serializerOptions, cancellationToken);
    if(result is not null) result.RateLimits = limits;
    return result;
  }

  

  public async IAsyncEnumerable<string> StreamChatAsync(CompletionRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
  {
    if (request is null)
      throw new ArgumentNullException(nameof(request));

    request.Stream = true;

    if (request?.Tools?.Count > 0)
      throw new NotImplementedException("Streaming is not yet supported with tool use.");

    using var content = JsonContent.Create(request, options: serializerOptions);

    var json = content.ReadAsStringAsync();
    using var response = await httpClient.PostAsync("messages", content, cancellationToken);
    response.EnsureSuccessStatusCode();

    using var stream = await response.Content.ReadAsStreamAsync();
    using var reader = new StreamReader(stream);
    while (!reader.EndOfStream)
    {
      var streamData = await reader.ReadLineAsync();
      if (string.IsNullOrWhiteSpace(streamData))
        continue;

      var eventData = streamData.GetEventStreamData();
      if (string.IsNullOrWhiteSpace(eventData))
        continue;

      var partial = JsonSerializer.Deserialize<ServerEvent>(eventData, serializerOptions);
      if (partial is null)
        continue;

      if (partial.Type != "content_block_delta")
        continue;

      var text = partial?.Delta?.Text ?? string.Empty;
      yield return text;
    }
  }

  
}