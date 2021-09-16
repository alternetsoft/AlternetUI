using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alternet.UI.Integration.IntelliSense.EventBinding.CSharp
{
    internal sealed class CSharpEventBinder : EventBinder
    {
        private CodeDomProvider typeProvider;

        internal CodeDomProvider TypeProvider
        {
            get
            {
                if (typeProvider == null)
                    typeProvider = CreateCodeProvider();
                return typeProvider;
            }
        }

        private string CapitalizeFirstLetter(string s)
        {
            if (s.Length == 0)
                return s;
            return char.ToUpperInvariant(s[0]) + s.Substring(1);
        }

        public override string CreateUniqueHandlerName(string codeText, MetadataEvent @event, MetadataType componentType, string componentName)
        {
            var st = CodeSemanticsHelper.ParseUserCode(codeText);
            var formClass = CodeSemanticsHelper.FindUserFormClassDefinition(st);

            if (formClass == null)
                throw new InvalidOperationException();

            var objectName = CapitalizeFirstLetter(componentName ?? componentType?.Name ?? "");

            string GenerateName(int? i)
            {
                var name = objectName + "_" + @event.Name;
                if (i == null)
                    return name;
                return name + "_" + i.Value;
            }

            var result = GenerateName(null);
            int index = 1;
            while (DoesFormMethodExist(formClass, result))
                result = GenerateName(index++);

            return result;
        }

        public override TextInsertion TryAddEventHandler(string codeText, MetadataEvent @event, string handlerName)
        {
            var st = CodeSemanticsHelper.ParseUserCode(codeText);
            var formClass = CodeSemanticsHelper.FindUserFormClassDefinition(st);

            if (formClass == null)
                return null;

            if (DoesFormMethodExist(formClass, handlerName))
                return null;

            var signature = GetDelegateSignature(@event.Type, handlerName);

            const int Indent = 4;
            var methodIndentString = new string(
                ' ',
                formClass.Parent is NamespaceDeclarationSyntax ? Indent * 2 : Indent);
            var indentString = new string(' ', Indent);
            var openingBrace = "{";
            var closingBrace = "}";

            var handlerCode = string.Format(
                    "\r\n{1}private {0}\r\n{1}{2}\r\n{1}{4}\r\n{1}{3}\r\n",
                    signature,
                    methodIndentString,
                    openingBrace,
                    closingBrace,
                    indentString);

            var closingBracketSpan = formClass.SyntaxTree.GetMappedLineSpan(formClass.CloseBraceToken.Span);
            return new TextInsertion(closingBracketSpan.StartLinePosition.Line, 0, handlerCode);
        }

        public override IEnumerable<string> GetSuitableHandlerMethodNames(string codeText, MetadataEvent @event)
        {
            var st = CodeSemanticsHelper.ParseUserCode(codeText);

            var formClass = CodeSemanticsHelper.FindUserFormClassDefinition(st);

            var eventSignature = GetDelegateMethodInfo(@event.Type);
            var requiredReturnType = eventSignature.ReturnType;
            var requiredParameterTypes = eventSignature.Parameters.Select(x => x.ParameterType).ToArray();

            var result = new List<string>();

            if (formClass != null)
            {
                foreach (var userMethod in formClass.Members.OfType<MethodDeclarationSyntax>())
                {
                    if (userMethod.TypeParameterList != null)
                        continue;

                    if (!CompareTypes(requiredReturnType, userMethod.ReturnType))
                        continue;

                    var userMethodParameterTypes = userMethod.ParameterList.Parameters.Select(x => x.Type);

                    if (!requiredParameterTypes.SequenceEqual(userMethodParameterTypes, CompareTypes))
                        continue;

                    result.Add(userMethod.Identifier.ValueText);
                }
            }

            return result.ToArray();
        }

        public override bool CanAddEventHandlers(string codeText)
        {
            var st = CodeSemanticsHelper.ParseUserCode(codeText);
            var formClass = CodeSemanticsHelper.FindUserFormClassDefinition(st);
            return formClass != null;
        }

        private bool DoesFormMethodExist(ClassDeclarationSyntax formClass, string methodName)
        {
            return formClass.Members.OfType<MethodDeclarationSyntax>().Any(
                x => x.Identifier.ValueText == methodName);
        }

        private string GetDelegateSignature(MetadataType delegateType, string methodName)
        {
            var method = GetDelegateMethodInfo(delegateType);

            var sb = new StringBuilder();

            sb.AppendFormat("{0} {1}(", GetTypeName(method.ReturnType), methodName);

            var parameters = method.Parameters;
            var parametersCount = parameters.Length;

            for (int i = 0; i < parametersCount; i++)
            {
                var parameter = parameters[i];
                sb.AppendFormat("{0} {1}", GetTypeName(parameter.ParameterType), parameter.Name);

                if (i < parametersCount - 1)
                    sb.Append(", ");
            }

            sb.Append(")");

            return sb.ToString();
        }

        private MetadataMethod GetDelegateMethodInfo(MetadataType delegateType)
        {
            return delegateType.Methods.First(x => x.Name == "Invoke");
        }

        private bool CompareTypes(MetadataType actual, TypeSyntax parsed)
        {
            return GetTypeShortName(GetTypeName(actual)) == GetTypeShortName(parsed.ToFullString().Trim());
        }

        private CodeDomProvider CreateCodeProvider()
        {
            return new Microsoft.CSharp.CSharpCodeProvider();
        }

        private string GetTypeName(MetadataType type)
        {
            var typeRef = new CodeTypeReference(type.FullName);
            return TypeProvider.GetTypeOutput(typeRef);
        }

        private string GetTypeShortName(string fullName)
        {
            // todo: use actual roslyn type binding to compare full names.
            var i = fullName.LastIndexOf('.');
            if (i == -1)
                return fullName;
            return fullName.Substring(i + 1);
        }
    }
}