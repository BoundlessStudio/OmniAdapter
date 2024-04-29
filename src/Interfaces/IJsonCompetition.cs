using Boundless.OmniAdapter.Models;

namespace Boundless.OmniAdapter.Interfaces;

public interface IJsonCompetition
{
  Task<ChatResponse> GetJsonAsync(ChatRequest chatRequest, CancellationToken cancellationToken = default);
}
