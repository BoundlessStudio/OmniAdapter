using Boundless.OmniAdapter.Models;

namespace Boundless.OmniAdapter.Interfaces;

public interface IImageCompletion
{
  Task<ImageResponse?> GetImageAsync(ImageRequest request, CancellationToken cancellationToken);
}
