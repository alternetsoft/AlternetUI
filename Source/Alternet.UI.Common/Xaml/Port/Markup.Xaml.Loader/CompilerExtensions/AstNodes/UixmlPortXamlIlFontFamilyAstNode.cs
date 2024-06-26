#pragma warning disable
#nullable disable
using Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions.Transformers;
using XamlX.Ast;
using XamlX.Emit;
using XamlX.IL;
using XamlX.TypeSystem;

namespace Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions.AstNodes
{
    class UixmlPortXamlIlFontFamilyAstNode: XamlAstNode, IXamlAstValueNode, IXamlAstILEmitableNode
    {
        private readonly UixmlPortXamlIlWellKnownTypes _types;
        private readonly string _text;

        public IXamlAstTypeReference Type { get; }
        
        public UixmlPortXamlIlFontFamilyAstNode(UixmlPortXamlIlWellKnownTypes types,
            string text,
            IXamlLineInfo lineInfo) : base(lineInfo)
        {
            _types = types;
            _text = text;
            Type = new XamlAstClrTypeReference(lineInfo, types.FontFamily, false);
        }
        
        public XamlILNodeEmitResult Emit(XamlEmitContext<IXamlILEmitter, XamlILNodeEmitResult> context, IXamlILEmitter codeGen)
        {
            codeGen
                .Ldloc(context.ContextLocal)
                .Castclass(context.Configuration.TypeMappings.UriContextProvider)
                .EmitCall(context.Configuration.TypeMappings.UriContextProvider.FindMethod(
                    "get_BaseUri", _types.Uri, false))
                .Ldstr(_text)
                .Newobj(_types.FontFamilyConstructorUriName);
            return XamlILNodeEmitResult.Type(0, _types.FontFamily);
        }
    }
}
