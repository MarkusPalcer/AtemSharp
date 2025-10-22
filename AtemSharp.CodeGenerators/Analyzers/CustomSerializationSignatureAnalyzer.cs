using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace AtemSharp.CodeGenerators.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CustomSerializationSignatureAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(DiagnosticDescriptors.CustomSerializationSignature);

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

            // Only check if class has a field with CustomSerializationAttribute
            var hasCustomSerializationField = classSymbol.GetMembers()
                .OfType<IFieldSymbol>()
                .Any(f => f.GetAttributes().Any(a => a.AttributeClass?.Name == "CustomSerializationAttribute" || a.AttributeClass?.Name == "CustomSerialization"));
            if (!hasCustomSerializationField)
                return;

            var methods = classSymbol.GetMembers().OfType<IMethodSymbol>().Where(m => m.Name == "SerializeInternal").ToList();
            if (!methods.Any())
            {
                // Report diagnostic if no SerializeInternal method exists
                var location = classSymbol.Locations.FirstOrDefault();
                if (location != null)
                {
                    var descriptor = DiagnosticDescriptors.CustomSerializationSignature;
                    // Update message: do not mention Span<byte>, only require SerializeInternal exists
                    var message = $"Class '{classSymbol.Name}' contains a field with CustomSerializationAttribute but does not have a SerializeInternal method.";
                    var customDescriptor = new DiagnosticDescriptor(
                        descriptor.Id,
                        descriptor.Title.ToString(),
                        message,
                        descriptor.Category,
                        descriptor.DefaultSeverity,
                        descriptor.IsEnabledByDefault,
                        descriptor.Description?.ToString());
                    var diagnostic = Diagnostic.Create(customDescriptor, location);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
