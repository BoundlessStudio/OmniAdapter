using Boundless.OmniAdapter.Interfaces;
using Boundless.OmniAdapter.Models;

namespace Boundless.OmniAdapter.Gemini;

public partial class GeminiClient : IChatCompletion
{
  private static Models.Usage Usage(UsageMetadata? usage)
  {
    return new Models.Usage(usage?.PromptToken ?? 0, usage?.CandidatesToken ?? 0, usage?.TotalToken ?? 0);
  }

  private static Role? ConvertRole(Models.Role role)
  {
    return role switch
    {
      Models.Role.User => Role.User,
      Models.Role.Assistant => Role.Model,
      _ => null,
    };
  }

  private static FinishReason FinishReason(string? reason)
  {
    return reason?.ToLowerInvariant() switch
    {
      "stop" => Models.FinishReason.Stop,
      "max_tokens" => Models.FinishReason.Length,
      "safety" => Models.FinishReason.ContentFilter,
      "recitation" => Models.FinishReason.ContentFilter,
      _ => Models.FinishReason.None
    };
  }

  private static IEnumerable<Content> ConvertMessageToContent(List<Message> messages)
  {
    foreach (var msg in messages)
    {
      var content = new Content()
      {
        Role = ConvertRole(msg.Role)
      };

      if (msg.Content is not null)
      {
        content.Parts.Add(new Part() { Text = msg.Content });
      }

      if (msg.ToolCallId is not null)
      {
        content.Parts.Add(new Part() { FunctionResponse = new FunctionResponse() { Name = msg.ToolCallId, Response = msg.Content } });
      }

      if(msg.ToolCalls is not null)
      {
        foreach (var t in msg.ToolCalls)
        {
          content.Parts.Add(new Part() { FunctionCall = new FunctionCall() { Name = t.Name, Args = t.Parameters } });
        }
      }

      yield return content;
    }
  
  }

  private static Content? ExtractInstructions(List<Message> instructions)
  {
    var system_instruction = new Content();

    foreach (var item in instructions)
      system_instruction.Parts.Add(new Part() { Text = item.Content });

    return instructions.Count > 0 ? system_instruction : null;
  }

  private static List<Tool>? ExtractTools(List<Function> functions)
  {
    if(functions.Count == 0)
      return null;

    return new List<Tool>()
    {
      new Tool()
      {
        FunctionDeclarations = functions.Select(fn => new FunctionDeclaration() 
        { 
          Name = fn.Name,
          Description = fn.Description,
          Parameters = fn.Schema,
        }).ToList(),
      }
    };
  }

  public async Task<ChatResponse> GetChatAsync(ChatRequest chatRequest, CancellationToken ct = default)
  {
    if (chatRequest is null)
      throw new ArgumentNullException(nameof(chatRequest));

    var instructions = chatRequest.Messages.Where(_ => _.Role == Models.Role.System).ToList();
    var messages = chatRequest.Messages.Where(_ => _.Role != Models.Role.System).ToList();

    var request = new CompletionRequest()
    {
      Model = chatRequest.Model,
      GenerationConfiguration = new GenerationConfiguration()
      {
        MaxTokens = chatRequest.MaxTokens ?? 1024,
        Temperature = chatRequest.Temperature ?? 0.8,
        ResponseMimeType = chatRequest.ResponseFormat == ResponseFormat.JsonObject ? "application/json" : null
      },
      Contents = ConvertMessageToContent(messages).ToList(),
      SystemInstruction = ExtractInstructions(instructions),
      Tools = ExtractTools(chatRequest.Functions),
    };

    var dto = await GetChatAsync(request, ct);
    if (dto is null)
      throw new InvalidOperationException("Failed Chat Operation.");

    var choice = dto.FirstChoice;
    if (choice is null)
      throw new InvalidOperationException("No Valid Choice in Chat Response.");

    var message = choice.Content;
    if (message is null)
      throw new InvalidOperationException("No Valid Message in Chat Choice.");

    var content = message.Parts.Where(_ => _.Text is not null).Select(_ => _.Text).Aggregate((acc, content) => acc + Environment.NewLine + content);
    var tools = message.Parts.Where(_ => _.FunctionCall is not null).Select(_ => new Models.Tool(_?.FunctionCall?.Name, _?.FunctionCall?.Name, _?.FunctionCall?.Args)).ToList();

    var response = new ChatResponse()
    {
      Id = Guid.NewGuid().ToString(),
      CreatedAt = DateTime.UtcNow,
      Content = content,
      FinishReason = FinishReason(choice.FinishReason),
      Role = Models.Role.Assistant,
      Tools = tools,
      RateLimits = null,
      Usage = Usage(dto.UsageMetadata)
    };
    return response;
  }
}