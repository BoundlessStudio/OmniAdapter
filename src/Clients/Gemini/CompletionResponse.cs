using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Gemini;

public class CompletionResponse
{
  [JsonPropertyName("candidates")]
  public List<Candidate>? Candidates { get; set; }

  [JsonPropertyName("prompt_feedback")]
  public PromptFeedback? PromptFeedback { get; set; }

  [JsonPropertyName("usage_metadata")]
  public UsageMetadata? UsageMetadata { get; set; }

  [JsonIgnore]
  public Candidate FirstChoice => Candidates?.FirstOrDefault(choice => choice.Index == 0) ?? throw new ArgumentOutOfRangeException();
}