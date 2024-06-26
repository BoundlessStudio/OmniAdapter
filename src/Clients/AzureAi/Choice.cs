﻿using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.AzureAi;

public class Choice
{
  /// <summary>
  /// The index of the choice in the list of choices.
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("index")]
  public int Index { get; set; }

  /// <summary>
  /// 
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("message")]
  public OutputMessage? Message { get; set; }

  /// <summary>
  /// A chat completion delta generated by streamed model responses.
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("delta")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
  public Delta? Delta { get; set; }

  /// <summary>
  /// 
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("logprobs")]
  public LogProb? LogProbs { get; set; }

  /// <summary>
  /// This will be:
  /// 'stop' if the model hit a natural stop point or a provided stop sequence, 
  /// 'length' if the maximum number of tokens specified in the request was reached, 
  /// 'content_filter' if content was omitted due to a flag from our content filters, 
  /// 'tool_calls' if the model called a tool.
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("finish_reason")]
  public string FinishReason { get; set; } = string.Empty;

  /// <summary>
  /// Azure AI content filter policy results.
  /// </summary>
  [JsonInclude]
  [JsonPropertyName("content_filter_results")]
  public ContentFilterResults? ContentFilterResults { get; set; }


  internal void CopyFrom(Choice other)
  {
    Index = other?.Index ?? 0;

    if (other?.Message != null)
    {
      Message = other.Message;
    }

    if (other?.Delta != null)
    {
      if (Message == null)
      {
        Message = new OutputMessage(other.Delta);
      }
      else
      {
        Message.CopyFrom(other.Delta);
      }
    }

    if (other?.LogProbs != null)
    {
      LogProbs = other.LogProbs;
    }

    if (!string.IsNullOrWhiteSpace(other?.FinishReason))
    {
      FinishReason = other.FinishReason;
    }

    if (other?.ContentFilterResults != null)
    {
      ContentFilterResults = other.ContentFilterResults;
    }
  }
}
