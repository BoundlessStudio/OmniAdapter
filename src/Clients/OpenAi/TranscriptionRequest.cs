namespace Boundless.OmniAdapter.OpenAi;

public class TranscriptionRequest
{
  /// <summary>
  /// The audio file stream (not file name) to transcribe, in one of these formats: flac, mp3, mp4, mpeg, mpga, m4a, ogg, wav, or webm.
  /// </summary>
  public Stream File { get; set; } = Stream.Null;

  /// <summary>
  /// The filename for the stream - the file extenstion is important to decoding process.
  /// </summary>
  public string FileName { get; set; } = string.Empty;

  /// <summary>
  /// ID of the model to use. Only whisper-1 (which is powered by our open source Whisper V2 model) is currently available.
  /// </summary>
  public string Model { get; private set; } = "whisper-1";

  /// <summary>
  /// The language of the input audio. Supplying the input language in ISO-639-1 format will improve accuracy and latency.
  /// </summary>
  public string? Language { get; set; }

  /// <summary>
  /// An optional text to guide the model's style or continue a previous audio segment. The prompt should match the audio language.
  /// </summary>
  public string? Prompt { get; set; }

  /// <summary>
  /// The sampling temperature, between 0 and 1. 
  /// Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic. 
  /// If set to 0, the model will use log probability to automatically increase the temperature until certain thresholds are hit.
  /// </summary>
  public double? Temperature { get; set; }

}

public class VerboseTranscriptionRequest : TranscriptionRequest
{

  /// <summary>
  /// The timestamp granularities to populate for this transcription.
  /// response_format must be set verbose_json to use timestamp granularities. 
  /// Either or both of these options are supported: word, or segment.
  /// </summary>
  /// <remarks>
  /// There is no additional latency for segment timestamps, but generating word timestamps incurs additional latency.
  /// </remarks>
  public List<string>? TimestampGranularities { get; set; }

}
