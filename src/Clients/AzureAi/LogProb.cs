using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Boundless.OmniAdapter.AzureAi;

public class LogProb
{
  [JsonInclude]
  [JsonPropertyName("content")]
  public List<TokenLogProb> Content { get; set; }

  
}

public class TokenLogProb
{
  [JsonInclude]
  [JsonPropertyName("token")]
  public string Token { get; set; }

  [JsonInclude]
  [JsonPropertyName("logprob")]
  public double Logprob { get; set; }

  [JsonInclude]
  [JsonPropertyName("bytes")]
  public List<int> Bytes { get; set; }

  [JsonInclude]
  [JsonPropertyName("top_logprobs")]
  public List<TokenTopLogProb> TopLogProbs { get; set; }
}

public class TokenTopLogProb
{
  [JsonInclude]
  [JsonPropertyName("token")]
  public string Token { get; set; }

  [JsonInclude]
  [JsonPropertyName("logprob")]
  public double Logprob { get; set; }

  [JsonInclude]
  [JsonPropertyName("bytes")]
  public List<int> Bytes { get; set; }
}
