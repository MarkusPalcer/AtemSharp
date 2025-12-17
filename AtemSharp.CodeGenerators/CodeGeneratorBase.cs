using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AtemSharp.CodeGenerators;

public abstract class CodeGeneratorBase  : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classDeclarations = context.SyntaxProvider
                                       .CreateSyntaxProvider(
                                            predicate: (node, _) => node is ClassDeclarationSyntax,
                                            transform: (ctx, _) => (ClassDeclarationSyntax)ctx.Node)
                                       .Where(classDecl => classDecl != null);

        var compilationAndClasses = context.CompilationProvider.Combine(classDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndClasses, Generate);
    }


    private void Generate(SourceProductionContext spc, (Compilation Left, ImmutableArray<ClassDeclarationSyntax> Right) tuple)
    {
        var (compilation, classDecls) = tuple;
        foreach (var classDecl in classDecls)
        {
            var semanticModel = compilation.GetSemanticModel(classDecl.SyntaxTree);
            var classSymbol = semanticModel.GetDeclaredSymbol(classDecl);

            if (classSymbol == null || !ClassFilter(classSymbol, classDecl))
            {
                continue;
            }

            ProcessClass(spc, classSymbol, classDecl);
        }
    }

    protected abstract void ProcessClass(SourceProductionContext spc, INamedTypeSymbol classSymbol, ClassDeclarationSyntax classDecl);

    protected virtual bool ClassFilter(INamedTypeSymbol classSymbol, ClassDeclarationSyntax classDecl) => true;
}
