#pragma warning disable
#nullable disable
using System.Linq;
using XamlX.Emit;
using XamlX.IL;
using XamlX.Transform;
using XamlX.TypeSystem;

namespace Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions.Transformers
{
    class UixmlPortXamlIlWellKnownTypes
    {
        public IXamlType UixmlPortObject { get; }
        public IXamlType DependencyObject { get; }
        public IXamlType IUixmlPortObject { get; }
        public IXamlType BindingPriority { get; }
        public IXamlType UixmlPortObjectExtensions { get; }
        public IXamlType UixmlPortProperty { get; }
        public IXamlType DependencyProperty { get; }
        public IXamlType UixmlPortPropertyT { get; }
        public IXamlType UixmlPortAttachedPropertyT { get; }
        //public IXamlType IBinding { get; }
        public IXamlType Binding { get; }
        public IXamlMethod UixmlPortObjectBindMethod { get; }
        public IXamlMethod UixmlPortObjectSetValueMethod { get; }
        public IXamlType IDisposable { get; }
        public XamlTypeWellKnownTypes XamlIlTypes { get; }
        public XamlLanguageTypeMappings XamlIlMappings { get; }
        public IXamlType Transitions { get; }
        public IXamlType AssignBindingAttribute { get; }
        public IXamlType UnsetValueType { get; }
        public IXamlType StyledElement { get; }
        public IXamlType IStyledElement { get; }
        public IXamlType NameScope { get; }
        public IXamlMethod NameScopeSetNameScope { get; }
        public IXamlType INameScope { get; }
        public IXamlMethod INameScopeRegister { get; }
        public IXamlMethod INameScopeComplete { get; }
        public IXamlType IPropertyInfo { get; }
        public IXamlType ClrPropertyInfo { get; }
        public IXamlType PropertyPath { get; }
        public IXamlType PropertyPathBuilder { get; }
        public IXamlType IPropertyAccessor { get; }
        public IXamlType PropertyInfoAccessorFactory { get; }
        public IXamlType CompiledBindingPathBuilder { get; }
        public IXamlType CompiledBindingPath { get; }
        public IXamlType CompiledBindingExtension { get; }

        public IXamlType ResolveByNameExtension { get; }

        public IXamlType DataTemplate { get; }
        public IXamlType IDataTemplate { get; }
        public IXamlType IItemsPresenterHost { get; }
        public IXamlType ItemsRepeater { get; }
        public IXamlType ReflectionBindingExtension { get; }

        public IXamlType RelativeSource { get; }
        public IXamlType UInt { get; }
        public IXamlType Int { get; }
        public IXamlType Long { get; }
        public IXamlType Uri { get; }
        public IXamlType FontFamily { get; }
        public IXamlConstructor FontFamilyConstructorUriName { get; }
        public IXamlType Thickness { get; }
        public IXamlConstructor ThicknessFullConstructor { get; }
        public IXamlType Point { get; }
        public IXamlConstructor PointFullConstructor { get; }
        /*public IXamlType Vector { get; }*/
        public IXamlConstructor VectorFullConstructor { get; }
        public IXamlType Size { get; }
        public IXamlConstructor SizeFullConstructor { get; }
        public IXamlType Matrix { get; }
        public IXamlConstructor MatrixFullConstructor { get; }
        public IXamlType CornerRadius { get; }
        public IXamlConstructor CornerRadiusFullConstructor { get; }
        public IXamlType GridLength { get; }
        public IXamlConstructor GridLengthConstructorValueType { get; }
        public IXamlType Color { get; }
        public IXamlType StandardCursorType { get; }
        public IXamlType Cursor { get; }
        public IXamlConstructor CursorTypeConstructor { get; }
        public IXamlType RowDefinition { get; }
        public IXamlType RowDefinitions { get; }
        public IXamlType ColumnDefinition { get; }
        public IXamlType ColumnDefinitions { get; }
        public IXamlType Classes { get; }
        public IXamlMethod ClassesBindMethod { get; }
        public IXamlProperty StyledElementClassesProperty { get; }
        public IXamlType IBrush { get; }
        public IXamlType ImmutableSolidColorBrush { get; }
        public IXamlConstructor ImmutableSolidColorBrushConstructorColor { get; }

        public UixmlPortXamlIlWellKnownTypes(TransformerConfiguration cfg)
        {
            XamlIlTypes = cfg.WellKnownTypes;

            UixmlPortObjectExtensions =
                cfg.TypeSystem.FindType(typeof(Alternet.UI.Port.UixmlPortObjectExtensions));

            DependencyObject = cfg.TypeSystem.FindType(typeof(Alternet.UI.Port.DependencyObject));
            DependencyProperty = cfg.TypeSystem.FindType(typeof(Alternet.UI.Port.DependencyProperty));

            Binding = cfg.TypeSystem.FindType(typeof(Alternet.UI.Port.Binding));
            IDisposable = cfg.TypeSystem.FindType(typeof(System.IDisposable));

            UixmlPortObjectBindMethod =
                UixmlPortObjectExtensions.FindMethod("Bind", IDisposable, false, DependencyObject,
                DependencyProperty,
                Binding, cfg.WellKnownTypes.Object);
            UnsetValueType = cfg.TypeSystem.FindType(typeof(Alternet.UI.UnsetValueType));

            IPropertyInfo = cfg.TypeSystem.FindType(typeof(Alternet.UI.Port.IPropertyInfo));
            ClrPropertyInfo = cfg.TypeSystem.FindType(typeof(Alternet.UI.Port.ClrPropertyInfo));

            UInt = cfg.TypeSystem.GetType("System.UInt32");
            Int = cfg.TypeSystem.GetType("System.Int32");
            Long = cfg.TypeSystem.GetType("System.Int64");
            Uri = cfg.TypeSystem.GetType("System.Uri");
        }
    }

    static class UixmlPortXamlIlWellKnownTypesExtensions
    {
        public static UixmlPortXamlIlWellKnownTypes GetUixmlPortTypes(this AstTransformationContext ctx)
        {
            if (ctx.TryGetItem<UixmlPortXamlIlWellKnownTypes>(out var rv))
                return rv;
            ctx.SetItem(rv = new UixmlPortXamlIlWellKnownTypes(ctx.Configuration));
            return rv;
        }
        
        public static UixmlPortXamlIlWellKnownTypes GetUixmlPortTypes(this XamlEmitContext<IXamlILEmitter, XamlILNodeEmitResult> ctx)
        {
            if (ctx.TryGetItem<UixmlPortXamlIlWellKnownTypes>(out var rv))
                return rv;
            ctx.SetItem(rv = new UixmlPortXamlIlWellKnownTypes(ctx.Configuration));
            return rv;
        }
    }
}
