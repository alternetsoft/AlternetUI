#pragma warning disable
#nullable disable
using System.Collections.Generic;
using System.Linq;
using Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions.Transformers;
using XamlX.Ast;
using XamlX.Emit;
using XamlX.IL;
using XamlX.Transform;
using XamlX.TypeSystem;

namespace Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions
{
    /*
        This file is used in the build task.
        ONLY use types from netstandard and XamlIl. NO dependencies on UixmlPort are allowed. Only strings.
        No, nameof isn't welcome here either
     */

    class UixmlPortXamlIlLanguage
    {
        public static (XamlLanguageTypeMappings language, XamlLanguageEmitMappings<IXamlILEmitter, XamlILNodeEmitResult> emit) Configure(IXamlTypeSystem typeSystem)
        {
            var runtimeHelpers = typeSystem.FindType(typeof(Port.XamlIlRuntimeHelpers));
            //var assignBindingAttribute = typeSystem.GetType("Alternet.UI.Data.AssignBindingAttribute");
            //var bindingType = typeSystem.GetType("Alternet.UI.Data.IBinding");
            var rv = new XamlLanguageTypeMappings(typeSystem)
            {
                SupportInitialize = typeSystem.GetType("System.ComponentModel.ISupportInitialize"),
                XmlnsAttributes =
                {
                    typeSystem.FindType(typeof(Alternet.UI.XmlnsDefinitionAttribute)),
                    //typeSystem.GetType("Alternet.UI.Metadata.XmlnsDefinitionAttribute"),
                },
                ContentAttributes =
                {
                    //typeSystem.GetType("Alternet.UI.Metadata.ContentAttribute")
                    typeSystem.FindType(typeof(Alternet.UI.ContentAttribute))
                },
                ProvideValueTarget = typeSystem.FindType(typeof(Alternet.UI.Port.IUixmlProvideValueTarget)),
                RootObjectProvider = typeSystem.FindType(typeof(Alternet.UI.Port.IUixmlRootObjectProvider)),
                RootObjectProviderIntermediateRootPropertyName = "IntermediateRootObject",
                UriContextProvider = typeSystem.FindType(typeof(Alternet.UI.Port.IUixmlUriContext)),
                ParentStackProvider =
                    typeSystem.FindType(typeof(Alternet.UI.Port.IUixmlPortXamlIlParentStackProvider)),

                XmlNamespaceInfoProvider =
                    typeSystem.FindType(typeof(Alternet.UI.Port.IUixmlPortXamlIlXmlNamespaceInfoProvider)),
                //DeferredContentPropertyAttributes = {typeSystem.GetType("Alternet.UI.Metadata.TemplateContentAttribute")},
                DeferredContentExecutorCustomizationDefaultTypeParameter
                = typeSystem.FindType(typeof(Alternet.UI.AbstractControl)),
                
                DeferredContentExecutorCustomizationTypeParameterDeferredContentAttributePropertyNames
                = new List<string>
                {
                    "TemplateResultType"
                },
                DeferredContentExecutorCustomization =
                    runtimeHelpers.FindMethod(m => m.Name == "DeferredTransformationFactoryV2"),
                UsableDuringInitializationAttributes =
                {
//                    typeSystem.GetType("Alternet.UI.Metadata.UsableDuringInitializationAttribute"),
                },
                InnerServiceProviderFactoryMethod =
                    runtimeHelpers.FindMethod(m => m.Name == "CreateInnerServiceProviderV1"),
            };
            rv.CustomAttributeResolver = new AttributeResolver(typeSystem, rv);

            var emit = new XamlLanguageEmitMappings<IXamlILEmitter, XamlILNodeEmitResult>
            {
                ProvideValueTargetPropertyEmitter = XamlIlUixmlPortPropertyHelper.EmitProvideValueTarget,
                ContextTypeBuilderCallback = (b, c) => EmitNameScopeField(rv, typeSystem, b, c)
            };
            return (rv, emit);
        }

        public const string ContextNameScopeFieldName = "UixmlPortNameScope";

        private static void EmitNameScopeField(XamlLanguageTypeMappings mappings,
            IXamlTypeSystem typeSystem,
            IXamlTypeBuilder<IXamlILEmitter> typebuilder, IXamlILEmitter constructor)
        {

            var nameScopeType = typeSystem.FindType(typeof(Alternet.UI.Markup.INameScope));
            var field = typebuilder.DefineField(nameScopeType, 
                ContextNameScopeFieldName, true, false);
            constructor
                .Ldarg_0()
                .Ldarg(1)
                .Ldtype(nameScopeType)
                .EmitCall(mappings.ServiceProvider.GetMethod(new FindMethodMethodSignature("GetService",
                    typeSystem.FindType("System.Object"), typeSystem.FindType("System.Type"))))
                .Stfld(field);
        }
        

        class AttributeResolver : IXamlCustomAttributeResolver
        {
            private readonly IXamlType _typeConverterAttribute;

            private readonly List<KeyValuePair<IXamlType, IXamlType>> _converters =
                new List<KeyValuePair<IXamlType, IXamlType>>();

            //private readonly IXamlType _uixmlPortList;
            //private readonly IXamlType _uixmlPortListConverter;


            public AttributeResolver(IXamlTypeSystem typeSystem, XamlLanguageTypeMappings mappings)
            {
                _typeConverterAttribute = mappings.TypeConverterAttributes.First();

                //void AddType(IXamlType type, IXamlType conv) 
                //    => _converters.Add(new KeyValuePair<IXamlType, IXamlType>(type, conv));

                //void Add(string type, string conv)
                //    => AddType(typeSystem.GetType(type), typeSystem.GetType(conv));
                
                //Add("Alternet.UI.Media.IImage","Alternet.UI.Markup.Xaml.Converters.BitmapTypeConverter");
                //Add("Alternet.UI.Media.Imaging.IBitmap","Alternet.UI.Markup.Xaml.Converters.BitmapTypeConverter");
                var ilist = typeSystem.GetType("System.Collections.Generic.IList`1");
               // AddType(ilist.MakeGenericType(typeSystem.GetType("Alternet.UI.Point")),
               //     typeSystem.GetType("Alternet.UI.Markup.Xaml.Converters.PointsListTypeConverter"));
                //Add("Alternet.UI.Controls.WindowIcon","Alternet.UI.Markup.Xaml.Converters.IconTypeConverter");
                //Add("System.Globalization.CultureInfo", "System.ComponentModel.CultureInfoConverter");
               // Add("System.Uri", "Alternet.UI.Markup.Xaml.Converters.UixmlPortUriTypeConverter");
                //Add("System.TimeSpan", "Alternet.UI.Markup.Xaml.Converters.TimeSpanTypeConverter");
               // Add("Alternet.UI.Media.FontFamily","Alternet.UI.Markup.Xaml.Converters.FontFamilyTypeConverter");
                //_uixmlPortList = typeSystem.GetType("Alternet.UI.Collections.UixmlPortList`1");
                //_uixmlPortListConverter = typeSystem.GetType("Alternet.UI.Collections.UixmlPortListConverter`1");
            }

            IXamlType LookupConverter(IXamlType type)
            {
                foreach(var p in _converters)
                    if (p.Key.Equals(type))
                        return p.Value;
                //if (type.GenericTypeDefinition?.Equals(_uixmlPortList) == true)
                //    return _uixmlPortListConverter.MakeGenericType(type.GenericArguments[0]);
                return null;
            }

            class ConstructedAttribute : IXamlCustomAttribute
            {
                public bool Equals(IXamlCustomAttribute other) => false;
                
                public IXamlType Type { get; }
                public List<object> Parameters { get; }
                public Dictionary<string, object> Properties { get; }

                public ConstructedAttribute(IXamlType type, List<object> parameters, Dictionary<string, object> properties)
                {
                    Type = type;
                    Parameters = parameters ?? new List<object>();
                    Properties = properties ?? new Dictionary<string, object>();
                }
            }
            
            public IXamlCustomAttribute GetCustomAttribute(IXamlType type, IXamlType attributeType)
            {
                if (attributeType.Equals(_typeConverterAttribute))
                {
                    var conv = LookupConverter(type);
                    if (conv != null)
                        return new ConstructedAttribute(_typeConverterAttribute, new List<object>() {conv}, null);
                }

                return null;
            }

            public IXamlCustomAttribute GetCustomAttribute(IXamlProperty property, IXamlType attributeType)
            {
                return null;
            }
        }

        public static bool CustomValueConverter(AstTransformationContext context,
            IXamlAstValueNode node, IXamlType type, out IXamlAstValueNode result)
        {
            if (!(node is XamlAstTextNode textNode))
            {
                result = null;
                return false;
            }

            var text = textNode.Text;
            var types = context.GetUixmlPortTypes();

            if (UixmlPortXamlIlLanguageParseIntrinsic.TryConvert(
                context,
                node,
                text,
                type,
                types,
                out result))
            {
                return true;
            }
            
            if (type.FullName == "Alternet.UI.UixmlPortProperty")
            {
                var scope =
                    context.ParentNodes().OfType<UixmlPortXamlIlTargetTypeMetadataNode>()
                    .FirstOrDefault();
                if (scope == null)
                    throw new XamlX.XamlLoadException(
                        "Unable to find the parent scope for UixmlPortProperty lookup", node);

                result = XamlIlUixmlPortPropertyHelper.CreateNode(
                    context,
                    text,
                    scope.TargetType,
                    node );
                return true;
            }

            result = null;
            return false;
        }
    }
}
