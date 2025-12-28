using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
namespace AtemSharp.CodeGenerators
{
    [SuppressMessage("MicrosoftCodeAnalysisReleaseTracking", "RS2000:Add analyzer diagnostic IDs to analyzer release")]
    public static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor SerializeInternalShouldNotBePublic = new(
            "GEN012",
            "SerializeInternal method should not be public",
            "Method 'SerializeInternal' should not be public. Change its accessibility to internal, protected, or private.",
            "Usage",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "SerializeInternal methods should not be public. Use internal, protected, or private instead."
        );
        public static readonly DiagnosticDescriptor MissingSerializeInternalMethod = new(
            "GEN011",
            "Missing SerializeInternal method",
            "Class '{0}' contains a field with CustomSerializationAttribute but does not have a SerializeInternal method",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "Any class with a field marked with CustomSerializationAttribute must have a SerializeInternal method."
        );
        public static DiagnosticDescriptor TemplateLoadError(string template) => new(
            id: "GEN001",
            title: "Template Load Error",
            messageFormat: $"Failed to load template {template}: {{0}}",
            category: "SourceGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        public static DiagnosticDescriptor TemplateNotFound(string template) => new(
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
                messageFormat: $"No extension method mapping found for field '{f.Name}' of type '{Helpers.GetSerializedFieldType(f).ToDisplayString()}'.",
                category: "SourceGenerator",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true
            );
        }

        public static DiagnosticDescriptor CreateRenderError(Exception ex) => new(
            id: "GEN004",
            title: "Template Render Error",
            messageFormat: $"Exception while rendering template: {ex.Message}",
            category: "SourceGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        public static readonly DiagnosticDescriptor CodeGenClassMustBePartial = new(
            "GEN005",
            "Class with DeserializedFieldAttribute or SerializedFieldAttribute must be partial",
            "Class '{0}' contains fields with DeserializedFieldAttribute or SerializedFieldAttribute but is not declared partial",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "Any class with fields marked with DeserializedFieldAttribute or SerializedFieldAttribute must be declared partial for code generation to inject deserialization logic.");


        public static readonly DiagnosticDescriptor SerializedClassMustHaveBufferSize = new(
            "GEN006",
            "Class with SerializedFieldAttribute fields must have a BuffersSizeAttribute",
            "Class '{0}' contains fields with SerializedFieldAttribute but does not have a BufferSizeAttribute",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "Any class with fields marked with SerializedFieldAttribute must have a BufferSizeAttribute to define the serialization buffer size.");

        public static readonly DiagnosticDescriptor CustomSerializationSignature = new(
            "GEN007",
            "Custom serialization method must have correct signature",
            "SerializeInternal does not match the required signature: 'void SerializeInternal(byte[])' or 'void SerializeInternal(byte[], ProtocolVersion)'",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "Custom serialization methods named SerializeInternal must have a first parameter of type byte[] and optionally a second parameter of type ProtocolVersion and return void.");

        public static readonly DiagnosticDescriptor FieldCannotBePublic = new(
            "GEN009",
            "Field with Serialization Attribute Cannot Be Public",
            "Field '{0}' is marked with DeserializedFieldAttribute or SerializedFieldAttribute and cannot be public",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "Fields marked with DeserializedFieldAttribute or SerializedFieldAttribute must not be public."
        );

        public static readonly DiagnosticDescriptor CustomSerializationNoPropertyConflict = new(
            "GEN010",
            "CustomSerializationAttribute and NoPropertyAttribute conflict",
            "Field '{0}' cannot have both CustomSerializationAttribute and NoPropertyAttribute",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "A field cannot have both CustomSerializationAttribute and NoPropertyAttribute. Remove one of them."
        );
    }
}
