using Json.Schema;
using System.Text.Json.Nodes;

namespace Boundless.OmniAdapter.Models;

public class Tool
{
  public Tool()
  {
  }

  public Tool(string? name, JsonNode? parameters)
  {
    this.Name = name;
    this.Parameters = parameters;
  }

  /// <summary>
  /// The name of the function to call.
  /// </summary>
  public string? Name { get; set; }


  /// <summary>
  /// 
  /// </summary>
  public JsonNode? Parameters { get; set; }
}