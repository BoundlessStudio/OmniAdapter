using Boundless.OmniAdapter.Anthropic;
using Boundless.OmniAdapter.Models;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
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


  private static RateLimits ExtractRateLimits(HttpResponseMessage response)
  {
    var limits = new Models.RateLimits();

    if (int.TryParse(response.Headers.GetValues("x-ratelimit-limit-requests").FirstOrDefault(), out int limitRequests))
    {
      limits.LimitRequests = limitRequests;
    }
    if (int.TryParse(response.Headers.GetValues("x-ratelimit-limit-tokens").FirstOrDefault(), out int limitTokens))
    {
      limits.LimitTokens = limitTokens;
    }
    if (int.TryParse(response.Headers.GetValues("x-ratelimit-remaining-requests").FirstOrDefault(), out int remainingRequests))
    {
      limits.RemainingRequests = remainingRequests;
    }
    if (int.TryParse(response.Headers.GetValues("x-ratelimit-remaining-tokens").FirstOrDefault(), out int remainingTokens))
    {
      limits.RemainingTokens = remainingTokens;
    }
    if (DateTimeOffset.TryParse(response.Headers.GetValues("x-ratelimit-reset-requests").FirstOrDefault(), out DateTimeOffset resetRequests))
    {
      limits.ResetRequests = resetRequests - DateTimeOffset.UtcNow;
    }
    if (DateTimeOffset.TryParse(response.Headers.GetValues("x-ratelimit-reset-tokens").FirstOrDefault(), out DateTimeOffset resetTokens))
    {
      limits.ResetTokens = resetTokens - DateTimeOffset.UtcNow;
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

      if (partial.Choices.Count == 0)
        continue;

      yield return partial;
    }
  }

  public async Task<TranscriptionResponse?> GetTranscriptionAsync(TranscriptionRequest request, CancellationToken cancellationToken = default)
  {
    if (request is null)
      throw new ArgumentNullException(nameof(request));

    var content = new MultipartFormDataContent();
    content.Add(new StreamContent(request.File), "file", request.FileName);
    content.Add(new StringContent(request.Model), "model");
    content.Add(new StringContent("json"), "response_format");

    // Adding other properties if they are not null
    if (request.Language != null) content.Add(new StringContent(request.Language), "language");
    if (request.Prompt != null) content.Add(new StringContent(request.Prompt), "prompt");
    if (request.Temperature.HasValue) content.Add(new StringContent(request.Temperature.Value.ToString()), "temperature");
    
    using var response = await httpClient.PostAsync("audio/transcriptions ", content, cancellationToken);
    response.EnsureSuccessStatusCode();

    var result = await response.Content.ReadFromJsonAsync<TranscriptionResponse>(serializerOptions, cancellationToken);
    return result;
  }

  public async Task<TranscriptionVerboseResponse?> GetVerboseTranscriptionAsync(VerboseTranscriptionRequest request, CancellationToken cancellationToken = default)
  {
    if (request is null)
      throw new ArgumentNullException(nameof(request));

    var content = new MultipartFormDataContent();
    content.Add(new StreamContent(request.File), "file", request.FileName);
    content.Add(new StringContent(request.Model), "model");
    content.Add(new StringContent("verbose_json"), "response_format");

    // Adding other properties if they are not null
    if (request.Language != null) content.Add(new StringContent(request.Language), "language");
    if (request.Prompt != null) content.Add(new StringContent(request.Prompt), "prompt");
    if (request.Temperature.HasValue) content.Add(new StringContent(request.Temperature.Value.ToString()), "temperature");
    if (request.TimestampGranularities != null && request.TimestampGranularities.Count > 0) content.Add(new StringContent(string.Join(",", request.TimestampGranularities)), "timestampGranularities");

    using var response = await httpClient.PostAsync("audio/transcriptions ", content, cancellationToken);
    response.EnsureSuccessStatusCode();

    var result = await response.Content.ReadFromJsonAsync<TranscriptionVerboseResponse>(serializerOptions, cancellationToken);
    return result;
  }

  public async Task<string?> GetTranscriptionAsync(TranscriptionRequest request, TextTranscriptionFormat format, CancellationToken cancellationToken = default)
  {
    if (request is null)
      throw new ArgumentNullException(nameof(request));

    var content = new MultipartFormDataContent();
    content.Add(new StreamContent(request.File), "file", request.FileName);
    content.Add(new StringContent(request.Model), "model");
    switch (format)
    {
      case TextTranscriptionFormat.Srt:
        content.Add(new StringContent("srt"), "response_format");
        break;
      case TextTranscriptionFormat.Vtt:
        content.Add(new StringContent("vtt"), "response_format");
        break;
      case TextTranscriptionFormat.Text:
      default:
        content.Add(new StringContent("text"), "response_format");
        break;
    }
    
    // Adding other properties if they are not null
    if (request.Language != null) content.Add(new StringContent(request.Language), "language");
    if (request.Prompt != null) content.Add(new StringContent(request.Prompt), "prompt");
    if (request.Temperature.HasValue) content.Add(new StringContent(request.Temperature.Value.ToString()), "temperature");

    using var response = await httpClient.PostAsync("audio/transcriptions ", content, cancellationToken);
    response.EnsureSuccessStatusCode();

    var result = await response.Content.ReadAsStringAsync();
    return result;
  }

  public async Task<Stream> GetSpeechAsync(SpeechRequest request, CancellationToken cancellationToken = default)
  {
    if (request is null)
      throw new ArgumentNullException(nameof(request));

    using var content = JsonContent.Create(request, options: serializerOptions);
    using var response = await httpClient.PostAsync("audio/speech", content, cancellationToken);
    response.EnsureSuccessStatusCode();

    var result = await response.Content.ReadAsStreamAsync();
    return result;
  }

  public async Task<ImageResponse?> GetImageAsync(ImageRequest request, CancellationToken cancellationToken = default)
  {
    if (request is null)
      throw new ArgumentNullException(nameof(request));

    if (string.IsNullOrEmpty(request.Prompt))
      throw new ValidationException("Prompt is required");

    if (request.Prompt.Length > 4000)
      throw new ValidationException("Prompt cannot be longer than 4000 characters.");

    if (request.Model == "dall-e-2")
    {
      var sizes = new[] { "256x256", "512x512", "1024x1024" };
      if (request.Size is not null && sizes.Contains(request.Size) == false)
        throw new ValidationException("Size must be one of '256x256', '512x512', or '1024x1024' for 'dall-e-2'.");
    }
    else if (request.Model == "dall-e-3")
    {
      var sizes = new[] { "1024x1024", "1792x1024", "1024x1792" };
      if (request.Size is not null && sizes.Contains(request.Size) == false)
        throw new ValidationException("Size must be one of '1024x1024', '1792x1024', or '1024x1792' for 'dall-e-3'.");

      if (string.IsNullOrEmpty(request.Style) == false && request.Style != "vivid" && request.Style != "natural")
        throw new ValidationException("Style must be 'vivid' or 'natural' for 'dall-e-3'.");
    }
    else
    {
      throw new ValidationException("Model must be either 'dall-e-2' or 'dall-e-3'.");
    } 

    using var content = JsonContent.Create(request, options: serializerOptions);
    using var response = await httpClient.PostAsync("/images/generations", content, cancellationToken);
    response.EnsureSuccessStatusCode();

    var result = await response.Content.ReadFromJsonAsync<ImageResponse>(serializerOptions, cancellationToken);
    return result;
  }
}