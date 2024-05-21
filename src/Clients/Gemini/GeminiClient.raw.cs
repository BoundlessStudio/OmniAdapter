using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Gemini;

public partial class GeminiClient : IDisposable
{
  private readonly JsonSerializerOptions serializerOptions;
  private readonly HttpClient httpClient;

  public GeminiClient(IHttpClientFactory factory, IOptions<GeminiSettings> options)
  {
    serializerOptions = new JsonSerializerOptions()
    {
      DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
      Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) },
      ReferenceHandler = ReferenceHandler.IgnoreCycles,
    };

    var key = options.Value.GeminiApiKey;

    httpClient = factory.CreateClient("Gemini");
    httpClient.BaseAddress = new Uri("https://generativelanguage.googleapis.com/v1beta/");
    httpClient.DefaultRequestHeaders.Add("x-goog-api-key", key);
  }


  public void Dispose()
  {
    httpClient.Dispose();
  }


  public async Task<CompletionResponse?> GetChatAsync(CompletionRequest request, CancellationToken cancellationToken = default)
  {
    if (request is null)
      throw new ArgumentNullException(nameof(request));

    using var content = JsonContent.Create(request, options: serializerOptions);
    using var response = await httpClient.PostAsync($"{request.Model}:generateContent", content, cancellationToken);

    response.EnsureSuccessStatusCode();

    var result = await response.Content.ReadFromJsonAsync<CompletionResponse>(serializerOptions, cancellationToken);
    return result;
  }


  //public async IAsyncEnumerable<CompletionResponse> StreamChatAsync(CompletionRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
  //{
  //if (request is null)
  //  throw new ArgumentNullException(nameof(request));

  //using var content = JsonContent.Create(request, options: serializerOptions);
  //using var response = await httpClient.PostAsync($"{request.Model}:streamGenerateContent?alt=sse", content, cancellationToken);
  //response.EnsureSuccessStatusCode();

  //using var stream = await response.Content.ReadAsStreamAsync();
  //using var reader = new StreamReader(stream);
  //while (!reader.EndOfStream)
  //{
  //  cancellationToken.ThrowIfCancellationRequested();

  //  var streamData = await reader.ReadLineAsync();
  //  if (string.IsNullOrWhiteSpace(streamData))
  //    continue;

  //  var eventData = streamData.GetEventStreamData();
  //  if (string.IsNullOrWhiteSpace(eventData))
  //    continue;

  //  if (eventData == "[DONE]")
  //    break;

  //  var partial = JsonSerializer.Deserialize<CompletionResponse>(eventData, serializerOptions);
  //  if (partial is null)
  //    continue;

  //  if (partial.Candidates.Count == 0)
  //    continue;

  //  yield return partial;
  //}
  //}
}