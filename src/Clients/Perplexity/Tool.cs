using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Perplexity;

public sealed class Tool
{
  public static Tool Retrieval { get; } = new() { Type = "retrieval" };

  public static Tool CodeInterpreter { get; } = new() { Type = "code_interpreter" };

  public Tool() { }

  public Tool(InputFunction function)
  {
    Function = function;
    Type = nameof(function);
  }

  public static implicit operator Tool(InputFunction function) => new(function);


  [JsonInclude]
  [JsonPropertyName("id")]
  public string? Id { get; private set; }

  [JsonInclude]
  [JsonPropertyName("index")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public int? Index { get; private set; }

  [JsonInclude]
  [JsonPropertyName("type")]
  public string? Type { get; private set; }

  [JsonInclude]
  [JsonPropertyName("function")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public InputFunction? Function { get; private set; }


}