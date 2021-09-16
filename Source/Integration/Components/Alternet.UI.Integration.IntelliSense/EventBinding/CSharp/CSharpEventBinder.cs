using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace Alternet.UI.Integration.IntelliSense.EventBinding.CSharp
{
    internal sealed class CSharpEventBinder : EventBinder
    {
        public override CreatedEventHandler CreateEventHandler(string codeText, MetadataEvent @event, string componentName)
        {
            throw new System.NotImplementedException();
        }

        public override IEnumerable<string> GetSuitableHandlerMethodNames(string codeText, MetadataEvent @event)
        {
            var st = CodeSemanticsHelper.ParseUserCode(codeText);

            var formClass = CodeSemanticsHelper.FindUserFormClassDefinition(st);

            var eventSignature = GetDelegateMethodInfo(@event.Type);
            var requiredReturnType = eventSignature.ReturnType;
            var requiredParameterTypes = eventSignature.ParameterTypes;

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

        private MetadataMethod GetDelegateMethodInfo(MetadataType delegateType)
        {
            return delegateType.Methods.First(x => x.Name == "Invoke");
        }

        private bool CompareTypes(MetadataType actual, TypeSyntax parsed)
        {
            return GetTypeShortName(GetTypeName(actual)) == GetTypeShortName(parsed.ToFullString().Trim());
        }

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

        CodeDomProvider CreateCodeProvider()
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