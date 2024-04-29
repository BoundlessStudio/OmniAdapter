using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Perplexity;

public sealed class InputTool
{

  public InputTool() { }


  public InputTool(InputFunction function)
  {
    Function = function;
  }

  public static implicit operator InputTool(InputFunction function) => new(function);


  [JsonInclude]
  [JsonPropertyName("type")]
  public string? Type => "function";

  [JsonInclude]
  [JsonPropertyName("function")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public InputFunction? Function { get; private set; }

}