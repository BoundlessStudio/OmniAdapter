using Boundless.OmniAdapter.Models;

namespace Boundless.OmniAdapter.Interfaces;

public interface IAudioCompletion
{
  Task<TranscriptionResponse> GetTranscriptionAsync(TranscriptionRequest request, CancellationToken cancellationToken);
  Task<SpeechResponse> GetSpeechAsync(SpeechRequest request, CancellationToken cancellationToken);
}
