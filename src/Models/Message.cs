namespace Boundless.OmniAdapter.Models;

public class Message
{
  /// <summary>
  /// The <see cref="Role"/> of the author of this message.
  /// </summary>
  public Role Role { get; set; }

  /// <summary>
  /// The contents of the message.
  /// </summary>
  public string? Content { get; set; }

  // Image?

  /// <summary>
  /// Optional, The name of the author of this message.<br/>
  /// May contain a-z, A-Z, 0-9, and underscores, with a maximum length of 64 characters.
  /// </summary>
  public string? Name { get; set; }
  
  /// <summary>
  /// 
  /// </summary>
  public List<Tool>? ToolCalls { get; set; }

  /// <summary>
  /// 
  /// </summary>
  public string? ToolCallId { get; set; }
}
