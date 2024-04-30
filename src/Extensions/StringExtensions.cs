namespace Boundless.OmniAdapter;

internal static class StringExtensions
{
  /// <summary>
  /// Attempts to get the event data from the string data.
  /// Returns false once the stream is done.
  /// </summary>
  /// <param name="streamData">Raw stream data.</param>
  /// <param name="eventData"></param>
  /// <returns>Parsed stream data.</returns>
  public static string GetEventStreamData(this string streamData)
  {
    const string dataTag = "data: ";
    var eventData = string.Empty;

    if (streamData.StartsWith(dataTag))
    {
      eventData = streamData.Substring(dataTag.Length).Trim();
    }

    return eventData;
  }

  public static StringContent ToJsonStringContent(this string json)
  {
    const string jsonContent = "application/json";
    return new StringContent(json, null, jsonContent);
  }
}
