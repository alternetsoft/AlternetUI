#pragma warning disable
#nullable disable
using XamlX.Ast;
using XamlX.Emit;

namespace XamlX.IL.Emitters
{
#if !XAMLX_INTERNAL
    public
#endif
    class PropertyValueManipulationEmitter : IXamlAstNodeEmitter<IXamlILEmitter, XamlILNodeEmitResult>
    {
        public XamlILNodeEmitResult Emit(IXamlAstNode node, XamlEmitContext<IXamlILEmitter, XamlILNodeEmitResult> context, IXamlILEmitter codeGen)
        {
            if (!(node is XamlPropertyValueManipulationNode pvm))
                return null;
            codeGen.EmitCall(pvm.Property.Getter);
            context.Emit(pvm.Manipulation, codeGen, null);
            
            return XamlILNodeEmitResult.Void(1);
        }
    }
}
