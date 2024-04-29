using Boundless.OmniAdapter.Models;

namespace Boundless.OmniAdapter.Interfaces;

public interface IChatStream
{
  IAsyncEnumerable<ChunkResponse> StreamChatAsync(ChatRequest chatRequest, CancellationToken cancellationToken = default);
}
