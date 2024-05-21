using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Gemini;

public class Content
{
  [JsonPropertyName("parts")]
  public List<Part> Parts { get; set; } = new List<Part>();

  [JsonPropertyName("role")]
  public Role? Role { get; set; }
}

public class Part
{
  [JsonPropertyName("text")]
  public string? Text { get; set; }

  [JsonPropertyName("inline_data")]
  public InlineData? InlineData { get; set; }

  [JsonPropertyName("function_call")]
  public FunctionCall? FunctionCall { get; set; }

  [JsonPropertyName("function_response")]
  public FunctionResponse? FunctionResponse { get; set; }

}

public class InlineData
{
  [JsonPropertyName("mime_type")]
  public string? MimeType { get; set; }

  [JsonPropertyName("data")]
  public string? Data { get; set; }
}

public class FunctionCall
{
  [JsonPropertyName("name")]
  public string? Name { get; set; }

  [JsonPropertyName("args")]
  public JsonNode? Args { get; set; }
}

public class FunctionResponse
{
  [JsonPropertyName("name")]
  public string? Name { get; set; }

  [JsonPropertyName("response")]
  public string? Response { get; set; }
}