using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Gemini;

public class Candidate
{
  [JsonPropertyName("content")]
  public Content? Content { get; set; }


  /// <summary>
  /// This will be:
  /// 'FINISH_REASON_UNSPECIFIED' Default value. This value is unused.
  /// 'STOP' Natural stop point of the model or provided stop sequence.
  /// 'MAX_TOKENS' The maximum number of tokens as specified in the request was reached.
  /// 'SAFETY' The candidate content was flagged for safety reasons.
  /// 'RECITATION' The candidate content was flagged for recitation reasons.
  /// 'OTHER' Unknown reason.
  /// </summary>
  [JsonPropertyName("finishReason")]
  public string FinishReason { get; set; } = string.Empty;

  [JsonPropertyName("index")]
  public int Index { get; set; }

  [JsonPropertyName("safetyRatings")]
  public List<SafetyRating> SafetyRatings { get; set; } = new List<SafetyRating>();
}
