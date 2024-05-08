using Boundless.OmniAdapter.Interfaces;
using Boundless.OmniAdapter.Models;
using System.Text;
using System.Text.Json;

namespace Boundless.OmniAdapter.Kernel
{
  public interface IKernel
  {
    Task<T?> GetObject<T>(IEnumerable<Message> messages, CancellationToken cancellationToken = default) where T : class;
    Task<string?> GetJson(IEnumerable<Message> messages, CancellationToken cancellationToken = default);
    Task<IEnumerable<Message>> RunThreadAsync(IEnumerable<Message> messages, CancellationToken cancellationToken = default);
  }

  internal class Kernel : IKernel
  {
    private readonly IChatCompletion chatCompletion;
    private readonly ChatSettings chatSetting;
    private readonly List<FunctionBinding> bindings;
    internal Kernel(IChatCompletion chat, ChatSettings setting, List<FunctionBinding> bindings)
    {
      this.chatCompletion = chat;
      this.chatSetting = setting;
      this.bindings = bindings;
    }

    public async Task<T?> GetObject<T>(IEnumerable<Message> messages, CancellationToken cancellationToken = default) where T : class
    {
      var json = await GetJson(messages, cancellationToken);
      if(json is null) return default;
      var obj = JsonSerializer.Deserialize<T>(json);
      return obj;
    }

    public async Task<string?> GetJson(IEnumerable<Message> messages, CancellationToken cancellationToken = default)
    {
      var batch = new List<Message>(messages);
      var builder = new StringBuilder();

      while (true)
      {
        var request = new ChatRequest()
        {
          Model = this.chatSetting.Model,
          Temperature = this.chatSetting.Temperature ?? 0.1,
          MaxTokens = this.chatSetting.MaxTokens ?? 1000,
          Messages = batch,
          ResponseFormat = ResponseFormat.JsonObject
        };

        ChatResponse response = await this.chatCompletion.GetChatAsync(request, cancellationToken);

        switch (response.FinishReason)
        {
          case FinishReason.Length:
            builder.Append(response.Content);
            batch.Add(new Message() { Role = Role.Assistant, Content = response.Content });
            //batch.Add(new Message() { Role = Role.User, Content = "continue" });
            break;
          case FinishReason.Stop:
            var json = builder.ToString();
            return json;
          default:
            throw new InvalidOperationException("Invalid finish reason.");
        }
      }
    }

    public async Task<IEnumerable<Message>> RunThreadAsync(IEnumerable<Message> messages, CancellationToken cancellationToken = default)
    {
      var batch = new List<Message>(messages);

      while (true)
      {
        var request = new ChatRequest()
        {
          Model = this.chatSetting.Model,
          Temperature = this.chatSetting.Temperature ?? 0.1,
          MaxTokens = this.chatSetting.MaxTokens ?? 1000,
          Messages = batch,
          Functions = this.bindings.Select(b => b.Function).ToList()
        };

        ChatResponse response = await this.chatCompletion.GetChatAsync(request, cancellationToken);

        switch (response.FinishReason)
        {
          case FinishReason.Length:
            // Continue the conversation
            batch.Add(new Message() { Role = Role.Assistant, Content = response.Content });
            // batch.Add(new Message() { Role = Role.User, Content = "continue" });
            break;
          case FinishReason.Tool:
            // Call the tools and resume the conversation
            batch.Add(new Message() { Role = Role.Assistant, Content = response.Content, ToolCalls = response.Tools.ToList() });
            foreach (var tool in response.Tools)
            {
              var binding = this.bindings.FirstOrDefault(b => b.Function.Name == tool.Name);
              if (binding is null) continue;

              var input = tool.Parameters.Deserialize(binding.Function.Input);
              var output = binding.Action.DynamicInvoke(input);
              var results = JsonSerializer.Serialize(output);
              batch.Add(new Message() { Role = Role.Tool, Content = results, Name = tool.Name, ToolCallId = tool.Id });
            }
            break;
          case FinishReason.ContentFilter:
          case FinishReason.Stop:
            // Return results
            return batch;
          case FinishReason.None:
          default:
            throw new InvalidOperationException("Invalid finish reason.");
        }
      }
    }

  }
}
