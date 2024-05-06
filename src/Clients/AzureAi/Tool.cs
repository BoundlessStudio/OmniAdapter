using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.AzureAi;

public sealed class Tool
{
  public static Tool Retrieval { get; } = new() { Type = "retrieval" };

  public static Tool CodeInterpreter { get; } = new() { Type = "code_interpreter" };

  public Tool() { }

    public Tool(Function function)
    {
        Function = function;
        Type = nameof(function);
    }

    public static implicit operator Tool(Function function) => new(function);


    [JsonInclude]
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonInclude]
    [JsonPropertyName("index")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Index { get;  set; }

    [JsonInclude]
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonInclude]
    [JsonPropertyName("function")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Function? Function { get; set; }


}