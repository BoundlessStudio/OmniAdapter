using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.OpenAi;

public class ImageResponse
{
  [JsonPropertyName("b64_json")]
  public string? Base64Json { get; set; }

  [JsonPropertyName("url")]
  public string? Url { get; set; }

  [JsonPropertyName("revised_prompt")]
  public string? RevisedPrompt { get; set; }
}
