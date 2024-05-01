namespace Boundless.OmniAdapter.Models;

public class ImageRequest
{
  /// <summary>
  /// A text description of the desired image(s). 
  /// </summary>
  public string Prompt { get; set; } = string.Empty;

  /// <summary>
  /// The model to use for image generation.
  /// </summary>
  public string Model { get; set; } = string.Empty;

  /// <summary>
  /// The number of images to generate.
  /// </summary>
  public int N { get; set; } = 1;

  /// <summary>
  /// The size of the generated images. 
  /// </summary>
  /// <remarks>
  /// Must be formated as: WxL
  /// </remarks>
  public string? Size { get; set; }
}
