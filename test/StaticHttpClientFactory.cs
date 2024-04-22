﻿namespace Boundless.OmniAdapter.Tests
{
  internal class StaticHttpClientFactory : IHttpClientFactory
  {
    private readonly HttpClient _client;

    public StaticHttpClientFactory(HttpClient client)
    {
      _client = client;
    }

    public HttpClient CreateClient(string name)
    {
      return _client;
    }
  }
}
