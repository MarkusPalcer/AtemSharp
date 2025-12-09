using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace AtemSharp.CodeGenerators.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MissingSerializeInternalAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(DiagnosticDescriptors.MissingSerializeInternalMethod);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSymbolAction(AnalyzeClass, SymbolKind.NamedType);
        }

        private void AnalyzeClass(SymbolAnalysisContext context)
        {
            var classSymbol = (INamedTypeSymbol)context.Symbol;
            if (classSymbol.TypeKind != TypeKind.Class)
            {
                return;
            }

            // Check if any field has CustomSerializationAttribute
            var hasCustomSerializationField = classSymbol.GetMembers()
                .OfType<IFieldSymbol>()
                .Any(f => f.GetAttributes().Any(a => a.AttributeClass?.Name == "CustomSerializationAttribute" || a.AttributeClass?.Name == "CustomSerialization"));

            var hasGeneratedSerializationCode = classSymbol.GetMembers()
                                                           .OfType<IFieldSymbol>()
                                                           .Any(f => f.GetAttributes().Any(a => a.AttributeClass?.Name == "SerializedFieldAttribute" || a.AttributeClass?.Name == "SerializedField"));

            if (!hasCustomSerializationField || !hasGeneratedSerializationCode)
            {
                return;
            }

            // Check for SerializeInternal method
            var hasSerializeInternal = classSymbol.GetMembers()
                .OfType<IMethodSymbol>()
                .Any(m => m.Name == "SerializeInternal"
                    && m.ReturnsVoid
                    && m.Parameters.Length == 1
                    && (
                        (m.Parameters[0].Type is IArrayTypeSymbol arr && arr.ElementType.SpecialType == SpecialType.System_Byte)
                        || (m.Parameters[0].Type.Name == "Span" && m.Parameters[0].Type is INamedTypeSymbol nts && nts.TypeArguments.Length == 1 && nts.TypeArguments[0].SpecialType == SpecialType.System_Byte)
                    )
                );

            if (!hasSerializeInternal)
            {
                // Report diagnostic on the class
                var location = classSymbol.Locations.FirstOrDefault();
                if (location != null)
                {
                    context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.MissingSerializeInternalMethod, location, classSymbol.Name));
                }
            }
        }
    }
}
