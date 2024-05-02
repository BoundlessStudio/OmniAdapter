using Json.Schema;
using System.Text.Json.Nodes;

namespace Boundless.OmniAdapter.Models;

public class Tool
{
  public Tool()
  {
  }

  public Tool(string? id, string? name, JsonNode? parameters)
  {
    this.Id = id;
    this.Name = name;
    this.Parameters = parameters;
  }

  /// <summary>
  /// The id of the tool call.
  /// </summary>
  public string? Id { get; set; }

  /// <summary>
  /// The name of the function to call.
  /// </summary>
  public string? Name { get; set; }


  /// <summary>
  /// 
  /// </summary>
  public JsonNode? Parameters { get; set; }
}