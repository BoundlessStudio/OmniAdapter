using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boundless.OmniAdapter.Anthropic.Models;

public class RateLimits
{
  public int LimitRequests { get; set; }
  public int LimitTokens { get; set; }
  public int RemainingRequests { get; set; }
  public int RemainingTokens { get; set; }
  public string? ResetRequests { get; set; }
  public string? ResetTokens { get; set; }
}