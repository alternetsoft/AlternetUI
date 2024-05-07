#nullable disable
using System.Linq;
using XamlX.Ast;
using XamlX.Transform;
using XamlX.IL;
using XamlX.Emit;

namespace Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions.Transformers
{
    class UixmlPortXamlIlRootObjectScope : IXamlAstTransformer
    {
        public IXamlAstNode Transform(AstTransformationContext context, IXamlAstNode node)
        {
            if (!context.ParentNodes().Any()
                && node is XamlValueWithManipulationNode mnode)
            {
                mnode.Manipulation = new XamlManipulationGroupNode(mnode,
                    new[]
                    {
                        mnode.Manipulation,
                        new HandleRootObjectScopeNode(mnode, context.GetUixmlPortTypes())
                    });
            }
            return node;
        }
        class HandleRootObjectScopeNode : XamlAstNode, IXamlAstManipulationNode
        {
            private readonly UixmlPortXamlIlWellKnownTypes _types;

            public HandleRootObjectScopeNode(IXamlLineInfo lineInfo,
                UixmlPortXamlIlWellKnownTypes types) : base(lineInfo)
            {
                _types = types;
            }
        }
        internal class Emitter : IXamlILAstNodeEmitter
        {
            public XamlILNodeEmitResult Emit(IXamlAstNode node, XamlEmitContext<IXamlILEmitter, XamlILNodeEmitResult> context, IXamlILEmitter codeGen)
            {
                if (!(node is HandleRootObjectScopeNode))
                {
                    return null;
                }
                var types = context.GetUixmlPortTypes();
                
                var next = codeGen.DefineLabel();
                var scopeField = context.RuntimeContext.ContextType.Fields.First(f =>
                    f.Name == UixmlPortXamlIlLanguage.ContextNameScopeFieldName);
                using (var local = codeGen.LocalsPool.GetLocal(types.StyledElement))
                {
                    codeGen
                        .Isinst(types.StyledElement)
                        .Dup()
                        .Stloc(local.Local)
                        .Brfalse(next)
                        .Ldloc(local.Local)
                        .Ldloc(context.ContextLocal)
                        .Ldfld(scopeField)
                        .EmitCall(types.NameScopeSetNameScope, true)
                        .MarkLabel(next)
                        .Ldloc(context.ContextLocal)
                        .Ldfld(scopeField)
                        .EmitCall(types.INameScopeComplete, true);
                }

                return XamlILNodeEmitResult.Void(1);
            }
        }
    }
}
