using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Gemini;

public class PromptFeedback
{
  [JsonPropertyName("blockReason")]
  public string? BlockReason { get; set; }

  [JsonPropertyName("safetyRatings")]
  public List<SafetyRating> SafetyRatings { get; set; } = new List<SafetyRating>();
}
