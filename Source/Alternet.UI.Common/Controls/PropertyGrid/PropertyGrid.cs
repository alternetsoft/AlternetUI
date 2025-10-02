using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Specialized grid for editing properties - in other words name = value pairs.
    /// </summary>
    /// <remarks>
    /// List of ready-to-use property classes include strings, numbers, flag sets, fonts,
    /// colors and many others. It is possible, for example, to categorize properties,
    /// set up a complete tree-hierarchy, add more than two columns, and set
    /// arbitrary per-property attributes.
    /// </remarks>
    [ControlCategory("Other")]
    public partial class PropertyGrid : Control, IPropertyGrid
    {
        internal const string PropEditClassCheckBox = "CheckBox";
        internal const string PropEditClassChoice = "Choice";
        internal const string PropEditClassTextCtrl = "TextCtrl";
        internal const string PropEditClassChoiceAndButton = "ChoiceAndButton";
        internal const string PropEditClassComboBox = "ComboBox";
        internal const string PropEditClassSpinCtrl = "SpinCtrl";
        internal const string PropEditClassTextCtrlAndButton = "TextCtrlAndButton";

        private static readonly IPropertyGridFactory DefaultFactory = new PropertyGridFactory();

        private static PropertyGridEditKindColor defaultEditKindColor =
            PropertyGridEditKindColor.ComboBox;

        private readonly BaseDictionary<PropertyGridItemHandle, IPropertyGridItem> items = new();
        private readonly HashSet<string> ignorePropNames = new();

        private int suppressIgnoreProps;

        static PropertyGrid()
        {
            AddInitializer(() =>
            {
                RegisterPropCreateFunc(typeof(Color), FuncCreatePropertyAsColor);
                RegisterPropCreateFunc(typeof(Font), FuncCreatePropertyAsFont);
                RegisterPropCreateFunc(typeof(Brush), FuncCreatePropertyAsBrush);
                RegisterPropCreateFunc(typeof(Pen), FuncCreatePropertyAsPen);
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyGrid"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public PropertyGrid(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyGrid"/> class.
        /// </summary>
        public PropertyGrid()
        {
            if(initializers is not null)
            {
                while (initializers.TryPop(out var action))
                {
                    action();
                }
            }
        }

        /// <summary>
        /// Occurs when a property selection has been changed, either by user action
        /// or by indirect program function.
        /// </summary>
        /// <remarks>
        /// For instance, collapsing a parent property programmatically causes any selected
        /// child property to become unselected, and may therefore cause this event to be generated.
        /// </remarks>
        public event EventHandler? PropertySelected;

        /// <summary>
        /// Occurs when property value has been changed by the user.
        /// </summary>
        public new event EventHandler? PropertyChanged;

        /// <summary>
        /// Occurs when button is clicked in the property editor.
        /// </summary>
        public event EventHandler? ButtonClick;

        /// <summary>
        /// Occurs when property value is about to be changed by the user.
        /// </summary>
        /// <remarks>
        /// You can veto this event to prevent the action.
        /// </remarks>
        public event EventHandler<CancelEventArgs>? PropertyChanging;

        /// <summary>
        /// Occurs when mouse moves over a property.
        /// </summary>
        public event EventHandler? PropertyHighlighted;

        /// <summary>
        /// Occurs when property is clicked with right mouse button.
        /// </summary>
        public event EventHandler? PropertyRightClick;

        /// <summary>
        /// Occurs when property is double-clicked with left mouse button.
        /// </summary>
        public event EventHandler? PropertyDoubleClick;

        /// <summary>
        /// Occurs when user collapses a property or category.
        /// </summary>
        public event EventHandler? ItemCollapsed;

        /// <summary>
        /// Occurs when user expands a property or category.
        /// </summary>
        public event EventHandler? ItemExpanded;

        /// <summary>
        /// Occurs when user is about to begin editing a property label.
        /// </summary>
        /// <remarks>
        /// You can veto this event to prevent the action.
        /// </remarks>
        public event EventHandler<CancelEventArgs>? LabelEditBegin;

        /// <summary>
        /// Occurs when user is about to end editing of a property label.
        /// </summary>
        /// <remarks>
        /// You can veto this event to prevent the action.
        /// </remarks>
        public event EventHandler<CancelEventArgs>? LabelEditEnding;

        /// <summary>
        /// Occurs when user starts resizing a column.
        /// </summary>
        /// <remarks>
        /// You can veto this event to prevent the action.
        /// </remarks>
        public event EventHandler<CancelEventArgs>? ColBeginDrag;

        /// <summary>
        /// Occurs when a column resize by the user is in progress. This event is also
        /// generated when user double-clicks the splitter in order to recenter it.
        /// </summary>
        public event EventHandler? ColDragging;

        /// <summary>
        /// Occurs after column resize by the user has finished.
        /// </summary>
        public event EventHandler? ColEndDrag;

        /// <summary>
        /// Occurs when new property item is created.
        /// </summary>
        /// <remarks>
        /// Use this event to override default property item creation mechanism
        /// used inside <see cref="SetProps"/> and similar methods where object's
        /// <see cref="PropertyInfo"/> is converted to the <see cref="IPropertyGridItem"/>.
        /// </remarks>
        public event CreatePropertyEventHandler? PropertyCustomCreate;

        /// <summary>
        /// Defines default style for the newly created
        /// <see cref="PropertyGrid"/> controls.
        /// </summary>
        public static PropertyGridCreateStyle DefaultCreateStyle { get; set; }
            = PropertyGridCreateStyle.DefaultStyle;

        /// <summary>
        /// Gets or sets default editor for <see cref="Color"/>.
        /// </summary>
        public static PropertyGridEditKindColor DefaultEditKindColor
        {
            get
            {
                return defaultEditKindColor;
            }

            set
            {
                if (value == PropertyGridEditKindColor.Default)
                    return;
                defaultEditKindColor = value;
            }
        }

        /// <summary>
        /// Gets whether <see cref="Items"/> has items.
        /// </summary>
        public bool HasItems => items.Count > 0;

        /// <summary>
        /// Gets first item in <see cref="Items"/>.
        /// </summary>
        public IPropertyGridItem? FirstItem => Items.FirstOrDefault();

        /// <summary>
        /// Gets <see cref="PropertyGridItem.Instance"/> of the first
        /// item in the <see cref="Items"/>.
        /// </summary>
        public object? FirstItemInstance
        {
            get
            {
                if (HasItems)
                    return Items.First().Instance;
                return null;
            }
        }

        /// <summary>
        /// Gets list of <see cref="IPropertyGridItem"/> added to this control.
        /// </summary>
        public ICollection<IPropertyGridItem> Items => items.Values;

        /// <summary>
        /// Contains list of property names to ignore in <see cref="AddProps"/>.
        /// </summary>
        public ICollection<string> IgnorePropNames => ignorePropNames;

        /// <inheritdoc/>
        public override IReadOnlyList<AbstractControl> AllChildrenInLayout
            => Array.Empty<AbstractControl>();

        /// <summary>
        /// Gets or sets whether boolean properties will be shown as checkboxes.
        /// Default is <c>true</c>.
        /// </summary>
        public bool BoolAsCheckBox { get; set; } = true;

        /// <summary>
        /// Gets or sets whether <see cref="Color"/> properties have alpha channel.
        /// Default is <c>true</c>.
        /// </summary>
        public bool ColorHasAlpha { get; set; } = true;

        /// <summary>
        /// Gets or sets default <see cref="Color"/> format when
        /// <see cref="ColorHasAlpha"/> is <c>true</c>.
        /// </summary>
        public string DefaultColorFormatRGBA { get; set; } = "({0}, {1}, {2}, {3})";

        /// <summary>
        /// Gets or sets default <see cref="Color"/> format when
        /// <see cref="ColorHasAlpha"/> is <c>false</c>.
        /// </summary>
        public string DefaultColorFormatRGB { get; set; } = "({0}, {1}, {2})";

        /// <summary>
        /// Gets property value used in the event handler.
        /// </summary>
        [Browsable(false)]
        public virtual object? EventPropValue
        {
            get
            {
                var result = EventPropValueAsVariant.AsObject;
                return result;
            }
        }

        /// <summary>
        /// Gets or sets flags used when property value
        /// is applied back to object instance in default <see cref="PropertyChanged"/>
        /// event handler.
        /// </summary>
        public PropertyGridApplyFlags ApplyFlags { get; set; } = PropertyGridApplyFlags.Default;

        /// <summary>
        /// Gets or sets different <see cref="PropertyGrid"/> features.
        /// </summary>
        public PropertyGridFeature Features { get; set; }

        /// <summary>
        /// Gets or sets optional <see cref="CultureInfo"/> used when property
        /// values are converted to/from string. Default is Null.
        /// </summary>
        public CultureInfo? Culture { get; set; }

        /// <summary>
        /// Defines default extended style for the newly created
        /// <see cref="PropertyGrid"/> controls.
        /// </summary>
        internal static PropertyGridCreateStyleEx DefaultCreateStyleEx { get; set; }
            = PropertyGridCreateStyleEx.DefaultStyle;

        /// <summary>
        /// Creates new <see cref="IPropertyGrid"/> instance.
        /// </summary>
        public static IPropertyGrid CreatePropertyGrid()
        {
            return new PropertyGrid();
        }

        /// <summary>
        /// Determines whether the specified property represents an enumeration,
        /// a flags enumeration, or neither.
        /// </summary>
        /// <remarks>This method evaluates the property's type to determine if it is
        /// an enumeration or a flags enumeration. If the property type is not an enumeration,
        /// <see cref="FlagsOrEnum.None"/> is returned.</remarks>
        /// <param name="instance">The object instance containing the property to evaluate.</param>
        /// <param name="propInfo">The metadata information for the property to evaluate.
        /// Cannot be null.</param>
        /// <returns>A <see cref="FlagsOrEnum"/> value indicating the type
        /// of the property: <see cref="FlagsOrEnum.Flags"/> if
        /// the property is a flags enumeration, <see cref="FlagsOrEnum.Enum"/>
        /// if the property is a standard
        /// enumeration, or <see cref="FlagsOrEnum.None"/> if the property is neither.</returns>
        public static FlagsOrEnum IsFlagsOrEnum(object instance, PropertyInfo propInfo)
        {
            var valueType = propInfo.PropertyType ?? typeof(object);

            var realType = AssemblyUtils.GetRealType(valueType);
            var isEnum = realType.IsEnum;

            if (isEnum)
            {
                var prm = PropertyGrid.ConstructNewItemParams(instance, propInfo);
                bool isFlags;
                if (prm.EnumIsFlags is null)
                    isFlags = AssemblyUtils.EnumIsFlags(realType);
                else
                    isFlags = prm.EnumIsFlags.Value;

                if (isFlags)
                    return FlagsOrEnum.Flags;
                return FlagsOrEnum.Enum;
            }
            else
            {
                return FlagsOrEnum.None;
            }
        }

        /// <summary>
        /// Creates new <see cref="IPropertyGridVariant"/> instance.
        /// </summary>
        public static IPropertyGridVariant CreateVariant()
        {
            return ControlFactory.Handler.CreateVariant();
        }

        /// <summary>
        /// Registers <see cref="IPropertyGridItem"/> create function for specific <see cref="Type"/>.
        /// </summary>
        /// <param name="type">Object type.</param>
        /// <param name="func">Create function.</param>
        public static void RegisterPropCreateFunc(Type type, PropertyGridItemCreate func)
        {
            var registry = GetTypeRegistry(type);
            registry.CreateFunc = func;
        }

        /// <summary>
        /// Sets custom label for the property.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="propName">Property name.</param>
        /// <param name="label">New custom label of the property.</param>
        /// <returns><c>true</c> if operation successful, <c>false</c> otherwise.</returns>
        public static bool SetCustomLabel<T>(string propName, string label)
            where T : class
        {
            var propInfo = AssemblyUtils.GetPropertySafe(typeof(T), propName);
            if (propInfo == null)
                return false;
            var propRegistry = GetPropRegistry(typeof(T), propInfo);
            propRegistry.NewItemParams.Label = label;
            return true;
        }

        /// <summary>
        /// Adds constant item (readonly string).
        /// </summary>
        /// <param name="label">Item label.</param>
        /// <param name="name">Item name (optional).</param>
        /// <param name="value">Item value.</param>
        /// <returns>Created and added property item.</returns>
        public virtual IPropertyGridItem AddConstItem(string label, string? name, object? value)
        {
            var item = CreateStringItem(label, name, value?.ToString() ?? string.Empty);
            Add(item);
            SetPropertyReadOnly(item, true, false);
            return item;
        }

        /// <summary>
        /// <inheritdoc cref="CreateStringItem"/>
        /// </summary>
        /// <remarks>
        /// This function uses <see cref="IPropertyGridNewItemParams.EditKindString"/> to
        /// select appropriate property editor.
        /// </remarks>
        public virtual IPropertyGridItem CreateStringItemWithKind(
            string label,
            string? name,
            string? value,
            IPropertyGridNewItemParams? prm)
        {
            IPropertyGridItem CreateDefault() => CreateStringItem(label, name, value, prm);

            if (prm == null || prm.EditKindString == null)
                return CreateDefault();
            switch (prm.EditKindString.Value)
            {
                case PropertyGridEditKindString.Simple:
                    return CreateStringItem(label, name, value, prm);
                case PropertyGridEditKindString.Long:
                    return CreateLongStringItem(label, name, value, prm);
                case PropertyGridEditKindString.Ellipsis:
                    var result = CreateDefault();
                    SetPropertyEditorByName(result, PropEditClassTextCtrlAndButton);
                    return result;
                case PropertyGridEditKindString.FileName:
                    return CreateFilenameItem(label, name, value, prm);
                case PropertyGridEditKindString.ImageFileName:
                    return CreateImageFilenameItem(label, name, value, prm);
                case PropertyGridEditKindString.Directory:
                    return CreateDirItem(label, name, value, prm);
                default:
                    return CreateDefault();
            }
        }

        /// <summary>
        /// Creates <see cref="char"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateCharItem(
            string label,
            string? name = null,
            char? value = null,
            IPropertyGridNewItemParams? prm = null)
        {
            var result = CreateStringItemWithKind(label, name, value?.ToString(), prm);
            SetPropertyMaxLength(result, 1);
            return result;
        }

        /// <inheritdoc/>
        public override void OnLayout()
        {
        }

        /// <summary>
        /// <inheritdoc cref="CreateColorItem"/>
        /// </summary>
        /// <remarks>
        /// This function uses <see cref="IPropertyGridNewItemParams.EditKindColor"/> to
        /// select appropriate property editor.
        /// </remarks>
        public virtual IPropertyGridItem CreateColorItemWithKind(
            string label,
            string? name,
            Color value,
            IPropertyGridNewItemParams? prm)
        {
            IPropertyGridItem Fn()
            {
                IPropertyGridItem CreateDefault()
                {
                    return CreateComboBox();
                }

                IPropertyGridItem CreateComboBox()
                {
                    var result = CreateColorItem(label, name, value, prm);
                    SetPropertyEditorByKnownName(result, PropertyGridKnownEditors.ComboBox);
                    return result;
                }

                IPropertyGridItem CreateChoice()
                {
                    var result = CreateColorItem(label, name, value, prm);
                    SetPropertyEditorByKnownName(result, PropertyGridKnownEditors.Choice);
                    return result;
                }

                IPropertyGridItem CreateChoiceAndButton()
                {
                    var result = CreateColorItem(label, name, value, prm);
                    SetPropertyEditorByKnownName(result, PropertyGridKnownEditors.ChoiceAndButton);
                    return result;
                }

                PropertyGridEditKindColor kind = DefaultEditKindColor;

                if (prm != null && prm.EditKindColor != null)
                    kind = prm.EditKindColor.Value;

                if (kind == PropertyGridEditKindColor.Default)
                    kind = DefaultEditKindColor;

                return kind switch
                {
                    PropertyGridEditKindColor.Default => CreateDefault(),
                    PropertyGridEditKindColor.TextBoxAndButton =>
                        CreateColorItem(label, name, value, prm),
                    PropertyGridEditKindColor.SystemColors =>
                        CreateSystemColorItem(label, name, value, prm),
                    PropertyGridEditKindColor.ComboBox => CreateComboBox(),
                    PropertyGridEditKindColor.Choice => CreateChoice(),
                    PropertyGridEditKindColor.ChoiceAndButton => CreateChoiceAndButton(),
                    _ => CreateDefault(),
                };
            }

            var result = Fn();

            if (ColorHasAlpha)
                SetPropertyKnownAttribute(result, PropertyGridItemAttrId.HasAlpha, true);

            return result;
        }

        /// <summary>
        /// Creates property for structures.
        /// </summary>
        /// <inheritdoc cref="CreatePropertyAsDummy"/>
        public virtual IPropertyGridItem CreatePropertyAsStruct(
            string label,
            string? name,
            object instance,
            PropertyInfo propInfo)
        {
            IPropertyGridItem? result;

            var realType = AssemblyUtils.GetRealType(propInfo.PropertyType);
            var registry = GetTypeRegistryOrNull(realType);
            var createFunc = registry?.CreateFunc;

            if (createFunc != null)
            {
                result = createFunc(this, label, name, instance, propInfo);
            }
            else
            {
                var value = GetStructPropertyValueForReload(null, instance, propInfo);
                var prm = ConstructNewItemParams(instance, propInfo);
                prm.TextReadOnly = true;
                result = CreateStringItemWithKind(label, name, value?.ToString(), prm);
                result.GetValueFuncForReload = GetStructPropertyValueForReload;

                OnPropertyCreated(result, instance, propInfo, prm);

                if (realType.IsValueType)
                {
                    AddChildren();
                }

                void AddChildren()
                {
                    value = propInfo.GetValue(instance);

                    try
                    {
                        if (realType.IsValueType)
                            value ??= Activator.CreateInstance(realType);
                    }
                    catch
                    {
                    }

                    if (value != null)
                    {
                        suppressIgnoreProps++;
                        try
                        {
                            var children = CreateProps(value, true);
                            result.AddChildren(children);
                        }
                        finally
                        {
                            suppressIgnoreProps--;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Creates <see cref="Font"/> property.
        /// </summary>
        /// <inheritdoc cref="CreatePropertyAsDummy"/>
        public virtual IPropertyGridItem CreatePropertyAsFont(
            string label,
            string? name,
            object instance,
            PropertyInfo propInfo)
        {
            PropertyGridAdapterFont adapter = new()
            {
                Instance = instance,
                PropInfo = propInfo,
            };
            var result = CreatePropertyWithAdapter(label, name, adapter);
            return result;
        }

        /// <summary>
        /// Create complex property with sub-properties using
        /// <see cref="PropertyGridAdapterGeneric"/> instance as child properties
        /// provider.
        /// </summary>
        /// <param name="adapter">Complex properties provider.</param>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <see cref="PropertyGridAdapterGeneric.Instance"/> or
        /// <see cref="PropertyGridAdapterGeneric.PropInfo"/> is null.
        /// </exception>
        public virtual IPropertyGridItem CreatePropertyWithAdapter(
            string label,
            string? name,
            PropertyGridAdapterGeneric adapter)
        {
            var propInfo = adapter.PropInfo!;
            var instance = adapter.Instance!;

            if (propInfo == null)
            {
                throw new ArgumentNullException(
                    nameof(adapter),
                    string.Format(ErrorMessages.Default.PropertyIsNull, "PropInfo"));
            }

            if (instance == null)
            {
                throw new ArgumentNullException(
                    nameof(adapter),
                    string.Format(ErrorMessages.Default.PropertyIsNull, "Instance"));
            }

            var value = GetStructPropertyValueForReload(null, instance, propInfo);
            var prm = ConstructNewItemParams(instance, propInfo);
            var result = CreateStringItemWithKind(label, name, value?.ToString(), prm);
            result.GetValueFuncForReload = GetStructPropertyValueForReload;
            SetPropertyReadOnly(result, true, false);
            OnPropertyCreated(result, instance, propInfo, prm);
            var list = adapter.CreateProps(this);
            result.AddChildren(list);
            return result;
        }

        /// <summary>
        /// Creates <see cref="Brush"/> property.
        /// </summary>
        /// <inheritdoc cref="CreatePropertyAsDummy"/>
        public virtual IPropertyGridItem CreatePropertyAsBrush(
            string label,
            string? name,
            object instance,
            PropertyInfo propInfo)
        {
            PropertyGridAdapterBrush adapter = new()
            {
                Instance = instance,
                PropInfo = propInfo,
            };
            var result = CreatePropertyWithAdapter(label, name, adapter);
            return result;
        }

        /// <summary>
        /// Creates <see cref="Pen"/> property.
        /// </summary>
        /// <inheritdoc cref="CreatePropertyAsDummy"/>
        public virtual IPropertyGridItem CreatePropertyAsPen(
            string label,
            string? name,
            object instance,
            PropertyInfo propInfo)
        {
            PropertyGridAdapterPen adapter = new()
            {
                Instance = instance,
                PropInfo = propInfo,
            };
            var result = CreatePropertyWithAdapter(label, name, adapter);
            return result;
        }

        /// <summary>
        /// Creates <see cref="decimal"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateDecimalItem(
            string label,
            string? name = null,
            decimal value = default,
            IPropertyGridNewItemParams? prm = null)
        {
            var result = CreateStringItemWithKind(label, name, value.ToString(), prm);
            return result;
        }

        /// <summary>
        /// Creates <see cref="sbyte"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateSByteItem(
            string label,
            string? name = null,
            sbyte value = 0,
            IPropertyGridNewItemParams? prm = null)
        {
            var result = CreateLongItem(label, name, value, prm);
            SetPropertyMinMax(result, TypeCode.SByte);
            result.PropertyEditorKind = PropertyGridEditKindAll.SByte;
            return result;
        }

        /// <summary>
        /// Sets minimal and maximal values of the numeric property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="code">Object type from which min and max values will be applied.</param>
        public virtual void SetPropertyMinMax(IPropertyGridItem prop, TypeCode code)
        {
            var min = AssemblyUtils.GetMinValue(code);
            var max = AssemblyUtils.GetMaxValue(code);
            SetPropertyMinMax(prop, min, max);
        }

        /// <summary>
        /// Sets minimal and maximal values of the numeric property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="min">Minimal property value.</param>
        /// <param name="max">Maximal property value.</param>
        /// <remarks>
        /// If <c>null</c> is passed in the <paramref name="max"/> or <paramref name="min"/>
        /// parameter, limit is not set.
        /// </remarks>
        public virtual void SetPropertyMinMax(IPropertyGridItem prop, object? min, object? max = null)
        {
            if (min is not null)
                SetPropertyKnownAttribute(prop, PropertyGridItemAttrId.Min, min);
            if (max is not null)
                SetPropertyKnownAttribute(prop, PropertyGridItemAttrId.Max, max);
        }

        /// <summary>
        /// Creates <see cref="short"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateInt16Item(
            string label,
            string? name = null,
            short value = 0,
            IPropertyGridNewItemParams? prm = null)
        {
            var result = CreateLongItem(label, name, value, prm);
            SetPropertyMinMax(result, TypeCode.Int16);
            result.PropertyEditorKind = PropertyGridEditKindAll.Int16;
            return result;
        }

        /// <summary>
        /// Creates <see cref="int"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateIntItem(
            string label,
            string? name = null,
            int value = 0,
            IPropertyGridNewItemParams? prm = null)
        {
            var result = CreateLongItem(label, name, value, prm);
            SetPropertyMinMax(result, TypeCode.Int32);
            result.PropertyEditorKind = PropertyGridEditKindAll.Int32;
            return result;
        }

        /// <summary>
        /// Creates <see cref="byte"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateByteItem(
            string label,
            string? name = null,
            byte value = 0,
            IPropertyGridNewItemParams? prm = null)
        {
            var result = CreateULongItem(label, name, value, prm);
            SetPropertyMinMax(result, TypeCode.Byte);
            result.PropertyEditorKind = PropertyGridEditKindAll.Byte;
            return result;
        }

        /// <summary>
        /// Creates <see cref="uint"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateUIntItem(
            string label,
            string? name = null,
            uint value = 0,
            IPropertyGridNewItemParams? prm = null)
        {
            var result = CreateULongItem(label, name, value, prm);
            SetPropertyMinMax(result, TypeCode.UInt32);
            result.PropertyEditorKind = PropertyGridEditKindAll.UInt32;
            return result;
        }

        /// <summary>
        /// Creates <see cref="ushort"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateUInt16Item(
            string label,
            string? name = null,
            ushort value = 0,
            IPropertyGridNewItemParams? prm = null)
        {
            var result = CreateULongItem(label, name, value, prm);
            SetPropertyMinMax(result, TypeCode.UInt16);
            result.PropertyEditorKind = PropertyGridEditKindAll.UInt16;
            return result;
        }

        /// <summary>
        /// Creates property using another object's property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name in <see cref="PropertyGrid"/>.</param>
        /// <param name="instance">Object instance which contains the property.</param>
        /// <param name="nameInInstance">Property name in <paramref name="instance"/>.</param>
        /// <returns></returns>
        public virtual IPropertyGridItem? CreateProperty(
            string? label,
            string? name,
            object instance,
            string nameInInstance)
        {
            if (instance == null)
                return null;
            var type = instance.GetType();
            var propInfo = AssemblyUtils.GetPropertySafe(type, nameInInstance);
            if (propInfo == null)
                return null;
            return CreateProperty(label, name, instance, propInfo);
        }

        /// <summary>
        /// Creates property using another object's property.
        /// </summary>
        /// <param name="instance">Object instance which contains the property.</param>
        /// <param name="nameInInstance">Property name in <paramref name="instance"/>.</param>
        /// <returns></returns>
        public virtual IPropertyGridItem? CreateProperty(object instance, string nameInInstance)
        {
            return CreateProperty(null, null, instance, nameInInstance);
        }

        /// <summary>
        /// Creates property using another object's property.
        /// </summary>
        /// <param name="instance">Object instance which contains the property.</param>
        /// <param name="p">Property information.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem? CreateProperty(object instance, PropertyInfo p)
        {
            return CreateProperty(null, null, instance, p);
        }

        /// <summary>
        /// Creates property using another object's property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="propName">Property name in <see cref="PropertyGrid"/>.</param>
        /// <param name="instance">Object instance which contains the property.</param>
        /// <param name="p">Property information.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem? CreateProperty(
            string? label,
            string? propName,
            object instance,
            PropertyInfo p)
        {
            propName ??= p.Name;
            label ??= propName;

            if (!p.CanRead)
                return null;
            ParameterInfo[] paramInfo = p.GetIndexParameters();
            if (paramInfo.Length > 0)
                return null;
            if (!AssemblyUtils.GetBrowsable(p))
                return null;

            if (PropertyCustomCreate is not null)
            {
                var args = new CreatePropertyEventArgs(label, propName, instance, p);
                PropertyCustomCreate(this, args);
                if (args.Handled)
                {
                    return args.PropertyItem;
                }
                else
                {
                    if (args.PropName is not null)
                        propName = args.PropName;
                    if (args.Label is not null)
                        label = args.Label;
                    p = args.PropInfo;
                    instance = args.Instance;
                }
            }

            var propType = p.PropertyType;
            IPropertyGridItem? prop = null;
            var realType = AssemblyUtils.GetRealType(propType);
            TypeCode typeCode = Type.GetTypeCode(realType);

            if (realType.IsEnum)
                prop = CreatePropertyAsEnum(label, propName, instance, p);
            else
            {
                switch (typeCode)
                {
                    case TypeCode.Empty:
                    case TypeCode.DBNull:
                        return null;
                    case TypeCode.Object:
                        prop = CreatePropertyAsStruct(label, propName, instance, p);
                        break;
                    case TypeCode.Boolean:
                        prop = CreatePropertyAsBool(label, propName, instance, p);
                        break;
                    case TypeCode.SByte:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.Byte:
                    case TypeCode.UInt32:
                    case TypeCode.UInt16:
                    case TypeCode.UInt64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                    case TypeCode.DateTime:
                    case TypeCode.Char:
                    case TypeCode.String:
                        prop = CreatePropertyAsString(label, propName, instance, p);
                        break;
                }
            }

            if(prop is not null)
            {
                prop.Instance = instance;
                prop.PropInfo = p;
            }

            return prop;
        }

        /// <summary>
        /// Creates <see cref="bool"/> property.
        /// </summary>
        /// <inheritdoc cref="CreatePropertyAsDummy"/>
        public virtual IPropertyGridItem CreatePropertyAsBool(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo)
        {
            var value = (bool)AssemblyUtils.GetPropValue(instance, propInfo, false);
            var prm = ConstructNewItemParams(instance, propInfo);
            var prop = CreateBoolItem(label, name, value, prm);
            OnPropertyCreated(prop, instance, propInfo, prm);
            return prop;
        }

        /// <summary>
        /// Converts property value to string using the specified or default
        /// <see cref="TypeConverter"/>.
        /// </summary>
        /// <param name="instance">Object instance.</param>
        /// <param name="propInfo">Property info.</param>
        /// <param name="typeConverter">Type converter. Optional.</param>
        /// <returns></returns>
        public virtual string PropValueToString(
            object instance,
            PropertyInfo propInfo,
            ref TypeConverter? typeConverter)
        {
            object? propValue = propInfo.GetValue(instance, null);
            string value = string.Empty;

            if (propValue is not null)
            {
                typeConverter ??=
                    StringConverters.Default.GetTypeConverter(propInfo.PropertyType);
                var tpc = typeConverter;

                var success = AvoidException(() =>
                {
                    if (tpc is not null)
                    {
                        if (tpc.CanConvertTo(typeof(string)))
                        {
                            value = tpc.ConvertToString(
                                null,
                                Culture,
                                propValue);
                        }
                        else
                        {
                            value = propValue.ToString();
                        }
                    }
                    else
                    {
                        value = propValue.ToString();
                    }
                });

                if (!success)
                {
                    AvoidException(() =>
                    {
                        value = propValue.ToString();
                    });
                }
            }

            return value;
        }

        /// <summary>
        /// Creates property with <see cref="string"/> editor.
        /// </summary>
        /// <param name="typeConverter">Type converter. Optional.</param>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="instance">Object instance which contains the property.</param>
        /// <param name="propInfo">Property information.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        /// <remarks>
        /// If <paramref name="label"/> or <paramref name="name"/> is null,
        /// <paramref name="propInfo"/> is used to get them.
        /// </remarks>
        public virtual IPropertyGridItem CreatePropertyAsString(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo,
                    TypeConverter? typeConverter = null)
        {
            var value = PropValueToString(instance, propInfo, ref typeConverter);
            var prm = ConstructNewItemParams(instance, propInfo);
            var prop = CreateStringItemWithKind(label, name, value, prm);
            OnPropertyCreated(prop, instance, propInfo, prm);
            prop.TypeConverter = typeConverter;
            return prop;
        }

        /// <summary>
        /// Creates <see cref="IPropertyGridItem"/> array from all public properties of
        /// the specified object.
        /// </summary>
        /// <param name="instance">Object instance which properties will be added.</param>
        /// <param name="sort">Optional. Equals <c>false</c> by default. If <c>true</c>,
        /// properties will be sorted ascending dy name.</param>
        public virtual IEnumerable<IPropertyGridItem> CreateProps(object instance, bool sort = false)
        {
            List<IPropertyGridItem> result = new();
            Type myType = instance.GetType();
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;

            IList<PropertyInfo> props =
                new List<PropertyInfo>(myType.GetProperties(bindingFlags));

            SortedList<string, PropertyInfo> addedNames = new();

            foreach (PropertyInfo p in props)
            {
                var propName = p.Name;
                if (addedNames.ContainsKey(propName))
                    continue;
                if (suppressIgnoreProps == 0 && ignorePropNames.Contains(propName))
                    continue;
                IPropertyGridItem? prop = CreateProperty(instance, p);
                if (prop == null)
                    continue;
                result.Add(prop);
                addedNames.Add(propName, p);
            }

            if (sort)
                result.Sort(PropertyGridItem.CompareByLabel);
            return result;
        }

        /// <summary>
        /// Adds all public properties of the specified object.
        /// </summary>
        /// <param name="instance">Object instance which properties will be added.</param>
        /// <param name="parent">Optional. Parent item to which properties are added.</param>
        /// <param name="sort">Optional. Equals <c>false</c> by default. If <c>true</c>,
        /// properties will be sorted ascending dy name.</param>
        /// <remarks>
        /// If <paramref name="parent"/> is <c>null</c> (default) properties are
        /// added on the root level.
        /// </remarks>
        public virtual void AddProps(
            object? instance,
            IPropertyGridItem? parent = null,
            bool sort = false)
        {
            if (instance == null)
                return;
            var props = CreateProps(instance, sort);
            foreach (var item in props)
            {
                Add(item, parent);
            }
        }

        /// <summary>
        /// Clears properties and calls <see cref="AddProps"/> afterwards.
        /// </summary>
        /// <param name="instance">Object instance which properties will be added.</param>
        /// <param name="sort">Optional. Equals <c>false</c> by default. If <c>true</c>,
        /// properties will be sorted ascending dy name.</param>
        public virtual void SetProps(object? instance, bool sort = false)
        {
            BeginUpdate();
            try
            {
                Clear();
                AddProps(instance, null, sort);
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <inheritdoc/>
        public override int EndUpdate()
        {
            var result = base.EndUpdate();
            Invalidate();
            return result;
        }

        /// <summary>
        /// Creates <see cref="Color"/> property.
        /// </summary>
        /// <inheritdoc cref="CreatePropertyAsDummy"/>
        public virtual IPropertyGridItem CreatePropertyAsColor(
            string? label,
            string? name,
            object instance,
            PropertyInfo propInfo)
        {
            IPropertyGridItem prop;
            string propName = propInfo.Name;
            label ??= propName;
            object? propValue = propInfo.GetValue(instance, null);
            propValue ??= Color.Empty;
            var prm = ConstructNewItemParams(instance, propInfo);
            var value = (Color)propValue;
            prop = CreateColorItemWithKind(label, name, value, prm);
            OnPropertyCreated(prop, instance, propInfo, prm);
            return prop;
        }

        /// <summary>
        /// Creates enumeration property.
        /// </summary>
        /// <inheritdoc cref="CreatePropertyAsDummy"/>
        public virtual IPropertyGridItem CreatePropertyAsEnum(
            string? label,
            string? name,
            object instance,
            PropertyInfo propInfo)
        {
            IPropertyGridItem prop;
            var propType = propInfo.PropertyType;
            string propName = propInfo.Name;
            label ??= propName;
            object? propValue = propInfo.GetValue(instance, null);
            var realType = AssemblyUtils.GetRealType(propType);
            var prm = ConstructNewItemParams(instance, propInfo);
            var choices = prm.Choices;
            bool isFlags;
            if (prm.EnumIsFlags is null)
                isFlags = AssemblyUtils.EnumIsFlags(realType);
            else
                isFlags = prm.EnumIsFlags.Value;

            choices ??= CreateChoicesOnce(realType);
            bool isNullable = AssemblyUtils.GetNullable(propInfo);
            propValue ??= 0;
            if (isFlags)
            {
                prop = CreateFlagsItem(
                    label,
                    name,
                    choices,
                    propValue!,
                    prm);
            }
            else
            {
                if (isNullable)
                    choices = choices.NullableChoices;
                prop = CreateChoicesItem(
                    label,
                    name,
                    choices,
                    propValue!,
                    prm);
            }

            OnPropertyCreated(prop, instance, propInfo, prm);
            return prop;
        }

        /// <summary>
        /// Adds items to the property grid.
        /// </summary>
        /// <param name="props">Items to add.</param>
        /// <param name="parent">Parent item or null.</param>
        public virtual void AddRange(
            IEnumerable<IPropertyGridItem> props,
            IPropertyGridItem? parent = null)
        {
            foreach (var prop in props)
                Add(prop, parent);
        }

        /// <summary>
        /// Creates and adds properties category.
        /// </summary>
        /// <remarks>
        /// Same as <see cref="CreatePropCategory"/> but additionally calls <see cref="Add"/>.
        /// </remarks>
        /// <param name="label">Category label.</param>
        /// <param name="name">Category name.</param>
        /// <param name="prm">Item create params.</param>
        public virtual IPropertyGridItem AddPropCategory(
            string label,
            string? name = null,
            IPropertyGridNewItemParams? prm = null)
        {
            var result = CreatePropCategory(label, name, prm);
            Add(result);
            return result;
        }

        /// <summary>
        /// Sets editor control of a property using its known name.
        /// </summary>
        /// <param name="prop">Property Item.</param>
        /// <param name="editorName">Known name of the editor.</param>
        public virtual void SetPropertyEditorByKnownName(
            IPropertyGridItem prop,
            PropertyGridKnownEditors editorName)
        {
            SetPropertyEditorByName(prop, editorName.ToString());
        }

        /// <summary>
        /// Sets all <see cref="PropertyGrid"/> colors.
        /// </summary>
        /// <param name="colors">New known color settings.</param>
        public virtual void ApplyKnownColors(PropertyGridKnownColors colors)
        {
            if (colors == PropertyGridKnownColors.Default)
                ApplyColors();
            else
                ApplyColors(PropertyGridColors.CreateColors(colors));
        }

        /// <summary>
        /// Changes lines color to the cell background color;
        /// </summary>
        public virtual void BackgroundToLineColor()
        {
            LineColor = CellBackgroundColor;
        }

        /// <summary>
        /// Creates <see cref="IPropertyGridColors"/> with current colors
        /// of the <see cref="PropertyGrid"/>.
        /// </summary>
        public virtual IPropertyGridColors GetCurrentColors()
        {
            var result = new PropertyGridColors
            {
                CaptionBackgroundColor = this.CaptionBackgroundColor,
                CaptionForegroundColor = this.CaptionForegroundColor,
                CellBackgroundColor = this.CellBackgroundColor,
                CellDisabledTextColor = this.CellDisabledTextColor,
                CellTextColor = this.CellTextColor,
                EmptySpaceColor = this.EmptySpaceColor,
                LineColor = this.LineColor,
                MarginColor = this.MarginColor,
                SelectionBackgroundColor = this.SelectionBackgroundColor,
                SelectionForegroundColor = this.SelectionForegroundColor,
            };

            return result;
        }

        /// <summary>
        /// Sets all <see cref="PropertyGrid"/> colors.
        /// </summary>
        /// <param name="colors">New color settings.</param>
        public virtual void ApplyColors(IPropertyGridColors? colors = null)
        {
            if (colors == null)
            {
                ResetColors();
                return;
            }

            BeginUpdate();
            try
            {
                if (colors.ResetColors)
                    ResetColors();

                if (colors.CaptionBackgroundColor is not null)
                    CaptionBackgroundColor = colors.CaptionBackgroundColor;
                if (colors.CaptionForegroundColor is not null)
                    CaptionForegroundColor = colors.CaptionForegroundColor;
                if (colors.CellDisabledTextColor is not null)
                    CellDisabledTextColor = colors.CellDisabledTextColor;
                if (colors.EmptySpaceColor is not null)
                    EmptySpaceColor = colors.EmptySpaceColor;
                if (colors.LineColor is not null)
                    LineColor = colors.LineColor;
                if (colors.MarginColor is not null)
                    MarginColor = colors.MarginColor;
                if (colors.SelectionBackgroundColor is not null)
                    SelectionBackgroundColor = colors.SelectionBackgroundColor;
                if (colors.SelectionForegroundColor is not null)
                    SelectionForegroundColor = colors.SelectionForegroundColor;
                if (colors.CellTextColor is not null)
                    CellTextColor = colors.CellTextColor;
                if (colors.CellBackgroundColor is not null)
                    CellBackgroundColor = colors.CellBackgroundColor;
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <summary>
        /// Sets known attribute for the property.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="attrName">Attribute identified.</param>
        /// <param name="value">Value of attribute.</param>
        /// <param name="argFlags">Optional. Use
        /// <see cref="PropertyGridItemValueFlags.Recurse"/> to set the attribute to child
        /// properties recursively.</param>
        /// <remarks>See <see cref="SetPropertyAttribute"/> for the details.</remarks>
        public virtual void SetPropertyKnownAttribute(
            IPropertyGridItem prop,
            PropertyGridItemAttrId attrName,
            object? value,
            PropertyGridItemValueFlags argFlags = 0)
        {
            SetPropertyAttribute(prop, attrName.ToString(), value, argFlags);
        }

        /// <summary>
        /// Gets <see cref="IPropertyGridItem"/> added to the control filtered by
        /// <see cref="IPropInfoAndInstance.Instance"/> (<paramref name="instance"/> param) and
        /// <see cref="IPropInfoAndInstance.PropInfo"/> (<paramref name="propInfo"/> param).
        /// </summary>
        /// <param name="instance">Instance filter parameter. Ignored if <c>null</c>.</param>
        /// <param name="propInfo">Property information filter parameter.
        /// Ignored if <c>null</c></param>
        public virtual IEnumerable<IPropertyGridItem> GetItemsFiltered(
            object? instance = null,
            PropertyInfo? propInfo = null)
        {
            var allItems = Items;
            if (instance == null && propInfo == null)
                return allItems;

            List<IPropertyGridItem> result = new();

            foreach (var item in allItems)
            {
                var instanceOk = instance == null || instance == item.Instance;
                var propInfoOk = propInfo == null || propInfo == item.PropInfo;

                if (instanceOk && propInfoOk)
                    result.Add(item);
            }

            return result;
        }

        /// <summary>
        /// Called after <see cref="IPropertyGridItem"/> created for the
        /// specified <paramref name="instance"/> and <paramref name="propInfo"/>.
        /// </summary>
        /// <param name="item">Property item.</param>
        /// <param name="instance">Instance that contains the property.</param>
        /// <param name="propInfo">Property info.</param>
        /// <param name="prm">Property item create parameters.</param>
        public virtual void OnPropertyCreated(
            IPropertyGridItem item,
            object instance,
            PropertyInfo propInfo,
            IPropertyGridNewItemParams? prm)
        {
            item.Instance = instance;
            item.PropInfo = propInfo;
            if (!propInfo.CanWrite)
            {
                if (prm is not null)
                {
                    if (prm.OnlyTextReadOnly is true)
                        SetPropertyFlag(item, PropertyGridItemFlags.NoEditor, true);
                    else
                        SetPropertyReadOnly(item, true);
                }
                else
                    SetPropertyReadOnly(item, true);
            }

            if (!item.IsFlags && AssemblyUtils.GetNullable(propInfo))
            {
                var value = propInfo.GetValue(instance);
                if (value is null)
                    SetPropertyValueUnspecified(item);
            }
        }

        /// <summary>
        /// Reloads values of all <see cref="IPropertyGridItem"/> items collected with
        /// <see cref="GetItemsFiltered"/>.
        /// </summary>
        /// <param name="instance">Instance filter parameter. Ignored if <c>null</c>.</param>
        /// <param name="propInfo">Property information filter parameter.
        /// Ignored if <c>null</c></param>
        public virtual bool ReloadPropertyValues(
            object? instance = null,
            PropertyInfo? propInfo = null)
        {
            return AvoidException(() =>
            {
                var filteredItems = GetItemsFiltered(instance, propInfo);
                if (filteredItems.First() == null)
                    return;
                BeginUpdate();
                try
                {
                    foreach (var item in filteredItems)
                    {
                        ReloadPropertyValue(item);
                    }
                }
                finally
                {
                    EndUpdate();
                }
            });
        }

        /// <summary>
        /// Gets all added property categories.
        /// </summary>
        public virtual IEnumerable<IPropertyGridItem> GetCategories()
        {
            List<IPropertyGridItem> result = new();
            foreach (var item in Items)
            {
                if (item.IsCategory)
                    result.Add(item);
            }

            return result;
        }

        /// <summary>
        /// Converts <see cref="Color"/> to <see cref="string"/> using
        /// <see cref="ColorHasAlpha"/>, <see cref="DefaultColorFormatRGBA"/>,
        /// <see cref="DefaultColorFormatRGB"/> properties
        /// </summary>
        /// <param name="color">Color to convert.</param>
        /// <returns></returns>
        public virtual string ColorToString(Color? color)
        {
            string result;

            if (color is null || !color.IsOk)
                result = string.Empty;
            else
            {
                if (color.IsNamedColor)
                    result = color.Name;
                else
                if (ColorHasAlpha)
                {
                    result = string.Format(
                        DefaultColorFormatRGBA,
                        color.R,
                        color.G,
                        color.B,
                        color.A);
                }
                else
                {
                    result = string.Format(
                        DefaultColorFormatRGB,
                        color.R,
                        color.G,
                        color.B);
                }
            }

            return result;
        }

        /// <summary>
        /// Executes specified action for all property items in <paramref name="props"/> collection.
        /// </summary>
        /// <typeparam name="T">Type of the action parameter.</typeparam>
        /// <param name="props">Collection of the properties.</param>
        /// <param name="action">Action to execute.</param>
        /// <param name="prmValue">Value of the first parameter.</param>
        /// <param name="suspendUpdate">if <c>true</c>, updates will be suspended.</param>
        public virtual void DoActionOnProperties<T>(
            IEnumerable<IPropertyGridItem> props,
            Action<IPropertyGridItem, T> action,
            T prmValue,
            bool suspendUpdate = true)
        {
            if (suspendUpdate) BeginUpdate();
            try
            {
                foreach (var item in props)
                {
                    action(item, prmValue);
                }
            }
            finally
            {
                if (suspendUpdate) EndUpdate();
            }
        }

        /// <summary>
        /// Initializes control with suggested defaults.
        /// </summary>
        /// <remarks>
        /// This method is used in our demos and other applications in order to
        /// override default control settings to make it more usable and compatible.
        /// You can study it to get the idea for custom default initialization.
        /// </remarks>
        public virtual void SuggestedInitDefaults()
        {
            SetVerticalSpacing();

            // This call makes property editing better if scrollbar is shown.
            // We add an empty column on the right.
            if (App.IsLinuxOS)
                SetColumnCount(3);
            CenterSplitter();
        }

        /// <summary>
        /// Raises <see cref="PropertySelected"/> event and <see cref="OnPropertySelected"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaisePropertySelected(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnPropertySelected(e);
            PropertySelected?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="PropertyChanged"/> event and <see cref="OnPropertyChanged"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaisePropertyChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnPropertyChanged(e);
            PropertyChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="PropertyChanging"/> event and <see cref="OnPropertyChanging"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaisePropertyChanging(CancelEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnPropertyChanging(e);
            PropertyChanging?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="PropertyHighlighted"/> event and
        /// <see cref="OnPropertyHighlighted"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaisePropertyHighlighted(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnPropertyHighlighted(e);
            PropertyHighlighted?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="PropertyRightClick"/> event and <see cref="OnPropertyRightClick"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaisePropertyRightClick(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnPropertyRightClick(e);
            PropertyRightClick?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="PropertyDoubleClick"/> event
        /// and <see cref="OnPropertyDoubleClick"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaisePropertyDoubleClick(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnPropertyDoubleClick(e);
            PropertyDoubleClick?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="ItemCollapsed"/> event and <see cref="OnItemCollapsed"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaiseItemCollapsed(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnItemCollapsed(e);
            ItemCollapsed?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="ItemExpanded"/> event and <see cref="OnItemExpanded"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaiseItemExpanded(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnItemExpanded(e);
            ItemExpanded?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="ColEndDrag"/> event and <see cref="OnColEndDrag"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaiseColEndDrag(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnColEndDrag(e);
            ColEndDrag?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="ButtonClick"/> event and <see cref="OnButtonClick"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaiseButtonClick(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnButtonClick(e);
            ButtonClick?.Invoke(this, e);

            var prop = EventProperty;
            if (prop == null)
                return;
            AvoidException(() =>
            {
                prop.RaiseButtonClick();
            });

            prop = EventProperty;
            var prm = prop?.Params;
            if (prm == null)
                return;
            AvoidException(() =>
            {
                prm.RaiseButtonClick(prop!);
            });
        }

        /// <summary>
        /// Raises <see cref="ColDragging"/> event and <see cref="OnColDragging"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaiseColDragging(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnColDragging(e);
            ColDragging?.Invoke(this, e);
        }

        /// <summary>
        /// Executes specified action for all property items in <paramref name="props"/> collection.
        /// </summary>
        /// <typeparam name="T1">Type of the first action parameter.</typeparam>
        /// <typeparam name="T2">Type of the second action parameter.</typeparam>
        /// <param name="props">Collection of the properties.</param>
        /// <param name="action">Action to execute.</param>
        /// <param name="prmValue1">Value of the first parameter.</param>
        /// <param name="prmValue2">Value of the second parameter.</param>
        /// <param name="suspendUpdate">if <c>true</c>, updates will be suspended.</param>
        public virtual void DoActionOnProperties<T1, T2>(
            IEnumerable<IPropertyGridItem> props,
            Action<IPropertyGridItem, T1, T2> action,
            T1 prmValue1,
            T2 prmValue2,
            bool suspendUpdate = true)
        {
            if (suspendUpdate) BeginUpdate();
            try
            {
                foreach (var item in props)
                {
                    action(item, prmValue1, prmValue2);
                }
            }
            finally
            {
                if (suspendUpdate) EndUpdate();
            }
        }

        /// <summary>
        /// Sets background color for all added property categories.
        /// </summary>
        /// <param name="color">Color value.</param>
        public virtual void SetCategoriesBackgroundColor(Color color)
        {
            SetPropertiesBackgroundColor(GetCategories(), color);
        }

        /// <summary>
        /// Sets background color for all properties in <paramref name="items"/> collection.
        /// </summary>
        /// <param name="color">Color value.</param>
        /// <param name="items">Collection of the property items.</param>
        public virtual void SetPropertiesBackgroundColor(
            IEnumerable<IPropertyGridItem> items,
            Color color)
        {
            DoActionOnProperties<Color, bool>(items, SetPropertyBackgroundColor, color, false);
        }

        /// <summary>
        /// Gets <see cref="IPropertyGridItem"/> from the item handler.
        /// </summary>
        /// <param name="ptr">Item handle.</param>
        /// <returns></returns>
        public virtual IPropertyGridItem? HandleToItem(PropertyGridItemHandle ptr)
        {
            if (items.TryGetValue(ptr, out IPropertyGridItem? result))
                return result;
            return null;
        }

        /// <summary>
        /// Raises <see cref="ColBeginDrag"/> event and <see cref="OnColBeginDrag"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaiseColBeginDrag(CancelEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnColBeginDrag(e);
            ColBeginDrag?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="LabelEditEnding"/> event and <see cref="OnLabelEditEnding"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaiseLabelEditEnding(CancelEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnLabelEditEnding(e);
            LabelEditEnding?.Invoke(this, e);
        }

        /// <summary>
        /// Sets property value as variant.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetPropertyValueAsVariant(
            IPropertyGridItem prop,
            IPropertyGridVariant value)
        {
            if (DisposingOrDisposed)
                return;
            Handler.SetPropertyValueAsVariant(prop, value);
        }

        /// <summary>
        /// Sets attached property value using
        /// <see cref="IPropInfoAndInstance"/> specified in <paramref name="prop"/> parameter.
        /// </summary>
        /// <param name="variant">Value to set.</param>
        /// <param name="prop">Item which contains attached object and property information.</param>
        /// <returns></returns>
        public virtual bool SetAttachedPropertyValue(
            IPropertyGridItem prop,
            IPropertyGridVariant variant)
        {
            if (DisposingOrDisposed)
                return false;

            var propInfo = prop.PropInfo;
            var instance = prop.Instance;

            if (instance == null || propInfo == null || !propInfo.CanWrite)
                return false;

            var success = AvoidException(() =>
            {
                var newValue = variant.GetCompatibleValue(prop);
                propInfo.SetValue(instance, newValue);
                UpdateStruct();
            });

            return success;

            void UpdateStruct()
            {
                var parent = prop.Parent;
                if (parent == null)
                    return;
                var parentInstance = parent.Instance;
                var parentPropInfo = parent.PropInfo;
                var parentIsStruct = AssemblyUtils.IsStruct(parentPropInfo?.PropertyType);
                if (!parentIsStruct)
                    return;
                parentPropInfo?.SetValue(parentInstance, instance);
            }
        }

        /// <summary>
        /// Raises <see cref="LabelEditBegin"/> event and <see cref="OnLabelEditBegin"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaiseLabelEditBegin(CancelEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnLabelEditBegin(e);
            LabelEditBegin?.Invoke(this, e);
        }

        /// <summary>
        /// Called after <see cref="IPropertyGridItem"/> created.
        /// </summary>
        /// <param name="item">Property item.</param>
        /// <param name="prm">Property item create parameters.</param>
        protected virtual void OnPropertyCreated(
            IPropertyGridItem item,
            IPropertyGridNewItemParams? prm)
        {
            if (prm is null)
                return;

            if (prm.TextReadOnly is not null && prm.TextReadOnly.Value)
                SetPropertyFlag(item, PropertyGridItemFlags.NoEditor, true);

            EnableEllipsis();

            void EnableEllipsis()
            {
                if (!item.CanHaveCustomEllipsis)
                    return;

                if (prm.HasEllipsis is null)
                    return;

                if (prm.HasEllipsis.Value)
                {
                    SetPropertyEditorByName(item, PropEditClassTextCtrlAndButton);
                }
            }
        }

        /// <summary>
        /// Called when user is about to begin editing a property label.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnLabelEditBegin(CancelEventArgs e)
        {
        }

        /// <summary>
        /// Called when user is about to end editing of a property label.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnLabelEditEnding(CancelEventArgs e)
        {
        }

        /// <summary>
        /// Called when user starts resizing a column.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnColBeginDrag(CancelEventArgs e)
        {
        }

        /// <summary>
        /// Called when a column resize by the user is in progress.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnColDragging(EventArgs e)
        {
        }

        /// <summary>
        /// Called after column resize by the user has finished.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnColEndDrag(EventArgs e)
        {
        }

        /// <summary>
        /// Called when button is clicked in the property editor and it is not processed by default.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnButtonClick(EventArgs e)
        {
        }

        /// <summary>
        /// Called when user expands a property or category.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnItemExpanded(EventArgs e)
        {
        }

        /// <summary>
        /// Called when user collapses a property or category.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnItemCollapsed(EventArgs e)
        {
        }

        /// <summary>
        /// Called when property is double-clicked with left mouse button.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnPropertyDoubleClick(EventArgs e)
        {
        }

        /// <summary>
        /// Called when property is clicked with right mouse button.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnPropertyRightClick(EventArgs e)
        {
        }

        /// <summary>
        /// Called when mouse moves over a property.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnPropertyHighlighted(EventArgs e)
        {
        }

        /// <summary>
        /// Called when property value is about to be changed by the user.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnPropertyChanging(CancelEventArgs e)
        {
        }

        /// <summary>
        /// Called when property value has been changed by the user.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnPropertyChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            var setValue = ApplyFlags.HasFlag(PropertyGridApplyFlags.PropInfoSetValue);
            if (!setValue)
                return;

            var prop = EventProperty;
            if (prop == null)
                return;

            var variant = EventPropValueAsVariant;
            var success = SetAttachedPropertyValue(prop, variant);

            if (success)
            {
                var reload = ApplyFlags.HasFlag(PropertyGridApplyFlags.ReloadAfterSetValue);
                var reloadAll = ApplyFlags.HasFlag(PropertyGridApplyFlags.ReloadAllAfterSetValue);

                if (reloadAll)
                    ReloadPropertyValues();
                else
                if (reload)
                    ReloadPropertyValues(prop.Instance, prop.PropInfo);
            }

            var propEvent = ApplyFlags.HasFlag(PropertyGridApplyFlags.PropEvent);
            if (propEvent)
            {
                AvoidException(() => { prop.RaisePropertyChanged(); });
            }
        }

        /// <summary>
        /// Called when a property selection has been changed, either by user action
        /// or by indirect program function.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnPropertySelected(EventArgs e)
        {
        }

        private static IPropertyGridItem FuncCreatePropertyAsColor(
            IPropertyGrid sender,
            string label,
            string? name,
            object instance,
            PropertyInfo propInfo)
        {
            return sender.CreatePropertyAsColor(label, name, instance, propInfo);
        }

        private static IPropertyGridItem FuncCreatePropertyAsFont(
            IPropertyGrid sender,
            string label,
            string? name,
            object instance,
            PropertyInfo propInfo)
        {
            return sender.CreatePropertyAsFont(label, name, instance, propInfo);
        }

        private static IPropertyGridItem FuncCreatePropertyAsBrush(
            IPropertyGrid sender,
            string label,
            string? name,
            object instance,
            PropertyInfo propInfo)
        {
            return sender.CreatePropertyAsBrush(label, name, instance, propInfo);
        }

        private static IPropertyGridItem FuncCreatePropertyAsPen(
            IPropertyGrid sender,
            string label,
            string? name,
            object instance,
            PropertyInfo propInfo)
        {
            return sender.CreatePropertyAsPen(label, name, instance, propInfo);
        }

        private string CorrectPropLabel(string label, IPropertyGridNewItemParams? prm)
        {
            string Fn(string s)
            {
                if (prm != null)
                {
                    if (Features.HasFlag(PropertyGridFeature.QuestionCharInNullable))
                    {
                        var addQuestion = (prm.IsNullable is not null) && prm.IsNullable.Value;
                        if (!addQuestion)
                        {
                            addQuestion = (prm.PropInfo is not null)
                                && AssemblyUtils.GetNullable(prm.PropInfo);
                        }

                        if (addQuestion)
                            return s + "?";
                    }
                }

                return s;
            }

            string? customLabel = prm?.Label;
            if (customLabel == null)
                return Fn(label);
            else
                return Fn(customLabel);
        }

        private string CorrectPropName(string? name)
        {
            if (name is null)
                return GetPropNameAsLabel();
            return name;
        }

        private object? GetStructPropertyValueForReload(
            IPropertyGridItem? item,
            object instance,
            PropertyInfo propInfo)
        {
            var asString = propInfo.GetValue(instance)?.ToString();
            return asString;
        }

        private object? GetRealInstance(IPropertyGridItem item)
        {
            var parent = item.Parent;
            var propInfo = item.PropInfo;
            var instance = item.Instance;

            if (parent == null || propInfo == null)
                return instance;

            var isAdapter = instance is PropertyGridAdapterGeneric;
            if (isAdapter)
                return instance;

            var isStruct = (instance != null) && AssemblyUtils.IsStruct(instance.GetType());

            if (isStruct)
            {
                var parentInstance = GetRealInstance(parent);

                if (parentInstance == null || parent.PropInfo == null)
                    return instance;

                var result = parent.PropInfo.GetValue(parentInstance);
                return result;
            }
            else
                return instance;
        }

        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="instance">Object instance which contains the property.</param>
        /// <param name="propInfo">Property information.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        /// <remarks>
        /// If <paramref name="label"/> or <paramref name="name"/> is null,
        /// <paramref name="propInfo"/> is used to get them.
        /// </remarks>
#pragma warning disable
        private IPropertyGridItem CreatePropertyAsDummy(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo)
        {
            return null;
        }
#pragma warning restore
    }
}