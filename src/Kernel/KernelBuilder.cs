using Boundless.OmniAdapter.Interfaces;
using Boundless.OmniAdapter.Models;

namespace Boundless.OmniAdapter.Kernel;

public class KernelBuilder
{
  private IChatCompletion? chatCompletion;
  private ChatSettings? chatSetting;
  private List<FunctionBinding> bindings = new List<FunctionBinding>();
  private IAudioCompletion? audioCompletion;
  private IImageCompletion? imageCompletion;

  public KernelBuilder()
  {
  }

  public KernelBuilder AddClient<C>(C completion) where C : IChatCompletion, IAudioCompletion, IImageCompletion
  {
    this.chatCompletion = completion;
    this.audioCompletion = completion;
    this.imageCompletion = completion;
    return this;
  }

  public KernelBuilder WithChatCompletion(ChatSettings settings)
  {
    this.chatSetting = settings;
    return this;
  }
  public KernelBuilder WithChatCompletion(IChatCompletion completion, ChatSettings settings)
  {
    this.chatCompletion = completion;
    this.chatSetting = settings;
    return this;
  }
  public KernelBuilder AddFunction<T>(T action) where T : Delegate
  {
    var fn = Function.CreateFrom(action);
    var binding = new FunctionBinding(fn, action);
    this.bindings.Add(binding);
    return this;
  }


  public KernelBuilder WithAudioCompletion(AudioSettings settings)
  {
    return this;
  }
  public KernelBuilder WithAudioCompletion(IAudioCompletion completion, AudioSettings settings)
  {
    this.audioCompletion = completion;
    return this;
  }

  public KernelBuilder WithImageCompletion(ImageSettings settings)
  {
    return this;
  }
  public KernelBuilder WithImageCompletion(IImageCompletion completion, ImageSettings settings)
  {
    this.imageCompletion = completion;
    return this;
  }

  

  public IKernel Build()
  {
    if (this.chatCompletion is null || this.chatSetting is null)
      throw new InvalidOperationException("Chat completion and settings must be set.");

    return new Kernel(this.chatCompletion, this.chatSetting, this.bindings);
  }
}


public class AudioSettings
{

}
public class ImageSettings
{

}