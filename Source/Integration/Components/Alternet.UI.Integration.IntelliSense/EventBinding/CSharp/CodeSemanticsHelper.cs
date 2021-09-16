using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.IO;
using System.Linq;
using System.Text;

namespace Alternet.UI.Integration.IntelliSense.EventBinding.CSharp
{
    internal static class CodeSemanticsHelper
    {
        internal static ClassDeclarationSyntax FindUserFormClassDefinition(CSharpSyntaxTree st)
        {
            // todo: need to properly analyze base types (Form) here.

            // todo: properly analyze partial classes which may contain
            // base class specification in a different file, and incomplete Form
            // class qualifications.
            var root = (CompilationUnitSyntax)st.GetRoot();
            var c = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault(); /*
                x => x.BaseList != null && x.BaseList.Types.Any(y => y.Type.ToFullString().Trim() == FormFilesUtility.FormBaseTypeFullName)*/
            return c;
        }

        internal static int GetCodeHeaderCommentEndOffset(string code)
        {
            var tree = ParseUserCode(code);
            var walker = new HeaderCommentsWalker();
            walker.Visit(tree.GetRoot());

            return walker.HeaderCommentsEndOffset;
        }

        internal static string GetDesignedClassName(string userCodeFileName)
        {
            var st = ParseUserCode(
                File.ReadAllText(userCodeFileName));

            var formClass = FindUserFormClassDefinition(st);
            if (formClass == null)
                return string.Empty;

            var sb = new StringBuilder();

            var ns = formClass.Parent as NamespaceDeclarationSyntax;
            if (ns != null)
                sb.AppendFormat("{0}.", GetIdentifierName(ns.Name));

            sb.Append(formClass.Identifier.ValueText);

            return sb.ToString();
        }

        internal static CSharpSyntaxTree ParseUserCode(string code)
        {
            return (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(SourceText.From(code));
        }

        private static string GetIdentifierName(TypeSyntax node)
        {
            switch (node.Kind())
            {
                case SyntaxKind.IdentifierName:
                    return (node as IdentifierNameSyntax).Identifier.Text;

                case SyntaxKind.QualifiedName:
                    var qualifiedNode = node as QualifiedNameSyntax;
                    return GetIdentifierName(qualifiedNode.Left) + "." + GetIdentifierName(qualifiedNode.Right);

                case SyntaxKind.PredefinedType:
                    return (node as PredefinedTypeSyntax).Keyword.ValueText;

                default:
                    return node.GetText().ToString();
            }
        }

        private class HeaderCommentsWalker : CSharpSyntaxWalker
        {
            public int HeaderCommentsEndOffset { get; private set; }

            public override void VisitClassDeclaration(ClassDeclarationSyntax node)
            {
                if (!TryGetLeadingComments(node))
                    base.VisitClassDeclaration(node);
            }

            public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
            {
                if (!TryGetLeadingComments(node))
                    base.VisitNamespaceDeclaration(node);
            }

            private bool TryGetLeadingComments(CSharpSyntaxNode node)
            {
                if (node.HasLeadingTrivia)
                {
                    HeaderCommentsEndOffset = node.GetLeadingTrivia().FullSpan.End;
                    return true;
                }

                return false;
            }
        }
    }
}
