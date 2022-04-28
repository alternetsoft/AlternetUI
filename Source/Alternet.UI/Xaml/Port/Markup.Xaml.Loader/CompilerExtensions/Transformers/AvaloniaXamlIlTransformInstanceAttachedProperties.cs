#nullable disable
using System.Collections.Generic;
using System.Linq;
using XamlX;
using XamlX.Ast;
using XamlX.Emit;
using XamlX.IL;
using XamlX.Transform;
using XamlX.TypeSystem;

namespace Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions.Transformers
{
    class UixmlPortXamlIlTransformInstanceAttachedProperties : IXamlAstTransformer
    {

        public IXamlAstNode Transform(AstTransformationContext context, IXamlAstNode node)
        {
            if (node is XamlAstNamePropertyReference prop 
                && prop.TargetType is XamlAstClrTypeReference targetRef 
                && prop.DeclaringType is XamlAstClrTypeReference declaringRef)
            {
                // Target and declared type aren't assignable but both inherit from UixmlPortObject
                var uixmlPortObject = context.Configuration.TypeSystem.FindType("Alternet.UI.UixmlPortObject");
                if (uixmlPortObject.IsAssignableFrom(targetRef.Type)
                    && uixmlPortObject.IsAssignableFrom(declaringRef.Type)
                    && !declaringRef.Type.IsAssignableFrom(targetRef.Type))
                {
                    // Instance property
                    var clrProp = declaringRef.Type.GetAllProperties().FirstOrDefault(p => p.Name == prop.Name);
                    if (clrProp != null
                        && (clrProp.Getter?.IsStatic == false || clrProp.Setter?.IsStatic == false))
                    {
                        var declaringType = (clrProp.Getter ?? clrProp.Setter)?.DeclaringType;
                        var uixmlPortPropertyFieldName = prop.Name + "Property";
                        var uixmlPortPropertyField = declaringType.Fields.FirstOrDefault(f => f.IsStatic && f.Name == uixmlPortPropertyFieldName);
                        if (uixmlPortPropertyField != null)
                        {
                            var uixmlPortPropertyType = uixmlPortPropertyField.FieldType;
                            while (uixmlPortPropertyType != null
                                   && !(uixmlPortPropertyType.Namespace == "UixmlPort"
                                        && (uixmlPortPropertyType.Name == "UixmlPortProperty"
                                            || uixmlPortPropertyType.Name == "UixmlPortProperty`1"
                                        )))
                            {
                                // Attached properties are handled by vanilla XamlIl
                                if (uixmlPortPropertyType.Name.StartsWith("AttachedProperty"))
                                    return node;
                                
                                uixmlPortPropertyType = uixmlPortPropertyType.BaseType;
                            }

                            if (uixmlPortPropertyType == null)
                                return node;

                            if (uixmlPortPropertyType.GenericArguments?.Count > 1)
                                return node;

                            var propertyType = uixmlPortPropertyType.GenericArguments?.Count == 1 ?
                                uixmlPortPropertyType.GenericArguments[0] :
                                context.Configuration.WellKnownTypes.Object;

                            return new UixmlPortAttachedInstanceProperty(prop, context.Configuration,
                                    declaringType, propertyType, uixmlPortPropertyType, uixmlPortObject,
                                    uixmlPortPropertyField);
                        }

                    }


                }
            }

            return node;
        }

        class UixmlPortAttachedInstanceProperty : XamlAstClrProperty, IXamlIlUixmlPortProperty
        {
            private readonly TransformerConfiguration _config;
            private readonly IXamlType _declaringType;
            private readonly IXamlType _uixmlPortPropertyType;
            private readonly IXamlType _uixmlPortObject;
            private readonly IXamlField _field;

            public UixmlPortAttachedInstanceProperty(XamlAstNamePropertyReference prop,
                TransformerConfiguration config,
                IXamlType declaringType,
                IXamlType type,
                IXamlType uixmlPortPropertyType,
                IXamlType uixmlPortObject,
                IXamlField field) : base(prop, prop.Name,
                declaringType, null)
            
            
            {
                _config = config;
                _declaringType = declaringType;
                _uixmlPortPropertyType = uixmlPortPropertyType;
                
                // XamlIl doesn't support generic methods yet
                if (_uixmlPortPropertyType.GenericArguments?.Count > 0)
                    _uixmlPortPropertyType = _uixmlPortPropertyType.BaseType;
                
                _uixmlPortObject = uixmlPortObject;
                _field = field;
                PropertyType = type;
                Setters.Add(new SetterMethod(this));
                Getter = new GetterMethod(this);
            }

            public IXamlType PropertyType { get;  }

            public IXamlField UixmlPortProperty => _field;
            
            class SetterMethod : IXamlPropertySetter, IXamlEmitablePropertySetter<IXamlILEmitter>
            {
                private readonly UixmlPortAttachedInstanceProperty _parent;

                public SetterMethod(UixmlPortAttachedInstanceProperty parent)
                {
                    _parent = parent;
                    Parameters = new[] {_parent._uixmlPortObject, _parent.PropertyType};
                }

                public IXamlType TargetType => _parent.DeclaringType;
                public PropertySetterBinderParameters BinderParameters { get; } = new PropertySetterBinderParameters();
                public IReadOnlyList<IXamlType> Parameters { get; }
                public void Emit(IXamlILEmitter emitter)
                {
                    var so = _parent._config.WellKnownTypes.Object;
                    var method = _parent._uixmlPortObject
                        .FindMethod(m => m.IsPublic && !m.IsStatic && m.Name == "SetValue"
                                         &&
                                         m.Parameters.Count == 3
                                         && m.Parameters[0].Equals(_parent._uixmlPortPropertyType)
                                         && m.Parameters[1].Equals(so)
                                         && m.Parameters[2].IsEnum
                        );
                    if (method == null)
                        throw new XamlTypeSystemException(
                            "Unable to find SetValue(UixmlPortProperty, object, BindingPriority) on UixmlPortObject");
                    using (var loc = emitter.LocalsPool.GetLocal(_parent.PropertyType))
                        emitter
                            .Stloc(loc.Local)
                            .Ldsfld(_parent._field)
                            .Ldloc(loc.Local);

                    if(_parent.PropertyType.IsValueType)
                        emitter.Box(_parent.PropertyType);
                    emitter        
                        .Ldc_I4(0)
                        .EmitCall(method);

                }
            }

            class GetterMethod :  IXamlCustomEmitMethod<IXamlILEmitter>
            {
                public GetterMethod(UixmlPortAttachedInstanceProperty parent) 
                {
                    Parent = parent;
                    DeclaringType = parent._declaringType;
                    Name = "UixmlPortObject:GetValue_" + Parent.Name;
                    Parameters = new[] {parent._uixmlPortObject};
                }
                public UixmlPortAttachedInstanceProperty Parent { get; }
                public bool IsPublic => true;
                public bool IsStatic => true;
                public string Name { get; protected set; }
                public IXamlType DeclaringType { get; }
                public IXamlMethod MakeGenericMethod(IReadOnlyList<IXamlType> typeArguments) 
                    => throw new System.NotSupportedException();


                public bool Equals(IXamlMethod other) =>
                    other is GetterMethod m && m.Name == Name && m.DeclaringType.Equals(DeclaringType);
                public IXamlType ReturnType => Parent.PropertyType;
                public IReadOnlyList<IXamlType> Parameters { get; }

                public IReadOnlyList<IXamlCustomAttribute> CustomAttributes => DeclaringType.CustomAttributes;

                public void EmitCall(IXamlILEmitter emitter)
                {
                    var method = Parent._uixmlPortObject
                        .FindMethod(m => m.IsPublic && !m.IsStatic && m.Name == "GetValue"
                                         &&
                                         m.Parameters.Count == 1
                                         && m.Parameters[0].Equals(Parent._uixmlPortPropertyType));
                    if (method == null)
                        throw new XamlTypeSystemException(
                            "Unable to find T GetValue<T>(UixmlPortProperty<T>) on UixmlPortObject");
                    emitter
                        .Ldsfld(Parent._field)
                        .EmitCall(method);
                    if (Parent.PropertyType.IsValueType)
                        emitter.Unbox_Any(Parent.PropertyType);

                }
            }
        }
    }
}
