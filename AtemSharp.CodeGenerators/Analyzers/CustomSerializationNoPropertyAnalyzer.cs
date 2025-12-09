using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace AtemSharp.CodeGenerators.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CustomSerializationNoPropertyAnalyzer : DiagnosticAnalyzer
    {

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(DiagnosticDescriptors.CustomSerializationNoPropertyConflict);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeField, Microsoft.CodeAnalysis.CSharp.SyntaxKind.FieldDeclaration);
        }

        private void AnalyzeField(SyntaxNodeAnalysisContext context)
        {
            var fieldDecl = (Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax)context.Node;
            var semanticModel = context.SemanticModel;
            var fieldSymbol = semanticModel.GetDeclaredSymbol(fieldDecl.Declaration.Variables.First(), context.CancellationToken) as IFieldSymbol;
            if (fieldSymbol == null)
            {
                return;
            }

            // Find attribute syntax nodes
            var customSerializationAttrs = fieldDecl.AttributeLists
                .SelectMany(al => al.Attributes)
                .Where(attr =>
                    semanticModel.GetTypeInfo(attr, context.CancellationToken).Type?.Name == "CustomSerializationAttribute" ||
                    semanticModel.GetTypeInfo(attr, context.CancellationToken).Type?.Name == "CustomSerialization")
                .ToList();
            var noPropertyAttrs = fieldDecl.AttributeLists
                .SelectMany(al => al.Attributes)
                .Where(attr =>
                    semanticModel.GetTypeInfo(attr, context.CancellationToken).Type?.Name == "NoPropertyAttribute" ||
                    semanticModel.GetTypeInfo(attr, context.CancellationToken).Type?.Name == "NoProperty")
                .ToList();

            if (customSerializationAttrs.Any() && noPropertyAttrs.Any())
            {
                // Report diagnostic on each attribute
                foreach (var attr in customSerializationAttrs.Concat(noPropertyAttrs))
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        DiagnosticDescriptors.CustomSerializationNoPropertyConflict,
                        attr.GetLocation(),
                        fieldSymbol.Name));
                }
            }
        }
    }
}
