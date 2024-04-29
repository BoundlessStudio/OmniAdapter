using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Boundless.OmniAdapter.Anthropic;

public class InputMessage
{
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
  public IEnumerable<Content>? Content { get; set; }
}


public class UserMessage : InputMessage
{
  public UserMessage(string content)
  {
    Role = Role.User;
    Content = new List<Content>() { new TextContent(content) };
  }
  public UserMessage(IEnumerable<Content> content)
  {
    Role = Role.User;
    Content = content.ToList();
  }
}

public class AssistantMessage : InputMessage
{
  public AssistantMessage(string content)
  {
    Role = Role.Assistant;
    Content = new List<Content>() { new TextContent(content) };
  }

  public AssistantMessage(IEnumerable<Content> content)
  {
    Role = Role.Assistant;
    Content = content;
  }
}