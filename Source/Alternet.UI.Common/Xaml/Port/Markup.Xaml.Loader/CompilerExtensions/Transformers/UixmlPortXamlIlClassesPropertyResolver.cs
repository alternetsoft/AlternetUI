#pragma warning disable
#nullable disable
using System.Collections.Generic;
using XamlX.Ast;
using XamlX.Emit;
using XamlX.IL;
using XamlX.Transform;
using XamlX.TypeSystem;

namespace Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions.Transformers
{
    class UixmlPortXamlIlResolveClassesPropertiesTransformer : IXamlAstTransformer
    {
        public IXamlAstNode Transform(AstTransformationContext context, IXamlAstNode node)
        {
            /*if (node is XamlAstNamePropertyReference prop
                && prop.TargetType is XamlAstClrTypeReference targetRef
                && prop.DeclaringType is XamlAstClrTypeReference declaringRef)
            {
                var types = context.GetUixmlPortTypes();
                if (types.StyledElement.IsAssignableFrom(targetRef.Type)
                    && types.Classes.Equals(declaringRef.Type))
                {
                    return new XamlAstClrProperty(node, "class:" + prop.Name, types.Classes,
                        null)
                    {
                        Setters = { new ClassValueSetter(types, prop.Name), new ClassBindingSetter(types, prop.Name) }
                    };
                }
            }
            */
            return node;
        }

       
        class ClassValueSetter :  IXamlEmitablePropertySetter<IXamlILEmitter>
        {
            private readonly UixmlPortXamlIlWellKnownTypes _types;
            private readonly string _className;

            public ClassValueSetter(UixmlPortXamlIlWellKnownTypes types, string className)
            {
                _types = types;
                _className = className;
                Parameters = new[] { types.XamlIlTypes.Boolean };
            }
            
            public void Emit(IXamlILEmitter emitter)
            {
                using (var value = emitter.LocalsPool.GetLocal(_types.XamlIlTypes.Boolean))
                {
                    emitter
                        .Stloc(value.Local)
                        .EmitCall(_types.StyledElementClassesProperty.Getter)
                        .Ldstr(_className)
                        .Ldloc(value.Local)
                        .EmitCall(_types.Classes.GetMethod(new FindMethodMethodSignature("Set",
                        _types.XamlIlTypes.Void, _types.XamlIlTypes.String, _types.XamlIlTypes.Boolean)));
                }
            }

            public IXamlType TargetType => _types.StyledElement;

            public PropertySetterBinderParameters BinderParameters { get; } =
                new PropertySetterBinderParameters { AllowXNull = false };
            public IReadOnlyList<IXamlType> Parameters { get; }
        }

        /*class ClassBindingSetter : IXamlEmitablePropertySetter<IXamlILEmitter>
        {
            private readonly UixmlPortXamlIlWellKnownTypes _types;
            private readonly string _className;

            public ClassBindingSetter(UixmlPortXamlIlWellKnownTypes types, string className)
            {
                _types = types;
                _className = className;
                Parameters = new[] {types.Binding};
            }
            
            public void Emit(IXamlILEmitter emitter)
            {
                using (var bloc = emitter.LocalsPool.GetLocal(_types.Binding))
                    emitter
                        .Stloc(bloc.Local)
                        .Ldstr(_className)
                        .Ldloc(bloc.Local)
                        // TODO: provide anchor?
                        .Ldnull();
                emitter.EmitCall(_types.ClassesBindMethod, true);
            }

            public IXamlType TargetType => _types.StyledElement;

            public PropertySetterBinderParameters BinderParameters { get; } =
                new PropertySetterBinderParameters { AllowXNull = false };
            public IReadOnlyList<IXamlType> Parameters { get; }
        }*/
    }
}
