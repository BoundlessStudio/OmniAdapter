﻿using Boundless.OmniAdapter.Interfaces;
using Boundless.OmniAdapter.Models;
using Polly;
using Polly.Retry;
using System.Text;
using System.Text.Json;
using System.Threading.RateLimiting;

namespace Boundless.OmniAdapter.Kernel
{
  public interface IKernel
  {

  }

  internal class Kernel : IKernel
  {
    private IReadOnlyList<IChatCompletion> chatCompletions;

    private ResiliencePipeline pipeline;

    internal Kernel(IReadOnlyList<IChatCompletion> chatCompletions)
    {
      this.chatCompletions = chatCompletions;

      var delay = new RetryStrategyOptions();
      var limiter = new SlidingWindowRateLimiter(new() { PermitLimit = 100, Window = TimeSpan.FromMinutes(1) });

      // Create an instance of builder that exposes various extensions for adding resilience strategies
      this.pipeline = new ResiliencePipelineBuilder()
        .AddTimeout(TimeSpan.FromSeconds(60))
        .AddRetry(delay)
        .AddRateLimiter(limiter)
        .Build();
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
      var completion = chatCompletions.FirstOrDefault();
      var batch = new List<Message>(messages);
      var builder = new StringBuilder();

      while (true)
      {
        var request = new ChatRequest()
        {
          //Model = "",
          Messages = batch,
          ResponseFormat = ResponseFormat.JsonObject
        };

        ChatResponse response = await pipeline.ExecuteAsync(async (CancellationToken token) => await completion.GetChatAsync(request, token), cancellationToken);

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
      var completion = chatCompletions.FirstOrDefault();
      var batch = new List<Message>(messages);

      while (true)
      {
        var request = new ChatRequest()
        {
          // Model = 
          Messages = batch,
          // Functions = 
        };

        ChatResponse response = await pipeline.ExecuteAsync(async (CancellationToken token) => await completion.GetChatAsync(request, token), cancellationToken);

        switch (response.FinishReason)
        {
          case FinishReason.Length:
            // Continue the conversation
            batch.Add(new Message() { Role = Role.Assistant, Content = response.Content });
            //batch.Add(new Message() { Role = Role.User, Content = "continue" });
            break;
          case FinishReason.Tool:
            // Call the tools and resume the conversation
            batch.Add(new Message() { Role = Role.Assistant, Content = response.Content, ToolCalls = response.Tools.ToList() });
            foreach (var tool in response.Tools)
            {
              var results = string.Empty;
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