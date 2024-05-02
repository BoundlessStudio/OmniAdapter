namespace Boundless.OmniAdapter.Models;

public class RateLimits
{
  public int LimitRequests { get; set; }
  public int LimitTokens { get; set; }
  public int RemainingRequests { get; set; }
  public int RemainingTokens { get; set; }
  public TimeSpan ResetRequests { get; set; } = TimeSpan.Zero;
  public TimeSpan ResetTokens { get; set; } = TimeSpan.Zero;
}