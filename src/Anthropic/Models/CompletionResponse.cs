using System.Reflection;
using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Anthropic.Models;

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

  /// <summary>
  /// 
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("type")]
  public string? Type { get; private set; }

  /// <summary>
  /// 
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("role")]
  public Role? Role { get; private set; }

  /// <summary>
  /// 
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("content")]
  public IReadOnlyList<Content>? Content { get; private set; }

  /// <summary>
  /// 
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("model")]
  public string? Model { get; private set; }


  /// <summary>
  /// 
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("stop_sequence")]
  public string? StopSequence { get; set; }

  /// <summary>
  /// This will be:
  /// 'end_turn' if the model reached a natural stopping point,
  /// 'length' if we exceeded the requested max_tokens or the model's maximum,
  /// 'stop_sequence' one of your provided custom stop_sequences was generated,
  /// 'tool_use' a tool should be called and results returned.
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("stop_reason")]
  public string? StopReason { get; set; }


  /// <summary>
  /// 
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("usage")]
  public Usage? Usage { get; private set; }

  /// <summary>
  /// 
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("rate_limits")]
  public RateLimits? RateLimits { get; internal set; }


}