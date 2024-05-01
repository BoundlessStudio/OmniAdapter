namespace Boundless.OmniAdapter.Models;

public class SpeechRequest
{
  public SpeechRequest()
  {
  }

  public SpeechRequest(string input, string? voice = null, string? responseFormat = null)
  {
    Input = input;
    Voice = voice;
    ResponseFormat = responseFormat;
  }

  /// <summary>
  /// The text to generate audio for. 
  /// </summary>
  public string Input { get; set; } = string.Empty;

  /// <summary>
  /// The voice to use when generating the audio. 
  /// </summary>
  public string? Voice { get; set; }

  /// <summary>
  /// The format to audio in.
  /// </summary>
  public string? ResponseFormat { get; set; }
}
