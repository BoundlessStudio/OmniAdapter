using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Anthropic.Models;

public class Message
{
  public Message()
  {
    this.Content = new List<Content>();
  }

  public Message(Role role, string text)
  {
    Role = role;
    Content = new List<Content> { new TextContent(text) };
  }

  public Message(Role role, IEnumerable<string> content)
  {
    Role = role;
    Content = content.Select(t => new TextContent(t)).Cast<Content>().ToList();
  }

  public Message(Role role, string media_type, string data)
  {
    Role = role;
    Content = new List<Content> { new ImageContent(media_type, data) };
  }

  /// <summary>
  /// The <see cref="Anthropic.Role"/> of the author of this message.
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("role")]
  public Role Role { get; set; }

  /// <summary>
  /// The contents of the message.
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("content")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public List<Content> Content { get; set; }
}
