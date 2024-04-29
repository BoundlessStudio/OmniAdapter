using Json.Schema;
using System.Text.Json.Nodes;

namespace Boundless.OmniAdapter;

public class Tool
{
  /// <summary>
  /// The name of the function to call.
  /// </summary>
  public string? Name { get; set; }


  /// <summary>
  /// 
  /// </summary>
  public JsonObject? Parameters { get; set; }
}