using System.ComponentModel.DataAnnotations;

namespace Boundless.OmniAdapter.Gemini;

public class GeminiSettings
{
  [Required(ErrorMessage = "GeminiApiKey required")]
  public string? GeminiApiKey { get; set; }
}
