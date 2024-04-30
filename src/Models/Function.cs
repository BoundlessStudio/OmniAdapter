using Json.Schema;
using System.Text.RegularExpressions;

namespace Boundless.OmniAdapter.Models;

public class Function
{
  private const string NameRegex = "^[a-zA-Z0-9_-]{1,64}$";

  public Function()
  {
    this.Name = string.Empty;
  }

  /// <summary>
  /// Creates a new function description to insert into a chat conversation.
  /// </summary>
  /// <param name="name">
  /// Required. The name of the function to generate arguments for based on the context in a message.<br/>
  /// May contain a-z, A-Z, 0-9, underscores and dashes, with a maximum length of 64 characters. Recommended to not begin with a number or a dash.
  /// </param>
  /// <param name="description">
  /// An optional description of the function, used by the API to determine if it is useful to include in the response.
  /// </param>
  /// <param name="parameters">
  /// An optional JSON describing the parameters of the function that the model can generate.
  /// </param>
  public Function(string name, string? description, JsonSchema? parameters)
  {
    if (Regex.IsMatch(name, NameRegex) == false) throw new ArgumentException($"The name of the function does not conform to naming standards: {NameRegex}");

    Name = name;
    Description = description;
    Parameters = parameters;
  }

  /// <summary>
  /// The name of the function to generate arguments for.<br/>
  /// May contain a-z, A-Z, 0-9, and underscores and dashes, with a maximum length of 64 characters.
  /// Recommended to not begin with a number or a dash.
  /// </summary>

  public string Name { get; set; }

  /// <summary>
  /// The optional description of the function.
  /// </summary>
  public string? Description { get; set; }

  /// <summary>
  /// 
  /// </summary>
  public JsonSchema? Parameters { get; set; }

}