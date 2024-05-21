using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Gemini;

public class UsageMetadata
{
  [JsonPropertyName("promptTokenCount")]
  public int PromptToken { get; set; }
  
  [JsonPropertyName("candidatesTokenCount")]
  public int CandidatesToken { get; set; }

  [JsonPropertyName("totalTokenCount")]
  public int TotalToken { get; set; }
}
