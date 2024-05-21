using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Gemini;

public class ToolConfiguration
{
  /// <summary>
  /// 
  /// </summary>
  [JsonPropertyName("functionCallingConfig")]
  public FunctionCallingConfiguration? FunctionCallingConfiguration { get; set; }
}

public class FunctionCallingConfiguration
{
  /// <summary>
  /// 
  /// </summary>
  /// <remarks>
  /// Options: MODE_UNSPECIFIED, AUTO, ANY, NONE
  /// </remarks>
  [JsonPropertyName("mode")]
  public string? Mode { get; set; }

  /// <summary>
  /// 
  /// </summary>
  [JsonPropertyName("allowed_function_names")]
  public List<string>? AllowedFunctionNames { get; set; }
}