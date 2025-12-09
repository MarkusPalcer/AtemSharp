using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AtemSharp.CodeGenerators.Analyzers
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SerializationFieldAccessibilityCodeFixProvider)), Shared]
    public class SerializationFieldAccessibilityCodeFixProvider : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create("GEN008", "GEN009");

        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root == null)
            {
                return;
            }

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var fieldDecl = root.FindToken(diagnosticSpan.Start).Parent?.AncestorsAndSelf().OfType<FieldDeclarationSyntax>().FirstOrDefault();
            if (fieldDecl == null)
            {
                return;
            }

            if (diagnostic.Id == "GEN008")
            {
                context.RegisterCodeFix(
                    CodeAction.Create(
                        "Remove readonly modifier",
                        c => RemoveReadonlyAsync(context.Document, fieldDecl, c),
                        nameof(SerializationFieldAccessibilityCodeFixProvider) + ".RemoveReadonly"),
                    diagnostic);
            }
            else if (diagnostic.Id == "GEN009")
            {
                // Offer to make private, internal, or protected
                context.RegisterCodeFix(
                    CodeAction.Create(
                        "Make field private",
                        c => ChangeAccessibilityAsync(context.Document, fieldDecl, SyntaxKind.PrivateKeyword, c),
                        nameof(SerializationFieldAccessibilityCodeFixProvider) + ".MakePrivate"),
                    diagnostic);
                context.RegisterCodeFix(
                    CodeAction.Create(
                        "Make field internal",
                        c => ChangeAccessibilityAsync(context.Document, fieldDecl, SyntaxKind.InternalKeyword, c),
                        nameof(SerializationFieldAccessibilityCodeFixProvider) + ".MakeInternal"),
                    diagnostic);
                context.RegisterCodeFix(
                    CodeAction.Create(
                        "Make field protected",
                        c => ChangeAccessibilityAsync(context.Document, fieldDecl, SyntaxKind.ProtectedKeyword, c),
                        nameof(SerializationFieldAccessibilityCodeFixProvider) + ".MakeProtected"),
                    diagnostic);
            }
        }

        private async Task<Document> RemoveReadonlyAsync(Document document, FieldDeclarationSyntax fieldDecl, CancellationToken cancellationToken)
        {
            var newModifiers = fieldDecl.Modifiers.Where(m => !m.IsKind(SyntaxKind.ReadOnlyKeyword));
            var newField = fieldDecl.WithModifiers(SyntaxFactory.TokenList(newModifiers));
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (root == null)
            {
                return document;
            }

            var newRoot = root.ReplaceNode(fieldDecl, newField);
            return document.WithSyntaxRoot(newRoot);
        }

        private async Task<Document> ChangeAccessibilityAsync(Document document, FieldDeclarationSyntax fieldDecl, SyntaxKind newAccessibility, CancellationToken cancellationToken)
        {
            var modifiers = fieldDecl.Modifiers;
            // Remove existing accessibility modifiers
            var newModifiers = SyntaxFactory.TokenList(modifiers.Where(m =>
                !m.IsKind(SyntaxKind.PublicKeyword) &&
                !m.IsKind(SyntaxKind.PrivateKeyword) &&
                !m.IsKind(SyntaxKind.InternalKeyword) &&
                !m.IsKind(SyntaxKind.ProtectedKeyword)));
            // Add the new one at the start
            newModifiers = newModifiers.Insert(0, SyntaxFactory.Token(newAccessibility));
            var newField = fieldDecl.WithModifiers(newModifiers);
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (root == null)
            {
                return document;
            }

            var newRoot = root.ReplaceNode(fieldDecl, newField);
            return document.WithSyntaxRoot(newRoot);
        }
    }
}
