using Boundless.OmniAdapter.Anthropic;
using Boundless.OmniAdapter.Anthropic.Models;
using Microsoft.Extensions.Options;
using Moq;

namespace Boundless.OmniAdapter.Tests;

[TestClass]
public class AnthropicClientTests
{
  private AnthropicClient _client;
  private string defaultModel = "claude-3-opus-20240229";

  [TestInitialize]
  public void Initialize()
  {
    var client = new HttpClient();
    var factory = new StaticHttpClientFactory(client);

    var settings = new AnthropicSettings
    {
      AnthropicApiKey = Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY"),
    };
    var options = new Mock<IOptions<AnthropicSettings>>();
    options.Setup(ap => ap.Value).Returns(settings);
    _client = new AnthropicClient(factory, options.Object);
  }

  [TestCleanup]
  public void Cleanup()
  {
    _client.Dispose();
  }

  [TestMethod]
  [DataRow(1)]
  [DataRow(10)]
  [DataRow(100)]
  [DataRow(1000)]
  [DataRow(4000)]
  public async Task RequestMaxTokens(int max_tokens)
  {
    var messages = new List<Message> {
      new Message(Role.User, "What model are your using?"),
    };

    var request = new CompletionRequest
    {
      Model = defaultModel,
      Messages = messages,
      SystemPrommpt = "You are a helpful assistant.",
      MaxTokens = max_tokens
    };

    var response = await _client.GetChatCompletionAsync(request);

    Assert.IsNotNull(response);
    Assert.AreEqual(1, response.Content?.Count);
    var content = response.Content?.FirstOrDefault();
    Assert.IsNotNull(content);
    
    Console.WriteLine(content.Text);
  }


  [TestMethod]
  public async Task RequestStreaming()
  {
    var messages = new List<Message> {
      new Message(Role.User, "What model are your using?"),
    };

    var request = new CompletionRequest
    {
      Model = defaultModel,
      Messages = messages,
      MaxTokens = 1000,
    };

    var stream = _client.StreamChatCompletionAsync(request);
    await foreach (var chunk in stream)
    {
      Console.Write(chunk);
    }
  }


}
