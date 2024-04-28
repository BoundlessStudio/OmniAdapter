
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Perplexity.Models;

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
  /// ID of the model to use.
  /// </summary>
  [JsonPropertyName("model")]
  public string Model { get; set; } = string.Empty;

  /// <summary>
  /// Number between -2.0 and 2.0.
  /// Positive values penalize new tokens based on their existing frequency in the text so far,
  /// decreasing the model's likelihood to repeat the same line verbatim.<br/>
  /// Defaults to 0
  /// </summary>
  [JsonPropertyName("frequency_penalty")]
  [Range(-2, 2)]
  public double? FrequencyPenalty { get; set; }


  /// <summary>
  /// The maximum number of tokens allowed for the generated answer.
  /// By default, the number of tokens the model can return will be (4096 - prompt tokens).
  /// </summary>
  [JsonPropertyName("max_tokens")]
  [Range(1,4000)]
  public int? MaxTokens { get; set; }

  /// <summary>
  /// Number between -2.0 and 2.0.
  /// Positive values penalize new tokens based on whether they appear in the text so far,
  /// increasing the model's likelihood to talk about new topics.<br/>
  /// Defaults to 0
  /// </summary>
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
  /// What sampling temperature to use, between 0 and 2.
  /// Higher values like 0.8 will make the output more random, while lower values like 0.2 will
  /// make it more focused and deterministic.
  /// We generally recommend altering this or top_p but not both.<br/>
  /// Defaults to 1
  /// </summary>
  [JsonPropertyName("temperature")]
  [Range(0.0,1.9)]
  public double? Temperature { get; set; }

  /// <summary>
  /// An alternative to sampling with temperature, called nucleus sampling,
  /// where the model considers the results of the tokens with top_p probability mass.
  /// So 0.1 means only the tokens comprising the top 10% probability mass are considered.
  /// We generally recommend altering this or temperature but not both.<br/>
  /// Defaults to 1
  /// </summary>
  [JsonPropertyName("top_p")]
  [Range(0.0,1.0)]
  public double? TopP { get; set; }

  /// <summary>
  /// 
  /// </summary>
  [JsonPropertyName("top_k")]
  [Range(0.0, 1.0)]
  public double? TopK { get; set; }

}