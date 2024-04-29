
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Groq;

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
  /// Possible values include llama3-8b-8192, llama3-70b-8192, mixtral-8x7b-32768, gemma-7b-it
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
  /// An object specifying the format that the model must output.
  /// Setting to <see cref="ChatResponseFormat.Json"/> enables JSON mode,
  /// which guarantees the message the model generates is valid JSON.
  /// </summary>
  /// <remarks>
  /// Important: When using JSON mode you must still instruct the model to produce JSON yourself via some conversation message,
  /// for example via your system message. If you don't do this, the model may generate an unending stream of
  /// whitespace until the generation reaches the token limit, which may take a lot of time and give the appearance
  /// of a "stuck" request. Also note that the message content may be partial (i.e. cut off) if finish_reason="length",
  /// which indicates the generation exceeded max_tokens or the conversation exceeded the max context length.
  /// </remarks>
  [JsonPropertyName("response_format")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public ResponseFormat ResponseFormat { get; set; } = new ResponseFormat(ChatResponseFormat.Text);


  /// <summary>
  /// This feature is in Beta. If specified, our system will make a best effort to sample deterministically,
  /// such that repeated requests with the same seed and parameters should return the same result.
  /// Determinism is not guaranteed, and you should refer to the system_fingerprint response parameter to
  /// monitor changes in the backend.
  /// </summary>
  [JsonPropertyName("seed")]
  public int? Seed { get; set; }

  /// <summary>
  /// Specifies where the results should stream and be returned at one time.
  /// Do not set this yourself.<br/>
  /// Defaults to false
  /// </summary>
  [JsonPropertyName("stream")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
  public bool? Stream { get; internal set; }

  /// <summary>
  /// Up to 4 sequences where the API will stop generating further tokens.
  /// </summary>
  [JsonPropertyName("stop")]
  public List<string>? Stop { get; set; }

  /// <summary>
  /// What sampling temperature to use, between 0 and 2.
  /// Higher values like 0.8 will make the output more random, while lower values like 0.2 will
  /// make it more focused and deterministic.
  /// We generally recommend altering this or top_p but not both.<br/>
  /// Defaults to 1
  /// </summary>
  [JsonPropertyName("temperature")]
  [Range(0.1,1.9)]
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
  /// A list of tools the model may call. Currently, only functions are supported as a tool.
  /// Use this to provide a list of functions the model may generate JSON inputs for.
  /// </summary>
  [JsonPropertyName("tools")]
  public IReadOnlyList<InputTool>? Tools { get; set; }

  /// <summary>
  /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
  /// </summary>
  [JsonPropertyName("user")]
  public string? User { get; set; }
}