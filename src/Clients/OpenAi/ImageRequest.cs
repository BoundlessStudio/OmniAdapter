using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Boundless.OmniAdapter.OpenAi;

public class ImageRequest
{
  /// <summary>
  /// A text description of the desired image(s). 
  /// The maximum length is 1000 characters for dall-e-2 and 4000 characters for dall-e-3.
  /// </summary>
  [JsonPropertyName("prompt")]
  public string Prompt { get; set; } = string.Empty;

  /// <summary>
  /// The model to use for image generation.
  /// </summary>
  [JsonPropertyName("model")]
  public string Model { get; set; } = "dall-e-3";

  /// <summary>
  /// The number of images to generate. Must be between 1 and 10. For dall-e-3, only n=1 is supported.
  /// </summary>
  [JsonPropertyName("n")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public int? N { get; set; }

  /// <summary>
  /// The quality of the image that will be generated. 
  /// hd creates images with finer details and greater consistency across the image. 
  /// This param is only supported for dall-e-3.
  /// </summary>
  [JsonPropertyName("quality")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? Quality { get; set; }

  /// <summary>
  /// The format in which the generated images are returned. 
  /// Must be one of url or b64_json. 
  /// URLs are only valid for 60 minutes after the image has been generated.
  /// </summary>
  [JsonPropertyName("response_format")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? ResponseFormat { get; set; }

  /// <summary>
  /// The size of the generated images. 
  /// Must be one of 256x256, 512x512, or 1024x1024 for dall-e-2. 
  /// Must be one of 1024x1024, 1792x1024, or 1024x1792 for dall-e-3 models.
  /// </summary>
  [JsonPropertyName("size")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? Size { get; set; }

  /// <summary>
  /// The style of the generated images. 
  /// Must be one of vivid or natural. 
  /// - Vivid causes the model to lean towards generating hyper-real and dramatic images. 
  /// - Natural causes the model to produce more natural, less hyper-real looking images. This param is only supported for dall-e-3.
  /// </summary>
  [JsonPropertyName("style")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? Style { get; set; }

  /// <summary>
  /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
  /// </summary>
  [JsonPropertyName("user")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? User { get; set; }
}
