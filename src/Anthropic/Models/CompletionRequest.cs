using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Anthropic.Models;

public class CompletionRequest
{
  public CompletionRequest()
  {
  }

  /// <summary>
  /// ID of the model to use.
  /// </summary>
  [JsonPropertyName("model")]
  public string Model { get; set; } = string.Empty;

  /// <summary>
  /// The messages to generate chat completions for, in the chat format.
  /// </summary>
  [JsonPropertyName("messages")]
  public List<InputMessage> Messages { get; set; } = new List<InputMessage>();

  /// <summary>
  /// The maximum number of tokens allowed for the generated answer.
  /// By default, the number of tokens the model can return will be (4096 - prompt tokens).
  /// </summary>
  [JsonPropertyName("max_tokens")]
  public int? MaxTokens { get; set; }

  /// <summary>
  /// A unique identifier representing your end-user, which can help Anthropic to monitor and detect abuse.
  /// </summary>
  [JsonPropertyName("metadata")]
  public Metadata? Metadata { get; set; }

  /// <summary>
  /// Up to 4 sequences where the API will stop generating further tokens.
  /// </summary>
  [JsonPropertyName("stop_sequences")]
  public List<string>? Stop { get; set; }

  /// <summary>
  /// Specifies where the results should stream and be returned at one time.
  /// Do not set this yourself.<br/>
  /// Defaults to false
  /// </summary>
  [JsonPropertyName("stream")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
  public bool? Stream { get; internal set; }

  /// <summary>
  /// A system prompt is a way of providing context and instructions to Claude, such as specifying a particular goal or role.
  /// </summary>
  [JsonPropertyName("system")]
  public string? SystemPrommpt { get; set; }

  /// <summary>
  /// Amount of randomness injected into the response.
  /// Defaults to 1.0. Ranges from 0.0 to 1.0. Use temperature closer to 0.0 for analytical / multiple choice, and closer to 1.0 for creative and generative tasks.
  /// Note that even with temperature of 0.0, the results will not be fully deterministic.
  /// </summary>
  [JsonPropertyName("temperature")]
  [Range(0.1, 1.0)]
  public double? Temperature { get; set; }

  /// <summary>
  /// A list of tools the model may call. Currently, only functions are supported as a tool.
  /// Use this to provide a list of functions the model may generate JSON inputs for.
  /// </summary>
  [JsonPropertyName("tools")]
  public IReadOnlyList<InputFunction>? Tools { get; set; }

  /// <summary>
  /// Only sample from the top K options for each subsequent token.
  /// Used to remove "long tail" low probability responses.
  /// We generally recommend altering this or temperature but not both.<br/>
  /// Defaults to 1
  /// </summary>
  [JsonPropertyName("top_k")]
  [Range(0.0, 1.0)]
  public double? TopK { get; set; }

  // <summary>
  /// An alternative to sampling with temperature, called nucleus sampling,
  /// where the model considers the results of the tokens with top_p probability mass.
  /// So 0.1 means only the tokens comprising the top 10% probability mass are considered.
  /// We generally recommend altering this or temperature but not both.<br/>
  /// Defaults to 1
  /// </summary>
  [JsonPropertyName("top_p")]
  [Range(0.0, 1.0)]
  public double? TopP { get; set; }

}