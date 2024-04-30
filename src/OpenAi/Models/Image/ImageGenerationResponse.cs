using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.OpenAi.Models.Image
{
    public class ImageGenerationResponse
    {
        [JsonInclude]
        [JsonPropertyName("created")]
        public int Created { get; private set; }

        [JsonInclude]
        [JsonPropertyName("data")]
        public IReadOnlyList<Image> Results { get; private set; }
    }
}
