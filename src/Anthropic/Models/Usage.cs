using System.Text.Json;
using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Anthropic.Models;

public sealed class Usage
{
  public Usage() { }

  public Usage(int promptTokens, int completionTokens)
  {
    PromptTokens = promptTokens;
    CompletionTokens = completionTokens;
  }

  [JsonInclude]
  [JsonPropertyName("input_tokens")]
  public int? PromptTokens { get; private set; }

  [JsonInclude]
  [JsonPropertyName("output_tokens")]
  public int? CompletionTokens { get; private set; }

  [JsonInclude]
  [JsonPropertyName("total_tokens")]
  public int? TotalTokens => PromptTokens + CompletionTokens;

  public static Usage operator +(Usage a, Usage b) => new((a.PromptTokens ?? 0) + (b.PromptTokens ?? 0),(a.CompletionTokens ?? 0) + (b.CompletionTokens ?? 0));
}
