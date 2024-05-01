using static Humanizer.In;

namespace Boundless.OmniAdapter.Models;

public class TranscriptionRequest
{
  public TranscriptionRequest()
  {
  }

  public TranscriptionRequest(Stream file, string fileName, string? language = null, string? prompt = null)
  {
    File = file;
    FileName = fileName;
    Language = language;
    Prompt = prompt;
  }

  /// <summary>
  /// The audio file stream to transcribe.
  /// </summary>
  public Stream File { get; set; } = Stream.Null;

  /// <summary>
  /// The filename for the stream - the file extenstion is important to decoding process.
  /// </summary>
  public string FileName { get; set; } = string.Empty;

  /// <summary>
  /// The language of the input audio. 
  /// </summary>
  /// <remarks>
  /// Supplying the input language in ISO-639-1 format will improve accuracy and latency.
  /// </remarks>
  public string? Language { get; set; }

  /// <summary>
  /// An optional text to guide the model's style or continue a previous audio segment. The prompt should match the audio language.
  /// </summary>
  public string? Prompt { get; set; }

}
