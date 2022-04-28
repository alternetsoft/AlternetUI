#nullable disable
using XamlX.Ast;
using XamlX.Transform;

namespace Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions.Transformers
{
    class UixmlPortXamlIlTransitionsTypeMetadataTransformer : IXamlAstTransformer
    {
        public IXamlAstNode Transform(AstTransformationContext context, IXamlAstNode node)
        {
            if (node is XamlAstObjectNode on)
            {
                foreach (var ch in on.Children)
                {
                    if (ch is XamlAstXamlPropertyValueNode pn
                        && pn.Property.GetClrProperty().Getter?.ReturnType.Equals(context.GetUixmlPortTypes().Transitions) == true)
                    {
                        for (var c = 0; c < pn.Values.Count; c++)
                        {
                            pn.Values[c] = new UixmlPortXamlIlTargetTypeMetadataNode(pn.Values[c], on.Type,
                                UixmlPortXamlIlTargetTypeMetadataNode.ScopeTypes.Transitions);
                        }
                    }
                }
            }
            return node;
        }
    }
}
