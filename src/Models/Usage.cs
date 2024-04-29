namespace Boundless.OmniAdapter;

public class Usage
{
  public Usage(int promptTokens, int completionTokens, int totalTokens)
  {
    PromptTokens = promptTokens;
    CompletionTokens = completionTokens;
    TotalTokens = totalTokens;
  }

  public int? PromptTokens { get; private set; }

  public int? CompletionTokens { get; private set; }

  public int? TotalTokens { get; private set; }
}
