using Boundless.OmniAdapter.Models;

namespace Boundless.OmniAdapter.Interfaces;

public interface IChatCompetition
{
  Task<ChatResponse> GetChatAsync(ChatRequest chatRequest, CancellationToken cancellationToken = default);
}
