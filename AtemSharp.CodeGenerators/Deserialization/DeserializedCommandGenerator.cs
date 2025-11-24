using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace AtemSharp.CodeGenerators.Deserialization
{
    [Generator]
    public class DeserializedCommandGenerator : IIncrementalGenerator
    {
        private const string Template = "DeserializeMethodTemplate.sbn";

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
                                       .Where(f => f.GetAttributes().Any(a => a.AttributeClass?.Name == "DeserializedFieldAttribute" || a.AttributeClass?.Name == "CustomDeserializationAttribute"))
                                       .Select(f => ProcessField(f, spc))
                                       .ToArray();

                    if (fields.Contains(null))
                    {
                        // Do not generate code if any field is missing a mapping
                        continue;
                    }

                    // Use ScribanLite for template rendering
                    var templateText = Helpers.LoadTemplate(Template, spc);
                    if (templateText is null) continue;

                    string source;
                    try
                    {
                        source = ScribanLite.Render(templateText, new Dictionary<string, object>
                        {
                            { "namespace", ns },
                            { "className", className },
                            { "deserializedFields", fields },
                            { "internalDeserialization", HasInternalDeserializationMethod(classDecl) ? "result.DeserializeInternal(rawCommand, protocolVersion);": string.Empty } ,
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

        private static string? CreatePropertyCode(IFieldSymbol f, SourceProductionContext spc)
        {
            var propertyName = Helpers.GetPropertyName(f);
            var fieldType = Helpers.GetFieldType(f);
            var serializedType = Helpers.GetSerializedFieldType(f);
            var hasProperty = !f.GetAttributes().Any(a => a.AttributeClass?.Name == "NoPropertyAttribute");

            var propertyCode = string.Empty;
            if (hasProperty)
            {
                var template = Helpers.LoadTemplate("DeserializedField_FullProperty.sbn", spc);
                if (template is null) return null;
                propertyCode = ScribanLite.Render(template, new Dictionary<string, object>
                {
                    { "propertyName", propertyName },
                    { "fieldName", f.Name },
                    { "fieldType", fieldType },
                    { "serializedType", serializedType },
                    { "msdoc", Helpers.GetFieldMsDocComment(f)}
                });
            }

            return propertyCode;
        }

        private static string? CreateDeserializationCode(IFieldSymbol f, SourceProductionContext spc)
        {
            if (f.GetAttributes().Any(a => a.AttributeClass?.Name == "CustomDeserializationAttribute")) return string.Empty;

            var offset = Helpers.GetDeserializationOffest(f);
            var propertyName = Helpers.GetPropertyName(f);
            var fieldType = Helpers.GetFieldType(f);
            var isDouble = fieldType == "double" || fieldType == "System.Double";
            var extensionMethod = Helpers.GetSerializationMethod(f);
            var scalingFactor = Helpers.GetScalingFactor(f);
            var serializedType = Helpers.GetSerializedFieldType(f);
            var isEnum = serializedType.TypeKind == TypeKind.Enum;
            var hasProperty = !f.GetAttributes().Any(a => a.AttributeClass?.Name == "NoPropertyAttribute");

            if (extensionMethod is null)
            {
                var descriptor = DiagnosticDescriptors.CreateFieldTypeError(f);
                spc.ReportDiagnostic(Diagnostic.Create(descriptor, f.Locations.FirstOrDefault() ?? Location.None));
                return null;
            }

            var scalingCode = string.Empty;
            if (scalingFactor.HasValue)
            {
                var scalingLiteral = isDouble
                                         ? scalingFactor.Value.ToString("0.0#############################", CultureInfo.InvariantCulture)
                                         : scalingFactor.Value.ToString("0", CultureInfo.InvariantCulture);

                scalingCode = $" / {scalingLiteral}";
            }

            var serializationTemplate = isEnum ? Helpers.LoadTemplate("DeserializedField_EnumDeserialization.sbn", spc) : Helpers.LoadTemplate("DeserializedField_Deserialization.sbn", spc);
            if (serializationTemplate is null) return null;

            return ScribanLite.Render(serializationTemplate, new Dictionary<string, object>
            {
                { "propertyName", hasProperty ? propertyName : f.Name },
                { "fieldType", fieldType },
                { "extensionMethod", extensionMethod },
                { "offset", offset },
                { "scaling", scalingCode },
                { "serializedType",  serializedType },
                { "customScalingFunction", Helpers.GetAttributeStringValue(f, "CustomScalingAttribute") ?? string.Empty },
            });
        }

        private static DeserializedField? ProcessField(IFieldSymbol f, SourceProductionContext spc)
        {
            var propertyCode = CreatePropertyCode(f, spc);
            var serializationCode = CreateDeserializationCode(f, spc);
            if (serializationCode is null || propertyCode is null) return null;

            return new DeserializedField
            {
                PropertyCode = propertyCode,
                DeserializationCode = serializationCode
            };
        }
    }
}
