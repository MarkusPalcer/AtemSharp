using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace AtemSharp.CodeGenerators.Serialization
{
    [Generator]
    public class SerializedCommandGenerator : IIncrementalGenerator
    {
        private const string Template = "SerializeMethodTemplate.sbn";

        private static string? GetPropertyCode(IFieldSymbol f, SourceProductionContext spc)
        {
            var flag = GetFlag(f);
            var propertyName = Helpers.GetPropertyName(f);
            var validationMethod = GetValidationMethod(f);
            var fieldType = Helpers.GetFieldType(f);

            // Determine Property-Template
            string? propertyTemplate;
            if (f.GetAttributes().Any(a => a.AttributeClass?.Name == "NoPropertyAttribute"))
            {
                propertyTemplate = Helpers.LoadTemplate("SerializedField_NoProperty.sbn", spc);
            }
            else if (flag is null)
            {
                propertyTemplate = Helpers.LoadTemplate("SerializedField_NoFlag.sbn", spc);
            }
            else
            {
                propertyTemplate = Helpers.LoadTemplate("SerializedField_FullProperty.sbn", spc);
            }

            if (propertyTemplate is null) return null;

            return ScribanLite.Render(propertyTemplate, new System.Collections.Generic.Dictionary<string, object>
            {
                { "propertyName", propertyName },
                { "fieldName", f.Name },
                { "fieldType", fieldType },
                { "flagBit", flag ?? 0 },
                { "validation", validationMethod is null ? string.Empty : $"{validationMethod}(value);" },
                { "msdoc", Helpers.GetFieldMsDocComment(f) }
            });
        }

        private static SerializedField? ProcessField(IFieldSymbol f, SourceProductionContext spc)
        {
            var propertyCode = GetPropertyCode(f, spc);
            var serializationCode = GetSerializationCode(f, spc);

            if (serializationCode is null || propertyCode is null) return null;

            return new SerializedField
            {
                SerializationCode = serializationCode,
                PropertyCode = propertyCode,
            };
        }

        private static string? GetSerializationCode(IFieldSymbol f, SourceProductionContext spc)
        {
            if (f.GetAttributes().Any(a => a.AttributeClass?.Name == "CustomSerializationAttribute")) return string.Empty;

            var offset = Helpers.GetSerializationOffest(f);
            var fieldType = Helpers.GetFieldType(f);
            var isDouble = fieldType == "double" || fieldType == "System.Double";
            var extensionMethod = Helpers.GetSerializationMethod(f);
            var scalingFactor = Helpers.GetScalingFactor(f);

            if (extensionMethod is null)
            {
                var descriptor = DiagnosticDescriptors.CreateFieldTypeError(f);
                spc.ReportDiagnostic(Diagnostic.Create(descriptor, f.Locations.FirstOrDefault() ?? Location.None));
                return null;
            }

            var extensionMethodType = Helpers.GetExtensionMethodType(extensionMethod);

            var serializationTemplate = Helpers.LoadTemplate("SerializedField_Serialization.sbn", spc);
            if (serializationTemplate is null) return null;

            var scalingCode = string.Empty;
            if (scalingFactor.HasValue)
            {
                var scalingLiteral = isDouble
                                         ? scalingFactor.Value.ToString("0.0#############################", CultureInfo.InvariantCulture)
                                         : scalingFactor.Value.ToString("0", CultureInfo.InvariantCulture);

                scalingCode = $" * {scalingLiteral}";
            }

            var serializationCode = ScribanLite.Render(serializationTemplate,
                                                       new System.Collections.Generic.Dictionary<string, object>
                                                       {
                                                           { "extensionMethod", extensionMethod },
                                                           { "extensionMethodType", extensionMethodType },
                                                           { "fieldName", f.Name },
                                                           { "scaling", fieldType },
                                                           { "offset", offset },
                                                           { "scalingFactor", scalingCode },
                                                           {
                                                               "customScalingFunction",
                                                               Helpers.GetAttributeStringValue(f, "CustomScalingAttribute") ??
                                                               string.Empty
                                                           }
                                                       });
            return serializationCode;
        }

        private static byte? GetFlag(IFieldSymbol f)
        {
            var attr = f.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == "SerializedFieldAttribute");
            if (attr != null && attr.ConstructorArguments.Length > 1)
            {
                var arg = attr.ConstructorArguments[1];
                return arg.Value is byte b ? (byte?)b : null;
            }

            attr = f.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == "CustomSerializationAttribute");
            if (attr != null && attr.ConstructorArguments.Length > 0)
            {
                var arg = attr.ConstructorArguments[0];
                return arg.Value is byte b ? (byte?)b : null;
            }

            return null;
        }


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
                    var classSymbol = semanticModel.GetDeclaredSymbol(classDecl);
                    if (classSymbol == null) continue;

                    // Check if class has SerializedCommand as base type in its type hierarchy
                    if (!InheritsFrom(classSymbol, "SerializedCommand"))
                        continue;

                    var ns = classSymbol.ContainingNamespace.ToDisplayString();
                    var className = classSymbol.Name;

                    // Find fields with SerializedFieldAttribute

                    var fields = classSymbol.GetMembers()
                                            .OfType<IFieldSymbol>()
                                            .Where(f => f.GetAttributes().Any(a => a.AttributeClass?.Name == "SerializedFieldAttribute" || a.AttributeClass?.Name == "CustomSerializationAttribute"))
                                            .Select(f => ProcessField(f, spc))
                                            .ToArray();

                    if (fields.Contains(null))
                    {
                        // Do not generate code if any field is missing a mapping
                        continue;
                    }

                    // Get BufferSizeAttribute from the class
                    var bufferSize = GetBufferSize(classSymbol);

                    if (bufferSize is null)
                    {
                        var diag = Diagnostic.Create(
                            DiagnosticDescriptors.SerializedClassMustHaveBufferSize,
                            classDecl.Identifier.GetLocation(),
                            classSymbol.Name);
                        spc.ReportDiagnostic(diag);
                        continue;
                    }

                    // Use ScribanLite for template rendering
                    var templateText = Helpers.LoadTemplate(Template, spc);
                    if (templateText is null) continue;
                    string source;
                    try
                    {
                        source = ScribanLite.Render(templateText, new System.Collections.Generic.Dictionary<string, object>
                        {
                            { "namespace", ns },
                            { "className", className },
                            { "serializedFields", fields },
                            { "bufferSize", bufferSize.Value },
                            {
                                "internalSerialization",
                                HasInternalSerializationMethod(classDecl, classSymbol, spc)
                                    ? "SerializeInternal(buffer);"
                                    : string.Empty
                            },
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

        private static int? GetBufferSize(INamedTypeSymbol classSymbol)
        {
            var bufferSizeAttr = classSymbol.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == "BufferSizeAttribute");
            if (bufferSizeAttr == null || bufferSizeAttr.ConstructorArguments.Length <= 0) return null;

            var arg = bufferSizeAttr.ConstructorArguments[0];
            if (arg.Value is int size) return size;

            return null;
        }

        private static bool InheritsFrom(INamedTypeSymbol? symbol, string baseTypeName)
        {
            while (symbol is { BaseType: { } })
            {
                if (symbol.BaseType.Name == baseTypeName)
                    return true;
                symbol = symbol.BaseType;
            }

            return false;
        }

        private bool HasInternalSerializationMethod(ClassDeclarationSyntax classDecl, INamedTypeSymbol classSymbol,
                                                    SourceProductionContext spc)
        {
            // Look for: void SerializeInternal(ReadOnlySpan<byte>)
            foreach (var member in classDecl.Members)
            {
                if (!(member is MethodDeclarationSyntax method)) continue;

                if (method.Identifier.Text != "SerializeInternal" ||
                    !method.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword) || m.IsKind(SyntaxKind.PrivateKeyword) ||
                                               m.IsKind(SyntaxKind.InternalKeyword))) continue;

                if (method.ReturnType is PredefinedTypeSyntax returnType && returnType.Keyword.IsKind(SyntaxKind.VoidKeyword) &&
                    method.ParameterList.Parameters.Count == 1 && method.ParameterList.Parameters[0].Type?.ToString() == "byte[]")
                {
                    return true;
                }
                else
                {
                    var diag = Diagnostic.Create(
                        DiagnosticDescriptors.CustomSerializationSignature,
                        classDecl.Identifier.GetLocation(),
                        classSymbol.Name);
                    spc.ReportDiagnostic(diag);

                    return false;
                }
            }

            return false;
        }

        private static string? GetValidationMethod(IFieldSymbol f)
            => Helpers.GetAttributeStringValue(f, "ValidationMethodAttribute");
    }
}
