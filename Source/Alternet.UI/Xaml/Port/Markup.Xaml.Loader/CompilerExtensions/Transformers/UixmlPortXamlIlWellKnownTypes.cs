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
        public IXamlType Vector { get; }
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
            //UixmlPortObject = cfg.TypeSystem.GetType("Alternet.UI.UixmlPortObject");
            //IUixmlPortObject = cfg.TypeSystem.GetType("Alternet.UI.IUixmlPortObject");
            UixmlPortObjectExtensions = cfg.TypeSystem.GetType("Alternet.UI.UixmlPortObjectExtensions");
            //UixmlPortProperty = cfg.TypeSystem.GetType("Alternet.UI.UixmlPortProperty");
            DependencyObject = cfg.TypeSystem.GetType("Alternet.UI.DependencyObject");
            DependencyProperty = cfg.TypeSystem.GetType("Alternet.UI.DependencyProperty");
            //UixmlPortPropertyT = cfg.TypeSystem.GetType("Alternet.UI.UixmlPortProperty`1");
            //UixmlPortAttachedPropertyT = cfg.TypeSystem.GetType("Alternet.UI.AttachedProperty`1");
            //BindingPriority = cfg.TypeSystem.GetType("Alternet.UI.Data.BindingPriority");
            Binding = cfg.TypeSystem.GetType("Alternet.UI.Binding");
            IDisposable = cfg.TypeSystem.GetType("System.IDisposable");
            //Transitions = cfg.TypeSystem.GetType("Alternet.UI.Animation.Transitions");
            //AssignBindingAttribute = cfg.TypeSystem.GetType("Alternet.UI.Data.AssignBindingAttribute");
            //UixmlPortObjectBindMethod = UixmlPortObjectExtensions.FindMethod("Bind", IDisposable, false, IUixmlPortObject,
            //    UixmlPortProperty,
            //    IBinding, cfg.WellKnownTypes.Object);
            UixmlPortObjectBindMethod = UixmlPortObjectExtensions.FindMethod("Bind", IDisposable, false, DependencyObject,
                DependencyProperty,
                Binding, cfg.WellKnownTypes.Object);
            UnsetValueType = cfg.TypeSystem.GetType("Alternet.UI.UnsetValueType");
            //StyledElement = cfg.TypeSystem.GetType("Alternet.UI.StyledElement");
            //IStyledElement = cfg.TypeSystem.GetType("Alternet.UI.IStyledElement");
            //INameScope = cfg.TypeSystem.GetType("Alternet.UI.Markup.INameScope");
            //INameScopeRegister = INameScope.GetMethod(
            //    new FindMethodMethodSignature("Register", XamlIlTypes.Void,
            //         XamlIlTypes.String, XamlIlTypes.Object)
            //    {
            //        IsStatic = false,
            //        DeclaringOnly = true,
            //        IsExactMatch = true
            //    });
            //INameScopeComplete = INameScope.GetMethod(
            //    new FindMethodMethodSignature("Complete", XamlIlTypes.Void)
            //    {
            //        IsStatic = false,
            //        DeclaringOnly = true,
            //        IsExactMatch = true
            //    });
            //NameScope = cfg.TypeSystem.GetType("Alternet.UI.Controls.NameScope");
            //NameScopeSetNameScope = NameScope.GetMethod(new FindMethodMethodSignature("SetNameScope",
            //    XamlIlTypes.Void, StyledElement, INameScope)
            //{ IsStatic = true });
            //UixmlPortObjectSetValueMethod = UixmlPortObject.FindMethod("SetValue", XamlIlTypes.Void,
            //    false, UixmlPortProperty, XamlIlTypes.Object, BindingPriority);
            IPropertyInfo = cfg.TypeSystem.GetType("Alternet.UI.IPropertyInfo");
            ClrPropertyInfo = cfg.TypeSystem.GetType("Alternet.UI.ClrPropertyInfo");
            //PropertyPath = cfg.TypeSystem.GetType("Alternet.UI.Data.Core.PropertyPath");
            //PropertyPathBuilder = cfg.TypeSystem.GetType("Alternet.UI.Data.Core.PropertyPathBuilder");
            //IPropertyAccessor = cfg.TypeSystem.GetType("Alternet.UI.Data.Core.Plugins.IPropertyAccessor");
            //PropertyInfoAccessorFactory = cfg.TypeSystem.GetType("Alternet.UI.Markup.Xaml.MarkupExtensions.CompiledBindings.PropertyInfoAccessorFactory");
            //CompiledBindingPathBuilder = cfg.TypeSystem.GetType("Alternet.UI.Markup.Xaml.MarkupExtensions.CompiledBindings.CompiledBindingPathBuilder");
            //CompiledBindingPath = cfg.TypeSystem.GetType("Alternet.UI.Markup.Xaml.MarkupExtensions.CompiledBindings.CompiledBindingPath");
            //CompiledBindingExtension = cfg.TypeSystem.GetType("Alternet.UI.Markup.Xaml.MarkupExtensions.CompiledBindingExtension");
            //ResolveByNameExtension = cfg.TypeSystem.GetType("Alternet.UI.Markup.Xaml.MarkupExtensions.ResolveByNameExtension");
            //DataTemplate = cfg.TypeSystem.GetType("Alternet.UI.Markup.Xaml.Templates.DataTemplate");
            //IDataTemplate = cfg.TypeSystem.GetType("Alternet.UI.Controls.Templates.IDataTemplate");
            //IItemsPresenterHost = cfg.TypeSystem.GetType("Alternet.UI.Controls.Presenters.IItemsPresenterHost");
            //ItemsRepeater = cfg.TypeSystem.GetType("Alternet.UI.Controls.ItemsRepeater");
            //ReflectionBindingExtension = cfg.TypeSystem.GetType("Alternet.UI.Markup.Xaml.MarkupExtensions.ReflectionBindingExtension");
            //RelativeSource = cfg.TypeSystem.GetType("Alternet.UI.Data.RelativeSource");
            UInt = cfg.TypeSystem.GetType("System.UInt32");
            Int = cfg.TypeSystem.GetType("System.Int32");
            Long = cfg.TypeSystem.GetType("System.Int64");
            Uri = cfg.TypeSystem.GetType("System.Uri");
            //FontFamily = cfg.TypeSystem.GetType("Alternet.UI.Media.FontFamily");
            //FontFamilyConstructorUriName = FontFamily.GetConstructor(new List<IXamlType> { Uri, XamlIlTypes.String });

            //(IXamlType, IXamlConstructor) GetNumericTypeInfo(string name, IXamlType componentType, int componentCount)
            //{
            //    var type = cfg.TypeSystem.GetType(name);
            //    var ctor = type.GetConstructor(Enumerable.Range(0, componentCount).Select(_ => componentType).ToList());

            //    return (type, ctor);
            //}

            //(Thickness, ThicknessFullConstructor) = GetNumericTypeInfo("Alternet.UI.Thickness", XamlIlTypes.Double, 4);
            //(Point, PointFullConstructor) = GetNumericTypeInfo("Alternet.UI.Point", XamlIlTypes.Double, 2);
            //(Vector, VectorFullConstructor) = GetNumericTypeInfo("Alternet.UI.Vector", XamlIlTypes.Double, 2);
            //(Size, SizeFullConstructor) = GetNumericTypeInfo("Alternet.UI.Size", XamlIlTypes.Double, 2);
            //(Matrix, MatrixFullConstructor) = GetNumericTypeInfo("Alternet.UI.Matrix", XamlIlTypes.Double, 6);
            //(CornerRadius, CornerRadiusFullConstructor) = GetNumericTypeInfo("Alternet.UI.CornerRadius", XamlIlTypes.Double, 4);

            //GridLength = cfg.TypeSystem.GetType("Alternet.UI.Controls.GridLength");
            //GridLengthConstructorValueType = GridLength.GetConstructor(new List<IXamlType> { XamlIlTypes.Double, cfg.TypeSystem.GetType("Alternet.UI.Controls.GridUnitType") });
            //Color = cfg.TypeSystem.GetType("Alternet.UI.Media.Color");
            //StandardCursorType = cfg.TypeSystem.GetType("Alternet.UI.Input.StandardCursorType");
            //Cursor = cfg.TypeSystem.GetType("Alternet.UI.Input.Cursor");
            //CursorTypeConstructor = Cursor.GetConstructor(new List<IXamlType> { StandardCursorType });
            //ColumnDefinition = cfg.TypeSystem.GetType("Alternet.UI.Controls.ColumnDefinition");
            //ColumnDefinitions = cfg.TypeSystem.GetType("Alternet.UI.Controls.ColumnDefinitions");
            //RowDefinition = cfg.TypeSystem.GetType("Alternet.UI.Controls.RowDefinition");
            //RowDefinitions = cfg.TypeSystem.GetType("Alternet.UI.Controls.RowDefinitions");
            //Classes = cfg.TypeSystem.GetType("Alternet.UI.Controls.Classes");
            //StyledElementClassesProperty =
            //    StyledElement.Properties.First(x => x.Name == "Classes" && x.PropertyType.Equals(Classes));
            //ClassesBindMethod = cfg.TypeSystem.GetType("Alternet.UI.StyledElementExtensions")
            //    .FindMethod( "BindClass", IDisposable, false, IStyledElement,
            //    cfg.WellKnownTypes.String,
            //    IBinding, cfg.WellKnownTypes.Object);

            //IBrush = cfg.TypeSystem.GetType("Alternet.UI.Media.IBrush");
            //ImmutableSolidColorBrush = cfg.TypeSystem.GetType("Alternet.UI.Media.Immutable.ImmutableSolidColorBrush");
            //ImmutableSolidColorBrushConstructorColor = ImmutableSolidColorBrush.GetConstructor(new List<IXamlType> { UInt });
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
