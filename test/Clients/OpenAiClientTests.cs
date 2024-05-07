using Boundless.OmniAdapter.OpenAi;
using Microsoft.Extensions.Options;
using Moq;
using Json.Schema;
using Json.Schema.Generation;
using System.Text.Json.Serialization;
using System.Reflection;
using Boundless.OmniAdapter.Tests.Utilities;

namespace Boundless.OmniAdapter.Tests.Clients;


[TestClass]
public class OpenAiClientTests
{
  private OpenAiClient _client;
  private string defaultModel = "gpt-4-turbo";

  [TestInitialize]
  public void Initialize()
  {
    var client = new HttpClient();
    var factory = new StaticHttpClientFactory(client);

    var settings = new OpenAiSettings
    {
      OpenAiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY"),
      OpenAiOrganization = Environment.GetEnvironmentVariable("OPENAI_ORGANIZATION"),
      OpenAiProject = Environment.GetEnvironmentVariable("OPENAI_PROJECT"),
    };
    var options = new Mock<IOptions<OpenAiSettings>>();
    options.Setup(ap => ap.Value).Returns(settings);
    _client = new OpenAiClient(factory, options.Object);
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
  [DataRow(1)]
  [DataRow(3)]
  [DataRow(5)]
  [DataRow(10)]
  public async Task RequestIterrations(int iterrations)
  {
    var messages = new List<InputMessage> {
      new SystemMessage("You are a helpful assistant."),
      new UserMessage("What model are your using?"),
    };

    var request = new CompletionRequest
    {
      Model = defaultModel,
      Messages = messages,
      MaxTokens = 100,
      N = iterrations,
    };

    var response = await _client.GetChatAsync(request);

    Assert.IsNotNull(response);
    Assert.IsNotNull(response.FirstChoice);
    Assert.AreEqual(iterrations, response.Choices.Count);
  }

  [TestMethod]
  public async Task RequestStop()
  {
    var messages = new List<InputMessage> {
      new SystemMessage("You are a helpful assistant."),
      new UserMessage("Create a list of 10 items"),
    };

    var request = new CompletionRequest
    {
      Model = defaultModel,
      Messages = messages,
      MaxTokens = 1000,
      Stop = new() { "6" }
    };

    var response = await _client.GetChatAsync(request);

    Assert.IsNotNull(response);
    Assert.IsNotNull(response.FirstChoice);
    var content = response.FirstChoice?.Message?.Content;
    Assert.IsNotNull(content);
    Assert.IsFalse(content.Contains("10."));

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
  [DataRow(-2)]
  [DataRow(-1)]
  [DataRow(0)]
  [DataRow(1)]
  [DataRow(2)]
  public async Task RequestFrequency(int frequency)
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
  [DataRow("A98FD88E-5D2B-4FC1-8411-B72A2A3F1462")]
  public async Task RequestUser(string user)
  {
    var messages = new List<InputMessage> {
      new SystemMessage("You are a helpful assistant."),
      new UserMessage("What model are your using?"),
    };

    var request = new CompletionRequest
    {
      Model = defaultModel,
      Messages = messages,
      MaxTokens = 100,
      User = user,
    };

    var response = await _client.GetChatAsync(request);

    Assert.IsNotNull(response);
    Assert.IsNotNull(response.FirstChoice);
    var content = response.FirstChoice?.Message?.Content;
    Assert.IsNotNull(content);

    Console.WriteLine(content);
  }


  [TestMethod]
  public async Task RequestSeed()
  {
    var messages = new List<InputMessage> {
      new SystemMessage("You are a helpful assistant."),
      new UserMessage("What model are your using?"),
    };

    var request = new CompletionRequest
    {
      Model = defaultModel,
      Messages = messages,
      MaxTokens = 100,
      Seed = 34578349,
    };

    var response = await _client.GetChatAsync(request);

    Assert.IsNotNull(response);
    Assert.IsNotNull(response.FirstChoice);
    var content = response.FirstChoice?.Message?.Content;
    Assert.IsNotNull(content);
    Console.WriteLine(content);

    var finger_print = "fp_ea6eb70039";
    Assert.AreEqual(finger_print, response.SystemFingerprint);
  }


  [TestMethod]
  [DataRow(false)]
  [DataRow(true)]
  public async Task RequestProbabilities(bool log)
  {
    var messages = new List<InputMessage> {
      new SystemMessage("You are a helpful assistant."),
      new UserMessage("What model are your using?"),
    };

    var request = new CompletionRequest
    {
      Model = defaultModel,
      Messages = messages,
      MaxTokens = 100,
      LogProbs = log,
    };

    var response = await _client.GetChatAsync(request);

    Assert.IsNotNull(response);
    Assert.IsNotNull(response.FirstChoice);

    if (log)
    {
      Assert.IsNotNull(response.FirstChoice.LogProbs);
    }
    else
    {
      Assert.IsNull(response.FirstChoice.LogProbs);
    }
  }

  public class Args
  {
    [System.ComponentModel.DataAnnotations.Required]
    [JsonPropertyName("input")]
    public string Input { get; set; } = string.Empty;

    [JsonPropertyName("table")]
    public string Table { get; set; }
  }

  [System.ComponentModel.Description("")]
  private void ActionTest(Args args)
  {
  }

  [System.ComponentModel.Description("")]
  private string FunctionTest()
  {
    return string.Empty;
  }

  private InputFunction GetFunction<T>(T action) where T : Delegate
  {
    var methodInfo = action.Method;
    var attribute = methodInfo.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();
    var parameters = methodInfo.GetParameters();
    if (parameters.Length > 1) throw new ArgumentOutOfRangeException("parameters");
    var parameter = parameters.FirstOrDefault();
    if (parameter is null)
    {
      return new InputFunction(methodInfo.Name, attribute?.Description, null);
    }
    else
    {
      var schema = new JsonSchemaBuilder().FromType(parameter.ParameterType).Build();
      return new InputFunction(methodInfo.Name, attribute?.Description, schema);
    }
  }

  [TestMethod]
  public async Task RequestTools()
  {
    var messages = new List<InputMessage> {
      new SystemMessage("You are a helpful assistant."),
      new UserMessage("What is result of ActionTest?"),
    };

    var fn1 = GetFunction(ActionTest);
    var fn2 = GetFunction(FunctionTest);

    var request = new CompletionRequest
    {
      Model = defaultModel,
      Messages = messages,
      MaxTokens = 100,
      Tools = [fn1, fn2]
    };

    var response = await _client.GetChatAsync(request);

    Assert.IsNotNull(response);
    Assert.IsNotNull(response.FirstChoice);
    Assert.AreEqual("tool_calls", response.FirstChoice.FinishReason);
    Assert.IsNotNull(response.FirstChoice.Message);
    Assert.IsNotNull(response.FirstChoice.Message.ToolCalls);
    Assert.AreEqual(1, response.FirstChoice.Message.ToolCalls.Count);
    var function = response.FirstChoice.Message.ToolCalls.First().Function;
    Assert.IsNotNull(function);

    Console.WriteLine(function.Name);
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
  public async Task RequestJsonMode()
  {
    var messages = new List<InputMessage> {
      new SystemMessage("You are a helpful assistant."),
      new UserMessage("Return a example json document."),
    };

    var request = new CompletionRequest
    {
      Model = defaultModel,
      Messages = messages,
      MaxTokens = 1000,
      ResponseFormat = ChatResponseFormat.JsonObject
    };

    var response = await _client.GetChatAsync(request);

    Assert.IsNotNull(response);
    Assert.IsNotNull(response.FirstChoice);
    var content = response.FirstChoice?.Message?.Content;
    Assert.IsNotNull(content);
    Console.WriteLine(content);
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
    Assert.AreEqual(23, response.Usage.PromptTokens);
    Assert.AreEqual(33, response.Usage.TotalTokens);
  }


  [TestMethod]
  public async Task RequestRateLimits()
  {
    var messages = new List<InputMessage> {
      new SystemMessage("You are a helpful assistant."),
      new UserMessage("What model are your using?"),
    };

    var request = new CompletionRequest
    {
      Model = defaultModel,
      Messages = messages,
      MaxTokens = 100,
    };

    var response = await _client.GetChatAsync(request);

    Assert.IsNotNull(response);
    Assert.IsNotNull(response.RateLimits);
    var limits = response.RateLimits;

    Console.WriteLine($"Limit");
    Console.WriteLine($" Requests: {limits.LimitRequests}");
    Console.WriteLine($" Tokens: {limits.LimitTokens}");
    Console.WriteLine($"Remaining");
    Console.WriteLine($" Requests: {limits.RemainingRequests}");
    Console.WriteLine($" Tokens: {limits.RemainingTokens}");
    Console.WriteLine($"Reset");
    Console.WriteLine($" Requests: {limits.ResetRequests}");
    Console.WriteLine($" Tokens: {limits.ResetTokens}");
  }


}
