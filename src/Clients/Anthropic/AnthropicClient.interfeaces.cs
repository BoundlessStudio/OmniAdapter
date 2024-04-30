using Boundless.OmniAdapter.Interfaces;
using Boundless.OmniAdapter.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Boundless.OmniAdapter.Anthropic;

public partial class AnthropicClient : IChatCompetition, IChatStream
{

  private static Role ConvertRole(Models.Role role)
  {
    return role switch
    {
      Models.Role.User => Role.User,
      Models.Role.Assistant => Role.Assistant,
      Models.Role.Tool => Role.Assistant,
      _ => throw new ArgumentException($"Unknown role: {role}"),
    };
  }

  private static FinishReason FinishReason(string? reason)
  {
    return reason switch
    {
      "end_turn" => Models.FinishReason.Stop,
      "stop_sequence" => Models.FinishReason.Stop,
      "max_tokens" => Models.FinishReason.Length,
      "tool_use" => Models.FinishReason.Tool,
      _ => Models.FinishReason.None
    };
  }

  private static Models.Usage Usage(Usage? usage)
  {
    return new Models.Usage(usage?.PromptTokens ?? 0, usage?.CompletionTokens ?? 0, usage?.TotalTokens ?? 0);
  }

  public async Task<ChatResponse> GetChatAsync(ChatRequest chatRequest, CancellationToken ct = default)
  {
    ArgumentNullException.ThrowIfNull(chatRequest);

    if (chatRequest.ResponseFormat == ResponseFormat.JsonObject)
      throw new ValidationException("Json mode is not supported by this provider. Use tools instead.");

    var request = new CompletionRequest()
    {
      // Metadata = new Metadata() { UserId = chatRequest.User }
      SystemPrommpt = chatRequest.Messages
        .Where(_ => _.Role == Models.Role.System)
        .Select(_ => _.Content ?? string.Empty)
        .Aggregate((acc, content) => acc + Environment.NewLine + content),
      MaxTokens = chatRequest.MaxTokens,
      Temperature = chatRequest.Temperature,
      Stream = false,
      Model = chatRequest.Model ?? throw new ArgumentNullException(nameof(ChatRequest.Model)),
      Tools = chatRequest.Functions.Select(fn => new InputFunction(fn.Name, fn.Description, fn.Parameters)).ToList(),
      Messages = chatRequest.Messages
        .Where(_ => _.Role != Models.Role.System)
        .Select(msg => new InputMessage()
        {
          Role = ConvertRole(msg.Role),
          Content = new List<Content>() { new TextContent(msg.Content ?? string.Empty) }
        }).ToList(),
    };
    var dto = await GetChatAsync(request, ct);
    if (dto is null)
      throw new InvalidOperationException("Failed Chat Operation.");

    var content = dto.Content is null ? string.Empty : dto.Content.Where(_ => _.Type == "text").Select(_ => _.Text).FirstOrDefault();
    var tools = dto.Content is null ? new List<Tool>() : dto.Content.Where(_ => _.Type == "tool").Select(_ => new Tool(_.ToolName, _.ToolInput)).ToList();

    var response = new ChatResponse()
    {
      Id = dto.Id ?? Guid.NewGuid().ToString(),
      CreatedAt = DateTime.UtcNow,
      Content = content,
      FinishReason = FinishReason(dto.StopReason),
      Role = Models.Role.Assistant,
      Tools = tools,
      RateLimits = dto.RateLimits,
      Usage = Usage(dto.Usage)
    };
    return response;
  }

  public async IAsyncEnumerable<ChunkResponse> StreamChatAsync(ChatRequest chatRequest, [EnumeratorCancellation] CancellationToken ct = default)
  {
    ArgumentNullException.ThrowIfNull(chatRequest);

    if (chatRequest.ResponseFormat == ResponseFormat.JsonObject)
      throw new ValidationException("Json mode is not supported by this provider.");

    if (chatRequest.Functions.Count > 0)
      throw new ValidationException("Functions are not supported by this provider.");

    var request = new CompletionRequest()
    {
      // Metadata = new Metadata() { UserId = chatRequest.User }
      SystemPrommpt = chatRequest.Messages
        .Where(_ => _.Role == Models.Role.System)
        .Select(_ => _.Content ?? string.Empty)
        .Aggregate((acc, content) => acc + Environment.NewLine + content),
      MaxTokens = chatRequest.MaxTokens,
      Temperature = chatRequest.Temperature,
      Stream = false,
      Model = chatRequest.Model ?? throw new ArgumentNullException(nameof(ChatRequest.Model)),
      Messages = chatRequest.Messages
        .Where(_ => _.Role != Models.Role.System)
        .Select(msg => new InputMessage()
        {
          Role = ConvertRole(msg.Role),
          Content = new List<Content>() { new TextContent(msg.Content ?? string.Empty) }
        }).ToList(),
    };
    var id = Guid.NewGuid().ToString();
    var enumerable = StreamChatAsync(request, ct);
    await foreach (var content in enumerable)
    {
      var chunk = new ChunkResponse()
      {
        Id = id,
        Content = content,
        CreatedAt = DateTime.UtcNow,
        FinishReason = Models.FinishReason.None,
      };
      yield return chunk;
    }
  }
}