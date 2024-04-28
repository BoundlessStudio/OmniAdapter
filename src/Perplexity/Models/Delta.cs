﻿using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Perplexity.Models
{
  public class Delta
  {
    /// <summary>
    /// The author of this message.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("role")]
    public Role Role { get; private set; }

    /// <summary>
    /// The contents of the message.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("content")]
    public string? Content { get; private set; }

    /// <summary>
    /// The tool calls generated by the model, such as function calls.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("tool_calls")]
    public IReadOnlyList<Tool>? ToolCalls { get; private set; }

    /// <summary>
    /// Optional, The name of the author of this message.<br/>
    /// May contain a-z, A-Z, 0-9, and underscores, with a maximum length of 64 characters.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("name")]
    public string? Name { get; private set; }
  }
}
