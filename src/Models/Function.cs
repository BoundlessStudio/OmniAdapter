using Json.Schema;
using Json.Schema.Generation;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Boundless.OmniAdapter.Models;

public class Function
{
  private const string NameRegex = "^[a-zA-Z0-9_-]{1,64}$";


  public static Function CreateFrom<T>(T action) where T : Delegate
  {
    var methodInfo = action.Method;
    var parameters = methodInfo.GetParameters();
    if (parameters.Length > 1 || parameters.Length == 0)
      throw new InvalidOperationException("The method you want to register must have a single input paratmter");

    var attribute = methodInfo.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();
    if (attribute is null)
      throw new InvalidOperationException("You must include a [Description] Attribute on the method you want to register.");

    var parameter = parameters.Single();
    var type = parameter.ParameterType;
    var schema = new JsonSchemaBuilder().FromType(type).Build();
    var fn = new Function(methodInfo.Name, attribute.Description, schema, type);
    return fn;
  }
  public static Function CreateFrom<T>(string name, string description, T action) where T : Delegate
  {
    var methodInfo = action.Method;
    var parameters = methodInfo.GetParameters();
    if (parameters.Length > 1 || parameters.Length == 0)
      throw new InvalidOperationException("The method you want to register must have a single input paratmter");

    var parameter = parameters.Single();
    var type = parameter.ParameterType;
    var schema = new JsonSchemaBuilder().FromType(type).Build();
    var fn = new Function(name, description, schema, type);
    return fn;
  }

  /// <summary>
  /// Creates a new function description to insert into a chat conversation.
  /// </summary>
  /// <param name="name">
  /// Required. The name of the function to generate arguments for based on the context in a message.<br/>
  /// May contain a-z, A-Z, 0-9, underscores and dashes, with a maximum length of 64 characters. Recommended to not begin with a number or a dash.
  /// </param>
  /// <param name="description">
  /// An description of the function, used by the API to determine if it is useful to include in the response.
  /// </param>
  /// <param name="parameters">
  /// An JSON describing the parameters of the function that the model can generate.
  /// </param>
  public Function(string name, string description, JsonSchema schema, Type input)
  {
    if (Regex.IsMatch(name, NameRegex) == false) 
      throw new ArgumentException($"The name of the function does not conform to naming standards: {NameRegex}");

    Name = name;
    Description = description;
    Schema = schema;
    Input = input;
  }

  /// <summary>
  /// The name of the function to generate arguments for.<br/>
  /// May contain a-z, A-Z, 0-9, and underscores and dashes, with a maximum length of 64 characters.
  /// Recommended to not begin with a number or a dash.
  /// </summary>

  public string Name { get; set; }

  /// <summary>
  /// The description of the function.
  /// </summary>
  public string Description { get; set; }

  /// <summary>
  /// 
  /// </summary>
  public JsonSchema Schema { get; set; }


  /// <summary>
  /// 
  /// </summary>
  [JsonIgnore]
  public Type Input { get; set; }

}