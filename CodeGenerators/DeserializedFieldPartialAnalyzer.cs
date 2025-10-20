using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGenerators
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DeserializedFieldPartialAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(DiagnosticDescriptors.DeserializedFieldPartialRule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
        }

        private void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
        {
            var classDecl = (ClassDeclarationSyntax)context.Node;
            if (classDecl.Modifiers.Any(SyntaxKind.PartialKeyword))
                return;

            var semanticModel = context.SemanticModel;
            foreach (var member in classDecl.Members)
            {
                if (member is FieldDeclarationSyntax fieldDecl)
                {
                    foreach (var variable in fieldDecl.Declaration.Variables)
                    {
                        var symbol = semanticModel.GetDeclaredSymbol(variable);
                        if (symbol == null)
                            continue;
                        foreach (var attr in symbol.GetAttributes())
                        {
                            if (attr.AttributeClass?.Name == "DeserializedFieldAttribute")
                            {
                                var diagnostic = Diagnostic.Create(DiagnosticDescriptors.DeserializedFieldPartialRule, classDecl.Identifier.GetLocation(), classDecl.Identifier.Text);
                                context.ReportDiagnostic(diagnostic);
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}
