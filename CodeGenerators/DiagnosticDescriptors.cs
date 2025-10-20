using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
namespace CodeGenerators
{
    [SuppressMessage("MicrosoftCodeAnalysisReleaseTracking", "RS2000:Add analyzer diagnostic IDs to analyzer release")]
    public static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor TemplateLoadError = new DiagnosticDescriptor(
            id: "GEN001",
            title: "Template Load Error",
            messageFormat: "Failed to load template: {0}",
            category: "SourceGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        public static DiagnosticDescriptor DeserializedTemplateLoadError = new DiagnosticDescriptor(
            id: "GEN002",
            title: "Template Not Found",
            messageFormat: "Could not find DeserializeMethodTemplate.sbn as an embedded resource",
            category: "SourceGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        public static DiagnosticDescriptor CreateFieldTypeError(IFieldSymbol f)
        {
            return new DiagnosticDescriptor(
                id: "GEN003",
                title: "Missing Span Extension Method",
                messageFormat: $"No deserialization-method mapping found for field '{f.Name}' of type '{f.Type.ToDisplayString()}'.",
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

        public static readonly DiagnosticDescriptor DeserializedFieldPartialRule = new DiagnosticDescriptor(
            "GEN005",
            "Class with DeserializedFieldAttribute must be partial",
            "Class '{0}' contains fields with DeserializedFieldAttribute but is not declared partial",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "Any class with fields marked with DeserializedFieldAttribute must be declared partial for code generation to inject deserialization logic.");
    }
}
