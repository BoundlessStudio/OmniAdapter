using Boundless.OmniAdapter.Models;

namespace Boundless.OmniAdapter.Interfaces;

public interface IJsonStream
{
  IAsyncEnumerable<ChunkResponse> StreamJsonAsync(ChatRequest chatRequest, CancellationToken cancellationToken = default);
}
