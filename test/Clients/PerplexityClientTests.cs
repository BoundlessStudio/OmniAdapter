using Boundless.OmniAdapter.Perplexity;
using Boundless.OmniAdapter.Tests.Utilities;
using Microsoft.Extensions.Options;
using Moq;

namespace Boundless.OmniAdapter.Tests.Clients;

[TestClass]
public class PerplexityClientTests
{
  private PerplexityClient _client;
  private string defaultModel = "mistral-7b-instruct";

  [TestInitialize]
  public void Initialize()
  {
    var client = new HttpClient();
    var factory = new StaticHttpClientFactory(client);

    var settings = new PerplexitySettings
    {
      PerplexityApiKey = Environment.GetEnvironmentVariable("PERPLEXITY_API_KEY"),
    };
    var options = new Mock<IOptions<PerplexitySettings>>();
    options.Setup(ap => ap.Value).Returns(settings);
    _client = new PerplexityClient(factory, options.Object);
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
    var messages = new List<InputMessage> {
      new SystemMessage("You are a helpful assistant."),
      new UserMessage("What model are your using?"),
    };

    var request = new CompletionRequest
    {
      Model = defaultModel,
      Messages = messages,
      MaxTokens = max_tokens
    };

    var response = await _client.GetChatAsync(request);

    Assert.IsNotNull(response);
    Assert.IsNotNull(response.FirstChoice);
    var content = response.FirstChoice?.Message?.Content;
    Assert.IsNotNull(content);

    Console.WriteLine(content);
  }

  [TestMethod]
  [DataRow(-2)]
  [DataRow(-1)]
  [DataRow(0)]
  [DataRow(1)]
  [DataRow(2)]
  public async Task RequestPresence(int presence)
  {
    var messages = new List<InputMessage> {
      new SystemMessage("You are a helpful assistant."),
      new UserMessage("What model are your using?"),
    };

    var request = new CompletionRequest
    {
      Model = defaultModel,
      Messages = messages,
      MaxTokens = 1000,
      PresencePenalty = presence
    };

    var response = await _client.GetChatAsync(request);

    Assert.IsNotNull(response);
    Assert.IsNotNull(response.FirstChoice);
    var content = response.FirstChoice?.Message?.Content;
    Assert.IsNotNull(content);

    Console.WriteLine(content);
  }

  [TestMethod]
  [DataRow(0.1)]
  [DataRow(1.0)]
  [DataRow(2.0)]
  public async Task RequestFrequency(double frequency)
  {
    var messages = new List<InputMessage> {
      new SystemMessage("You are a helpful assistant."),
      new UserMessage("What model are your using?"),
    };

    var request = new CompletionRequest
    {
      Model = defaultModel,
      Messages = messages,
      MaxTokens = 1000,
      FrequencyPenalty = frequency
    };

    var response = await _client.GetChatAsync(request);

    Assert.IsNotNull(response);
    Assert.IsNotNull(response.FirstChoice);
    var content = response.FirstChoice?.Message?.Content;
    Assert.IsNotNull(content);

    Console.WriteLine(content);
  }

  [TestMethod]
  [DataRow(0)]
  [DataRow(0.1)]
  [DataRow(0.3)]
  [DataRow(0.3)]
  [DataRow(0.5)]
  [DataRow(0.7)]
  [DataRow(0.9)]
  [DataRow(1)]
  [DataRow(1.1)]
  [DataRow(1.3)]
  [DataRow(1.5)]
  public async Task RequestTemperature(double temperature)
  {
    var messages = new List<InputMessage> {
      new SystemMessage("You are a helpful assistant."),
      new UserMessage("What model are your using?"),
    };

    var request = new CompletionRequest
    {
      Model = defaultModel,
      Messages = messages,
      MaxTokens = 1000,
      Temperature = temperature,
    };

    var response = await _client.GetChatAsync(request);

    Assert.IsNotNull(response);
    Assert.IsNotNull(response.FirstChoice);
    var content = response.FirstChoice?.Message?.Content;
    Assert.IsNotNull(content);

    Console.WriteLine(content);
  }

  [TestMethod]
  [DataRow(0)]
  [DataRow(0.1)]
  [DataRow(0.3)]
  [DataRow(0.3)]
  [DataRow(0.5)]
  [DataRow(0.7)]
  [DataRow(0.9)]
  [DataRow(1)]
  public async Task RequestTopP(double p)
  {
    var messages = new List<InputMessage> {
      new SystemMessage("You are a helpful assistant."),
      new UserMessage("What model are your using?"),
    };

    var request = new CompletionRequest
    {
      Model = defaultModel,
      Messages = messages,
      MaxTokens = 1000,
      TopP = p,
    };

    var response = await _client.GetChatAsync(request);

    Assert.IsNotNull(response);
    Assert.IsNotNull(response.FirstChoice);
    var content = response.FirstChoice?.Message?.Content;
    Assert.IsNotNull(content);

    Console.WriteLine(content);
  }

  [TestMethod]
  public async Task RequestStreaming()
  {
    var messages = new List<InputMessage> {
      new SystemMessage("You are a helpful assistant."),
      new UserMessage("What model are your using?"),
    };

    var request = new CompletionRequest
    {
      Model = defaultModel,
      Messages = messages,
      MaxTokens = 1000,
    };

    var stream = _client.StreamChatAsync(request);
    await foreach (var chunk in stream)
    {
      Console.Write(chunk.FirstChoice?.Delta?.Content ?? string.Empty);
    }
  }

  [TestMethod]
  public async Task RequestUsage()
  {
    var messages = new List<InputMessage> {
      new SystemMessage("You are a helpful assistant."),
      new UserMessage("What model are your using?"),
    };

    var request = new CompletionRequest
    {
      Model = defaultModel,
      Messages = messages,
      MaxTokens = 10,
    };

    var response = await _client.GetChatAsync(request);

    Assert.IsNotNull(response);
    Assert.IsNotNull(response.Usage);
    Assert.AreEqual(10, response.Usage.CompletionTokens);
    Assert.AreEqual(22, response.Usage.PromptTokens);
    Assert.AreEqual(32, response.Usage.TotalTokens);
  }

}
