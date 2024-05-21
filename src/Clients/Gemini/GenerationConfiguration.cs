using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Gemini;

public class GenerationConfiguration
{
  [JsonPropertyName("stopSequences")]
  public List<string> StopSequences { get; set; } = new List<string>();

  [JsonPropertyName("temperature")]
  public double Temperature { get; set; }

  [JsonPropertyName("maxOutputTokens")]
  public int MaxTokens { get; set; }

  [JsonPropertyName("topP")]
  public double TopP { get; set; }

  [JsonPropertyName("topK")]
  public int TopK { get; set; }

  [JsonPropertyName("response_mime_type")]
  public string? ResponseMimeType { get; set; }
}