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
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MissingSerializeInternalCodeFixProvider)), Shared]
    public class MissingSerializeInternalCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(DiagnosticDescriptors.MissingSerializeInternalMethod.Id);

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root == null)
            {
                return;
            }

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var node = root.FindNode(diagnosticSpan);
            var classDecl = node as ClassDeclarationSyntax ?? node.AncestorsAndSelf().OfType<ClassDeclarationSyntax>().FirstOrDefault();
            if (classDecl == null)
            {
                return;
            }

            context.RegisterCodeFix(
                Microsoft.CodeAnalysis.CodeActions.CodeAction.Create(
                    "Add SerializeInternal(byte[] buffer) method",
                    c => AddSerializeInternalAsync(context.Document, classDecl, c, useSpan: false),
                    nameof(MissingSerializeInternalCodeFixProvider) + ".AddByteArray"),
                diagnostic);
        }

        private async Task<Document> AddSerializeInternalAsync(Document document, ClassDeclarationSyntax classDecl, CancellationToken cancellationToken, bool useSpan)
        {
            var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
            MethodDeclarationSyntax methodDecl;
            var todoStatement = SyntaxFactory.ParseStatement("// TODO: implement\n");
            if (!useSpan)
            {
                methodDecl = SyntaxFactory.MethodDeclaration(
                        SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                        "SerializeInternal")
                    .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                    .WithParameterList(SyntaxFactory.ParameterList(
                        SyntaxFactory.SingletonSeparatedList(
                            SyntaxFactory.Parameter(SyntaxFactory.Identifier("buffer"))
                                .WithType(SyntaxFactory.ArrayType(
                                    SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ByteKeyword)),
                                    SyntaxFactory.SingletonList(SyntaxFactory.ArrayRankSpecifier()))))))
                    .WithBody(SyntaxFactory.Block(todoStatement));
            }
            else
            {
                methodDecl = SyntaxFactory.MethodDeclaration(
                        SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                        "SerializeInternal")
                    .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                    .WithParameterList(
                        SyntaxFactory.ParameterList(
                            SyntaxFactory.SingletonSeparatedList(
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("buffer"))
                                    .WithType(
                                        SyntaxFactory.GenericName(SyntaxFactory.Identifier("Span"))
                                            .WithTypeArgumentList(
                                                SyntaxFactory.TypeArgumentList(
                                                    SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                        SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ByteKeyword))
                                                    )
                                                )
                                            )
                                    )
                            )
                        )
                    )
                    .WithBody(SyntaxFactory.Block(todoStatement));
            }
            editor.AddMember(classDecl, methodDecl);
            return editor.GetChangedDocument();
        }
    }
}
