//using System.Text.Json.Nodes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;


namespace Boundless.OmniAdapter.Generators;

[Generator]
public class FunctionGenerator : ISourceGenerator
{
  public void Initialize(GeneratorInitializationContext context)
  {
    // No initialization required for this example
  }

  public void Execute(GeneratorExecutionContext context)
  {
    //var functions = new List<MemberDeclarationSyntax>();

    //foreach (var syntaxTree in context.Compilation.SyntaxTrees)
    //{
    //  var semanticModel = context.Compilation.GetSemanticModel(syntaxTree);
    //  var methods = syntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>();

    //  foreach (var method in methods)
    //  {
    //    var symbol = semanticModel.GetDeclaredSymbol(method);
    //    var attribute = symbol.GetAttributes().FirstOrDefault(ad =>
    //        ad.AttributeClass.ToDisplayString() == typeof(AiFunctionAttribute).FullName);

    //    if (attribute != null)
    //    {
    //      var name = attribute.NamedArguments.FirstOrDefault(kvp => kvp.Key == "Name").Value.Value.ToString();
    //      var description = attribute.NamedArguments.FirstOrDefault(kvp => kvp.Key == "Description").Value.Value.ToString();

    //      // Creating JSON Schema for parameters
    //      var parameters = new JsonObject();
    //      foreach (var parameter in method.ParameterList.Parameters)
    //      {
    //        var paramType = parameter.Type.ToString();
    //        parameters[paramType] = JsonValue.Create(parameter.Identifier.ToString());
    //      }

    //      var property = PropertyDeclaration(ParseTypeName("FunctionTool"), Identifier(name))
    //          .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
    //          .WithAccessorList(AccessorList(SingletonList(
    //              AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
    //          )))
    //          .WithInitializer(EqualsValueClause(
    //              ObjectCreationExpression(ParseTypeName("FunctionTool"))
    //              .AddArgumentListArguments(
    //                  Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(name))),
    //                  Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(description))),
    //                  Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(parameters.ToJsonString())))
    //              )))
    //          .WithLeadingTrivia(Comment($"// {description}"))
    //          .NormalizeWhitespace();

    //      functions.Add(property);
    //    }
    //  }
    //}

    //var classDeclaration = ClassDeclaration("Tools")
    //    .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
    //    .AddMembers(functions.ToArray());

    //var namespaceDeclaration = NamespaceDeclaration(ParseName("Generated"))
    //    .AddMembers(classDeclaration);

    //var compilationUnit = CompilationUnit()
    //    .AddUsings(UsingDirective(ParseName("System")))
    //    .AddUsings(UsingDirective(ParseName("System.Text.Json.Nodes")))
    //    .AddMembers(namespaceDeclaration)
    //    .NormalizeWhitespace();

    var buidler = new StringBuilder();
    buidler.AppendLine("using System;");
    buidler.AppendLine("using System.Text.Json.Nodes;");
    buidler.AppendLine("namespace Generated.Tools;");
    buidler.AppendLine("public class Functions");
    buidler.AppendLine("{");
    buidler.AppendLine("");
    buidler.AppendLine("}");

    context.AddSource("Tools.g.cs", SourceText.From(buidler.ToString(), Encoding.UTF8));
  }
}