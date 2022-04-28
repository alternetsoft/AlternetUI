#nullable disable
using XamlX.Transform;
using XamlX.TypeSystem;

namespace Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions
{
    class UixmlPortXamlIlCompilerConfiguration : TransformerConfiguration
    {
        public XamlIlClrPropertyInfoEmitter ClrPropertyEmitter { get; }
        public XamlIlPropertyInfoAccessorFactoryEmitter AccessorFactoryEmitter { get; }

        public UixmlPortXamlIlCompilerConfiguration(IXamlTypeSystem typeSystem, 
            IXamlAssembly defaultAssembly, 
            XamlLanguageTypeMappings typeMappings,
            XamlXmlnsMappings xmlnsMappings,
            XamlValueConverter customValueConverter,
            XamlIlClrPropertyInfoEmitter clrPropertyEmitter,
            XamlIlPropertyInfoAccessorFactoryEmitter accessorFactoryEmitter,
            IXamlIdentifierGenerator identifierGenerator = null)
            : base(typeSystem, defaultAssembly, typeMappings, xmlnsMappings, customValueConverter, identifierGenerator)
        {
            ClrPropertyEmitter = clrPropertyEmitter;
            AccessorFactoryEmitter = accessorFactoryEmitter;
            AddExtra(ClrPropertyEmitter);
            AddExtra(AccessorFactoryEmitter);
        }
    }
}
