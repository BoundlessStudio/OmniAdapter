using Boundless.OmniAdapter.Interfaces;
using Boundless.OmniAdapter.Models;

namespace Boundless.OmniAdapter.Kernel;

public class KernelBuilder
{
  private IChatCompletion? chatCompletion;
  private ChatSettings? chatSetting;
  private List<FunctionBinding> bindings = new List<FunctionBinding>();

  public KernelBuilder()
  {
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