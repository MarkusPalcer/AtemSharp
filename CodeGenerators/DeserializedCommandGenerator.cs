using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CodeGenerators
{
    [Generator]
    public class DeserializedCommandGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var classDeclarations = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: (node, _) => node is ClassDeclarationSyntax cds && cds.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)),
                    transform: (ctx, _) => (ClassDeclarationSyntax)ctx.Node)
                .Where(classDecl => classDecl != null);

            var compilationAndClasses = context.CompilationProvider.Combine(classDeclarations.Collect());

            context.RegisterSourceOutput(compilationAndClasses, (spc, tuple) =>
            {
                var (compilation, classDecls) = tuple;
                foreach (var classDecl in classDecls)
                {
                    var semanticModel = compilation.GetSemanticModel(classDecl.SyntaxTree);
                    var symbol = semanticModel.GetDeclaredSymbol(classDecl);
                    if (symbol == null) continue;
                    if (!symbol.Interfaces.Any(i => i.Name == "IDeserializedCommand")) continue;

                    var ns = symbol.ContainingNamespace.ToDisplayString();
                    var className = symbol.Name;

                    // Find fields with DeserializedFieldAttribute
                    var deserializedFieldResults = symbol.GetMembers()
                                                         .OfType<IFieldSymbol>()
                                                         .Where(f => f.GetAttributes().Any(a => a.AttributeClass?.Name == "DeserializedFieldAttribute"))
                                                         .Select(f => {
                                                              var attr = f.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == "DeserializedFieldAttribute");
                                                              uint offset = 0;
                                                              if (attr != null && attr.ConstructorArguments.Length > 0)
                                                              {
                                                                  var arg = attr.ConstructorArguments[0];
                                                                  if (arg.Value is int intVal)
                                                                      offset = (uint)intVal;
                                                              }
                                                              // Derive property name from field name (remove leading underscores, capitalize first letter)
                                                              var propertyName = f.Name.TrimStart('_');
                                                              if (!string.IsNullOrEmpty(propertyName) && propertyName.Length > 1)
                                                                  propertyName = char.ToUpper(propertyName[0]) + propertyName.Substring(1);
                                                              else if (!string.IsNullOrEmpty(propertyName))
                                                                  propertyName = propertyName.ToUpper();

                                                              var fieldType = f.Type.ToDisplayString();
                                                              var isDouble = fieldType == "double" || fieldType == "System.Double";

                                                              var baseMethod = f.GetSerializationMethod();
                                                              var extensionMethod = baseMethod != null ? $"Read{baseMethod}" : "";

                                                              // Check for ScalingFactorAttribute
                                                              var scalingFactor = 1.0;
                                                              var scalingAttr = f.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == "ScalingFactorAttribute");
                                                              if (scalingAttr != null && scalingAttr.ConstructorArguments.Length > 0)
                                                              {
                                                                  var arg = scalingAttr.ConstructorArguments[0];
                                                                  if (arg.Value is double d)
                                                                      scalingFactor = d;
                                                                  else if (arg.Value is float fval)
                                                                      scalingFactor = (double)fval;
                                                                  else if (arg.Value is int ival)
                                                                      scalingFactor = (double)ival;
                                                              }

                                                              string deserializeExpression;
                                                              if (isDouble || scalingFactor != 1.0)
                                                              {
                                                                  // Always render scalingFactor as a floating-point literal
                                                                  var scalingLiteral = scalingFactor.ToString("0.0#############################", System.Globalization.CultureInfo.InvariantCulture);
                                                                  deserializeExpression = $"rawCommand.{extensionMethod}({offset}) / {scalingLiteral}";
                                                              }
                                                              else
                                                              {
                                                                  deserializeExpression = $"rawCommand.{extensionMethod}({offset})";
                                                              }

                                                              return new {
                                                                  Field = f,
                                                                  DeserializedField = new DeserializedField(
                                                                      f.Name,
                                                                      propertyName,
                                                                      fieldType,
                                                                      offset,
                                                                      deserializeExpression
                                                                  ),
                                                                  ExtensionMethod = extensionMethod
                                                              };
                                                          })
                                                         .ToArray();

                    // Report error for each field with missing extension method
                    var hasMissingExtensionMethod = false;
                    foreach (var result in deserializedFieldResults)
                    {
                        if (result.ExtensionMethod == null)
                        {
                            hasMissingExtensionMethod = true;
                            var descriptor = new DiagnosticDescriptor(
                                id: "GEN003",
                                title: "Missing Span Extension Method",
                                messageFormat: $"No SpanExtension method mapping found for field '{result.Field.Name}' of type '{result.Field.Type.ToDisplayString()}'.",
                                category: "SourceGenerator",
                                DiagnosticSeverity.Error,
                                isEnabledByDefault: true
                            );
                            spc.ReportDiagnostic(Diagnostic.Create(descriptor, result.Field.Locations.FirstOrDefault() ?? Location.None));
                        }
                    }
                    if (hasMissingExtensionMethod)
                    {
                        // Do not generate code if any field is missing a mapping
                        continue;
                    }

                    var deserializedFields = deserializedFieldResults.Select(r => r.DeserializedField).ToArray();

                    // Load template from embedded resource
                    var assembly = typeof(DeserializedCommandGenerator).Assembly;
                    var resourceName = assembly.GetManifestResourceNames()
                                               .FirstOrDefault(n => n.EndsWith("DeserializeMethodTemplate.sbn"));
                    string? templateText = null;
                    if (resourceName != null)
                    {
                        try
                        {
                            using (var stream = assembly.GetManifestResourceStream(resourceName))
                            using (var reader = new System.IO.StreamReader(stream))
                            {
                                templateText = reader.ReadToEnd();
                            }
                        }
                        catch (Exception ex)
                        {
                            var descriptor = new DiagnosticDescriptor(
                                id: "GEN001",
                                title: "Template Load Error",
                                messageFormat: $"Failed to load template: {ex.Message}",
                                category: "SourceGenerator",
                                DiagnosticSeverity.Error,
                                isEnabledByDefault: true
                            );
                            spc.ReportDiagnostic(Diagnostic.Create(descriptor, Location.None));
                            continue;
                        }
                    }
                    else
                    {
                        var descriptor = new DiagnosticDescriptor(
                            id: "GEN002",
                            title: "Template Not Found",
                            messageFormat: "Could not find DeserializeMethodTemplate.sbn as an embedded resource.",
                            category: "SourceGenerator",
                            DiagnosticSeverity.Error,
                            isEnabledByDefault: true
                        );
                        spc.ReportDiagnostic(Diagnostic.Create(descriptor, Location.None));
                        continue;
                    }

                    // Use ScribanLite for template rendering
                    string source;
                    try
                    {
                        var context = new System.Collections.Generic.Dictionary<string, object>
                        {
                            { "namespace", ns },
                            { "className", className },
                            { "deserializedFields", deserializedFields }
                        };
                        source = ScribanLite.ScribanLite.Render(templateText, context);
                    }
                    catch (Exception ex)
                    {
                        var descriptor = new DiagnosticDescriptor(
                            id: "GEN005",
                            title: "Template Render Error",
                            messageFormat: $"Exception while rendering template: {ex.Message}",
                            category: "SourceGenerator",
                            DiagnosticSeverity.Error,
                            isEnabledByDefault: true
                        );
                        spc.ReportDiagnostic(Diagnostic.Create(descriptor, Location.None));
                        continue;
                    }

                    spc.AddSource($"{className}.g.cs", SourceText.From(source, Encoding.UTF8));
                }
            });
        }
    }
}
