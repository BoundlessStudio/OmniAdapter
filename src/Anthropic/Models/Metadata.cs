using System.Text.Json.Serialization;

namespace Boundless.OmniAdapter.Anthropic.Models
{
  public class Metadata
  {
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }
  }
}
