using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.OpenAi;

public class SpeechRequest
{
  /// <summary>
  /// One of the available TTS models: tts-1 or tts-1-hd
  /// </summary>
  [JsonPropertyName("model")]
  public string Model { get; set; } = string.Empty;

  /// <summary>
  /// The text to generate audio for. The maximum length is 4096 characters.
  /// </summary>
  [JsonPropertyName("input")]
  public string Input { get; set; } = string.Empty;

  /// <summary>
  /// The voice to use when generating the audio. Supported voices are alloy, echo, fable, onyx, nova, and shimmer.
  /// </summary>
  [JsonPropertyName("voice")]
  public string? Voice { get; set; }

  /// <summary>
  /// The format to audio in. Supported formats are mp3, opus, aac, flac, wav, and pcm.
  /// </summary>
  [JsonPropertyName("response_format")]
  public string? ResponseFormat { get; set; }

  /// <summary>
  /// The speed of the generated audio. Select a value from 0.25 to 4.0. 1.0 is the default.
  /// </summary>
  [JsonPropertyName("speed")]
  public double? Speed { get; set; }
}
