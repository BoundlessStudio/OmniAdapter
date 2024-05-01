using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.OpenAi;

public class SpeechRequest
{
  /// <summary>
  /// One of the available TTS models: tts-1 or tts-1-hd
  /// </summary>
  [RegularExpression("tts-1|tts-1-hd", ErrorMessage = "Model must be either 'tts-1' or 'tts-1-hd'")]
  [JsonPropertyName("model")]
  public string Model { get; set; } = string.Empty;

  /// <summary>
  /// The text to generate audio for. The maximum length is 4096 characters.
  /// </summary>
  [StringLength(4096, ErrorMessage = "Input text cannot exceed 4096 characters")]
  [JsonPropertyName("input")]
  public string Input { get; set; } = string.Empty;

  /// <summary>
  /// The voice to use when generating the audio. Supported voices are alloy, echo, fable, onyx, nova, and shimmer.
  /// </summary>
  [RegularExpression("alloy|echo|fable|onyx|nova|shimmer", ErrorMessage = "Voice must be one of the supported types: alloy, echo, fable, onyx, nova, shimmer")]
  [JsonPropertyName("voice")]
  public string Voice { get; set; } = string.Empty;

  /// <summary>
  /// The format to audio in. Supported formats are mp3, opus, aac, flac, wav, and pcm.
  /// </summary>
  [RegularExpression("mp3|opus|aac|flac|wav|pcm", ErrorMessage = "Format must be one of the supported types: mp3, opus, aac, flac, wav, pcm")]
  [JsonPropertyName("response_format")]
  public string? ResponseFormat { get; set; }

  /// <summary>
  /// The speed of the generated audio. Select a value from 0.25 to 4.0. 1.0 is the default.
  /// </summary>
  [Range(0.25, 4.0, ErrorMessage = "Speed must be between 0.25 and 4.0")]
  [JsonPropertyName("speed")]
  public double? Speed { get; set; }
}
