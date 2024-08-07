#pragma warning disable
#nullable disable
using System.Collections.Generic;
using System.Linq;
using Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions.Transformers;
using XamlX;
using XamlX.Ast;
using XamlX.Emit;
using XamlX.IL;
using XamlX.Parsers;
using XamlX.Transform;
using XamlX.Transform.Transformers;
using XamlX.TypeSystem;

namespace Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions
{
    class UixmlPortXamlIlCompiler : XamlILCompiler
    {
        private readonly IXamlType _contextType;
        private readonly UixmlPortXamlIlDesignPropertiesTransformer _designTransformer;
        //private readonly UixmlPortBindingExtensionTransformer _bindingTransformer;

        private UixmlPortXamlIlCompiler(TransformerConfiguration configuration, XamlLanguageEmitMappings<IXamlILEmitter, XamlILNodeEmitResult> emitMappings)
            : base(configuration, emitMappings, true)
        {
            void InsertAfter<T>(params IXamlAstTransformer[] t) 
                => Transformers.InsertRange(Transformers.FindIndex(x => x is T) + 1, t);

            //void InsertBefore<T>(params IXamlAstTransformer[] t) 
            //    => Transformers.InsertRange(Transformers.FindIndex(x => x is T), t);


            // Before everything else

            _designTransformer = new UixmlPortXamlIlDesignPropertiesTransformer();

            Transformers.Insert(0, new IgnoredDirectivesTransformer());

            //Transformers.Insert(0, new XNameTransformer());
            //Transformers.Insert(1, new IgnoredDirectivesTransformer());
            //Transformers.Insert(2, _designTransformer = new UixmlPortXamlIlDesignPropertiesTransformer());
            //Transformers.Insert(3, _bindingTransformer = new UixmlPortBindingExtensionTransformer());


            //// Targeted
            //InsertBefore<PropertyReferenceResolver>(
            //    new UixmlPortXamlIlResolveClassesPropertiesTransformer(),
            //    new UixmlPortXamlIlTransformInstanceAttachedProperties(),
            //    new UixmlPortXamlIlTransformSyntheticCompiledBindingMembers());
            //InsertAfter<PropertyReferenceResolver>(
            //    new UixmlPortXamlIlUixmlPortPropertyResolver(),
            //    new UixmlPortXamlIlReorderClassesPropertiesTransformer()
            //);

            InsertAfter<PropertyReferenceResolver>(
                new UixmlPortXamlIlUixmlPortPropertyResolver()
            );

            //InsertBefore<ContentConvertTransformer>(                
            //    new UixmlPortXamlIlBindingPathParser(),
            //    new UixmlPortXamlIlSelectorTransformer(),
            //    new UixmlPortXamlIlControlTemplateTargetTypeMetadataTransformer(),
            //    new UixmlPortXamlIlPropertyPathTransformer(),
            //    new UixmlPortXamlIlSetterTransformer(),
            //    new UixmlPortXamlIlConstructorServiceProviderTransformer(),
            //    new UixmlPortXamlIlTransitionsTypeMetadataTransformer(),
            //    new UixmlPortXamlIlResolveByNameMarkupExtensionReplacer()
            //);

            //// After everything else
            //InsertBefore<NewObjectTransformer>(
            //    new AddNameScopeRegistration(),
            //    new UixmlPortXamlIlDataContextTypeTransformer(),
            //    new UixmlPortXamlIlBindingPathTransformer(),
            //    new UixmlPortXamlIlCompiledBindingsMetadataRemover()
            //    );

            //Transformers.Add(new UixmlPortXamlIlMetadataRemover());
            //Transformers.Add(new UixmlPortXamlIlRootObjectScope());

            //Emitters.Add(new UixmlPortNameScopeRegistrationXamlIlNodeEmitter());
            //Emitters.Add(new UixmlPortXamlIlRootObjectScope.Emitter());
        }
        public UixmlPortXamlIlCompiler(TransformerConfiguration configuration,
            XamlLanguageEmitMappings<IXamlILEmitter, XamlILNodeEmitResult> emitMappings,
            IXamlTypeBuilder<IXamlILEmitter> contextTypeBuilder)
            : this(configuration, emitMappings)
        {
            _contextType = CreateContextType(contextTypeBuilder);
        }

        
        public UixmlPortXamlIlCompiler(TransformerConfiguration configuration,
            XamlLanguageEmitMappings<IXamlILEmitter, XamlILNodeEmitResult> emitMappings,
            IXamlType contextType) : this(configuration, emitMappings)
        {
            _contextType = contextType;
        }
        
        public const string PopulateName = "__UixmlPortXamlIlPopulate";
        public const string BuildName = "__UixmlPortXamlIlBuild";

        public bool IsDesignMode
        {
            get => _designTransformer.IsDesignMode;
            set => _designTransformer.IsDesignMode = value;
        }

        public bool DefaultCompileBindings
        {
            //get => _bindingTransformer.CompileBindingsByDefault;
            //set => _bindingTransformer.CompileBindingsByDefault = value;
            get => false;
            set { }
        }

        public void ParseAndCompile(
            string xaml,
            string baseUri,
            IFileSource fileSource,
            IXamlTypeBuilder<IXamlILEmitter> tb,
            IXamlType overrideRootType)
        {
            var parsed = XDocumentXamlParser.Parse(xaml, new Dictionary<string, string>
            {
                {XamlNamespaces.Blend2008, XamlNamespaces.Blend2008}
            });
            
            var rootObject = (XamlAstObjectNode)parsed.Root;

            var classDirective = rootObject.Children
                .OfType<XamlAstXmlDirective>().FirstOrDefault(x =>
                    x.Namespace == XamlNamespaces.Xaml2006
                    && x.Name == "Class");

            IXamlType xamlType = null;

            if (classDirective is not null)
            {
                xamlType =
                    _configuration.TypeSystem.GetTypeOrNull(((XamlAstTextNode)classDirective.Values[0]).Text);
            }

            xamlType ??= _configuration.TypeSystem.FindType(typeof(Window));

            var rootType =
                classDirective != null ?
                    new XamlAstClrTypeReference(classDirective, xamlType, false) :
                    TypeReferenceResolver.ResolveType(CreateTransformationContext(parsed, true),
                        (XamlAstXmlTypeReference)rootObject.Type, true);
            
            
            if (overrideRootType != null)
            {
                if (!rootType.Type.IsAssignableFrom(overrideRootType))
                    throw new XamlX.XamlLoadException(
                        $"Unable to substitute {rootType.Type.GetFqn()} with {overrideRootType.GetFqn()}", rootObject);
                rootType = new XamlAstClrTypeReference(rootObject, overrideRootType, false);
            }

            OverrideRootType(parsed, rootType);

            Transform(parsed);
            Compile(parsed, tb, _contextType, PopulateName, BuildName, "__UixmlPortXamlIlNsInfo", baseUri, fileSource);
            
        }

        public void OverrideRootType(XamlDocument doc, IXamlAstTypeReference newType)
        {
            var root = (XamlAstObjectNode)doc.Root;
            var oldType = root.Type;
            if (oldType.Equals(newType))
                return;

            root.Type = newType;
            foreach (var child in root.Children.OfType<XamlAstXamlPropertyValueNode>())
            {
                if (child.Property is XamlAstNamePropertyReference prop)
                {
                    if (prop.DeclaringType.Equals(oldType))
                        prop.DeclaringType = newType;
                    if (prop.TargetType.Equals(oldType))
                        prop.TargetType = newType;
                }
            }
        }
    }
}
