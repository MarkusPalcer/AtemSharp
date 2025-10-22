using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace AtemSharp.CodeGenerators.Analyzers
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SerializeInternalShouldNotBePublicCodeFixProvider)), Shared]
    public class SerializeInternalShouldNotBePublicCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(DiagnosticDescriptors.SerializeInternalShouldNotBePublic.Id);

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root == null)
                return;
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var node = root.FindNode(diagnosticSpan);
            var methodDecl = node as MethodDeclarationSyntax ?? node.AncestorsAndSelf().OfType<MethodDeclarationSyntax>().FirstOrDefault();
            if (methodDecl == null)
                return;

            // Offer three fixes: internal, protected, private
            context.RegisterCodeFix(
                Microsoft.CodeAnalysis.CodeActions.CodeAction.Create(
                    "Change to internal",
                    c => ChangeAccessibilityAsync(context.Document, methodDecl, SyntaxKind.InternalKeyword, c),
                    nameof(SerializeInternalShouldNotBePublicCodeFixProvider) + ".Internal"),
                diagnostic);
            context.RegisterCodeFix(
                Microsoft.CodeAnalysis.CodeActions.CodeAction.Create(
                    "Change to protected",
                    c => ChangeAccessibilityAsync(context.Document, methodDecl, SyntaxKind.ProtectedKeyword, c),
                    nameof(SerializeInternalShouldNotBePublicCodeFixProvider) + ".Protected"),
                diagnostic);
            context.RegisterCodeFix(
                Microsoft.CodeAnalysis.CodeActions.CodeAction.Create(
                    "Change to private",
                    c => ChangeAccessibilityAsync(context.Document, methodDecl, SyntaxKind.PrivateKeyword, c),
                    nameof(SerializeInternalShouldNotBePublicCodeFixProvider) + ".Private"),
                diagnostic);
        }

        private async Task<Document> ChangeAccessibilityAsync(Document document, MethodDeclarationSyntax methodDecl, SyntaxKind newAccessibility, CancellationToken cancellationToken)
        {
            var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
            var newModifiers = methodDecl.Modifiers;
            // Remove public/internal/protected/private
            newModifiers = new SyntaxTokenList(newModifiers.Where(m => !m.IsKind(SyntaxKind.PublicKeyword) && !m.IsKind(SyntaxKind.InternalKeyword) && !m.IsKind(SyntaxKind.ProtectedKeyword) && !m.IsKind(SyntaxKind.PrivateKeyword)));
            newModifiers = newModifiers.Insert(0, SyntaxFactory.Token(newAccessibility));
            var newMethodDecl = methodDecl.WithModifiers(newModifiers);
            editor.ReplaceNode(methodDecl, newMethodDecl);
            return editor.GetChangedDocument();
        }
    }
}
