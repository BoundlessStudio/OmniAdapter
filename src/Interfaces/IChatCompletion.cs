using Boundless.OmniAdapter.Models;

namespace Boundless.OmniAdapter.Interfaces;

public interface IChatCompletion
{
  Task<ChatResponse> GetChatAsync(ChatRequest chatRequest, CancellationToken cancellationToken = default);
}
