using System.ComponentModel.DataAnnotations;

namespace Boundless.OmniAdapter.OpenAi;

public class OpenAiSettings
{
  [Required(ErrorMessage = "OpenAiApiKey required")]
  public string? OpenAiApiKey { get; set; }
  public string? OpenAiOrganization { get; set; }
  public string? OpenAiProject { get; set; }
}
