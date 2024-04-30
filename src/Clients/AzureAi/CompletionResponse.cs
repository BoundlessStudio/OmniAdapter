using System.Reflection;
using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.AzureAi;

public class CompletionResponse
{
  public CompletionResponse()
  {
  }

  /// <summary>
  /// A unique identifier for the chat completion.
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("id")]
  public string? Id { get; private set; }

  [JsonInclude]
  [JsonPropertyName("object")]
  public string? Object { get; private set; }

  /// <summary>
  /// The Unix timestamp (in seconds) of when the chat completion was created.
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("created")]
  public int CreatedAtUnixTimeSeconds { get; private set; }

  [JsonIgnore]
  public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTimeSeconds).DateTime;

  [JsonInclude]
  [JsonPropertyName("model")]
  public string? Model { get; private set; }

  /// <summary>
  /// This fingerprint represents the backend configuration that the model runs with.
  /// Can be used in conjunction with the seed request parameter to understand when
  /// backend changes have been made that might impact determinism.
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("system_fingerprint")]
  public string? SystemFingerprint { get; private set; }

  [JsonInclude]
  [JsonPropertyName("usage")]
  public Usage? Usage { get; private set; }


  [JsonInclude]
  [JsonPropertyName("rate_limits")]
  public Models.RateLimits? RateLimits { get; internal set; }
  

  [JsonIgnore]
  private List<Choice> choices = new List<Choice>();

  /// <summary>
  /// A list of chat completion choices. Can be more than one if n is greater than 1.
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("choices")]
  public IReadOnlyList<Choice> Choices
  {
    get => choices;
    private set => choices = value.ToList();
  }

  [JsonIgnore]
  public Choice FirstChoice => Choices?.FirstOrDefault(choice => choice.Index == 0) ?? throw new ArgumentOutOfRangeException();

  [JsonInclude]
  [JsonPropertyName("prompt_filter_results")]
  public List<PromptFilterResults>? PromptFilterResults { get; internal set; }

}