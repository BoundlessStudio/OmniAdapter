namespace Boundless.OmniAdapter;

public class RateLimits
{
  public int LimitRequests { get; set; }
  public int LimitTokens { get; set; }
  public int RemainingRequests { get; set; }
  public int RemainingTokens { get; set; }
  public string? ResetRequests { get; set; }
  public string? ResetTokens { get; set; }
}