#nullable disable
using System.Linq;
using XamlX.Ast;
using XamlX.Transform;
using XamlX.TypeSystem;

namespace Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions.Transformers
{
    class UixmlPortXamlIlControlTemplateTargetTypeMetadataTransformer : IXamlAstTransformer
    {
        public IXamlAstNode Transform(AstTransformationContext context, IXamlAstNode node)
        {
            if (!(node is XamlAstObjectNode on
                  && on.Type.GetClrType().FullName == "Alternet.UI.Markup.Xaml.Templates.ControlTemplate"))
                return node;
            var tt = on.Children.OfType<XamlAstXamlPropertyValueNode>().FirstOrDefault(ch =>
                                              ch.Property.GetClrProperty().Name == "TargetType");

            if (context.ParentNodes().FirstOrDefault() is UixmlPortXamlIlTargetTypeMetadataNode)
                // Deja vu. I've just been in this place before
                return node;

            IXamlAstTypeReference targetType;

            var templatableBaseType = context.Configuration.TypeSystem.GetType("Alternet.UI.Controls.Control");
            
            if ((tt?.Values.FirstOrDefault() is XamlTypeExtensionNode tn))
            {
                targetType = tn.Value;
            }
            else
            {
                var parentScope = context.ParentNodes().OfType<UixmlPortXamlIlTargetTypeMetadataNode>()
                    .FirstOrDefault();
                if (parentScope?.ScopeType == UixmlPortXamlIlTargetTypeMetadataNode.ScopeTypes.Style)
                    targetType = parentScope.TargetType;
                else if (context.ParentNodes().Skip(1).FirstOrDefault() is XamlAstObjectNode directParentNode
                         && templatableBaseType.IsAssignableFrom(directParentNode.Type.GetClrType()))
                    targetType = directParentNode.Type;
                else
                    targetType = new XamlAstClrTypeReference(node,
                        templatableBaseType, false);
            }
                
                

            return new UixmlPortXamlIlTargetTypeMetadataNode(on, targetType,
                UixmlPortXamlIlTargetTypeMetadataNode.ScopeTypes.ControlTemplate);
        }
    }

    class UixmlPortXamlIlTargetTypeMetadataNode : XamlValueWithSideEffectNodeBase
    {
        public IXamlAstTypeReference TargetType { get; set; }
        public ScopeTypes ScopeType { get; }

        internal enum ScopeTypes
        {
            Style,
            ControlTemplate,
            Transitions
        }
        
        public UixmlPortXamlIlTargetTypeMetadataNode(IXamlAstValueNode value, IXamlAstTypeReference targetType,
            ScopeTypes type)
            : base(value, value)
        {
            TargetType = targetType;
            ScopeType = type;
        }
    }
}
