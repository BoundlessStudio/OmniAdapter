using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.OpenAi.Models.Image
{
    public sealed class Image
    {
        [JsonInclude]
        [JsonPropertyName("url")]
        public string Url { get; private set; }

        [JsonInclude]
        [JsonPropertyName("b64_json")]
        public string B64_Json { get; private set; }

        [JsonInclude]
        [JsonPropertyName("revised_prompt")]
        public string RevisedPrompt { get; private set; }

        public static implicit operator string(Image result) => result?.ToString();

        public override string ToString()
            => !string.IsNullOrWhiteSpace(Url)
                ? Url
                : !string.IsNullOrWhiteSpace(B64_Json)
                    ? B64_Json
                    : string.Empty;
    }
}
