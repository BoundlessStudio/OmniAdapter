using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Anthropic.Models;

public class ServerEvent
{
  [JsonPropertyName("type")]
  public string? Type { get; set; }

  [JsonPropertyName("message")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public CompletionResponse? Message { get; set; }

  [JsonPropertyName("index")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public int? Index { get; set; }

  [JsonPropertyName("content_block")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public Content? ContentBlock { get; set; }

  [JsonPropertyName("delta")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public Content? Delta { get; set; }
}
