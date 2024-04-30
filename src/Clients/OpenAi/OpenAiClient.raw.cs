using Boundless.OmniAdapter.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.OpenAi;

public partial class OpenAiClient : IDisposable
{
  private readonly JsonSerializerOptions serializerOptions;
  private readonly HttpClient httpClient;

  public OpenAiClient(IHttpClientFactory factory, IOptions<OpenAiSettings> options)
  {
    serializerOptions = new JsonSerializerOptions()
    {
      DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
      Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) },
      ReferenceHandler = ReferenceHandler.IgnoreCycles,
    };

    var key = options.Value.OpenAiApiKey;
    var organization = options.Value.OpenAiOrganization;
    var project = options.Value.OpenAiProject;

    httpClient = factory.CreateClient("OpenAi");
    httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {key}");
    httpClient.DefaultRequestHeaders.Add("User-Agent", "BoundlessAi");
    httpClient.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v2");
    if (organization is not null) httpClient.DefaultRequestHeaders.Add("OpenAI-Organization", organization);
    if (project is not null) httpClient.DefaultRequestHeaders.Add("OpenAI-Project", project);
  }

  public void Dispose()
  {
    httpClient.Dispose();
  }

  public async Task<CompletionResponse?> GetChatAsync(CompletionRequest request, CancellationToken cancellationToken = default)
  {
    ArgumentNullException.ThrowIfNull(request);

    request.Stream = false;

    using var content = JsonContent.Create(request, options: serializerOptions);
    using var response = await httpClient.PostAsync("chat/completions", content, cancellationToken);
    response.EnsureSuccessStatusCode();

    var result = await response.Content.ReadFromJsonAsync<CompletionResponse>(serializerOptions, cancellationToken);
    if (result is not null)
    {
      result.RateLimits = new RateLimits
      {
        LimitRequests = int.Parse(response.Headers.GetValues("x-ratelimit-limit-requests").FirstOrDefault() ?? "0"),
        LimitTokens = int.Parse(response.Headers.GetValues("x-ratelimit-limit-tokens").FirstOrDefault() ?? "0"),
        RemainingRequests = int.Parse(response.Headers.GetValues("x-ratelimit-remaining-requests").FirstOrDefault() ?? "0"),
        RemainingTokens = int.Parse(response.Headers.GetValues("x-ratelimit-remaining-tokens").FirstOrDefault() ?? "0"),
        ResetRequests = response.Headers.GetValues("x-ratelimit-reset-requests").FirstOrDefault(),
        ResetTokens = response.Headers.GetValues("x-ratelimit-reset-tokens").FirstOrDefault()
      };
    }

    return result;
  }


  public async IAsyncEnumerable<CompletionResponse> StreamChatAsync(CompletionRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
  {
    ArgumentNullException.ThrowIfNull(request);

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

      if (partial.Choices.Count == 0)
        continue;

      yield return partial;
    }
  }

};