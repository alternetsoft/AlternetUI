#nullable disable
using XamlX.Ast;
using XamlX.Transform;

namespace Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions.Transformers
{
    class UixmlPortXamlIlReorderClassesPropertiesTransformer : IXamlAstTransformer
    {
        public IXamlAstNode Transform(AstTransformationContext context, IXamlAstNode node)
        {
            if (node is XamlAstObjectNode obj)
            {
                IXamlAstNode classesNode = null;
                IXamlAstNode firstSingleClassNode = null;
                var types = context.GetUixmlPortTypes();
                foreach (var child in obj.Children)
                {
                    if (child is XamlAstXamlPropertyValueNode propValue
                        && propValue.Property is XamlAstClrProperty prop)
                    {
                        if (prop.DeclaringType.Equals(types.Classes))
                        {
                            if (firstSingleClassNode == null)
                                firstSingleClassNode = child;
                        }
                        else if (prop.Name == "Classes" && prop.DeclaringType.Equals(types.StyledElement))
                            classesNode = child;
                    }
                }

                if (classesNode != null && firstSingleClassNode != null)
                {
                    obj.Children.Remove(classesNode);
                    obj.Children.Insert(obj.Children.IndexOf(firstSingleClassNode), classesNode);
                }
            }

            return node;
        }
    }
}
