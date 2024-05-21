using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Gemini;

public class CompletionRequest
{
  public CompletionRequest()
  {
  }

  /// <summary>
  /// 
  /// </summary>
  [JsonPropertyName("contents")]
  public List<Content> Contents { get; set; } = new List<Content>();

  /// <summary>
  /// The name of the model that will complete your prompt.
  /// Possible values include models/gemini-pro, models/gemini-pro-vision, models/gemini-ultra
  /// </summary>
  [JsonPropertyName("model")]
  public string? Model { get; set; } = "models/gemini-pro";

  /// <summary>
  /// 
  /// </summary>
  [JsonPropertyName("systemInstruction")]
  public Content? SystemInstruction { get; set; }

  /// <summary>
  /// 
  /// </summary>
  [JsonPropertyName("tools")]
  public List<Tool>? Tools { get; set; }

  /// <summary>
  /// 
  /// </summary>
  [JsonPropertyName("toolConfig")]
  public ToolConfiguration? ToolConfiguration { get; set; }

  /// <summary>
  /// 
  /// </summary>
  [JsonPropertyName("safetySettings")]
  public List<SafetySetting>? SafetySettings { get; set; }

  /// <summary>
  /// 
  /// </summary>
  [JsonPropertyName("generationConfig")]
  public GenerationConfiguration? GenerationConfiguration { get; set; }
}