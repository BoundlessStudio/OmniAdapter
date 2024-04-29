using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.AzureAi;


public abstract class InputMessage
{
  /// <summary>
  /// The <see cref="OpenAI.Role"/> of the author of this message.
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
  public string? Content { get; set; }

  /// <summary>
  /// Optional, The name of the author of this message.<br/>
  /// May contain a-z, A-Z, 0-9, and underscores, with a maximum length of 64 characters.
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("name")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? Name { get; set; }

  [JsonInclude]
  [JsonPropertyName("tool_calls")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public List<string>? ToolCalls { get; set; }

  [JsonInclude]
  [JsonPropertyName("tool_call_id")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? ToolCallId { get; set; }
}

public class SystemMessage : InputMessage
{
  public SystemMessage(string content)
  {
    Role = Role.System;
    Content = content;
  } 
}

public class UserMessage : InputMessage
{
  public UserMessage(string content, string? name = null)
  {
    Role = Role.User;
    Content = content;
    Name = name;
  }
}

public class AssistantMessage : InputMessage
{
  public AssistantMessage(string content, string? name = null)
  {
    Role = Role.Assistant;
    Content = content;
    Name = name;
  }

  public AssistantMessage(List<string> tools, string? name = null)
  {
    Role = Role.Assistant;
    ToolCalls = tools;
    Name = name;
  }
}

public class ToolMessage : InputMessage
{
  public ToolMessage(string content, Tool tool)
  {
    Role = Role.Tool;
    Content = content;
    ToolCallId = tool.Id;
  }
}

