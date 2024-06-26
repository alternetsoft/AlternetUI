#pragma warning disable
#nullable disable
using System.Linq;
using XamlX;
using XamlX.Ast;
using XamlX.Transform;

namespace Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions.Transformers
{
    class IgnoredDirectivesTransformer : IXamlAstTransformer
    {
        public IXamlAstNode Transform(AstTransformationContext context, IXamlAstNode node)
        {
            if (node is XamlAstObjectNode no)
            {
                foreach (var d in no.Children.OfType<XamlAstXmlDirective>().ToList())
                {
                    if (d.Namespace == XamlNamespaces.Xaml2006)
                    {
                        if (d.Name == "Precompile" ||
                            d.Name == "Class" ||
                            d.Name == "FieldModifier" ||
                            d.Name == "ClassModifier")
                            no.Children.Remove(d);
                    }
                }
            }

            return node;
        }
    }
}
