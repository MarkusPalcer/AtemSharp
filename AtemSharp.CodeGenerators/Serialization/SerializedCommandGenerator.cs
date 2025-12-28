using System.Globalization;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AtemSharp.CodeGenerators.Serialization;

[Generator]
public class SerializedCommandGenerator : CodeGeneratorBase
{
    protected override bool ClassFilter(INamedTypeSymbol classSymbol, ClassDeclarationSyntax classDecl) =>
        Helpers.InheritsFrom(classSymbol, "SerializedCommand")
     && classDecl.Modifiers.Any(SyntaxKind.PartialKeyword)
     && classSymbol.GetMembers().OfType<IFieldSymbol>().Any();

    protected override void ProcessClass(SourceProductionContext spc, INamedTypeSymbol classSymbol, ClassDeclarationSyntax classDecl)
    {
        var ns = classSymbol.ContainingNamespace.ToDisplayString();
        var className = classSymbol.Name;

        // Find fields with SerializedFieldAttribute

        var fields = classSymbol.GetMembers()
                                .OfType<IFieldSymbol>()
                                .Where(f => f.GetAttributes().Any(a => a.AttributeClass?.Name is "SerializedFieldAttribute"
                                                                                              or "SerializedField"
                                                                                              or "CustomSerializationAttribute"
                                                                                              or "CustomSerialization"))
                                .Select(f => ProcessField(f, spc))
                                .ToArray();

        if (fields.Contains(null) || fields.Length == 0)
        {
            // Do not generate code if any field is missing a mapping
            return;
        }

        // Get BufferSizeAttribute from the class
        var bufferSize = GetBufferSize(classSymbol);

        if (bufferSize is null)
        {
            var diag = Diagnostic.Create(DiagnosticDescriptors.SerializedClassMustHaveBufferSize, classDecl.Identifier.GetLocation(),
                                         classSymbol.Name);
            spc.ReportDiagnostic(diag);
            return;
        }

        var internalSerialization = GetInternalSerializationCode(classDecl, classSymbol, spc);

        var namespaces = new[]
        {
            "using System;",
            "using System.Reflection;",
            "using System.Diagnostics.CodeAnalysis;",
            "using AtemSharp;",
            "using AtemSharp.State.Info;",
            "using static AtemSharp.Commands.SerializationExtensions;"
        }.Concat(fields.Select(x => x!.NamespaceCode)).Distinct();

        var fileContent = $$"""
                            {{string.Join("\n", namespaces)}}

                            namespace {{ns}};

                            #nullable enable annotations

                            public partial class {{className}}
                            {
                                {{string.Join("\n", fields.Select(x => x!.PropertyCode))}}

                                /// <inheritdoc />
                                public override byte[] Serialize(ProtocolVersion version) {
                                    var commandVersion = typeof({{className}}).GetCustomAttribute<CommandAttribute>()?.MinimumVersion;
                                    if (commandVersion is not null && commandVersion < version) {
                                        throw new InvalidOperationException($"The command {{className}} only works for protocol version minimum {commandVersion}, but you are using protocol version {version}");
                                    }

                                    var buffer = new byte[{{bufferSize.Value}}];
                                    buffer.WriteUInt8((byte)this.Flag, 0);

                                    {{string.Join("\n", fields.Select(x => x!.SerializationCode))}}

                                    {{internalSerialization}}

                                    return buffer;
                                }
                            }
                            #nullable restore
                            """;

        spc.AddSource($"{className}.g.cs", fileContent);
    }

    private static SerializedField? ProcessField(IFieldSymbol f, SourceProductionContext spc)
    {
        var serializationCode = GetSerializationCode(f, spc);
        if (serializationCode is null)
        {
            return null;
        }

        var propertyCode = GetPropertyCode(f);

        return new SerializedField
        {
            SerializationCode = serializationCode,
            PropertyCode = propertyCode,
            NamespaceCode = Helpers.CreateNamespaceCode(f)
        };
    }

    private static string GetPropertyCode(IFieldSymbol f)
    {
        if (f.GetAttributes().Any(a => a.AttributeClass?.Name == "NoPropertyAttribute"))
        {
            return string.Empty;
        }

        var flag = GetFlag(f);
        var flagCode = flag is null ? string.Empty : $"Flag |= 1 << {flag};";
        var propertyName = Helpers.GetPropertyName(f);
        var validationMethod = GetValidationMethod(f);
        var validationCode = validationMethod is null ? string.Empty : $"{validationMethod}(value);";
        var fieldType = Helpers.GetFieldType(f);
        var docComment = Helpers.GetFieldMsDocComment(f);
        var visibility = f.GetAttributes().Any(a => a.AttributeClass?.Name == "InternalPropertyAttribute") ? "internal" : "public";

        var setter = f.IsReadOnly
                         ? string.Empty
                         : $$"""
                           set {
                               {{validationCode}}
                               {{f.Name}} = value;
                               {{flagCode}}
                           }
                           """;

        return $$"""
                 {{docComment}}
                 {{visibility}} {{fieldType}} {{propertyName}}
                 {
                     [ExcludeFromCodeCoverage]
                     get => {{f.Name}};
                     {{setter}}
                 }
                 """;
    }

    private static string? GetSerializationCode(IFieldSymbol f, SourceProductionContext spc)
    {
        if (f.GetAttributes().Any(a => a.AttributeClass?.Name == "CustomSerializationAttribute"))
        {
            return string.Empty;
        }

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

        var scalingCode = string.Empty;
        if (scalingFactor.HasValue)
        {
            var scalingLiteral = isDouble
                                     ? scalingFactor.Value.ToString("0.0#############################", CultureInfo.InvariantCulture)
                                     : scalingFactor.Value.ToString("0", CultureInfo.InvariantCulture);

            scalingCode = $" * {scalingLiteral}";
        }

        var customScalingFunction = Helpers.GetAttributeStringValue(f, "CustomScalingAttribute") ??
                                    string.Empty;

        return $"buffer.Write{extensionMethod}(({extensionMethodType}){customScalingFunction}({f.Name} {scalingCode}), {offset});";
    }

    private string GetInternalSerializationCode(ClassDeclarationSyntax classDecl, INamedTypeSymbol classSymbol,
                                                SourceProductionContext spc)
    {
        // Look for: void SerializeInternal(ReadOnlySpan<byte>)
        foreach (var member in classDecl.Members)
        {
            if (!(member is MethodDeclarationSyntax method))
            {
                continue;
            }

            if (method.Identifier.Text != "SerializeInternal" ||
                !method.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword) || m.IsKind(SyntaxKind.PrivateKeyword) ||
                                           m.IsKind(SyntaxKind.InternalKeyword)))
            {
                continue;
            }

            if (method.ReturnType is PredefinedTypeSyntax returnType && returnType.Keyword.IsKind(SyntaxKind.VoidKeyword))
            {
                var parameterTypes = method.ParameterList.Parameters.Select(p => p.Type?.ToString()).ToArray();

                if (parameterTypes.SequenceEqual(new[] { "byte[]" }))
                {
                    return "SerializeInternal(buffer);";
                }

                if (parameterTypes.SequenceEqual(new[] { "byte[]", "ProtocolVersion" }))
                {
                    return "SerializeInternal(buffer, version);";
                }
            }

            var diag = Diagnostic.Create(
                DiagnosticDescriptors.CustomSerializationSignature,
                classDecl.Identifier.GetLocation(),
                classSymbol.Name);
            spc.ReportDiagnostic(diag);

            return string.Empty;
        }

        return string.Empty;
    }

    private static byte? GetFlag(IFieldSymbol f)
    {
        var attr = f.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == "SerializedFieldAttribute");
        if (attr is { ConstructorArguments.Length: > 1 })
        {
            var arg = attr.ConstructorArguments[1];
            return arg.Value is byte b ? b : null;
        }

        attr = f.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == "CustomSerializationAttribute");
        if (attr is { ConstructorArguments.Length: > 0 })
        {
            var arg = attr.ConstructorArguments[0];
            return arg.Value is byte b ? b : null;
        }

        return null;
    }

    private static int? GetBufferSize(INamedTypeSymbol classSymbol)
    {
        var bufferSizeAttr = classSymbol.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == "BufferSizeAttribute");
        if (bufferSizeAttr == null || bufferSizeAttr.ConstructorArguments.Length <= 0)
        {
            return null;
        }

        var arg = bufferSizeAttr.ConstructorArguments[0];
        if (arg.Value is int size)
        {
            return size;
        }

        return null;
    }

    private static string? GetValidationMethod(IFieldSymbol f)
        => Helpers.GetAttributeStringValue(f, "ValidationMethodAttribute");
}
