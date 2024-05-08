using Boundless.OmniAdapter.Interfaces;
using Boundless.OmniAdapter.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Boundless.OmniAdapter.AzureAi;

public partial class AzureAiClient : IChatCompletion, IChatStream
{
  private static Models.Role ConvertRole(Role role)
  {
    return role switch
    {
      Role.System => Models.Role.System,
      Role.User => Models.Role.User,
      Role.Assistant => Models.Role.Assistant,
      Role.Tool => Models.Role.Tool,
      _ => throw new ArgumentException($"Unknown role: {role}"),
    };
  }

  private static Role ConvertRole(Models.Role role)
  {
    return role switch
    {
      Models.Role.System => Role.System,
      Models.Role.User => Role.User,
      Models.Role.Assistant => Role.Assistant,
      Models.Role.Tool => Role.Tool,
      _ => throw new ArgumentException($"Unknown role: {role}"),
    };
  }

  private static FinishReason FinishReason(string? reason)
  {
    return reason switch
    {
      "stop" => Models.FinishReason.Stop,
      "length" => Models.FinishReason.Length,
      "content_filter" => Models.FinishReason.ContentFilter,
      "tool_calls" => Models.FinishReason.Tool,
      _ => Models.FinishReason.None
    };
  }

  private static Models.Usage Usage(Usage? usage)
  {
    return new Models.Usage(usage?.PromptTokens ?? 0, usage?.CompletionTokens ?? 0, usage?.TotalTokens ?? 0);
  }

  public async Task<ChatResponse> GetChatAsync(ChatRequest chatRequest, CancellationToken ct = default)
  {
    if (chatRequest is null)
      throw new ArgumentNullException(nameof(chatRequest));

    if (chatRequest.Model is not null)
      throw new ValidationException("The model changed for this provider. To change the model change the Deployment.");

    var request = new CompletionRequest()
    {
      // User = chatRequest.User,
      MaxTokens = chatRequest.MaxTokens,
      Temperature = chatRequest.Temperature,
      Stream = false,
      N = 1,
      ResponseFormat = new ResponseFormat(chatRequest.ResponseFormat == Models.ResponseFormat.JsonObject ? ChatResponseFormat.JsonObject : ChatResponseFormat.Text),
      Tools = chatRequest.Functions.Select(fn => new InputTool(new InputFunction(fn.Name, fn.Description, fn.Schema))).ToList(),
      Messages = chatRequest.Messages.Select(msg => new InputMessage()
      {
        Content = msg.Content,
        Name = msg.Name,
        Role = ConvertRole(msg.Role),
        ToolCallId = msg.ToolCallId,
        ToolCalls = msg.ToolCalls.Select((_, i) => new Tool()
        {
          Id = _.Id,
          Index = i,
          Function = new Function() { Name = _.Name, Description = string.Empty, Parameters = _.Parameters }
        }).ToList()
      }).ToList(),
    };
    var dto = await GetChatAsync(request, ct);
    if (dto is null)
      throw new InvalidOperationException("Failed Chat Operation.");

    var choice = dto.FirstChoice;
    if (choice is null)
      throw new InvalidOperationException("No Valid Choice in Chat Response.");

    var message = choice.Message;
    if (message is null)
      throw new InvalidOperationException("No Valid Message in Chat Choice.");

    var response = new ChatResponse()
    {
      Id = dto.Id ?? Guid.NewGuid().ToString(),
      CreatedAt = dto.CreatedAt,
      Content = message.Content ?? string.Empty,
      FinishReason = FinishReason(choice.FinishReason),
      Role = ConvertRole(message.Role),
      Tools = message?.ToolCalls?.Select(_ => new Models.Tool(_.Id, _.Function?.Name, _.Function?.Parameters))?.ToList() ?? new List<Models.Tool>(),
      RateLimits = dto.RateLimits,
      Usage = Usage(dto.Usage)
    };
    return response;
  }

  public async IAsyncEnumerable<ChunkResponse> StreamChatAsync(ChatRequest chatRequest, [EnumeratorCancellation] CancellationToken ct = default)
  {
    if (chatRequest is null)
      throw new ArgumentNullException(nameof(chatRequest));

    if (chatRequest.Model is not null)
      throw new ValidationException("The model changed for this provider. To change the model change the Deployment.");

    var request = new CompletionRequest()
    {
      // User = chatRequest.User,
      MaxTokens = chatRequest.MaxTokens,
      Temperature = chatRequest.Temperature,
      Stream = true,
      N = 1,
      ResponseFormat = new ResponseFormat(chatRequest.ResponseFormat == Models.ResponseFormat.JsonObject ? ChatResponseFormat.JsonObject : ChatResponseFormat.Text),
      Tools = chatRequest.Functions.Select(fn => new InputTool(new InputFunction(fn.Name, fn.Description, fn.Schema))).ToList(),
      Messages = chatRequest.Messages.Select(msg => new InputMessage()
      {
        Content = msg.Content,
        Name = msg.Name,
        Role = ConvertRole(msg.Role),
        ToolCallId = msg.ToolCallId,
      }).ToList(),
    };

    var id = Guid.NewGuid().ToString();
    var enumerable = StreamChatAsync(request, ct);
    await foreach (var item in enumerable)
    {
      var choice = item.Choices.FirstOrDefault();
      if (choice is null) continue;

      var chunk = new ChunkResponse()
      {
        Id = item.Id ?? id,
        Content = choice.Delta?.Content ?? string.Empty,
        CreatedAt = item.CreatedAt,
        FinishReason = FinishReason(choice?.FinishReason),
      };
      yield return chunk;
    }
  }
}