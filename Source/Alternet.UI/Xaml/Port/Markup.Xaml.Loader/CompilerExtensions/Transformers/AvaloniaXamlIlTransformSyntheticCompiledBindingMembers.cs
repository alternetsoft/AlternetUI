#nullable disable
using XamlX.Ast;
using XamlX.Transform;

namespace Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions.Transformers
{
    class UixmlPortXamlIlTransformSyntheticCompiledBindingMembers : IXamlAstTransformer
    {
        public IXamlAstNode Transform(AstTransformationContext context, IXamlAstNode node)
        {
            if (node is XamlAstNamePropertyReference prop
               && prop.TargetType is XamlAstClrTypeReference targetRef
               && targetRef.GetClrType().Equals(context.GetUixmlPortTypes().CompiledBindingExtension))
            {
                if (prop.Name == "ElementName")
                {
                    return new UixmlPortSyntheticCompiledBindingProperty(node,
                        SyntheticCompiledBindingPropertyName.ElementName);
                }
                else if (prop.Name == "RelativeSource")
                {
                    return new UixmlPortSyntheticCompiledBindingProperty(node,
                        SyntheticCompiledBindingPropertyName.RelativeSource);
                }
            }

            return node;
        }
    }

    enum SyntheticCompiledBindingPropertyName
    {
        ElementName,
        RelativeSource
    }

    class UixmlPortSyntheticCompiledBindingProperty : XamlAstNode, IXamlAstPropertyReference
    {
        public SyntheticCompiledBindingPropertyName Name { get; }

        public UixmlPortSyntheticCompiledBindingProperty(
            IXamlLineInfo lineInfo,
            SyntheticCompiledBindingPropertyName name)
            : base(lineInfo)
        {
            Name = name;
        }
    }
}
