using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Anthropic.Models;

public class Content
{
  public Content()
  {
  }

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
    this.MediaType = media_type;
    this.Data = data;
  }
}
