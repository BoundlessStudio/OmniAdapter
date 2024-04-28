using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.AzureAi.Models;

public sealed class ResponseFormat
{
  public ResponseFormat() => Type = ChatResponseFormat.Text;

  public ResponseFormat(ChatResponseFormat format) => Type = format;

  [JsonInclude]
  [JsonPropertyName("type")]
  public ChatResponseFormat Type { get; private set; }

  public static implicit operator ChatResponseFormat(ResponseFormat format) => format.Type;

  public static implicit operator ResponseFormat(ChatResponseFormat format) => new(format);
}