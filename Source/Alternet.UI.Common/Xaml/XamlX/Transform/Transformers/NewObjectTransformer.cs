#pragma warning disable
using System.Linq;
using XamlX.Ast;

namespace XamlX.Transform.Transformers
{
#if !XAMLX_INTERNAL
    public
#endif
    class NewObjectTransformer : IXamlAstTransformer
    {
        public IXamlAstNode Transform(AstTransformationContext context, IXamlAstNode node)
        {
            if (node is XamlAstConstructableObjectNode obj)
            {
                return new XamlValueWithManipulationNode(obj,
                    new XamlAstNewClrObjectNode(obj, obj.Type.GetClrTypeReference(), obj.Constructor, obj.Arguments),
                    new XamlObjectInitializationNode(obj,
                        new XamlManipulationGroupNode(obj)
                        {
                            Children = obj.Children.Cast<IXamlAstManipulationNode>().ToList()
                        }, obj.Type.GetClrType()));
            }

            return node;
        }
    }
}
