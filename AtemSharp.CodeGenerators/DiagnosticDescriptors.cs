using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
namespace AtemSharp.CodeGenerators
{
    [SuppressMessage("MicrosoftCodeAnalysisReleaseTracking", "RS2000:Add analyzer diagnostic IDs to analyzer release")]
    public static class DiagnosticDescriptors
    {
        public static DiagnosticDescriptor TemplateLoadError(string template) => new DiagnosticDescriptor(
            id: "GEN001",
            title: "Template Load Error",
            messageFormat: $"Failed to load template {template}: {{0}}",
            category: "SourceGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        public static DiagnosticDescriptor TemplateNotFound(string template) => new DiagnosticDescriptor(
            id: "GEN002",
            title: "Template Not Found",
            messageFormat: $"Could not find {template} as an embedded resource",
            category: "SourceGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        public static DiagnosticDescriptor CreateFieldTypeError(IFieldSymbol f)
        {
            return new DiagnosticDescriptor(
                id: "GEN003",
                title: "Missing Span Extension Method",
                messageFormat: $"No extension method mapping found for field '{f.Name}' of type '{f.Type.ToDisplayString()}'.",
                category: "SourceGenerator",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true
            );
        }

        public static DiagnosticDescriptor CreateRenderError(Exception ex) => new DiagnosticDescriptor(
            id: "GEN004",
            title: "Template Render Error",
            messageFormat: $"Exception while rendering template: {ex.Message}",
            category: "SourceGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        public static readonly DiagnosticDescriptor CodeGenClassMustBePartial = new DiagnosticDescriptor(
            "GEN005",
            "Class with DeserializedFieldAttribute or SerializedFieldAttribute must be partial",
            "Class '{0}' contains fields with DeserializedFieldAttribute or SerializedFieldAttribute but is not declared partial",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "Any class with fields marked with DeserializedFieldAttribute or SerializedFieldAttribute must be declared partial for code generation to inject deserialization logic.");


        public static readonly DiagnosticDescriptor SerializedClassMustHaveBufferSize = new DiagnosticDescriptor(
            "GEN006",
            "Class with SerializedFieldAttribute fields must have a BuffersSizeAttribute",
            "Class '{0}' contains fields with SerializedFieldAttribute but does not have a BufferSizeAttribute",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "Any class with fields marked with SerializedFieldAttribute must have a BufferSizeAttribute to define the serialization buffer size.");

        public static readonly DiagnosticDescriptor CustomSerializationSignature = new DiagnosticDescriptor(
            "GEN007",
            "Custom serialization method must have ReadOnlySpan<byte> parameter and return void",
            "Class '{0}' contains a SeralizeInternal method but it does not match the required signature of 'void SerializeInternal(ReadOnlySpan<byte> buffer)', instead the first argument is of type {1}",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "Custom serialization methods named SerializeInternal must have a single parameter of type ReadOnlySpan<byte> and return void.");


        public static readonly DiagnosticDescriptor FieldCannotBeReadonly = new DiagnosticDescriptor(
            "GEN008",
            "Field with Serialization Attribute Cannot Be Readonly",
            "Field '{0}' is marked with DeserializedFieldAttribute or SerializedFieldAttribute and cannot be readonly",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "Fields marked with DeserializedFieldAttribute or SerializedFieldAttribute must not be readonly."
        );

        public static readonly DiagnosticDescriptor FieldCannotBePublic = new DiagnosticDescriptor(
            "GEN009",
            "Field with Serialization Attribute Cannot Be Public",
            "Field '{0}' is marked with DeserializedFieldAttribute or SerializedFieldAttribute and cannot be public",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "Fields marked with DeserializedFieldAttribute or SerializedFieldAttribute must not be public."
        );

    }
}
