using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Alternet.UI.SourceGenerators
{
    internal class SyntaxReceiver : ISyntaxReceiver
    {
        public List<SyntaxNode> ArgumentsToValidate { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
#pragma warning disable SA1513 // Closing brace should be followed by blank line
            if (syntaxNode is InvocationExpressionSyntax
                {
                    ArgumentList:
                    {
                        Arguments:
                        {
                            Count: <= 2                           // 1 parameter for the enum value, 1 optional for the parameter name
                        } arguments
                    },
                    Expression: MemberAccessExpressionSyntax
                    {
                        Name:
                        {
                            Identifier:
                            {
                                ValueText: "Validate"
                            }
                        },
                        Expression: MemberAccessExpressionSyntax  // For: SourceGenerated.EnumValidator.Validate(..)
                        {
                            Name:
                            {
                                Identifier:
                                {
                                    ValueText: "EnumValidator"
                                }
                            }
                        } or IdentifierNameSyntax                 // For: EnumValidator.Validate(..) with a using statement
                        {
                            Identifier:
                            {
                                ValueText: "EnumValidator"
                            }
                        }
                    }
                })
            {
                ArgumentsToValidate.Add(arguments.First().Expression);
            }
#pragma warning restore SA1513 // Closing brace should be followed by blank line
        }
    }
}
