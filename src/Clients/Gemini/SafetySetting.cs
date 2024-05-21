using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Gemini;
public class SafetySetting
{
  [JsonPropertyName("category")]
  public string? Category { get; set; }

  [JsonPropertyName("threshold")]
  public string? Threshold { get; set; }
}
