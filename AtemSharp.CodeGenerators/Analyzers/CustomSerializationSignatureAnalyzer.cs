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
                return;

            foreach (var method in methods)
            {
                if (!method.ReturnsVoid || method.Parameters.Length != 1)
                {
                    ReportDiagnostic(method, "invalid signature");
                    continue;
                }
                var param = method.Parameters[0];
                var isByteArray = param.Type is IArrayTypeSymbol arr && arr.ElementType.SpecialType == SpecialType.System_Byte;
                if (!isByteArray)
                {
                    ReportDiagnostic(method, param.Type.ToDisplayString());
                }
            }

            void ReportDiagnostic(IMethodSymbol method, string paramType)
            {
                var location = method.Locations.FirstOrDefault();
                if (location != null)
                {
                    var descriptor = DiagnosticDescriptors.CustomSerializationSignature;
                    var message = string.Format(descriptor.MessageFormat.ToString(), classSymbol.Name, paramType);
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
