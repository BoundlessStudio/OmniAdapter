namespace Boundless.OmniAdapter.Models;

public class SpeechResponse
{
  /// <summary>
  /// The audio file stream to transcribe.
  /// </summary>
  public Stream File { get; set; } = Stream.Null;

  /// <summary>
  /// The filename for the stream - the file extenstion is important to decoding process.
  /// </summary>
  public string FileName { get; set; } = string.Empty;


  public static explicit operator TranscriptionRequest(SpeechResponse response) => new TranscriptionRequest(response.File, response.FileName);
}
