using Json.Schema;
using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Gemini;

public class Tool
{
  [JsonPropertyName("generationConfig")]
  public List<FunctionDeclaration> FunctionDeclarations { get; set; } = new List<FunctionDeclaration>();
}

public class FunctionDeclaration
{
  [JsonPropertyName("name")]
  public string? Name { get; set; }

  [JsonPropertyName("description")]
  public string? Description { get; set; }

  [JsonPropertyName("parameters")]
  public JsonSchema? Parameters { get; set; }
}