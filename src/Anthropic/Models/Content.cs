using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Anthropic.Models;

public class Content
{
  /// <summary>
  /// Type of content text or image
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("type")]
  public string? Type { get; set; }

  /// <summary>
  /// Text content
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("text")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? Text { get; set; }

  /// <summary>
  /// 
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("source")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public ImageSource? Source { get; set; }


  /// <summary>
  /// A unique identifier for this particular tool use block. This will be used to match up the tool results later.
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("id")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? ToolId { get; set; }

  /// <summary>
  /// The name of the tool being used.
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("name")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? ToolName { get; set; }

  /// <summary>
  /// An object containing the input being passed to the tool, conforming to the tool's input_schema.
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("input")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public JsonObject? ToolInput { get; set; }

  /// <summary>
  ///  The id of the tool use request this is a result for.
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("tool_use_id")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? ToolUseId { get; set; }

  /// <summary>
  /// The result of the tool, as a string.
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("content")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? ToolResult { get; set; }

  /// <summary>
  /// Was the results of the tool an error?
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("is_error")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public bool? ToolError { get; set; }
}

public class ImageSource
{
  /// <summary>
  /// Type of content text or image
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("type")]
  public string? Type { get; set; } = "base64";

  /// <summary>
  /// image/jpeg, image/png, image/gif, and image/webp
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("media_type")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? MediaType { get; set; }

  /// <summary>
  /// Base64 encoded image data
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("data")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? Data { get; set; }
}



public class TextContent : Content
{
  public TextContent(string text)
  {
    this.Type = "text";
    this.Text = text;
  }
}



public class ImageContent : Content
{
  public ImageContent(string media_type, string data)
  {
    this.Type = "image";
    this.Source = new ImageSource()
    {
      MediaType = media_type,
      Data = data,
    };
  }
}


public class ToolContent : Content
{
  public ToolContent()
  {
  }

  public ToolContent(string id, string name, JsonObject input)
  {
    this.ToolId = id;
    this.ToolName = name;
    this.ToolInput = input;
  }
}



public class ToolResultContent : Content
{
  public ToolResultContent(string id, string content)
  {
    this.ToolUseId = id;
    this.ToolResult = content;
  }
}
