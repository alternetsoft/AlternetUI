#nullable disable
using XamlX.Ast;
using XamlX.Transform;

namespace Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions.Transformers
{
    class UixmlPortXamlIlCompiledBindingsMetadataRemover : IXamlAstTransformer
    {
        public IXamlAstNode Transform(AstTransformationContext context, IXamlAstNode node)
        {
            while (true)
            {
                if (node is NestedScopeMetadataNode nestedScope)
                    node = nestedScope.Value;
                else if (node is UixmlPortXamlIlDataContextTypeMetadataNode dataContextType)
                    node = dataContextType.Value;
                else if (node is UixmlPortXamlIlCompileBindingsNode compileBindings)
                    node = compileBindings.Value;
                else
                    return node;
            }
        }
    }
}
