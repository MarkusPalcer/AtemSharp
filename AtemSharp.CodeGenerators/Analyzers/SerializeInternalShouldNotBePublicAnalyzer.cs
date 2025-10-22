using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace AtemSharp.CodeGenerators.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SerializeInternalShouldNotBePublicAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(DiagnosticDescriptors.SerializeInternalShouldNotBePublic);

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
                return;

            var methods = classSymbol.GetMembers().OfType<IMethodSymbol>().Where(m => m.Name == "SerializeInternal").ToList();
            foreach (var method in methods)
            {
                if (method.DeclaredAccessibility == Accessibility.Public)
                {
                    var location = method.Locations.FirstOrDefault();
                    if (location != null)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(
                            DiagnosticDescriptors.SerializeInternalShouldNotBePublic,
                            location));
                    }
                }
            }
        }
    }
}
