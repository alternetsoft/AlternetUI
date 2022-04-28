#nullable disable
using System.Linq;
using XamlX.Ast;
using XamlX.Transform;

namespace Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions.Transformers
{
    class UixmlPortXamlIlUixmlPortPropertyResolver : IXamlAstTransformer
    {
        public IXamlAstNode Transform(AstTransformationContext context, IXamlAstNode node)
        {
            if (node is XamlAstClrProperty prop)
            {
                var n = prop.Name + "Property";
                var field =
                    prop.DeclaringType.Fields
                    .FirstOrDefault(f => f.Name == n);
                if (field != null)
                    return new XamlIlUixmlPortProperty(prop, field, context.GetUixmlPortTypes());
            }

            return node;
        }
    }
}
