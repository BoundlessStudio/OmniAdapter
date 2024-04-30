using Boundless.OmniAdapter.Anthropic;

using Boundless.OmniAdapter.Tests.Utilities;
using Json.Schema;
using Json.Schema.Generation;
using Microsoft.Extensions.Options;
using Moq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Tests.Clients;

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
        var messages = new List<InputMessage> {
      new UserMessage("What model are your using?"),
    };

        var request = new CompletionRequest
        {
            Model = defaultModel,
            Messages = messages,
            SystemPrommpt = "You are a helpful assistant.",
            MaxTokens = max_tokens
        };

        var response = await _client.GetChatAsync(request);

        Assert.IsNotNull(response);
        Assert.AreEqual(1, response.Content?.Count);
        var content = response.Content?.FirstOrDefault();
        Assert.IsNotNull(content);

        Console.WriteLine(content.Text);
    }

    [TestMethod]
    public async Task RequestStop()
    {
        var messages = new List<InputMessage> {
      new UserMessage("Create a list of 10 items"),
    };

        var request = new CompletionRequest
        {
            SystemPrommpt = "You are a helpful assistant.",
            Model = defaultModel,
            Messages = messages,
            MaxTokens = 1000,
            Stop = new() { "6" }
        };

        var response = await _client.GetChatAsync(request);

        Assert.IsNotNull(response);
        Assert.AreEqual(1, response.Content?.Count);
        var content = response.Content?.FirstOrDefault();
        Assert.IsNotNull(content);

        Console.WriteLine(content.Text);
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
    public async Task RequestTemperature(double temperature)
    {
        var messages = new List<InputMessage> {
      new UserMessage("What model are your using?"),
    };

        var request = new CompletionRequest
        {
            SystemPrommpt = "You are a helpful assistant.",
            Model = defaultModel,
            Messages = messages,
            MaxTokens = 1000,
            Temperature = temperature,
        };

        var response = await _client.GetChatAsync(request);

        Assert.IsNotNull(response);
        Assert.AreEqual(1, response.Content?.Count);
        var content = response.Content?.FirstOrDefault();
        Assert.IsNotNull(content);

        Console.WriteLine(content.Text);
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
      new UserMessage("What model are your using?"),
    };

        var request = new CompletionRequest
        {
            SystemPrommpt = "You are a helpful assistant.",
            Model = defaultModel,
            Messages = messages,
            MaxTokens = 1000,
            TopP = p,
        };

        var response = await _client.GetChatAsync(request);

        Assert.IsNotNull(response);
        Assert.AreEqual(1, response.Content?.Count);
        var content = response.Content?.FirstOrDefault();
        Assert.IsNotNull(content);

        Console.WriteLine(content.Text);
    }


    [TestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(40)]
    [DataRow(100)]
    public async Task RequestTopK(double k)
    {
        var messages = new List<InputMessage> {
      new UserMessage("What model are your using?"),
    };

        var request = new CompletionRequest
        {
            SystemPrommpt = "You are a helpful assistant.",
            Model = defaultModel,
            Messages = messages,
            MaxTokens = 1000,
            TopK = k
        };

        var response = await _client.GetChatAsync(request);

        Assert.IsNotNull(response);
        Assert.AreEqual(1, response.Content?.Count);
        var content = response.Content?.FirstOrDefault();
        Assert.IsNotNull(content);

        Console.WriteLine(content.Text);
    }


    public class Args
    {
        [Required]
        [JsonPropertyName("input")]
        public string Input { get; set; } = string.Empty;

        [JsonPropertyName("table")]
        public string? Table { get; set; }
    }

    [System.ComponentModel.Description("")]
    private void ActionTest(Args args)
    {
    }

    private InputFunction GetFunction<T>(T action) where T : Delegate
    {
        var methodInfo = action.Method;
        var attribute = methodInfo.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();
        var parameters = methodInfo.GetParameters();
        if (parameters.Length > 1) throw new ArgumentOutOfRangeException("parameters");
        var schemas = parameters.Select(p => new JsonSchemaBuilder().FromType(p.ParameterType).Build()).ToList();
        return new InputFunction(methodInfo.Name, attribute?.Description, schemas.FirstOrDefault());
    }

    [TestMethod]
    public async Task RequestTools()
    {
        var messages = new List<InputMessage> {
      new UserMessage("What is result of ActionTest? with input = abc and table = example1"),
    };

        var fn1 = GetFunction(ActionTest);

        var request = new CompletionRequest
        {
            SystemPrommpt = "You are a helpful assistant.",
            Model = defaultModel,
            Messages = messages,
            MaxTokens = 1000,
            Tools = [fn1]
        };
        var json = JsonSerializer.Serialize(request);

        var response = await _client.GetChatAsync(request);

        Assert.IsNotNull(response);
        Assert.AreEqual("tool_use", response.StopReason);
        Assert.IsNotNull(response.Content);
        var content = response.Content.FirstOrDefault(_ => _.Type == "tool_use");
        Assert.IsNotNull(content);
        Assert.IsNotNull(content.ToolId);
        Assert.IsNotNull(content.ToolName);
        Assert.IsNotNull(content.ToolInput);

        Console.WriteLine(content.ToolInput);
    }

    [TestMethod]
    public async Task RequestStreaming()
    {
        var messages = new List<InputMessage> {
      new UserMessage("What model are your using?"),
    };

        var request = new CompletionRequest
        {
            SystemPrommpt = "You are a helpful assistant.",
            Model = defaultModel,
            Messages = messages,
            MaxTokens = 1000,
        };

        var stream = _client.StreamChatAsync(request);
        await foreach (var chunk in stream)
        {
            Console.Write(chunk);
        }
    }


}
