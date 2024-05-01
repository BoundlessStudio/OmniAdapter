using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Models;

public class ImageResponse
{
  [JsonPropertyName("url")]
  public string? Url { get; set; }

  [JsonPropertyName("revised_prompt")]
  public string? RevisedPrompt { get; set; }
}
