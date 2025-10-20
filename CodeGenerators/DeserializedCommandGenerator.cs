using System;
using System.IO;
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
                                                predicate: (node, _) =>
                                                    node is ClassDeclarationSyntax cds &&
                                                    cds.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)),
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
                    var fields = symbol.GetMembers()
                                       .OfType<IFieldSymbol>()
                                       .Where(f => f.GetAttributes().Any(a => a.AttributeClass?.Name == "DeserializedFieldAttribute"))
                                       .Select(f => ProcessField(f, spc))
                                       .ToArray();

                    if (fields.Contains(null))
                    {
                        // Do not generate code if any field is missing a mapping
                        continue;
                    }

                    // Load template from embedded resource
                    var assembly = typeof(DeserializedCommandGenerator).Assembly;
                    var resourceName = assembly.GetManifestResourceNames()
                                               .FirstOrDefault(n => n.EndsWith("DeserializeMethodTemplate.sbn"));
                    string? templateText;
                    if (resourceName != null)
                    {
                        try
                        {
                            using var stream = assembly.GetManifestResourceStream(resourceName);
                            if (stream == null)
                                throw new InvalidOperationException("Resource stream is null.");

                            using var reader = new StreamReader(stream);

                            templateText = reader.ReadToEnd();
                        }
                        catch (Exception ex)
                        {
                            spc.ReportDiagnostic(Diagnostic.Create(
                                                     DiagnosticDescriptors.TemplateLoadError,
                                                     Location.None,
                                                     ex.Message));
                            continue;
                        }
                    }
                    else
                    {
                        var descriptor = DiagnosticDescriptors.DeserializedTemplateLoadError;
                        spc.ReportDiagnostic(Diagnostic.Create(descriptor, Location.None));
                        continue;
                    }

                    // Use ScribanLite for template rendering
                    string source;
                    try
                    {
                        source = ScribanLite.ScribanLite.Render(templateText, new System.Collections.Generic.Dictionary<string, object>
                        {
                            { "namespace", ns },
                            { "className", className },
                            { "deserializedFields", fields },
                            { "internalDeserialization", HasInternalDeserializationMethod(classDecl) ? "result.DeserializeInternal(rawCommand, protocolVersion);": string.Empty } 
                        });
                    }
                    catch (Exception ex)
                    {
                        spc.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.CreateRenderError(ex), Location.None));
                        continue;
                    }

                    spc.AddSource($"{className}.g.cs", SourceText.From(source, Encoding.UTF8));
                }
            });
        }

        private static bool HasInternalDeserializationMethod(ClassDeclarationSyntax classDecl)
        {
            // Look for: void DeserializeInternal(ReadOnlySpan<byte>, ProtocolVersion)
            foreach (var member in classDecl.Members)
            {
                if (member is MethodDeclarationSyntax method)
                {
                    if (method.Identifier.Text == "DeserializeInternal" &&
                        method.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword) || m.IsKind(SyntaxKind.PrivateKeyword) || m.IsKind(SyntaxKind.InternalKeyword)) &&
                        method.ReturnType is PredefinedTypeSyntax returnType && returnType.Keyword.IsKind(SyntaxKind.VoidKeyword) &&
                        method.ParameterList.Parameters.Count == 2)
                    {
                        var param1 = method.ParameterList.Parameters[0];
                        var param2 = method.ParameterList.Parameters[1];
                        if (param1.Type?.ToString() == "ReadOnlySpan<byte>" && param2.Type?.ToString().Contains("ProtocolVersion") == true)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private static DeserializedField? ProcessField(IFieldSymbol f, SourceProductionContext spc)
        {
            var offset = GetOffset(f);
            var propertyName = GetPropertyName(f);
            var fieldType = GetFieldType(f, out var isDouble);
            var extensionMethod = GetExtensionMethod(f);
            var scalingFactor = GetScalingFactor(f);
            var isEnum = f.Type.TypeKind == TypeKind.Enum;

            if (extensionMethod is null)
            {
                var descriptor = DiagnosticDescriptors.CreateFieldTypeError(f);
                spc.ReportDiagnostic(Diagnostic.Create(descriptor, f.Locations.FirstOrDefault() ?? Location.None));
                return null;
            }

            return new DeserializedField(
                f.Name,
                propertyName,
                fieldType,
                offset,
                GenerateDeserializationExpression(isDouble, scalingFactor, extensionMethod, offset, fieldType, isEnum)
            );
        }


        private static string GenerateDeserializationExpression(bool isDouble, double scalingFactor, string extensionMethod, uint offset,
                                                                string fieldType, bool isEnum)
        {
            string expr;
            if (isDouble)
            {
                // Always render scalingFactor as a floating-point literal
                var scalingLiteral =
                    scalingFactor.ToString("0.0#############################", System.Globalization.CultureInfo.InvariantCulture);
                expr = $"rawCommand.{extensionMethod}({offset}) / {scalingLiteral}";
            }
            else
            {
                expr = $"rawCommand.{extensionMethod}({offset})";
            }

            if (isEnum)
            {
                // Prepend cast to enum type
                expr = $"({fieldType})({expr})";
            }

            return expr;
        }

        private static double GetScalingFactor(IFieldSymbol f)
        {
            // Check for ScalingFactorAttribute
            var scalingFactor = 1.0;
            var scalingAttr = f.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == "ScalingFactorAttribute");
            if (scalingAttr == null || scalingAttr.ConstructorArguments.Length <= 0) return scalingFactor;

            var arg = scalingAttr.ConstructorArguments[0];
            scalingFactor = arg.Value switch
            {
                double value => value,
                float value => value,
                int value => value,
                _ => scalingFactor
            };

            return scalingFactor;
        }

        private static string? GetExtensionMethod(IFieldSymbol f)
        {
            var baseMethod = f.GetSerializationMethod();
            return baseMethod != null ? $"Read{baseMethod}" : null;
        }

        private static string GetFieldType(IFieldSymbol f, out bool isDouble)
        {
            var fieldType = f.Type.ToDisplayString();
            isDouble = fieldType == "double" || fieldType == "System.Double";
            return fieldType;
        }

        private static string GetPropertyName(IFieldSymbol f)
        {
            // Derive property name from field name (remove leading underscores, capitalize first letter)
            var propertyName = f.Name.TrimStart('_');
            if (!string.IsNullOrEmpty(propertyName) && propertyName.Length > 1)
                propertyName = char.ToUpper(propertyName[0]) + propertyName.Substring(1);
            else if (!string.IsNullOrEmpty(propertyName))
                propertyName = propertyName.ToUpper();
            return propertyName;
        }

        private static uint GetOffset(IFieldSymbol f)
        {
            var attr = f.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == "DeserializedFieldAttribute");
            uint offset = 0;
            if (attr != null && attr.ConstructorArguments.Length > 0)
            {
                var arg = attr.ConstructorArguments[0];
                if (arg.Value is int intVal)
                    offset = (uint)intVal;
            }

            return offset;
        }
    }
}
