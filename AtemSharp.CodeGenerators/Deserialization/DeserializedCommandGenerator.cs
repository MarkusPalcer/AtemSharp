using System.Globalization;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AtemSharp.CodeGenerators.Deserialization
{
    [Generator]
    public class DeserializedCommandGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var classDeclarations = context.SyntaxProvider
                                           .CreateSyntaxProvider(
                                                predicate: (node, _) =>
                                                    node is ClassDeclarationSyntax,
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
                    if (symbol == null)
                    {
                        continue;
                    }

                    if (!symbol.Interfaces.Any(i => i.Name == "IDeserializedCommand"))
                    {
                        continue;
                    }

                    var ns = symbol.ContainingNamespace.ToDisplayString();
                    var className = symbol.Name;

                    // Find fields with DeserializedFieldAttribute
                    var fields = symbol.GetMembers()
                                       .OfType<IFieldSymbol>()
                                       .Where(f => f.GetAttributes()
                                                    .Any(a => a.AttributeClass?.Name is "DeserializedFieldAttribute"
                                                                                     or "CustomDeserializationAttribute"))
                                       .Select(f => ProcessField(f, spc))
                                       .ToArray();

                    if (fields.Contains(null) || fields.Length == 0)
                    {
                        // Do not generate code if any field is missing a mapping
                        continue;
                    }

                    var namespaces = fields.Select(x => x!.NamespaceCode).Distinct();

                    var internalDeserialization = GetInternalDeserializationMethod(classDecl);

                    var fileContent = $$"""
                                        using System;
                                        using System.Diagnostics.CodeAnalysis;

                                        using AtemSharp;
                                        using AtemSharp.State.Info;
                                        using AtemSharp.Lib;

                                        {{string.Join("\n", namespaces)}}

                                        namespace {{ns}};

                                        #nullable enable annotations

                                        public partial class {{className}}
                                        {
                                            {{string.Join("\n", fields.Select(x => x!.PropertyCode))}}

                                            public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
                                            {
                                                var result = new {{className}} {
                                                    {{string.Join("\n", fields.Select(x => x!.DeserializationCode))}}
                                                };

                                                {{internalDeserialization}}
                                                return result;
                                            }
                                        }
                                        #nullable restore
                                        """;

                    spc.AddSource($"{className}.g.cs", fileContent);
                }
            });
        }

        private static string GetInternalDeserializationMethod(ClassDeclarationSyntax classDecl)
        {
            // Look for: void DeserializeInternal(ReadOnlySpan<byte>, ProtocolVersion)
            foreach (var member in classDecl.Members)
            {
                if (member is not MethodDeclarationSyntax method)
                {
                    continue;
                }

                if (method.Identifier.Text != "DeserializeInternal")
                {
                    continue;
                }

                if (!method.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword) || m.IsKind(SyntaxKind.PrivateKeyword) || m.IsKind(SyntaxKind.InternalKeyword)))
                {
                    continue;
                }

                if (method.ReturnType is not PredefinedTypeSyntax returnType || !returnType.Keyword.IsKind(SyntaxKind.VoidKeyword))
                {
                    continue;
                }

                var parameterTypes = method.ParameterList.Parameters.Select(p => p.Type?.ToString()).ToArray();

                if (parameterTypes.SequenceEqual(new[] { "ReadOnlySpan<byte>" }))
                {
                    return "result.DeserializeInternal(rawCommand);";
                }

                if (parameterTypes.SequenceEqual(new[] { "ReadOnlySpan<byte>", "ProtocolVersion" }))
                {
                    return "result.DeserializeInternal(rawCommand, protocolVersion);";
                }

            }

            return string.Empty;
        }

        private static string CreatePropertyCode(IFieldSymbol f)
        {
            var hasProperty = !f.GetAttributes().Any(a => a.AttributeClass?.Name == "NoPropertyAttribute");
            if (!hasProperty)
            {
                return string.Empty;
            }

            var propertyName = Helpers.GetPropertyName(f);
            var fieldType = Helpers.GetFieldType(f);

            return $$"""
                     {{Helpers.GetFieldMsDocComment(f)}}
                     [ExcludeFromCodeCoverage]
                     public {{fieldType}} {{propertyName}}
                     {
                         get => {{f.Name}};
                         internal set {
                             {{f.Name}} = value;
                         }
                     }
                     """;
        }

        private static string? CreateDeserializationCode(IFieldSymbol f, SourceProductionContext spc)
        {
            if (f.GetAttributes().Any(a => a.AttributeClass?.Name == "CustomDeserializationAttribute"))
            {
                return string.Empty;
            }

            var extensionMethod = Helpers.GetSerializationMethod(f);
            if (extensionMethod is null)
            {
                var descriptor = DiagnosticDescriptors.CreateFieldTypeError(f);
                spc.ReportDiagnostic(Diagnostic.Create(descriptor, f.Locations.FirstOrDefault() ?? Location.None));
                return null;
            }

            var offset = Helpers.GetDeserializationOffest(f);
            var propertyName = Helpers.GetPropertyName(f);
            var fieldType = Helpers.GetFieldType(f);
            var isDouble = fieldType == "double" || fieldType == "System.Double";
            var scalingFactor = Helpers.GetScalingFactor(f);
            var serializedType = Helpers.GetSerializedFieldType(f);
            var isEnum = serializedType.TypeKind == TypeKind.Enum;
            var hasProperty = !f.GetAttributes().Any(a => a.AttributeClass?.Name == "NoPropertyAttribute");

            var scalingCode = string.Empty;
            if (scalingFactor.HasValue)
            {
                var scalingLiteral = isDouble
                                         ? scalingFactor.Value.ToString("0.0#############################", CultureInfo.InvariantCulture)
                                         : scalingFactor.Value.ToString("0", CultureInfo.InvariantCulture);

                scalingCode = $" / {scalingLiteral}";
            }

            var customScalingFunction = Helpers.GetAttributeStringValue(f, "CustomScalingAttribute") ?? string.Empty;

            return isEnum
                       ? $"    {(hasProperty ? propertyName : f.Name)} = ({fieldType})({customScalingFunction}(({serializedType})rawCommand.Read{extensionMethod}({offset})){scalingCode}),"
                       : $"    {(hasProperty ? propertyName : f.Name)} = ({fieldType})({customScalingFunction}(rawCommand.Read{extensionMethod}({offset})){scalingCode}),";
        }

        private static DeserializedField? ProcessField(IFieldSymbol f, SourceProductionContext spc)
        {
            var serializationCode = CreateDeserializationCode(f, spc);
            if (serializationCode is null)
            {
                return null;
            }

            var propertyCode = CreatePropertyCode(f);

            var namespaceCode = CreateNamespaceCode(f);

            return new DeserializedField
            {
                PropertyCode = propertyCode,
                DeserializationCode = serializationCode,
                NamespaceCode = namespaceCode,
            };
        }

        private static string CreateNamespaceCode(IFieldSymbol fieldSymbol)
        {
            var type = fieldSymbol.Type;
            var ns = type.ContainingNamespace;

            if (ns is null || string.IsNullOrEmpty(ns.ToString()))
            {
                return string.Empty;
            }

            return $"using {ns};";
        }
    }
}
