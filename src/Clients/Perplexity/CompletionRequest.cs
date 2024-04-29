
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Perplexity;
public class CompletionRequest
{
  public CompletionRequest()
  {
  }

  /// <summary>
  /// The messages to generate chat completions for, in the chat format.
  /// </summary>
  [JsonPropertyName("messages")]
  public List<InputMessage> Messages { get; set; } = new List<InputMessage>();

  /// <summary>
  /// The name of the model that will complete your prompt.
  /// Possible values include: sonar-small-chat, sonar-small-online, sonar-medium-chat, sonar-medium-online, mistral-7b-instruct, and mixtral-8x7b-instruct.
  /// </summary>
  [JsonPropertyName("model")]
  public string Model { get; set; } = string.Empty;

  /// <summary>
  /// A multiplicative penalty greater than 0. 
  /// Values greater than 1.0 penalize new tokens based on their existing frequency in the text so far, 
  /// decreasing the model's likelihood to repeat the same line verbatim. A value of 1.0 means no penalty.
  /// </summary>
  /// <remarks>
  /// Incompatible with presence_penalty.
  /// </remarks>
  [JsonPropertyName("frequency_penalty")]
  [Range(-2, 2)]
  public double? FrequencyPenalty { get; set; }

  /// <summary>
  /// The maximum number of completion tokens returned by the API. 
  /// The total number of tokens requested in max_tokens plus the number 
  /// of prompt tokens sent in messages must not exceed the context window token limit of model requested. 
  /// If left unspecified, then the model will generate tokens until 
  /// either it reaches its stop token or the end of its context window.
  /// </summary>
  [JsonPropertyName("max_tokens")]
  [Range(1,4000)]
  public int? MaxTokens { get; set; }

  /// <summary>
  /// A value between -2.0 and 2.0. 
  /// Positive values penalize new tokens based on whether they appear in the text so far, 
  /// increasing the model's likelihood to talk about new topics. 
  /// </summary>
  /// <remarks>
  /// Incompatible with frequency_penalty.
  /// </remarks>
  [JsonPropertyName("presence_penalty")]
  [Range(-2, 2)]
  public double? PresencePenalty { get; set; }

  /// <summary>
  /// Specifies where the results should stream and be returned at one time.
  /// Do not set this yourself.<br/>
  /// Defaults to false
  /// </summary>
  [JsonPropertyName("stream")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
  public bool? Stream { get; internal set; }

  /// <summary>
  /// The amount of randomness in the response, valued between 0 inclusive and 2 exclusive. 
  /// Higher values are more random, and lower values are more deterministic.
  /// </summary>
  [JsonPropertyName("temperature")]
  [Range(0.0,1.9)]
  public double? Temperature { get; set; }

  /// <summary>
  /// The nucleus sampling threshold, valued between 0 and 1 inclusive. 
  /// For each subsequent token, the model considers the results of the tokens with top_p probability mass.
  /// We recommend either altering top_k or top_p, but not both.
  /// </summary>
  [JsonPropertyName("top_p")]
  [Range(0.0,1.0)]
  public double? TopP { get; set; }

  /// <summary>
  /// The number of tokens to keep for highest top-k filtering, 
  /// specified as an integer between 0 and 2048 inclusive. If set to 0, top-k filtering is disabled. 
  /// We recommend either altering top_k or top_p, but not both.
  /// </summary>
  [JsonPropertyName("top_k")]
  [Range(0.0, 1.0)]
  public double? TopK { get; set; }

}