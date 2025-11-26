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
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CustomSerializationNoPropertyCodeFixProvider)), Shared]
    public class CustomSerializationNoPropertyCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(DiagnosticDescriptors.CustomSerializationNoPropertyConflict.Id);

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root == null)
                return;
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var node = root.FindNode(diagnosticSpan);

            // Try to find the FieldDeclarationSyntax node
            var fieldDecl = node switch
            {
                FieldDeclarationSyntax fds => fds,
                VariableDeclaratorSyntax { Parent.Parent: FieldDeclarationSyntax parentFds } => parentFds,
                _ => node.AncestorsAndSelf().OfType<FieldDeclarationSyntax>().FirstOrDefault()
            };

            if (fieldDecl != null)
            {
                // Remove CustomSerializationAttribute
                context.RegisterCodeFix(
                    Microsoft.CodeAnalysis.CodeActions.CodeAction.Create(
                        "Remove CustomSerializationAttribute",
                        c => RemoveAttributeAsync(context.Document, fieldDecl, "CustomSerializationAttribute", c),
                        nameof(CustomSerializationNoPropertyCodeFixProvider) + ".RemoveCustomSerializationAttribute"),
                    diagnostic);
                // Remove NoPropertyAttribute
                context.RegisterCodeFix(
                    Microsoft.CodeAnalysis.CodeActions.CodeAction.Create(
                        "Remove NoPropertyAttribute",
                        c => RemoveAttributeAsync(context.Document, fieldDecl, "NoPropertyAttribute", c),
                        nameof(CustomSerializationNoPropertyCodeFixProvider) + ".RemoveNoPropertyAttribute"),
                    diagnostic);
            }
        }

        private async Task<Document> RemoveAttributeAsync(Document document, FieldDeclarationSyntax fieldDecl, string attributeName, CancellationToken cancellationToken)
        {
            var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
            var newAttributeLists = new SyntaxList<AttributeListSyntax>();
            var attrNameNoSuffix = attributeName.EndsWith("Attribute") ? attributeName.Substring(0, attributeName.Length - 9) : attributeName;
            foreach (var attrList in fieldDecl.AttributeLists)
            {
                var newAttrs = attrList.Attributes.Where(a => {
                    var name = a.Name.ToString();
                    return !(name == attributeName || name == attrNameNoSuffix || name.EndsWith("." + attributeName) || name.EndsWith("." + attrNameNoSuffix));
                }).ToList();
                if (newAttrs.Count > 0)
                {
                    newAttributeLists = newAttributeLists.Add(SyntaxFactory.AttributeList(SyntaxFactory.SeparatedList(newAttrs)));
                }
                // If newAttrs.Count == 0, skip adding this attribute list (removes empty lists)
            }
            // Preserve leading trivia (including MSDoc-comments)
            var newFieldDecl = fieldDecl.WithAttributeLists(newAttributeLists)
                                         .WithLeadingTrivia(fieldDecl.GetLeadingTrivia());
            editor.ReplaceNode(fieldDecl, newFieldDecl);
            return editor.GetChangedDocument();
        }
    }
}
