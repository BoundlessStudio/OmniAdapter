using Boundless.OmniAdapter.Interfaces;

namespace Boundless.OmniAdapter.Kernel
{
  public class KernelBuilder
  {
    private List<IChatCompletion> chatCompletions;
    private List<IAudioCompletion> audioCompletions;
    private List<IImageCompletion> imageCompletions;

    public KernelBuilder()
    {
      this.chatCompletions = new List<IChatCompletion>();
      this.audioCompletions = new List<IAudioCompletion>();
      this.imageCompletions = new List<IImageCompletion>();
    }

    public KernelBuilder WithChatCompletion(IChatCompletion chatCompletion)
    {
      chatCompletions.Add(chatCompletion);
      return this;
    }

    public KernelBuilder WithAudioCompletion(IAudioCompletion audioCompletion)
    {
      audioCompletions.Add(audioCompletion);
      return this;
    }

    public KernelBuilder WithImageCompletion(IImageCompletion imageCompletion)
    {
      this.imageCompletions.Add(imageCompletion);
      return this;
    }

    public IKernel Build()
    {
      return new Kernel(chatCompletions, audioCompletions, imageCompletions);
    }
  }
}
