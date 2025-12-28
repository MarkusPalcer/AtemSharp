using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AtemSharp.CodeGenerators.State;

[Generator]
public class StateGenerator : CodeGeneratorBase
{
    private static readonly string[] HardCodedNamespaces =
        {
            "System.ComponentModel",
            "System.Runtime.CompilerServices"
        };

    protected override void ProcessClass(SourceProductionContext spc, INamedTypeSymbol classSymbol, ClassDeclarationSyntax classDecl)
    {
        var fields = classSymbol.GetMembers()
                                .OfType<IFieldSymbol>()
                                .Where(f => f.AssociatedSymbol is null)
                                .Where(f => !f.GetAttributes().Any(a => a.AttributeClass?.Name is "IgnoreDataMember" or "IgnoreDataMemberAttribute"))
                                .Where(f => !f.Name.Contains("<"))
                                .Select(ProcessField)
                                .ToArray();

        var usings = HardCodedNamespaces.Concat(fields.Where(x => x.Namespace != string.Empty)
                       .Select(x => x.Namespace))
         .Distinct()
         .Select(x => $"using {x};")
         .ToArray();

        spc.AddSource($"{classSymbol.Name}.g.cs", $$"""
                                                      #nullable enable

                                                      {{string.Join("\n", usings)}}

                                                      namespace {{classSymbol.ContainingNamespace.ToDisplayString()}};

                                                      partial class {{classSymbol.Name}} : INotifyPropertyChanged {

                                                        {{string.Join("\n", fields.Select(x => x.FieldCode))}}

                                                        public event PropertyChangedEventHandler? PropertyChanged;

                                                        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
                                                        {
                                                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                                                        }
                                                      }
                                                    """);
    }

    private ProcessedField ProcessField(IFieldSymbol field)
    {
        var propertyName = Helpers.GetPropertyName(field);
        var isReadOnly = field.IsReadOnly || field.GetAttributes().Any(a => a.AttributeClass?.Name is "ReadOnly" or "ReadOnlyAttribute");
        var type = Helpers.GetFieldType(field);

        var setterCode = isReadOnly
                             ? string.Empty
                             : $$"""
                                 set {
                                    if (object.Equals({{field.Name}}, value)) {
                                        return;
                                    }
                                    Send{{propertyName}}UpdateCommand(value);
                                 }
                                 """;

        var sendUpdateDeclaration = isReadOnly ? string.Empty : $"private partial void Send{propertyName}UpdateCommand({type} value);";

        var code = $$"""
                    public {{type}} {{propertyName}} {
                        get => {{field.Name}};
                        {{setterCode}}
                    }

                    internal void Update{{propertyName}}({{type}} value) {
                        {{field.Name}} = value;
                        {{propertyName}}Changed?.Invoke(this, EventArgs.Empty);
                        OnPropertyChanged();
                    }

                    public event EventHandler? {{propertyName}}Changed;

                    {{sendUpdateDeclaration}}
                 """;

        return new ProcessedField
        {
            FieldCode = code,
            Namespace = field.Type.ContainingNamespace.ToString() ?? string.Empty,
        };
    }

    protected override bool ClassFilter(INamedTypeSymbol classSymbol, ClassDeclarationSyntax classDecl)
    {
        return classDecl.Modifiers.Any(SyntaxKind.PartialKeyword)
            && (classSymbol.ContainingNamespace.ToString() ?? string.Empty).Contains(".State");
    }

    public class ProcessedField
    {
        public string FieldCode { get; set; } = string.Empty;
        public string Namespace { get; set; } = string.Empty;
    }
}
