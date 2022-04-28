#nullable disable
using Alternet.UI;
using Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions.Transformers;
using XamlX.Ast;
using XamlX.Emit;
using XamlX.IL;

namespace Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions.AstNodes
{
    class UixmlPortXamlIlGridLengthAstNode : XamlAstNode, IXamlAstValueNode, IXamlAstILEmitableNode
    {
        private readonly UixmlPortXamlIlWellKnownTypes _types;
        private readonly GridLength _gridLength;

        public UixmlPortXamlIlGridLengthAstNode(IXamlLineInfo lineInfo, UixmlPortXamlIlWellKnownTypes types, GridLength gridLength) : base(lineInfo)
        {
            _types = types;
            _gridLength = gridLength;

            Type = new XamlAstClrTypeReference(lineInfo, types.GridLength, false);
        }
        
        public IXamlAstTypeReference Type { get; }
        
        public XamlILNodeEmitResult Emit(XamlEmitContext<IXamlILEmitter, XamlILNodeEmitResult> context, IXamlILEmitter codeGen)
        {
            codeGen
                .Ldc_R8(_gridLength.Value)
                .Ldc_I4((int)_gridLength.GridUnitType)
                .Newobj(_types.GridLengthConstructorValueType);
            
            return XamlILNodeEmitResult.Type(0, Type.GetClrType());
        }
    }
}
