using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Alternet.UI.SourceGenerator
{
    [Generator]
    public class DelegateSourceGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var classDeclarations = context.SyntaxProvider.CreateSyntaxProvider(
                predicate: (s, _) => s is DelegateDeclarationSyntax,
                transform: (ctx, _) => TransformSyntax(ctx)
            ).Where(v => v != null);

            var merged = context.CompilationProvider.Combine(classDeclarations.Collect());

            context.RegisterSourceOutput(merged, (ctx, source) =>
                Execute(source.Left, source.Right, ctx));
        }

        private static DelegateDeclarationSyntax? TransformSyntax(GeneratorSyntaxContext context)
        {
            //In this method we may either convert the syntax node matched
            //by the predicate to the actual
            //node we want to operate against, or we can
            //just return our existing node as is

            /*var node = (DelegateDeclarationSyntax)context.Node;

            var ns = node.SyntaxTree.GetCompilationUnitRoot()
                .DescendantNodes().OfType<NamespaceDeclarationSyntax>().First();

            var name = ns.Name.ToString();

            var parentType = node.Ancestors().OfType<ClassDeclarationSyntax>().FirstOrDefault();

            var fileName = Path.GetFileName(node.SyntaxTree.FilePath);*/

            return null;
            
            /*return node;*/
        }

        private static void Execute(
            Compilation compilation,
            ImmutableArray<DelegateDeclarationSyntax?> delegates,
            SourceProductionContext context)
        {
            if (delegates.IsDefaultOrEmpty)
                return;
            /*
            var methods = new List<MethodDeclarationSyntax>();

            foreach (var @delegate in delegates)
            {
                var model = compilation.GetSemanticModel(@delegate.SyntaxTree);

                var symbol = model.GetDeclaredSymbol(@delegate);
                var method = CreateDelegateHandler(symbol);

                methods.Add(method);
            }

            StructDeclarationSyntax classSyntax = StructDeclaration("DelegateHolder")
                .AddModifiers(Token(SyntaxKind.InternalKeyword), Token(SyntaxKind.UnsafeKeyword), Token(SyntaxKind.PartialKeyword))
                .AddMembers(methods.ToArray());

            var compilationUnit = CompilationUnit()
                .AddMembers(
                    NamespaceDeclaration(
                        IdentifierName("ClrDebug")
                    ).AddMembers(classSyntax)
                )
                .AddUsings(
                    UsingDirective(IdentifierName("System")),
                    UsingDirective(IdentifierName("System.Runtime.InteropServices")),
                    UsingDirective(IdentifierName("System.Runtime.InteropServices.Marshalling")),
                    UsingDirective(IdentifierName("ClrDebug.Extensions")).WithStaticKeyword(Token(SyntaxKind.StaticKeyword))
                );

            context.AddSource("DelegateHolder.g.cs", compilationUnit.NormalizeWhitespace().ToFullString());
            */
        }
    }
}