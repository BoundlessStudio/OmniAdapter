namespace Boundless.OmniAdapter.Models;

public class TranscriptionResponse
{
  public string Text { get; set; } = string.Empty;

  public static explicit operator SpeechRequest(TranscriptionResponse response) => new SpeechRequest(response.Text);
}
