using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AtemSharp.CodeGenerators.Analyzers
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AddPartialKeywordCodeFixProvider)), Shared]
    public class AddPartialKeywordCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create("GEN005");

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root == null) return;

            var diagnostic = context.Diagnostics[0];
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var classDecl = root.FindToken(diagnosticSpan.Start).Parent?.AncestorsAndSelf().OfType<ClassDeclarationSyntax>().FirstOrDefault();
            if (classDecl == null) return;

            context.RegisterCodeFix(
                Microsoft.CodeAnalysis.CodeActions.CodeAction.Create(
                    title: "Add partial keyword",
                    createChangedDocument: c => AddPartialAsync(context.Document, classDecl, c),
                    equivalenceKey: "AddPartialKeyword"),
                diagnostic);
        }

        private async Task<Document> AddPartialAsync(Document document, ClassDeclarationSyntax classDecl, CancellationToken cancellationToken)
        {
            var newModifiers = classDecl.Modifiers.Add(SyntaxFactory.Token(SyntaxKind.PartialKeyword));
            var newClassDecl = classDecl.WithModifiers(newModifiers);
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            var newRoot = root?.ReplaceNode(classDecl, newClassDecl);
            return newRoot != null ? document.WithSyntaxRoot(newRoot) : document;
        }
    }
}
