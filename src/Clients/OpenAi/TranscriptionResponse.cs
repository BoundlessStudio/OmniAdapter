namespace Boundless.OmniAdapter.OpenAi;


public class TranscriptionResponse
{
  public string Text { get; set; } = string.Empty;
}

public class TranscriptionVerboseResponse : TranscriptionResponse
{
  public string Language { get; set; } = string.Empty;
   public double Duration { get; set; } = 0.0;
  public List<Segment> Segments { get; set; } = new List<Segment>();
}

public class Segment
{
  public int Id { get; set; } = 0;
  public double Seek { get; set; } = 0.0;
  public double Start { get; set; } = 0.0;
  public double End { get; set; } = 0.0;
  public string Text { get; set; } = string.Empty;
  public List<int>? Tokens { get; set; } = new List<int>();
  public double? Temperature { get; set; } = 0.0;
  public double? AvgLogprob { get; set; } = 0.0;
  public double? CompressionRatio { get; set; } = 0.0;
  public double? NoSpeechProb { get; set; } = 0.0;
}
