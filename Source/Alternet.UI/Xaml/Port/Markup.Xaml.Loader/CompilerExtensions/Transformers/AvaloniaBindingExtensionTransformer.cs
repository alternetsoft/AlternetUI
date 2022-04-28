#nullable disable
using System.Linq;
using XamlX;
using XamlX.Ast;
using XamlX.Transform;


using XamlParseException = XamlX.XamlParseException;

namespace Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions.Transformers
{
    class UixmlPortBindingExtensionTransformer : IXamlAstTransformer
    {
        public bool CompileBindingsByDefault { get; set; }

        public IXamlAstNode Transform(AstTransformationContext context, IXamlAstNode node)
        {
            if (context.ParentNodes().FirstOrDefault() is UixmlPortXamlIlCompileBindingsNode)
            {
                return node;
            }

            if (node is XamlAstObjectNode obj)
            {
                foreach (var item in obj.Children)
                {
                    if (item is XamlAstXmlDirective directive)
                    {
                        if (directive.Namespace == XamlNamespaces.Xaml2006
                            && directive.Name == "CompileBindings"
                            && directive.Values.Count == 1)
                        {
                            if (!(directive.Values[0] is XamlAstTextNode text
                                && bool.TryParse(text.Text, out var compileBindings)))
                            {
                                throw new XamlParseException("The value of x:CompileBindings must be a literal boolean value.", directive.Values[0]);
                            }

                            obj.Children.Remove(directive);

                            return new UixmlPortXamlIlCompileBindingsNode(obj, compileBindings);
                        }
                    }
                }
            }

            // Convert the <Binding> tag to either a CompiledBinding or ReflectionBinding tag.

            if (node is XamlAstXmlTypeReference tref
                && tref.Name == "Binding"
                && tref.XmlNamespace == "https://github.com/uixmlPortui")
            {
                tref.IsMarkupExtension = true;

                var compileBindings = context.ParentNodes()
                    .OfType<UixmlPortXamlIlCompileBindingsNode>()
                    .FirstOrDefault()
                    ?.CompileBindings ?? CompileBindingsByDefault;

                tref.Name = compileBindings ? "CompiledBinding" : "ReflectionBinding";
            }
            return node;
        }
    }

    internal class UixmlPortXamlIlCompileBindingsNode : XamlValueWithSideEffectNodeBase
    {
        public UixmlPortXamlIlCompileBindingsNode(IXamlAstValueNode value, bool compileBindings)
            : base(value, value)
        {
            CompileBindings = compileBindings;
        }

        public bool CompileBindings { get; }
    }
}
