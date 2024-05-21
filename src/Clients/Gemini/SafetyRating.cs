using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Gemini;
public class SafetyRating
{
  [JsonPropertyName("category")]
  public string? Category { get; set; }

  [JsonPropertyName("probability")]
  public string? Probability { get; set; }

  [JsonPropertyName("blocked")]
  public bool Blocked { get; set; }
}
