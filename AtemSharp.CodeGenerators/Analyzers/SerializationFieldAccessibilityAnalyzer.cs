using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace AtemSharp.CodeGenerators.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SerializationFieldAccessibilityAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(
                DiagnosticDescriptors.FieldCannotBeReadonly,
                DiagnosticDescriptors.FieldCannotBePublic
            );

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.RegisterSymbolAction(AnalyzeField, SymbolKind.Field);
        }

        private static void AnalyzeField(SymbolAnalysisContext context)
        {
            var fieldSymbol = (IFieldSymbol)context.Symbol;

            // Ignore fields with NoPropertyAttribute
            bool hasNoProperty = fieldSymbol.GetAttributes().Any(attr => attr.AttributeClass?.Name == "NoPropertyAttribute");
            if (hasNoProperty)
                return;

            foreach (var attribute in fieldSymbol.GetAttributes())
            {
                var attrName = attribute.AttributeClass?.Name;
                if (attrName == "DeserializedFieldAttribute" || attrName == "SerializedFieldAttribute")
                {
                    if (fieldSymbol.IsReadOnly)
                    {
                        var diag = Diagnostic.Create(
                            DiagnosticDescriptors.FieldCannotBeReadonly,
                            fieldSymbol.Locations[0],
                            fieldSymbol.Name
                        );
                        context.ReportDiagnostic(diag);
                    }
                    if (fieldSymbol.DeclaredAccessibility == Accessibility.Public)
                    {
                        var diag = Diagnostic.Create(
                            DiagnosticDescriptors.FieldCannotBePublic,
                            fieldSymbol.Locations[0],
                            fieldSymbol.Name
                        );
                        context.ReportDiagnostic(diag);
                    }
                    break;
                }
            }
        }
    }
}
