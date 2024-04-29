namespace Boundless.OmniAdapter.Models;

public class ChatRequest
{
  /// <summary>
  /// The messages to generate chat completions for, in the chat format.
  /// </summary>
  public List<Message> Messages { get; set; } = new List<Message>();

  /// <summary>
  /// The name of the model that will complete your prompt.
  /// </summary>
  public string? Model { get; set; }

  /// <summary>
  /// The maximum number of tokens allowed for the generated answer.
  /// </summary>
  /// <remarks>
  /// Defaults to 4096
  /// </remarks>
  public int MaxTokens { get; set; } = 4096;

  /// <summary>
  /// What sampling temperature to use, between 0 and 1.
  /// </summary>
  /// <remarks>
  /// Defaults to 0.8
  /// </remarks>
  public double Temperature { get; set; } = 0.8;

  /// <summary>
  /// Use this to provide a list of functions the model may generate JSON inputs for.
  /// </summary>
  public IEnumerable<Function>? Functions { get; set; }

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
  //public ResponseFormat ResponseFormat { get; set; } = new ResponseFormat(ChatResponseFormat.Text);

}
