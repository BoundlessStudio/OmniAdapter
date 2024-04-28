using Boundless.OmniAdapter.Anthropic.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Anthropic;

public class AnthropicClient
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

  /// <summary>
  /// Creates a completion for the chat message.
  /// </summary>
  /// <param name="chatRequest">The chat request which contains the message content, <see cref="CompletionRequest"/>.</param>
  /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
  /// <exception cref="ArgumentNullException"></exception>
  /// <exception cref="InvalidOperationException"></exception>
  /// <exception cref="HttpRequestException"></exception>
  /// <exception cref="TaskCanceledException"></exception>
  /// <exception cref="UriFormatException"></exception>
  /// <returns><see cref="CompletionResponse"/>.</returns>
  public async Task<CompletionResponse?> GetChatAsync(CompletionRequest request, CancellationToken cancellationToken = default)
  {
    ArgumentNullException.ThrowIfNull(request);

    request.Stream = false;

    using var content = JsonContent.Create(request, options: serializerOptions);
    using var response = await httpClient.PostAsync("messages", content, cancellationToken);
    response.EnsureSuccessStatusCode();
    var result = await response.Content.ReadFromJsonAsync<CompletionResponse>(serializerOptions, cancellationToken);
    if (result is not null)
    {
      result.RateLimits = new RateLimits
      {
        LimitRequests = int.Parse(response.Headers.GetValues("anthropic-ratelimit-requests-limit").FirstOrDefault() ?? "0"),
        LimitTokens = int.Parse(response.Headers.GetValues("anthropic-ratelimit-tokens-limit").FirstOrDefault() ?? "0"),
        RemainingRequests = int.Parse(response.Headers.GetValues("anthropic-ratelimit-requests-remaining").FirstOrDefault() ?? "0"),
        RemainingTokens = int.Parse(response.Headers.GetValues("anthropic-ratelimit-tokens-remaining").FirstOrDefault() ?? "0"),
        ResetRequests = response.Headers.GetValues("anthropic-ratelimit-requests-reset").FirstOrDefault(),
        ResetTokens = response.Headers.GetValues("anthropic-ratelimit-tokens-reset").FirstOrDefault()
      };
    }

    return result;
  }

  /// <summary>
  /// Created a completion for the chat message and stream the results as they come in.
  /// </summary>
  /// <param name="chatRequest">The chat request which contains the message content.</param>
  /// <exception cref="ArgumentNullException"></exception>
  /// <exception cref="InvalidOperationException"></exception>
  /// <exception cref="HttpRequestException"></exception>
  /// <exception cref="TaskCanceledException"></exception>
  /// <exception cref="UriFormatException"></exception>
  /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
  /// <returns><see cref="ChatChuckResponse"/>.</returns>
  public async IAsyncEnumerable<string> StreamChatAsync(CompletionRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
  {
    ArgumentNullException.ThrowIfNull(request);

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

  public void Dispose()
  {
    httpClient.Dispose();
  }
}