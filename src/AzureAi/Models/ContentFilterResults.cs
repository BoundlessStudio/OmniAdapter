using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.AzureAi.Models;

public class ContentFilterResult
{
  [JsonPropertyName("filtered")]
  public bool Filtered { get; set; }

  [JsonPropertyName("severity")]
  public string? Severity { get; set; }
}

public class ContentFilterResults
{

  [JsonPropertyName("hate")]
  public ContentFilterResult Hate { get; set; } = new();

  [JsonPropertyName("self_harm")]
  public ContentFilterResult SelfHarm { get; set; } = new();

  [JsonPropertyName("sexual")]
  public ContentFilterResult Sexual { get; set; } = new();

  [JsonPropertyName("violence")]
  public ContentFilterResult Violence { get; set; } = new();
}


public class PromptFilterResults
{
  [JsonPropertyName("prompt_index")]
  public int? PromptIndex { get; set; }

  [JsonPropertyName("content_filter_results")]
  public ContentFilterResults ContentFilterResults { get; set; } = new();
}
