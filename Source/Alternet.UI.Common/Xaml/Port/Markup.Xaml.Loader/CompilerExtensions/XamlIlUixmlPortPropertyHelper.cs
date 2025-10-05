#pragma warning disable
#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI.Markup.Xaml.Parsers;
using Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions.Transformers;
using Alternet.UI.Utilities;
using XamlX.Ast;
using XamlX.Transform;
using XamlX.Transform.Transformers;
using XamlX.TypeSystem;
using XamlX.Emit;
using XamlX.IL;

using XamlIlEmitContext = XamlX.Emit.XamlEmitContext<XamlX.IL.IXamlILEmitter, XamlX.IL.XamlILNodeEmitResult>;
using IXamlIlAstEmitableNode = XamlX.Emit.IXamlAstEmitableNode<XamlX.IL.IXamlILEmitter, XamlX.IL.XamlILNodeEmitResult>;

namespace Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions
{
    class XamlIlUixmlPortPropertyHelper
    {
        public static bool EmitProvideValueTarget(XamlIlEmitContext context, IXamlILEmitter emitter,
            XamlAstClrProperty property)
        {
            if (Emit(context, emitter, property))
                return true;
            var foundClr = property.DeclaringType.Properties.FirstOrDefault(p => p.Name == property.Name);
            if (foundClr == null)
                return false;
            context
                .Configuration.GetExtra<XamlIlClrPropertyInfoEmitter>()
                .Emit(context, emitter, foundClr);
            return true;
        }
        
        public static bool Emit(XamlIlEmitContext context, IXamlILEmitter emitter, XamlAstClrProperty property)
        {
            if (property is IXamlIlUixmlPortProperty ap)
            {
                emitter.Ldsfld(ap.UixmlPortProperty);
                return true;
            }
            var type = property.DeclaringType;
            var name = property.Name + "Property";
            var found = type.Fields.FirstOrDefault(f => f.IsStatic && f.Name == name);
            if (found == null)
                return false;

            emitter.Ldsfld(found);
            return true;
        }
        
        public static bool Emit(XamlIlEmitContext context, IXamlILEmitter emitter, IXamlProperty property)
        {
            var type = (property.Getter ?? property.Setter).DeclaringType;
            var name = property.Name + "Property";
            var found = type.Fields.FirstOrDefault(f => f.IsStatic && f.Name == name);
            if (found == null)
                return false;

            emitter.Ldsfld(found);
            return true;
        }

        public static IXamlIlUixmlPortPropertyNode CreateNode(AstTransformationContext context,
            string propertyName, IXamlAstTypeReference selectorTypeReference, IXamlLineInfo lineInfo)
        {
            XamlAstNamePropertyReference forgedReference;
            
            var parser = new PropertyParser();
            
            var parsedPropertyName = parser.Parse(new CharacterReader(propertyName.AsSpan()));
            if(parsedPropertyName.owner == null)
                forgedReference = new XamlAstNamePropertyReference(lineInfo, selectorTypeReference,
                    propertyName, selectorTypeReference);
            else
            {
                var xmlOwner = parsedPropertyName.ns;
                if (xmlOwner != null)
                    xmlOwner += ":";
                xmlOwner += parsedPropertyName.owner;
                
                var tref = TypeReferenceResolver.ResolveType(context, xmlOwner, false, lineInfo, true);

                var propertyFieldName = parsedPropertyName.name + "Property";
                var found = tref.Type.GetAllFields()
                    .FirstOrDefault(f => f.IsStatic && f.IsPublic && f.Name == propertyFieldName);
                if (found == null)
                    throw new XamlX.XamlParseException(
                        $"Unable to find {propertyFieldName} field on type {tref.Type.GetFullName()}", lineInfo);
                return new XamlIlUixmlPortPropertyFieldNode(context.GetUixmlPortTypes(), lineInfo, found);
            }

            var clrProperty =
                ((XamlAstClrProperty)new PropertyReferenceResolver().Transform(context,
                    forgedReference));
            return new XamlIlUixmlPortPropertyNode(lineInfo,
                context.Configuration.TypeSystem.GetType("Alternet.UI.UixmlPortProperty"),
                clrProperty);
        }

        public static IXamlType GetUixmlPortPropertyType(IXamlField field,
            UixmlPortXamlIlWellKnownTypes types, IXamlLineInfo lineInfo)
        {
            var uixmlPortPropertyType = field.FieldType;
            while (uixmlPortPropertyType != null)
            {
                if (uixmlPortPropertyType.GenericTypeDefinition?.Equals(types.UixmlPortPropertyT) == true)
                {
                    return uixmlPortPropertyType.GenericArguments[0];
                }

                uixmlPortPropertyType = uixmlPortPropertyType.BaseType;
            }

            throw new XamlX.XamlParseException(
                $"{field.Name}'s type {field.FieldType} doesn't inherit from  UixmlPortProperty<T>, make sure to use typed properties",
                lineInfo);

        }
    }

    interface IXamlIlUixmlPortPropertyNode : IXamlAstValueNode
    {
        IXamlType UixmlPortPropertyType { get; }
    }
    
    class XamlIlUixmlPortPropertyNode : XamlAstNode, IXamlAstValueNode, IXamlIlAstEmitableNode, IXamlIlUixmlPortPropertyNode
    {
        public XamlIlUixmlPortPropertyNode(IXamlLineInfo lineInfo, IXamlType type, XamlAstClrProperty property) : base(lineInfo)
        {
            Type = new XamlAstClrTypeReference(this, type, false);
            Property = property;
            UixmlPortPropertyType = Property.Getter?.ReturnType
                                   ?? Property.Setters.First().Parameters[0];
        }

        public XamlAstClrProperty Property { get; }

        public IXamlAstTypeReference Type { get; }
        public XamlILNodeEmitResult Emit(XamlIlEmitContext context, IXamlILEmitter codeGen)
        {
            if (!XamlIlUixmlPortPropertyHelper.Emit(context, codeGen, Property))
                throw new XamlX.XamlLoadException(Property.Name + " is not an UixmlPortProperty", this);
            return XamlILNodeEmitResult.Type(0, Type.GetClrType());
        }

        public IXamlType UixmlPortPropertyType { get; }
    }

    class XamlIlUixmlPortPropertyFieldNode : XamlAstNode, IXamlAstValueNode, IXamlIlAstEmitableNode, IXamlIlUixmlPortPropertyNode
    {
        private readonly IXamlField _field;

        public XamlIlUixmlPortPropertyFieldNode(UixmlPortXamlIlWellKnownTypes types,
            IXamlLineInfo lineInfo, IXamlField field) : base(lineInfo)
        {
            _field = field;
            UixmlPortPropertyType = XamlIlUixmlPortPropertyHelper.GetUixmlPortPropertyType(field,
                types, lineInfo);
        }
        
        

        public IXamlAstTypeReference Type => new XamlAstClrTypeReference(this, _field.FieldType, false);
        public XamlILNodeEmitResult Emit(XamlIlEmitContext context, IXamlILEmitter codeGen)
        {
            codeGen.Ldsfld(_field);
            return XamlILNodeEmitResult.Type(0, _field.FieldType);
        }

        public IXamlType UixmlPortPropertyType { get; }
    }

    interface IXamlIlUixmlPortProperty
    {
        IXamlField UixmlPortProperty { get; }
    }
    
    class XamlIlUixmlPortProperty : XamlAstClrProperty, IXamlIlUixmlPortProperty
    {
        public IXamlField UixmlPortProperty { get; }
        public XamlIlUixmlPortProperty(XamlAstClrProperty original, IXamlField field,
            UixmlPortXamlIlWellKnownTypes types)
            :base(original, original.Name, original.DeclaringType, original.Getter, original.Setters)
        {
            UixmlPortProperty = field;
            CustomAttributes = original.CustomAttributes;
            /*
            if (!original.CustomAttributes.Any(ca => ca.Type.Equals(types.AssignBindingAttribute)))
                Setters.Insert(0, new BindingSetter(types, original.DeclaringType, field));
            */
            
            //Setters.Insert(0, new UnsetValueSetter(types, original.DeclaringType, field));
        }

        abstract class UixmlPortPropertyCustomSetter : IXamlPropertySetter, IXamlEmitablePropertySetter<IXamlILEmitter>
        {
            protected UixmlPortXamlIlWellKnownTypes Types;
            protected IXamlField UixmlPortProperty;

            public UixmlPortPropertyCustomSetter(UixmlPortXamlIlWellKnownTypes types,
                IXamlType declaringType,
                IXamlField uixmlPortProperty)
            {
                Types = types;
                UixmlPortProperty = uixmlPortProperty;
                TargetType = declaringType;
            }

            public IXamlType TargetType { get; }

            public PropertySetterBinderParameters BinderParameters { get; } = new PropertySetterBinderParameters
            {
                AllowXNull = false
            };

            public IReadOnlyList<IXamlType> Parameters { get; set; }
            public abstract void Emit(IXamlILEmitter codegen);
        }

        class UnsetValueSetter : UixmlPortPropertyCustomSetter
        {
            public UnsetValueSetter(UixmlPortXamlIlWellKnownTypes types, IXamlType declaringType, IXamlField uixmlPortProperty) 
                : base(types, declaringType, uixmlPortProperty)
            {
                Parameters = new[] {types.UnsetValueType};
            }

            public override void Emit(IXamlILEmitter codegen)
            {
                var unsetValue = Types.UixmlPortProperty.Fields.First(f => f.Name == "UnsetValue");
                codegen
                    // Ignore the instance and load one from the static field to avoid extra local variable
                    .Pop()
                    .Ldsfld(UixmlPortProperty)
                    .Ldsfld(unsetValue)
                    .Ldc_I4(0)
                    .EmitCall(Types.UixmlPortObjectSetValueMethod, true);
            }
        }
    }
}
