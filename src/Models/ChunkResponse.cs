namespace Boundless.OmniAdapter.Models;

public class ChunkResponse
{
  /// <summary>
  /// 
  /// </summary>
  public string Id { get; set; } = Guid.NewGuid().ToString();

  /// <summary>
  /// 
  /// </summary>
  public DateTime CreatedAt { get; set; }

  /// <summary>
  /// The contents of the message.
  /// </summary>
  public string? Content { get; set; }


  /// <summary>
  /// This will be:
  /// <see cref="FinishReason.Stop"/>
  /// <see cref="FinishReason.Length"/>
  /// <see cref="FinishReason.ContentFilter"/>
  /// <see cref="FinishReason.ToolCalls"/>
  /// </summary>
  public FinishReason? FinishReason { get; set; }

}