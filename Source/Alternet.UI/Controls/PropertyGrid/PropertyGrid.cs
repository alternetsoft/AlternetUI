using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /*
    - multibuttons in prop editor   https://docs.wxwidgets.org/3.2/classwx_p_g_multi_button.html
    - How to hide lines? (set their color to bk color)
    - propgrid MakeAsPanel() also color scheme
    - PropertyGridItem Dispose item? when?
    - Time, DateTime
     */

    /// <summary>
    /// Specialized grid for editing properties - in other words name = value pairs.
    /// </summary>
    /// <remarks>
    /// List of ready-to-use property classes include strings, numbers, flag sets, fonts,
    /// colors and many others. It is possible, for example, to categorize properties,
    /// set up a complete tree-hierarchy, add more than two columns, and set
    /// arbitrary per-property attributes.
    /// </remarks>
    public class PropertyGrid : Control
    {
        internal const string PropEditClassNameCheckBox = "CheckBox";
        internal const string PropEditClassNameChoice = "Choice";
        internal const string PropEditClassNameTextCtrl = "TextCtrl";
        internal const string PropEditClassNameChoiceAndButton = "ChoiceAndButton";
        internal const string PropEditClassNameComboBox = "ComboBox";
        internal const string PropEditClassNameSpinCtrl = "SpinCtrl";
        internal const string PropEditClassNameTextCtrlAndButton = "TextCtrlAndButton";
        internal static readonly string NameAsLabel = Native.PropertyGrid.NameAsLabel;
        private const int PGDONTRECURSE = 0x00000000;
        private const int PGRECURSE = 0x00000020;
        private const int PGSORTTOPLEVELONLY = 0x00000200;
        private static AdvDictionary<Type, IPropertyGridChoices>? choicesCache = null;

        private readonly AdvDictionary<IntPtr, IPropertyGridItem> items = new();
        private readonly PropertyGridVariant variant = new();
        private readonly HashSet<string> ignorePropNames = new();

        static PropertyGrid()
        {
        }

        /// <summary>
        /// Occurs when exception is raised.
        /// </summary>
        public event EventHandler<PropertyGridExceptionEventArgs>? ProcessException;

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
        public event EventHandler? PropertyChanged;

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
        /// Defines default style for the newly created
        /// <see cref="PropertyGrid"/> controls.
        /// </summary>
        public static PropertyGridCreateStyle DefaultCreateStyle { get; set; }
            = PropertyGridCreateStyle.DefaultStyle;

        /// <summary>
        /// Gets list of <see cref="IPropertyGridItem"/> added to this control.
        /// </summary>
        public IEnumerable<IPropertyGridItem> Items => items.Values;

        /// <summary>
        /// Contains list of property names to ignore in <see cref="AddProps"/>.
        /// </summary>
        public ICollection<string> IgnorePropNames => ignorePropNames;

        /// <summary>
        /// Gets or sets whether boolean properties will be shown as checkboxes.
        /// Default is <c>true</c>.
        /// </summary>
        public bool BoolAsCheckBox { get; set; } = true;

        /// <summary>
        /// Gets or sets whether <see cref="Color"/> properties have alpha chanel.
        /// Default is <c>true</c>.
        /// </summary>
        public bool ColorHasAlpha { get; set; } = true;

        /// <summary>
        /// Gets property value used in the event handler.
        /// </summary>
        public object? EventPropValue
        {
            get
            {
                return EventPropValueAsVariant.AsObject;
            }
        }

        /// <summary>
        /// Gets property value used in the event handler as <see cref="IPropertyGridVariant"/>.
        /// </summary>
        public IPropertyGridVariant EventPropValueAsVariant
        {
            get
            {
                IntPtr handle = NativeControl.EventPropValue;
                PropertyGridVariant propValue = new(handle);
                return propValue;
            }
        }

        /// <summary>
        /// Gets or sets flags used when property value
        /// is applied back to object instance in default <see cref="PropertyChanged"/>
        /// event handler.
        /// </summary>
        public PropertyGridApplyFlags ApplyFlags { get; set; } = PropertyGridApplyFlags.Default;

        /// <summary>
        /// Gets or sets validation failure behavior flags used in the event handler.
        /// </summary>
        [Browsable(false)]
        public PropertyGridValidationFailure EventValidationFailureBehavior
        {
            get
            {
                return (PropertyGridValidationFailure)NativeControl.EventValidationFailureBehavior;
            }

            set
            {
                NativeControl.EventValidationFailureBehavior = (int)value;
            }
        }

        /// <summary>
        /// Gets column index on which event is fired.
        /// </summary>
        /// <remarks>
        /// Use it in the event handlers.
        /// </remarks>
        [Browsable(false)]
        public int EventColumn
        {
            get
            {
                return NativeControl.EventColumn;
            }
        }

        /// <summary>
        /// Gets property used in the event handler.
        /// </summary>
        [Browsable(false)]
        public IPropertyGridItem? EventProperty
        {
            get
            {
                return PtrToItem(NativeControl.EventProperty);
            }
        }

        /// <summary>
        /// Gets property name used in the event handler.
        /// </summary>
        [Browsable(false)]
        public string EventPropName
        {
            get
            {
                return NativeControl.EventPropertyName;
            }
        }

        /// <summary>
        /// Gets or sets validation failure message used in the event handler.
        /// </summary>
        [Browsable(false)]
        public string EventValidationFailureMessage
        {
            get
            {
                return NativeControl.EventValidationFailureMessage;
            }

            set
            {
                NativeControl.EventValidationFailureMessage = value;
            }
        }

        /// <summary>
        /// Defines visual style and behavior of the <see cref="PropertyGrid"/> control.
        /// </summary>
        /// <remarks>
        /// When this property is changed, control is recreated.
        /// </remarks>
        public PropertyGridCreateStyle CreateStyle
        {
            get
            {
                return (PropertyGridCreateStyle)Handler.CreateStyle;
            }

            set
            {
                Handler.CreateStyle = (int)value;
            }
        }

        /// <summary>
        /// Defines extended style of the <see cref="PropertyGrid"/> control.
        /// </summary>
        /// <remarks>
        /// When this property is changed, control is recreated.
        /// </remarks>
        public PropertyGridCreateStyleEx CreateStyleEx
        {
            get
            {
                return (PropertyGridCreateStyleEx)Handler.CreateStyleEx;
            }

            set
            {
                Handler.CreateStyleEx = (int)value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public bool HasBorder
        {
            get
            {
                return NativeControl.HasBorder;
            }

            set
            {
                NativeControl.HasBorder = value;
            }
        }

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.PropertyGrid;

        /// <summary>
        /// Defines default extended style for the newly created
        /// <see cref="PropertyGrid"/> controls.
        /// </summary>
        internal static PropertyGridCreateStyleEx DefaultCreateStyleEx { get; set; }
            = PropertyGridCreateStyleEx.DefaultStyle;

        internal new NativePropertyGridHandler Handler
        {
            get
            {
                CheckDisposed();
                return (NativePropertyGridHandler)base.Handler;
            }
        }

        internal Native.PropertyGrid NativeControl => Handler.NativeControl;

        /// <summary>
        /// Checks system screen design used for laying out various dialogs.
        /// </summary>
        public static bool IsSmallScreen()
        {
            return Native.PropertyGrid.IsSmallScreen();
        }

        /// <summary>
        /// Creates property choices list for use with <see cref="CreateFlagsProperty"/> and
        /// <see cref="CreateChoicesProperty"/>.
        /// </summary>
        public static IPropertyGridChoices CreateChoices()
        {
            return new PropertyGridChoices();
        }

        /// <summary>
        /// Creates new variant instance for use with <see cref="PropertyGrid"/>
        /// </summary>
        public static IPropertyGridVariant CreateVariant()
        {
            return new PropertyGridVariant();
        }

        /// <summary>
        /// Creates property choices list for the given enumeration type or returns it from
        /// the internal cache if it was previously created.
        /// </summary>
        /// <remarks>
        /// Result can be used in <see cref="CreateFlagsProperty"/> and
        /// <see cref="CreateChoicesProperty"/>.
        /// </remarks>
        public static IPropertyGridChoices CreateChoicesOnce(Type enumType)
        {
            choicesCache ??= new();
            if (choicesCache.TryGetValue(enumType, out IPropertyGridChoices? result))
                return result;
            result = CreateChoices(enumType);
            choicesCache.Add(enumType, result);
            return result;
        }

        /// <summary>
        /// Creates property choices list for the given enumeration type.
        /// </summary>
        /// <remarks>
        /// Result can be used in <see cref="CreateFlagsProperty"/> and
        /// <see cref="CreateChoicesProperty"/>.
        /// </remarks>
        public static IPropertyGridChoices CreateChoices(Type enumType)
        {
            var result = CreateChoices();

            if (!enumType.IsEnum)
                return result;

            var values = Enum.GetValues(enumType);
            var names = Enum.GetNames(enumType);

            for (int i = 0; i < values.Length; i++)
            {
                var value = values.GetValue(i);
                result.Add(names[i], (int)value!);
            }

            return result;
        }

        /// <summary>
        /// Enables or disables automatic translation for enum list labels and
        /// flags child property labels.
        /// </summary>
        /// <param name="enable"><c>true</c> enables automatic translation, <c>false</c>
        /// disables it.</param>
        public static void AutoGetTranslation(bool enable)
        {
            Native.PropertyGrid.AutoGetTranslation(enable);
        }

        /// <summary>
        /// Registers all type handlers for use in <see cref="PropertyGrid"/>.
        /// </summary>
        public static void InitAllTypeHandlers()
        {
            Native.PropertyGrid.InitAllTypeHandlers();
        }

        /// <summary>
        /// Registers additional editors for use in <see cref="PropertyGrid"/>.
        /// </summary>
        public static void RegisterAdditionalEditors()
        {
            Native.PropertyGrid.RegisterAdditionalEditors();
        }

        /// <summary>
        /// Sets string constants for <c>true</c> and <c>false</c> words
        /// used in <see cref="bool"/> properties.
        /// </summary>
        /// <param name="trueChoice"></param>
        /// <param name="falseChoice"></param>
        public static void SetBoolChoices(string trueChoice, string falseChoice)
        {
            Native.PropertyGrid.SetBoolChoices(trueChoice, falseChoice);
        }

        /// <summary>
        /// Creates <see cref="string"/> property with ellipsis button which opens
        /// <see cref="FileDialog"/> when pressed.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        /// <remarks>
        /// In order to setup filename and attached <see cref="FileDialog"/>, you can use
        /// <see cref="PropertyGrid.SetPropertyKnownAttribute"/> for
        /// <see cref="PropertyGridItemAttrId.DialogTitle"/>,
        /// <see cref="PropertyGridItemAttrId.InitialPath"/>,
        /// <see cref="PropertyGridItemAttrId.ShowFullPath"/>,
        /// <see cref="PropertyGridItemAttrId.Wildcard"/> attributes.
        /// </remarks>
        public virtual IPropertyGridItem CreateFilenameProperty(
            string label,
            string? name = null,
            string? value = null)
        {
            value ??= string.Empty;
            var handle = NativeControl.CreateFilenameProperty(label, CorrectPropName(name), value!);
            var result = new PropertyGridItem(this, handle, label, name, value)
            {
                PropertyEditorKind = TypeCode.String.ToString() + ".Filename",
            };
            OnPropertyCreated(result);
            return result;
        }

        /// <summary>
        /// Creates <see cref="string"/> property with ellipsis button which opens
        /// <see cref="SelectDirectoryDialog"/> when pressed.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        /// <remarks>
        /// In order to setup folder name and attached <see cref="SelectDirectoryDialog"/>,
        /// you can use
        /// <see cref="PropertyGrid.SetPropertyKnownAttribute"/> for
        /// <see cref="PropertyGridItemAttrId.DialogTitle"/>,
        /// <see cref="PropertyGridItemAttrId.InitialPath"/>,
        /// <see cref="PropertyGridItemAttrId.ShowFullPath"/> attributes.
        /// </remarks>
        public virtual IPropertyGridItem CreateDirProperty(
            string label,
            string? name = null,
            string? value = null)
        {
            value ??= string.Empty;
            var handle = NativeControl.CreateDirProperty(label, CorrectPropName(name), value!);
            var result = new PropertyGridItem(this, handle, label, name, value)
            {
                PropertyEditorKind = TypeCode.String.ToString() + ".Dir",
            };
            OnPropertyCreated(result);
            return result;
        }

        /// <summary>
        /// Creates <see cref="string"/> property with ellipsis button which opens
        /// <see cref="FileDialog"/> when pressed.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        /// <remarks>
        /// In order to setup filename and attached <see cref="FileDialog"/>, you can use
        /// <see cref="PropertyGrid.SetPropertyKnownAttribute"/> for
        /// <see cref="PropertyGridItemAttrId.DialogTitle"/>,
        /// <see cref="PropertyGridItemAttrId.InitialPath"/>,
        /// <see cref="PropertyGridItemAttrId.ShowFullPath"/> attributes.
        /// </remarks>
        /// <remarks>
        /// This function is similar to <see cref="CreateFilenameProperty"/> but wildcards
        /// are limited to supported image file extensions.
        /// </remarks>
        public virtual IPropertyGridItem CreateImageFilenameProperty(
            string label,
            string? name = null,
            string? value = null)
        {
            value ??= string.Empty;
            var handle = NativeControl.CreateImageFilenameProperty(
                label,
                CorrectPropName(name),
                value!);
            var result = new PropertyGridItem(this, handle, label, name, value)
            {
                PropertyEditorKind = TypeCode.String.ToString() + ".ImageFilename",
            };
            OnPropertyCreated(result);
            return result;
        }

        /// <summary>
        /// Creates <see cref="Color"/> property with system colors.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateSystemColorProperty(
            string label,
            string? name,
            Color value)
        {
            var handle = NativeControl.CreateSystemColorProperty(label, CorrectPropName(name), value);
            var result = new PropertyGridItem(this, handle, label, name, value)
            {
                PropertyEditorKind = "Color.System",
            };
            OnPropertyCreated(result);
            return result;
        }

        /// <summary>
        /// Creates <see cref="string"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateStringProperty(
            string label,
            string? name = null,
            string? value = null)
        {
            value ??= string.Empty;
            var handle = NativeControl.CreateStringProperty(label, CorrectPropName(name), value!);
            var result = new PropertyGridItem(this, handle, label, name, value)
            {
                PropertyEditorKind = TypeCode.String.ToString(),
            };
            OnPropertyCreated(result);
            return result;
        }

        /// <summary>
        /// Creates <see cref="char"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateCharProperty(
            string label,
            string? name = null,
            string? value = null)
        {
            if (!string.IsNullOrEmpty(value))
                value = value![0].ToString();
            var result = CreateStringProperty(label, name, value);
            SetPropertyMaxLength(result, 1);
            return result;
        }

        /// <summary>
        /// Creates <see cref="bool"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateBoolProperty(
            string label,
            string? name = null,
            bool value = false)
        {
            var handle = NativeControl.CreateBoolProperty(label, CorrectPropName(name), value);
            var result = new PropertyGridItem(this, handle, label, name, value)
            {
                PropertyEditorKind = TypeCode.Boolean.ToString(),
            };
            OnPropertyCreated(result);
            return result;
        }

        /// <summary>
        /// Creates <see cref="long"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateLongProperty(
            string label,
            string? name = null,
            long value = 0)
        {
            var handle = NativeControl.CreateIntProperty(label, CorrectPropName(name), value);
            var result = new PropertyGridItem(this, handle, label, name, value)
            {
                PropertyEditorKind = TypeCode.Int64.ToString(),
            };
            OnPropertyCreated(result);
            return result;
        }

        /// <summary>
        /// Creates <see cref="double"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateDoubleProperty(
            string label,
            string? name = null,
            double value = default)
        {
            var handle = NativeControl.CreateFloatProperty(label, CorrectPropName(name), value);
            var result = new PropertyGridItem(this, handle, label, name, value)
            {
                PropertyEditorKind = TypeCode.Double.ToString(),
            };
            SetPropertyMinMax(result, TypeCode.Double);
            OnPropertyCreated(result);
            return result;
        }

        /// <summary>
        /// Creates <see cref="float"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateFloatProperty(
            string label,
            string? name = null,
            double value = default)
        {
            var handle = NativeControl.CreateFloatProperty(label, CorrectPropName(name), value);
            var result = new PropertyGridItem(this, handle, label, name, value)
            {
                PropertyEditorKind = TypeCode.Single.ToString(),
            };
            SetPropertyMinMax(result, TypeCode.Single);
            OnPropertyCreated(result);
            return result;
        }

        /// <summary>
        /// Creates <see cref="Color"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateColorProperty(
            string label,
            string? name,
            Color value)
        {
            var handle = NativeControl.CreateColorProperty(label, CorrectPropName(name), value);
            var result = new PropertyGridItem(this, handle, label, name, value)
            {
                PropertyEditorKind = "Color",
            };
            OnPropertyCreated(result);
            return result;
        }

        /// <summary>
        /// Creates <see cref="ulong"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateULongProperty(
            string label,
            string? name = null,
            ulong value = 0)
        {
            var handle = NativeControl.CreateUIntProperty(label, CorrectPropName(name), value);
            var result = new PropertyGridItem(this, handle, label, name, value)
            {
                PropertyEditorKind = TypeCode.UInt64.ToString(),
            };
            OnPropertyCreated(result);
            return result;
        }

        /// <summary>
        /// Creates <see cref="string"/> property with additional edit dialog for
        /// entering long values.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateLongStringProperty(
            string label,
            string? name = null,
            string? value = null)
        {
            value ??= string.Empty;
            var handle = NativeControl.CreateLongStringProperty(
                label,
                CorrectPropName(name),
                value);
            var result = new PropertyGridItem(this, handle, label, name, value)
            {
                PropertyEditorKind = TypeCode.String.ToString() + ".Long",
            };
            OnPropertyCreated(result);
            return result;
        }

        /// <summary>
        /// Creates <see cref="DateTime"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateDateProperty(
            string label,
            string? name = null,
            DateTime? value = null)
        {
            DateTime dt;

            if (value == null)
                dt = DateTime.Now;
            else
                dt = value.Value;

            var handle = NativeControl.CreateDateProperty(
                label,
                CorrectPropName(name),
                dt);
            var result = new PropertyGridItem(this, handle, label, name, value)
            {
                PropertyEditorKind = TypeCode.DateTime.ToString() + ".Date",
            };
            OnPropertyCreated(result);
            return result;
        }

        /// <summary>
        /// Creates <see cref="Font"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="instance">Object instance which contains the property.</param>
        /// <param name="p">Property information.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        /// <remarks>
        /// If <paramref name="label"/> or <paramref name="name"/> is null,
        /// <paramref name="p"/> is used to get them.
        /// </remarks>
        public virtual IPropertyGridItem CreatePropertyAsFont(
            string label,
            string? name,
            object instance,
            PropertyInfo p)
        {
            PropertyGridAdapterFont adapter = new()
            {
                Instance = instance,
                PropInfo = p,
            };
            var result = CreateStringProperty(label, name, "(Font)");
            SetPropertyReadOnly(result, true, false);

            var choices = PropertyGridAdapterFont.FontNameChoices;

            var itemName = CreateChoicesProperty(
                "Name",
                null,
                choices,
                adapter.NameAsIndex);
            itemName.Instance = adapter;
            itemName.PropInfo = AssemblyUtils.GetPropInfo(adapter, "NameAsIndex");

            var itemSizeInPoints = CreateProperty(adapter, "SizeInPoints")!;
            var itemIsBold = CreateProperty(adapter, "Bold")!;
            var itemIsItalic = CreateProperty(adapter, "Italic")!;
            var itemIsStrikethrough = CreateProperty(adapter, "Strikethrough")!;
            var itemIsUnderlined = CreateProperty(adapter, "Underlined")!;

            result.Children.Add(itemName!);
            result.Children.Add(itemSizeInPoints!);
            result.Children.Add(itemIsBold!);
            result.Children.Add(itemIsItalic!);
            result.Children.Add(itemIsStrikethrough!);
            result.Children.Add(itemIsUnderlined!);
            return result;
        }

        /// <summary>
        /// Creates <see cref="Brush"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="instance">Object instance which contains the property.</param>
        /// <param name="p">Property information.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        /// <remarks>
        /// If <paramref name="label"/> or <paramref name="name"/> is null,
        /// <paramref name="p"/> is used to get them.
        /// </remarks>
        public virtual IPropertyGridItem CreatePropertyAsBrush(
            string label,
            string? name,
            object instance,
            PropertyInfo p)
        {
            PropertyGridAdapterBrush adapter = new()
            {
                Instance = instance,
                PropInfo = p,
            };
            var result = CreateStringProperty(label, name, "(Brush)");
            SetPropertyReadOnly(result, true, false);

            var itemBrushType = CreateProperty(adapter, "BrushType")!;
            var itemColor = CreateProperty(adapter, "Color")!;
            var itemEndColor = CreateProperty(adapter, "EndColor")!;
            var itemLinearGradientStart = CreateProperty(adapter, "LinearGradientStart")!;
            var itemLinearGradientEnd = CreateProperty(adapter, "LinearGradientEnd")!;
            var itemRadialGradientCenter = CreateProperty(adapter, "RadialGradientCenter")!;
            var itemRadialGradientOrigin = CreateProperty(adapter, "RadialGradientOrigin")!;
            var itemRadialGradientRadius = CreateProperty(adapter, "RadialGradientRadius")!;
            var itemGradientStops = CreateProperty(adapter, "GradientStops")!;
            var itemHatchStyle = CreateProperty(adapter, "HatchStyle")!;

            result.Children.Add(itemBrushType!);
            result.Children.Add(itemColor!);
            result.Children.Add(itemEndColor!);
            result.Children.Add(itemLinearGradientStart!);
            result.Children.Add(itemLinearGradientEnd!);
            result.Children.Add(itemRadialGradientCenter!);
            result.Children.Add(itemRadialGradientOrigin!);
            result.Children.Add(itemRadialGradientRadius!);
            result.Children.Add(itemGradientStops!);
            result.Children.Add(itemHatchStyle!);

            return result;
        }

        /// <summary>
        /// Creates <see cref="Pen"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="instance">Object instance which contains the property.</param>
        /// <param name="p">Property information.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        /// <remarks>
        /// If <paramref name="label"/> or <paramref name="name"/> is null,
        /// <paramref name="p"/> is used to get them.
        /// </remarks>
        public virtual IPropertyGridItem CreatePropertyAsPen(
            string label,
            string? name,
            object instance,
            PropertyInfo p)
        {
            PropertyGridAdapterPen adapter = new()
            {
                Instance = instance,
                PropInfo = p,
            };
            IPropertyGridItem result =
                CreateStringProperty(label, name, "(Pen)");
            SetPropertyReadOnly(result, true, false);

            var itemColor = CreateProperty(adapter, "Color")!;
            var itemDashStyle = CreateProperty(adapter, "DashStyle")!;
            var itemLineCap = CreateProperty(adapter, "LineCap")!;
            var itemLineJoin = CreateProperty(adapter, "LineJoin")!;
            var itemWidth = CreateProperty(adapter, "Width")!;

            result.Children.Add(itemColor!);
            result.Children.Add(itemDashStyle!);
            result.Children.Add(itemLineCap!);
            result.Children.Add(itemLineJoin!);
            result.Children.Add(itemWidth!);

            return result;
        }

        /// <summary>
        /// Creates <see cref="decimal"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateDecimalProperty(
            string label,
            string? name = null,
            decimal value = default)
        {
            var result = CreateStringProperty(label, name, value.ToString());
            SetPropertyValidator(result, ValueValidatorFactory.DecimalValidator);
            return result;
        }

        /// <summary>
        /// Creates <see cref="sbyte"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateSByteProperty(
            string label,
            string? name = null,
            sbyte value = 0)
        {
            var result = CreateLongProperty(label, name, value);
            SetPropertyMinMax(result, TypeCode.SByte);
            result.PropertyEditorKind = TypeCode.SByte.ToString();
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
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateInt16Property(
            string label,
            string? name = null,
            short value = 0)
        {
            var result = CreateLongProperty(label, name, value);
            SetPropertyMinMax(result, TypeCode.Int16);
            result.PropertyEditorKind = TypeCode.Int16.ToString();
            return result;
        }

        /// <summary>
        /// Creates <see cref="int"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateIntProperty(
            string label,
            string? name = null,
            int value = 0)
        {
            var result = CreateLongProperty(label, name, value);
            SetPropertyMinMax(result, TypeCode.Int32);
            result.PropertyEditorKind = TypeCode.Int32.ToString();
            return result;
        }

        /// <summary>
        /// Creates <see cref="byte"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateByteProperty(
            string label,
            string? name = null,
            byte value = 0)
        {
            var result = CreateULongProperty(label, name, value);
            SetPropertyMinMax(result, TypeCode.Byte);
            result.PropertyEditorKind = TypeCode.Byte.ToString();
            return result;
        }

        /// <summary>
        /// Creates <see cref="uint"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateUIntProperty(
            string label,
            string? name = null,
            uint value = 0)
        {
            var result = CreateULongProperty(label, name, value);
            SetPropertyMinMax(result, TypeCode.UInt32);
            result.PropertyEditorKind = TypeCode.UInt32.ToString();
            return result;
        }

        /// <summary>
        /// Creates <see cref="ushort"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateUInt16Property(
            string label,
            string? name = null,
            ushort value = 0)
        {
            var result = CreateULongProperty(label, name, value);
            SetPropertyMinMax(result, TypeCode.UInt16);
            result.PropertyEditorKind = TypeCode.UInt16.ToString();
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
            var propInfo = type.GetProperty(nameInInstance);
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
        public IPropertyGridItem? CreateProperty(object instance, string nameInInstance)
        {
            return CreateProperty(null, null, instance, nameInInstance);
        }

        /// <summary>
        /// Creates property using another object's property.
        /// </summary>
        /// <param name="instance">Object instance which contains the property.</param>
        /// <param name="p">Property information.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public IPropertyGridItem? CreateProperty(object instance, PropertyInfo p)
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
            if (!p.CanRead)
                return null;
            ParameterInfo[] paramInfo = p.GetIndexParameters();
            if (paramInfo.Length > 0)
                return null;
            if (!AssemblyUtils.GetBrowsable(p))
                return null;

            var setPropReadonly = false;
            propName ??= p.Name;
            label ??= propName;
            var propType = p.PropertyType;
            object? propValue = p.GetValue(instance, null);
            IPropertyGridItem? prop = null;

            var realType = AssemblyUtils.GetRealType(propType);
            TypeCode typeCode = Type.GetTypeCode(realType);

            if (propType.IsEnum)
                prop = CreatePropertyAsEnum(label, propName, instance, p);
            else
            {
                switch (typeCode)
                {
                    case TypeCode.Empty:
                    case TypeCode.DBNull:
                        return null;
                    case TypeCode.Object:
                        prop = CreateAsClass();
                        break;
                    case TypeCode.Boolean:
                        propValue ??= false;
                        prop = CreateBoolProperty(label!, propName, (bool)propValue!);
                        break;
                    case TypeCode.SByte:
                        propValue ??= default(sbyte);
                        prop = CreateSByteProperty(label!, propName, (sbyte)propValue!);
                        break;
                    case TypeCode.Int16:
                        propValue ??= default(short);
                        prop = CreateInt16Property(label!, propName, (short)propValue!);
                        break;
                    case TypeCode.Int32:
                        propValue ??= default(int);
                        prop = CreateIntProperty(label!, propName, (int)propValue!);
                        break;
                    case TypeCode.Int64:
                        propValue ??= default(long);
                        prop = CreateLongProperty(label!, propName, (long)propValue!);
                        break;
                    case TypeCode.Byte:
                        propValue ??= default(byte);
                        prop = CreateByteProperty(label!, propName, (byte)propValue!);
                        break;
                    case TypeCode.UInt32:
                        propValue ??= default(uint);
                        prop = CreateUIntProperty(label!, propName, (uint)propValue!);
                        break;
                    case TypeCode.UInt16:
                        propValue ??= default(ushort);
                        prop = CreateUInt16Property(label!, propName, (ushort)propValue!);
                        break;
                    case TypeCode.UInt64:
                        propValue ??= default(ulong);
                        prop = CreateULongProperty(label!, propName, (ulong)propValue!);
                        break;
                    case TypeCode.Single:
                        propValue ??= 0F;
                        prop = CreateFloatProperty(label!, propName, (float)propValue!);
                        break;
                    case TypeCode.Double:
                        propValue ??= 0D;
                        prop = CreateDoubleProperty(label!, propName, (double)propValue!);
                        break;
                    case TypeCode.Decimal:
                        propValue ??= 0M;
                        prop = CreateDecimalProperty(label!, propName, (decimal)propValue!);
                        break;
                    case TypeCode.DateTime:
                        propValue ??= DateTime.Now;
                        prop = CreateDateProperty(label!, propName, (DateTime)propValue);
                        break;
                    case TypeCode.Char:
                        prop = CreateCharProperty(label!, propName, propValue?.ToString());
                        break;
                    case TypeCode.String:
                        prop = CreateStringProperty(label!, propName, propValue?.ToString());
                        break;
                }
            }

            if (!p.CanWrite || setPropReadonly)
                SetPropertyReadOnly(prop!, true);
            prop!.Instance = instance;
            prop!.PropInfo = p;
            return prop;

            IPropertyGridItem? CreateAsClass()
            {
                IPropertyGridItem? result;
                if (realType == typeof(Color))
                {
                    result = CreatePropertyAsColor(label, propName, instance, p);
                    return result;
                }

                if (realType == typeof(Font))
                {
                    result = CreatePropertyAsFont(label!, propName, instance, p);
                    setPropReadonly = true;
                    return result;
                }

                if (realType == typeof(Brush))
                {
                    result = CreatePropertyAsBrush(label!, propName, instance, p);
                    setPropReadonly = true;
                    return result;
                }

                if (realType == typeof(Pen))
                {
                    result = CreatePropertyAsPen(label!, propName, instance, p);
                    setPropReadonly = true;
                    return result;
                }

                result = CreateStringProperty(
                    label!,
                    propName,
                    propValue?.ToString());
                setPropReadonly = true;
                return result;
            }
        }

        /// <summary>
        /// Creates <see cref="IPropertyGridItem"/> array from all public properties of
        /// the specified object.
        /// </summary>
        /// <param name="instance">Object instance which properties will be added.</param>
        public virtual IEnumerable<IPropertyGridItem> CreateProps(object instance)
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
                if (ignorePropNames.Contains(propName))
                    continue;
                IPropertyGridItem? prop = CreateProperty(instance, p);
                if (prop == null)
                    continue;
                result.Add(prop!);
                addedNames.Add(propName, p);
            }

            return result;
        }

        /// <summary>
        /// Adds all public properties of the specified object.
        /// </summary>
        /// <param name="instance">Object instance which properties will be added.</param>
        /// <param name="parent">Optional. Parent item to which properties are added.</param>
        /// <remarks>
        /// If <paramref name="parent"/> is <c>null</c> (default) properties are
        /// added on the root level.
        /// </remarks>
        public virtual void AddProps(object instance, IPropertyGridItem? parent = null)
        {
            var props = CreateProps(instance);
            foreach (var item in props)
            {
                Add(item, parent);
            }
        }

        /// <summary>
        /// Clears properties and calls <see cref="AddProps"/> afterwards.
        /// </summary>
        /// <param name="instance">Object instance which properties will be added.</param>
        public virtual void SetProps(object? instance)
        {
            BeginUpdate();
            try
            {
                Clear();
                if (instance == null)
                    return;
                AddProps(instance);
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <summary>
        /// Creates <see cref="Color"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="instance">Object instance which contains the property.</param>
        /// <param name="p">Property information.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        /// <remarks>
        /// If <paramref name="label"/> or <paramref name="name"/> is null,
        /// <paramref name="p"/> is used to get them.
        /// </remarks>
        public virtual IPropertyGridItem CreatePropertyAsColor(
            string? label,
            string? name,
            object instance,
            PropertyInfo p)
        {
            IPropertyGridItem prop;
            string propName = p.Name;
            label ??= propName;
            object? propValue = p.GetValue(instance, null);
            propValue ??= Color.Black;
            prop = CreateColorProperty(label, name, (Color)propValue);
            if (ColorHasAlpha)
                SetPropertyKnownAttribute(prop, PropertyGridItemAttrId.HasAlpha, true);
            OnPropertyCreated(prop, instance, p);
            return prop;
        }

        /// <summary>
        /// Creates enumeration property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="instance">Object instance which contains the property.</param>
        /// <param name="p">Property information.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        /// <remarks>
        /// If <paramref name="label"/> or <paramref name="name"/> is null,
        /// <paramref name="p"/> is used to get them.
        /// </remarks>
        public virtual IPropertyGridItem CreatePropertyAsEnum(
            string? label,
            string? name,
            object instance,
            PropertyInfo p)
        {
            IPropertyGridItem prop;
            var propType = p.PropertyType;
            string propName = p.Name;
            label ??= propName;
            object? propValue = p.GetValue(instance, null);
            var flagsAttr = propType.GetCustomAttribute(typeof(FlagsAttribute));
            var choices = PropertyGrid.CreateChoicesOnce(propType);
            if (flagsAttr == null)
            {
                prop = CreateChoicesProperty(
                    label,
                    name,
                    choices,
                    propValue!);
            }
            else
            {
                prop = CreateFlagsProperty(
                    label,
                    name,
                    choices,
                    propValue!);
            }

            OnPropertyCreated(prop, instance, p);
            return prop;
        }

        /// <summary>
        /// Creates enumeration property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="choices">Enumeration elements.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateChoicesProperty(
            string label,
            string? name,
            IPropertyGridChoices choices,
            object? value = null)
        {
            value ??= 0;
            var handle = NativeControl.CreateEnumProperty(
                label,
                CorrectPropName(name),
                choices.Handle,
                (int)value);
            var result = new PropertyGridItem(this, handle, label, name, value)
            {
                PropertyEditorKind = "Enum",
            };
            OnPropertyCreated(result);
            return result;
        }

        /// <summary>
        /// Creates editable enumeration property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="choices">Enumeration elements.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateEditEnumProperty(
            string label,
            string? name,
            IPropertyGridChoices choices,
            string? value = null)
        {
            value ??= string.Empty;
            var handle = NativeControl.CreateEditEnumProperty(
                label,
                CorrectPropName(name),
                choices.Handle,
                value);
            var result = new PropertyGridItem(this, handle, label, name, value)
            {
                PropertyEditorKind = "Enum.Edit",
            };
            OnPropertyCreated(result);
            return result;
        }

        /// <summary>
        /// Creates flags property (like enumeration with Flags attribute).
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="choices">Elements.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateFlagsProperty(
            string label,
            string? name,
            IPropertyGridChoices choices,
            object? value = null)
        {
            value ??= 0;
            var handle = NativeControl.CreateFlagsProperty(
                label,
                CorrectPropName(name),
                choices.Handle,
                (int)value);
            var result = new PropertyGridItem(this, handle, label, name, value)
            {
                PropertyEditorKind = "Enum.Flags",
            };
            OnPropertyCreated(result);
            return result;
        }

        /// <summary>
        /// Deletes all items from the property grid.
        /// </summary>
        public virtual void Clear()
        {
            items.Clear();
            NativeControl.Clear();
        }

        /// <summary>
        /// Adds item to the property grid.
        /// </summary>
        /// <param name="prop">Item to add.</param>
        /// <param name="parent">Parent item or null.</param>
        public virtual void Add(IPropertyGridItem prop, IPropertyGridItem? parent = null)
        {
            if (prop == null)
                return;
            SetAsCheckBox(prop);
            if (parent == null)
                NativeControl.Append(prop.Handle);
            else
                NativeControl.AppendIn(parent.Handle, prop.Handle);

            items.Add(prop.Handle, prop);
            if (prop.HasChildren)
            {
                foreach (IPropertyGridItem child in prop.Children)
                {
                    Add(child, prop);
                }

                Collapse(prop);
            }

            void SetAsCheckBox(IPropertyGridItem p)
            {
                var kind = p.PropertyEditorKind;

                if (BoolAsCheckBox && (kind == TypeCode.Boolean.ToString() || kind == "Enum.Flags"))
                {
                    SetPropertyKnownAttribute(p, PropertyGridItemAttrId.UseCheckbox, true);
                }
            }
        }

        /// <summary>
        /// Creates properties category.
        /// </summary>
        /// <param name="label">Category label.</param>
        /// <param name="name">Category name.</param>
        /// <returns>Category declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreatePropCategory(string label, string? name = null)
        {
            var handle = NativeControl.CreatePropCategory(
                label,
                CorrectPropName(name));
            var result = new PropertyGridItem(this, handle, label, name, null)
            {
                IsCategory = true,
            };
            OnPropertyCreated(result);
            return result;
        }

        /// <summary>
        /// Gets property name of the <see cref="IPropertyGridItem"/>.
        /// </summary>
        /// <param name="property">Property Item.</param>
        public string GetPropertyName(IPropertyGridItem property)
        {
            return NativeControl.GetPropertyName(property.Handle);
        }

        /// <summary>
        /// Sorts properties.
        /// </summary>
        /// <param name="topLevelOnly"><c>true</c> to sort only top level properties,
        /// <c>false</c> otherwise.</param>
        public void Sort(bool topLevelOnly = false)
        {
            var flags = topLevelOnly ? PGSORTTOPLEVELONLY : 0;
            NativeControl.Sort(flags);
        }

        /// <summary>
        /// Sets property readonly flag.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="isSet">New Readonly flag value.</param>
        /// <param name="recurse"><c>true</c> to change readonly flag recursively
        /// for child propereties, <c>false</c> otherwise.</param>
        public void SetPropertyReadOnly(IPropertyGridItem prop, bool isSet, bool recurse = true)
        {
            var flags = recurse ? PGRECURSE : PGDONTRECURSE;

            NativeControl.SetPropertyReadOnly(prop.Handle, isSet, flags);
        }

        /// <summary>
        /// Allows property value to be unspecified.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public void SetPropertyValueUnspecified(IPropertyGridItem prop)
        {
            NativeControl.SetPropertyValueUnspecified(prop.Handle);
        }

        /// <summary>
        /// Appends property as a child of other property.
        /// </summary>
        /// <param name="prop">Parent property item.</param>
        /// <param name="newproperty">Property item to add as a child.</param>
        /// <remarks>
        /// It is better to fill <see cref="IPropertyGridItem.Children"/> and
        /// to add property using <see cref="Add"/>.
        /// </remarks>
        public virtual void AppendIn(IPropertyGridItem prop, IPropertyGridItem newproperty)
        {
            NativeControl.AppendIn(prop.Handle, newproperty.Handle);
        }

        /// <summary>
        /// Collapses (hides) all sub properties of the given property.
        /// </summary>
        /// <param name="prop">Property item to collapse.</param>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        public virtual bool Collapse(IPropertyGridItem prop)
        {
            return NativeControl.Collapse(prop.Handle);
        }

        /// <summary>
        /// Removes property from the <see cref="PropertyGrid"/>.
        /// </summary>
        /// <param name="prop">Property item to remove.</param>
        public virtual void RemoveProperty(IPropertyGridItem prop)
        {
            NativeControl.RemoveProperty(prop.Handle);
            items.Remove(prop.Handle);
        }

        /// <summary>
        /// Disables property.
        /// </summary>
        /// <param name="prop">Property item to disable.</param>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        public virtual bool DisableProperty(IPropertyGridItem prop)
        {
            return NativeControl.DisableProperty(prop.Handle);
        }

        /// <summary>
        /// Changes enabled state of the property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="enable">New enabled state value.</param>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        public virtual bool EnableProperty(IPropertyGridItem prop, bool enable = true)
        {
            return NativeControl.EnableProperty(prop.Handle, enable);
        }

        /// <summary>
        /// Expands (shows) all sub properties of the given property.
        /// </summary>
        /// <param name="prop">Property item to expand.</param>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        public virtual bool Expand(IPropertyGridItem prop)
        {
            return NativeControl.Expand(prop.Handle);
        }

        /// <summary>
        /// Gets client data associated with the property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual IntPtr GetPropertyClientData(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyClientData(prop.Handle);
        }

        /// <summary>
        /// Gets help string associated with the property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual string GetPropertyHelpString(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyHelpString(prop.Handle);
        }

        /// <summary>
        /// Gets label associated with the property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual string GetPropertyLabel(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyLabel(prop.Handle);
        }

        /// <summary>
        /// Gets property value as <see cref="string"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual string GetPropertyValueAsString(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyValueAsString(prop.Handle);
        }

        /// <summary>
        /// Gets property value as <see cref="long"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual long GetPropertyValueAsLong(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyValueAsLong(prop.Handle);
        }

        /// <summary>
        /// Gets property value as <see cref="ulong"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual ulong GetPropertyValueAsULong(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyValueAsULong(prop.Handle);
        }

        /// <summary>
        /// Gets property value as <see cref="int"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual int GetPropertyValueAsInt(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyValueAsInt(prop.Handle);
        }

        /// <summary>
        /// Gets property value as <see cref="bool"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual bool GetPropertyValueAsBool(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyValueAsBool(prop.Handle);
        }

        /// <summary>
        /// Gets property value as <see cref="double"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual double GetPropertyValueAsDouble(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyValueAsDouble(prop.Handle);
        }

        /// <summary>
        /// Gets property value as <see cref="DateTime"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual DateTime GetPropertyValueAsDateTime(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyValueAsDateTime(prop.Handle);
        }

        /// <summary>
        /// Hides or shows property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="hide"><c>true</c> to hide the property, <c>false</c> to show
        /// the property.</param>
        /// <param name="recurse">Perform operation recursively for the child items.</param>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        public virtual bool HideProperty(IPropertyGridItem prop, bool hide, bool recurse = true)
        {
            var flags = recurse ? PGRECURSE : PGDONTRECURSE;
            return NativeControl.HideProperty(prop.Handle, hide, flags);
        }

        /// <summary>
        /// Inserts property before another property.
        /// </summary>
        /// <param name="priorThis">Property item before which other property
        /// will be inserted.</param>
        /// <param name="newproperty">Property item to insert.</param>
        public virtual void Insert(IPropertyGridItem priorThis, IPropertyGridItem newproperty)
        {
            NativeControl.Insert(priorThis.Handle, newproperty.Handle);
        }

        /// <summary>
        /// Inserts property in parent property at specified index.
        /// </summary>
        /// <param name="parent">Parent property item.</param>
        /// <param name="index">Insert position.</param>
        /// <param name="newproperty">Property item to insert.</param>
        public virtual void Insert(IPropertyGridItem parent, int index, IPropertyGridItem newproperty)
        {
            NativeControl.InsertByIndex(parent.Handle, index, newproperty.Handle);
        }

        /// <summary>
        /// Gets whether property is category.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property is category, <c>false</c> otherwise.</returns>
        public virtual bool IsPropertyCategory(IPropertyGridItem prop)
        {
            return NativeControl.IsPropertyCategory(prop.Handle);
        }

        /// <summary>
        /// Gets whether property is enabled.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property is enabled, <c>false</c> otherwise.</returns>
        public virtual bool IsPropertyEnabled(IPropertyGridItem prop)
        {
            return NativeControl.IsPropertyEnabled(prop.Handle);
        }

        /// <summary>
        /// Gets whether property is expanded.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property is expanded, <c>false</c> otherwise.</returns>
        public virtual bool IsPropertyExpanded(IPropertyGridItem prop)
        {
            return NativeControl.IsPropertyExpanded(prop.Handle);
        }

        /// <summary>
        /// Gets whether property is modified.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property is modified, <c>false</c> otherwise.</returns>
        public virtual bool IsPropertyModified(IPropertyGridItem prop)
        {
            return NativeControl.IsPropertyModified(prop.Handle);
        }

        /// <summary>
        /// Gets whether property is selected.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property is selected, <c>false</c> otherwise.</returns>
        public virtual bool IsPropertySelected(IPropertyGridItem prop)
        {
            return NativeControl.IsPropertySelected(prop.Handle);
        }

        /// <summary>
        /// Gets whether property is shown.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property is shown, <c>false</c> otherwise.</returns>
        public virtual bool IsPropertyShown(IPropertyGridItem prop)
        {
            return NativeControl.IsPropertyShown(prop.Handle);
        }

        /// <summary>
        /// Gets whether property value is unspecified.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property value is unspecified, <c>false</c> otherwise.</returns>
        public virtual bool IsPropertyValueUnspecified(IPropertyGridItem prop)
        {
            return NativeControl.IsPropertyValueUnspecified(prop.Handle);
        }

        /// <summary>
        /// Disables (limit = true) or enables (limit = false) text editor of a property,
        /// if it is not the sole mean to edit the value.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="limit"><c>true</c> to disable text editor, <c>false</c> otherwise.</param>
        public virtual void LimitPropertyEditing(IPropertyGridItem prop, bool limit = true)
        {
            NativeControl.LimitPropertyEditing(prop.Handle, limit);
        }

        /// <summary>
        /// Replaces existing property with newly created property.
        /// </summary>
        /// <param name="prop">Property item to be replaced.</param>
        /// <param name="newProp">New property item.</param>
        public virtual void ReplaceProperty(IPropertyGridItem prop, IPropertyGridItem newProp)
        {
            NativeControl.ReplaceProperty(prop.Handle, newProp.Handle);
        }

        /// <summary>
        /// Sets background colour of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="color">New background color.</param>
        /// <param name="recurse"><c>true</c> causes color to be set recursively,
        /// <c>false</c> only sets color for the property in question and not
        /// any of its children.</param>
        public virtual void SetPropertyBackgroundColor(
            IPropertyGridItem prop,
            Color color,
            bool recurse = true)
        {
            var flags = recurse ? PGRECURSE : PGDONTRECURSE;
            NativeControl.SetPropertyBackgroundColor(prop.Handle, color, flags);
        }

        /// <summary>
        /// Resets text and background colors of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="recurse"><c>true</c> causes color to be reset recursively,
        /// <c>false</c> only resets color for the property in question and not
        /// any of its children.</param>
        public virtual void SetPropertyColorsToDefault(IPropertyGridItem prop, bool recurse = true)
        {
            var flags = recurse ? PGRECURSE : PGDONTRECURSE;
            NativeControl.SetPropertyColorsToDefault(prop.Handle, flags);
        }

        /// <summary>
        /// Sets text colour of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="color">New text color.</param>
        /// <param name="recurse"><c>true</c> causes color to be set recursively,
        /// <c>false</c> only sets color for the property in question and not
        /// any of its children.</param>
        public virtual void SetPropertyTextColor(
            IPropertyGridItem prop,
            Color color,
            bool recurse = true)
        {
            var flags = recurse ? PGRECURSE : PGDONTRECURSE;
            NativeControl.SetPropertyTextColor(prop.Handle, color, flags);
        }

        /// <summary>
        /// Restores user-editable state.
        /// </summary>
        /// <param name="src">String generated by <see cref="SaveEditableState"/>.</param>
        /// <param name="restoreStates">Which parts to restore from source string.</param>
        /// <returns><c>true</c> if there were no problems during reading the state,
        /// <c>false</c> otherwise.</returns>
        /// <remarks>
        /// If some parts of state (such as scrolled or splitter position) fail to restore
        /// correctly, please make sure that you call this function after
        /// <see cref="PropertyGrid"/> size has been set.
        /// </remarks>
        public virtual bool RestoreEditableState(
            string src,
            PropertyGridEditableState restoreStates = PropertyGridEditableState.AllStates)
        {
            return NativeControl.RestoreEditableState(src, (int)restoreStates);
        }

        /// <summary>
        /// Redraws given property.
        /// </summary>
        /// <param name="p">Property item.</param>
        /// <remarks>
        /// This function reselects the property and may cause excess flicker.
        /// </remarks>
        public virtual void RefreshProperty(IPropertyGridItem p)
        {
            NativeControl.RefreshProperty(p.Handle);
        }

        /// <summary>
        /// Used to acquire user-editable state (selected property,
        /// expanded properties, scrolled position, splitter positions).
        /// </summary>
        /// <param name="includedStates">Which parts of state to include.</param>
        /// <returns></returns>
        /// <remarks>
        /// Use <see cref="RestoreEditableState"/> to read state back to <see cref="PropertyGrid"/>.
        /// </remarks>
        public virtual string SaveEditableState(
            PropertyGridEditableState includedStates =
                PropertyGridEditableState.AllStates)
        {
            return NativeControl.SaveEditableState((int)includedStates);
        }

        /// <summary>
        /// Sets proportion of an auto-stretchable column.
        /// </summary>
        /// <param name="column">Column index.</param>
        /// <param name="proportion"></param>
        /// <returns><c>true</c> on success, <c>false</c> on failure.</returns>
        /// <remarks>
        /// <see cref="PropertyGridCreateStyle.SplitterAutoCenter"/> style needs to be used
        /// to indicate that columns are auto-resizable.
        /// </remarks>
        public virtual bool SetColumnProportion(uint column, int proportion)
        {
            return NativeControl.SetColumnProportion(column, proportion);
        }

        /// <summary>
        /// Gets auto-resize proportion of the given column.
        /// </summary>
        /// <param name="column">Column index.</param>
        public virtual int GetColumnProportion(uint column)
        {
            return NativeControl.GetColumnProportion(column);
        }

        /// <summary>
        /// Gets background color of first cell of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual Color GetPropertyBackgroundColor(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyBackgroundColor(prop.Handle);
        }

        /// <summary>
        /// Returns text color of first cell of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual Color GetPropertyTextColor(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyTextColor(prop.Handle);
        }

        /// <summary>
        /// Sets client data of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="clientData">Client data associated with the property.</param>
        public virtual void SetPropertyClientData(IPropertyGridItem prop, IntPtr clientData)
        {
            NativeControl.SetPropertyClientData(prop.Handle, clientData);
        }

        /// <summary>
        /// Sets label of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="newproplabel">New property label.</param>
        /// <remarks>
        /// Properties under same parent may have same labels. However, property
        /// names must still remain unique.
        /// </remarks>
        public virtual void SetPropertyLabel(IPropertyGridItem prop, string newproplabel)
        {
            NativeControl.SetPropertyLabel(prop.Handle, newproplabel);
        }

        /// <summary>
        /// Sets help string of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="helpString">Help string associated with the property.</param>
        public virtual void SetPropertyHelpString(IPropertyGridItem prop, string helpString)
        {
            NativeControl.SetPropertyHelpString(prop.Handle, helpString);
        }

        /// <summary>
        /// Sets maximum length of text in property text editor.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="maxLen">Maximum number of characters of the text the user can enter
        /// in the text editor. If it is 0, the length is not limited and the text can be
        /// as long as it is supported by the underlying native text control widget.</param>
        /// <returns><c>true</c> if maximum length was set, <c>false</c> otherwise.</returns>
        public virtual bool SetPropertyMaxLength(IPropertyGridItem prop, int maxLen)
        {
            return NativeControl.SetPropertyMaxLength(prop.Handle, maxLen);
        }

        /// <summary>
        /// Sets property value as <see cref="long"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetPropertyValueAsLong(IPropertyGridItem prop, long value)
        {
            NativeControl.SetPropertyValueAsLong(prop.Handle, value);
        }

        /// <summary>
        /// Sets property value as <see cref="int"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetPropertyValueAsInt(IPropertyGridItem prop, int value)
        {
            NativeControl.SetPropertyValueAsInt(prop.Handle, value);
        }

        /// <summary>
        /// Sets property value as <see cref="double"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetPropertyValueAsDouble(IPropertyGridItem prop, double value)
        {
            NativeControl.SetPropertyValueAsDouble(prop.Handle, value);
        }

        /// <summary>
        /// Sets property value as <see cref="bool"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetPropertyValueAsBool(IPropertyGridItem prop, bool value)
        {
            NativeControl.SetPropertyValueAsBool(prop.Handle, value);
        }

        /// <summary>
        /// Sets property value as <see cref="string"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetPropertyValueAsStr(IPropertyGridItem prop, string value)
        {
            NativeControl.SetPropertyValueAsStr(prop.Handle, value);
        }

        /// <summary>
        /// Sets property value as <see cref="DateTime"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetPropertyValueAsDateTime(IPropertyGridItem prop, DateTime value)
        {
            NativeControl.SetPropertyValueAsDateTime(prop.Handle, value);
        }

        /// <summary>
        /// Adjusts how <see cref="PropertyGrid"/> behaves when invalid value is
        /// entered in a property.
        /// </summary>
        /// <param name="vfbFlags">Validation failure flags.</param>
        public virtual void SetValidationFailureBehavior(PropertyGridValidationFailure vfbFlags)
        {
            NativeControl.SetValidationFailureBehavior((int)vfbFlags);
        }

        /// <summary>
        /// Sorts children of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="recurse"><c>true</c> to perform recursive sorting, <c>false</c>
        /// otherwise.</param>
        public virtual void SortChildren(IPropertyGridItem prop, bool recurse = false)
        {
            var flags = recurse ? PGRECURSE : PGDONTRECURSE;
            NativeControl.SortChildren(prop.Handle, flags);
        }

        /// <summary>
        /// Sets editor control of a property usinbg its name.
        /// </summary>
        /// <param name="prop">Property Item.</param>
        /// <param name="editorName">Name of the editor.</param>
        /// <remarks>
        /// Names of built-in editors are: TextCtrl, Choice, ComboBox, CheckBox,
        /// TextCtrlAndButton, and ChoiceAndButton. Additional editors include
        /// SpinCtrl and DatePickerCtrl, but using them requires
        /// calling <see cref="RegisterAdditionalEditors"/> prior use.
        /// </remarks>
        public virtual void SetPropertyEditorByName(IPropertyGridItem prop, string editorName)
        {
            NativeControl.SetPropertyEditorByName(prop.Handle, editorName);
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
                    SetCaptionBackgroundColor(colors.CaptionBackgroundColor.Value);
                if (colors.CaptionForegroundColor is not null)
                    SetCaptionTextColor(colors.CaptionForegroundColor.Value);
                if (colors.CellDisabledTextColor is not null)
                    SetCellDisabledTextColor(colors.CellDisabledTextColor.Value);
                if (colors.EmptySpaceColor is not null)
                    SetEmptySpaceColor(colors.EmptySpaceColor.Value);
                if (colors.LineColor is not null)
                    SetLineColor(colors.LineColor.Value);
                if (colors.MarginColor is not null)
                    SetMarginColor(colors.MarginColor.Value);
                if (colors.SelectionBackgroundColor is not null)
                    SetSelectionBackgroundColor(colors.SelectionBackgroundColor.Value);
                if (colors.SelectionForegroundColor is not null)
                    SetSelectionTextColor(colors.SelectionForegroundColor.Value);
                if (colors.CellTextColor is not null)
                    SetCellTextColor(colors.CellTextColor.Value);
                if (colors.CellBackgroundColor is not null)
                    SetCellBackgroundColor(colors.CellBackgroundColor.Value);
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <summary>
        /// Adds new action trigger (keyboard key association).
        /// </summary>
        /// <param name="action">Action for which triggers (keyboard key associations)
        /// will be added.</param>
        /// <param name="keycode">Key code.</param>
        /// <param name="modifiers">Key mnodifiers (Ctrl, Shift, Alt) of the key.</param>
        public virtual void AddActionTrigger(
            PropertyGridKeyboardAction action,
            Key keycode,
            ModifierKeys modifiers = 0)
        {
            NativeControl.AddActionTrigger((int)action, (int)keycode, (int)modifiers);
        }

        /// <summary>
        /// Removes added action triggers for the given action.
        /// </summary>
        /// <param name="action">Action for which triggers (keyboard key associations) will
        /// be removed.</param>
        public virtual void ClearActionTriggers(PropertyGridKeyboardAction action)
        {
            NativeControl.ClearActionTriggers((int)action);
        }

        /// <summary>
        /// Dedicates a specific keycode to <see cref="PropertyGrid"/>. This means that
        /// such key presses will not be redirected to editor controls.
        /// </summary>
        /// <param name="keycode"></param>
        /// <remarks>
        /// Using this function allows, for example, navigation between properties using
        /// arrow keys even when the focus is in the editor control.
        /// </remarks>
        public virtual void DedicateKey(Key keycode)
        {
            NativeControl.DedicateKey((int)keycode);
        }

        /// <summary>
        /// Centers the splitter.
        /// </summary>
        /// <param name="enableAutoResizing">If <c>true</c>, automatic column resizing is
        /// enabled (only applicable if window style
        /// <see cref="PropertyGridCreateStyle.SplitterAutoCenter"/> is used).</param>
        public virtual void CenterSplitter(bool enableAutoResizing = false)
        {
            NativeControl.CenterSplitter(enableAutoResizing);
        }

        /// <summary>
        /// Call when editor widget's contents is modified.
        /// </summary>
        /// <remarks>
        /// For example, this is called when changes text in <see cref="TextBox"/>
        /// (used in string or int property).
        /// </remarks>
        public virtual void EditorsValueWasModified()
        {
            NativeControl.EditorsValueWasModified();
        }

        /// <summary>
        /// Reverse of <see cref="EditorsValueWasModified"/>.
        /// </summary>
        /// <remarks>
        /// This function should only be called by custom properties.
        /// </remarks>
        public virtual void EditorsValueWasNotModified()
        {
            NativeControl.EditorsValueWasNotModified();
        }

        /// <summary>
        /// Enables or disables (shows/hides) categories according to parameter enable.
        /// </summary>
        /// <param name="enable"></param>
        /// <returns><c>true</c> if operation successful, <c>false</c> otherwise.</returns>
        /// <remarks>
        /// This functions deselects selected property, if any. Validation failure
        /// option <see cref="PropertyGridValidationFailure.StayInProperty"/> is not
        /// respected, i.e.selection is cleared even if editor had invalid value.
        /// </remarks>
        public virtual bool EnableCategories(bool enable)
        {
            return NativeControl.EnableCategories(enable);
        }

        /// <summary>
        /// Reduces column sizes to minimum possible, while still retaining fully
        /// visible grid contents (labels, images).
        /// </summary>
        /// <returns>Minimum size for the grid to still display everything.</returns>
        /// <remarks>
        /// Does not work well with <see cref="PropertyGridCreateStyle.SplitterAutoCenter"/>
        /// window style. This function only works properly if grid size prior to call
        /// was already fairly large.
        /// </remarks>
        public virtual Size FitColumns()
        {
            return NativeControl.FitColumns();
        }

        /// <summary>
        /// Gets number of columns currently on grid.
        /// </summary>
        public virtual uint GetColumnCount()
        {
            return NativeControl.GetColumnCount();
        }

        /// <summary>
        /// Gets height of highest characters of used font.
        /// </summary>
        public virtual int GetFontHeight()
        {
            return NativeControl.GetFontHeight();
        }

        /// <summary>
        /// Gets margin width.
        /// </summary>
        public virtual int GetMarginWidth()
        {
            return NativeControl.GetMarginWidth();
        }

        /// <summary>
        /// Gets height of a single grid row (in pixels).
        /// </summary>
        public virtual int GetRowHeight()
        {
            return NativeControl.GetRowHeight();
        }

        /// <summary>
        /// Gets current splitter x position.
        /// </summary>
        /// <param name="splitterIndex">Splitter index (starting from 0).</param>
        public virtual int GetSplitterPosition(uint splitterIndex = 0)
        {
            return NativeControl.GetSplitterPosition(splitterIndex);
        }

        /// <summary>
        /// Gets current vertical spacing.
        /// </summary>
        public virtual int GetVerticalSpacing()
        {
            return NativeControl.GetVerticalSpacing();
        }

        /// <summary>
        /// Gets whether a property editor control has focus.
        /// </summary>
        /// <returns><c>true</c> if a property editor control has focus, <c>false</c>
        /// otherwise.</returns>
        public virtual bool IsEditorFocused()
        {
            return NativeControl.IsEditorFocused();
        }

        /// <summary>
        /// Gets whether editor's value was marked modified.
        /// </summary>
        /// <returns><c>true</c> if editor's value was marked modified, <c>false</c>
        /// otherwise.</returns>
        public virtual bool IsEditorsValueModified()
        {
            return NativeControl.IsEditorsValueModified();
        }

        /// <summary>
        /// Gets whether any property has been modified by the user.
        /// </summary>
        /// <returns><c>true</c> if any property has been modified by the user, <c>false</c>
        /// otherwise.</returns>
        public virtual bool IsAnyModified()
        {
            return NativeControl.IsAnyModified();
        }

        /// <summary>
        /// Resets all colors used in <see cref="PropertyGrid"/> to default values.
        /// </summary>
        public virtual void ResetColors()
        {
            NativeControl.ResetColors();
        }

        /// <summary>
        /// Resets column sizes and splitter positions, based on proportions.
        /// </summary>
        /// <param name="enableAutoResizing">If <c>true</c>, automatic column resizing
        /// is enabled (only applicable if control style
        /// <see cref="PropertyGridCreateStyle.SplitterAutoCenter"/> is used).</param>
        public virtual void ResetColumnSizes(bool enableAutoResizing = false)
        {
            NativeControl.ResetColumnSizes(enableAutoResizing);
        }

        /// <summary>
        /// Makes given column editable by user.
        /// </summary>
        /// <param name="column">The index of the column to make editable.</param>
        /// <param name="editable">Using <c>false</c> here will disable column
        /// from being editable.</param>
        public virtual void MakeColumnEditable(uint column, bool editable = true)
        {
            NativeControl.MakeColumnEditable(column, editable);
        }

        /// <summary>
        /// Creates label editor for given column, for property that is currently selected.
        /// </summary>
        /// <param name="column">Which column's label to edit. Note that you should not use
        /// value 1, which is reserved for property value column.</param>
        /// <remarks>
        /// When multiple selection is enabled, this applies to all selected properties.
        /// </remarks>
        public virtual void BeginLabelEdit(uint column = 0)
        {
            NativeControl.BeginLabelEdit(column);
        }

        /// <summary>
        /// Ends label editing, if any.
        /// </summary>
        /// <param name="commit">Use <c>true</c> (default) to store edited label text in
        /// property cell data.</param>
        public virtual void EndLabelEdit(bool commit = true)
        {
            NativeControl.EndLabelEdit(commit);
        }

        /// <summary>
        /// Sets number of columns (2 or more).
        /// </summary>
        /// <param name="colCount">Number of columns.</param>
        public virtual void SetColumnCount(int colCount)
        {
            NativeControl.SetColumnCount(colCount);
        }

        /// <summary>
        /// Sets x coordinate of the splitter.
        /// </summary>
        /// <param name="newXPos">Splitter position.</param>
        /// <param name="col">Column index.</param>
        /// <remarks>
        /// Splitter position cannot exceed grid size, and therefore setting it during
        /// form creation may fail as initial grid size is often smaller than desired
        /// splitter position, especially when advanced sizers are being used.
        /// </remarks>
        public virtual void SetSplitterPosition(int newXPos, int col = 0)
        {
            NativeControl.SetSplitterPosition(newXPos, col);
        }

        /// <summary>
        /// Returns (visual) text representation of the unspecified property value.
        /// </summary>
        public virtual string GetUnspecifiedValueText()
        {
            return NativeControl.GetUnspecifiedValueText(0);
        }

        /// <summary>
        /// Set virtual width for this particular page.
        /// </summary>
        /// <param name="width">New virtual width.</param>
        /// <remarks>
        /// Width -1 indicates that the virtual width should be disabled.
        /// </remarks>
        public virtual void SetVirtualWidth(int width)
        {
            NativeControl.SetVirtualWidth(width);
        }

        /// <summary>
        /// Moves splitter as left as possible, while still allowing all labels to be shown in full.
        /// </summary>
        /// <param name="privateChildrenToo">If <c>false</c>, will still allow private children
        /// to be cropped.</param>
        public virtual void SetSplitterLeft(bool privateChildrenToo = false)
        {
            NativeControl.SetSplitterLeft(privateChildrenToo);
        }

        /// <summary>
        /// Sets vertical spacing.
        /// </summary>
        /// <param name="vspacing">Vertical spacing.</param>
        /// <remarks>
        /// Can be 1, 2, or 3 - a value relative to font height. Value of 2 should be
        /// default on most platforms.
        /// </remarks>
        /// <remarks>
        /// If <paramref name="vspacing"/> is null,
        /// <see cref="PlatformDefaults.PropertyGridVerticalSpacing"/> is used.
        /// </remarks>
        public virtual void SetVerticalSpacing(int? vspacing = null)
        {
            int v;
            if (vspacing is null)
                v = AllPlatformDefaults.PlatformCurrent.PropertyGridVerticalSpacing;
            else
                v = (int)vspacing;

            v = Math.Min(v, 3);
            v = Math.Max(v, 1);

            NativeControl.SetVerticalSpacing(v);
        }

        /// <summary>
        /// Gets whether control has virtual width specified with
        /// <see cref="SetVirtualWidth"/>.
        /// </summary>
        public virtual bool HasVirtualWidth()
        {
            return NativeControl.HasVirtualWidth();
        }

        /// <summary>
        /// Gets number of common values.
        /// </summary>
        public virtual uint GetCommonValueCount()
        {
            return NativeControl.GetCommonValueCount();
        }

        /// <summary>
        /// Gets label of given common value.
        /// </summary>
        /// <param name="i">Index of the commo nvalue.</param>
        public virtual string GetCommonValueLabel(uint i)
        {
            return NativeControl.GetCommonValueLabel(i);
        }

        /// <summary>
        /// Gets index of common value that will truly change value to unspecified.
        /// </summary>
        public virtual int GetUnspecifiedCommonValue()
        {
            return NativeControl.GetUnspecifiedCommonValue();
        }

        /// <summary>
        /// Sets index of common value that will truly change value to unspecified.
        /// </summary>
        /// <param name="index">Index of the common value.</param>
        /// <remarks>
        /// Using -1 will set none to have such effect. Default is 0.
        /// </remarks>
        public virtual void SetUnspecifiedCommonValue(int index)
        {
            NativeControl.SetUnspecifiedCommonValue(index);
        }

        /// <summary>
        /// Refreshes any active editor control.
        /// </summary>
        public virtual void RefreshEditor()
        {
            NativeControl.RefreshEditor();
        }

        /// <summary>
        /// You can use this function, for instance, to detect in events if property's
        /// SetValueInEvent was already called in editor's event handler.
        /// </summary>
        /// <remarks>
        /// It really only detects if was value was changed using property's SetValueInEvent(),
        /// which is usually used when a 'picker' dialog is displayed. If value was written by
        /// "normal means" in property's StringToValue() or IntToValue(), then this function
        /// will return false (on the other hand, property's event handler is not even
        /// called in those cases).
        /// </remarks>
        public virtual bool WasValueChangedInEvent()
        {
            return NativeControl.WasValueChangedInEvent();
        }

        /// <summary>
        /// Gets current Y spacing.
        /// </summary>
        public virtual int GetSpacingY()
        {
            return NativeControl.GetSpacingY();
        }

        /// <summary>
        /// Unfocuses or closes editor if one was open, but does not deselect property.
        /// </summary>
        public virtual bool UnfocusEditor()
        {
            return NativeControl.UnfocusEditor();
        }

        /// <summary>
        /// Returns last item which could be iterated using given flags.
        /// </summary>
        /// <param name="flags">Flags to limit returned properties.</param>
        /// <returns></returns>
        public virtual IPropertyGridItem? GetLastItem(PropertyGridIteratorFlags flags)
        {
            return PtrToItem(NativeControl.GetLastItem((int)flags));
        }

        /// <summary>
        /// Returns "root property".
        /// </summary>
        /// <remarks>
        /// Root property does not have name, etc. and it is not visible.
        /// It is only useful for accessing its children.
        /// </remarks>
        public virtual IPropertyGridItem? GetRoot()
        {
            return PtrToItem(NativeControl.GetRoot());
        }

        /// <summary>
        /// Gets currently selected property.
        /// </summary>
        public virtual IPropertyGridItem? GetSelectedProperty()
        {
            return PtrToItem(NativeControl.GetSelectedProperty());
        }

        /// <summary>
        /// Changes value of a property, as if from an editor.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">Property value.</param>
        /// <remarks>
        /// Use this instead of <see cref="SetPropertyValueAsVariant"/> if you need the value to
        /// run through validation process, and also send <see cref="PropertyChanged"/>
        /// event.
        /// </remarks>
        /// <remarks>
        /// Since this function sends <see cref = "PropertyChanged"/> event, it should not
        /// be called from <see cref = "PropertyChanged"/> event handler.
        /// </remarks>
        /// <returns>
        /// <c>true</c> if value was successfully changed.
        /// </returns>
        public virtual bool ChangePropertyValue(IPropertyGridItem prop, object value)
        {
            return NativeControl.ChangePropertyValue(prop.Handle, ToVariant(value).Handle);
        }

        /// <summary>
        /// Changes value of a property, as if from an editor.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">Property value.</param>
        /// <remarks>See <see cref="ChangePropertyValue"/> for more details.</remarks>
        public virtual bool ChangePropertyValueAsVariant(
            IPropertyGridItem prop,
            IPropertyGridVariant value)
        {
            return NativeControl.ChangePropertyValue(prop.Handle, value.Handle);
        }

        /// <summary>
        /// Sets property value as variant.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">New property value.</param>
        public void SetPropertyValueAsVariant(IPropertyGridItem prop, IPropertyGridVariant value)
        {
            NativeControl.SetPropertyValueAsVariant(prop.Handle, value.Handle);
        }

        /// <summary>
        /// Sets image associated with the property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="bmp">Image.</param>
        public virtual void SetPropertyImage(IPropertyGridItem prop, ImageSet? bmp)
        {
            NativeControl.SetPropertyImage(prop.Handle, bmp?.NativeImageSet);
        }

        /// <summary>
        /// Sets an attribute for the property.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="attrName">Text identifier of attribute. See
        /// <see cref="PropertyGridItemAttrId"/> for the known attribute names.</param>
        /// <param name="value">Value of attribute.</param>
        /// <param name="argFlags">Optional. Use
        /// <see cref="PropertyGridItemValueFlags.Recurse"/> to set the attribute to child
        /// properties recursively.</param>
        /// <remarks>
        /// Setting attribute's value to <c>null</c> will simply remove it from property's
        /// set of attributes.
        /// </remarks>
        /// <remarks>
        /// Property is refreshed with new settings after calling this method.
        /// </remarks>
        public virtual void SetPropertyAttribute(
            IPropertyGridItem prop,
            string attrName,
            object? value = null,
            PropertyGridItemValueFlags argFlags = 0)
        {
            NativeControl.SetPropertyAttribute(
                prop.Handle,
                attrName,
                ToVariant(value).Handle,
                (int)argFlags);
        }

        /// <summary>
        /// Sets an attribute for the property.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="attrName">Text identifier of attribute. See
        /// <see cref="PropertyGridItemAttrId"/> for the known attribute names.</param>
        /// <param name="value">Value of attribute.</param>
        /// <param name="argFlags">Optional. Use
        /// <see cref="PropertyGridItemValueFlags.Recurse"/> to set the attribute to child
        /// properties recursively.</param>
        /// <remarks>
        /// Property is refreshed with new settings after calling this method.
        /// </remarks>
        public virtual void SetPropertyAttributeAsVariant(
            IPropertyGridItem prop,
            string attrName,
            IPropertyGridVariant value,
            PropertyGridItemValueFlags argFlags = 0)
        {
            NativeControl.SetPropertyAttribute(
                prop.Handle,
                attrName,
                value.Handle,
                (int)argFlags);
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
        /// Sets an attribute for all the properties.
        /// </summary>
        /// <param name="attrName">Text identifier of attribute. See
        /// <see cref="PropertyGridItemAttrId"/> for the known attribute names.</param>
        /// <param name="value">Value of attribute.</param>
        public virtual void SetPropertyAttributeAll(string attrName, object value)
        {
            NativeControl.SetPropertyAttributeAll(attrName, ToVariant(value).Handle);
        }

        /// <summary>
        /// Sets an attribute for all the properties.
        /// </summary>
        /// <param name="attrName">Text identifier of attribute. See
        /// <see cref="PropertyGridItemAttrId"/> for the known attribute names.</param>
        /// <param name="value">Value of attribute.</param>
        public virtual void SetPropertyAttributeAll(string attrName, IPropertyGridVariant value)
        {
            NativeControl.SetPropertyAttributeAll(attrName, value.Handle);
        }

        /// <summary>
        /// Scrolls and/or expands items to ensure that the given item is visible.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if something was actually done.</returns>
        public virtual bool EnsureVisible(IPropertyGridItem prop)
        {
            return NativeControl.EnsureVisible(prop.Handle);
        }

        /// <summary>
        /// Selects a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="focus">If <c>true</c>, move keyboard focus to the created
        /// editor right away.</param>
        /// <returns><c>true</c> if selection finished successfully. Usually only fails
        /// if current value in editor is not valid.</returns>
        /// <remarks>
        /// Editor widget is automatically created, but not focused unless focus is true.
        /// </remarks>
        /// <remarks>
        /// This function doesn't send <see cref="PropertySelected"/> event. It also clears
        /// any previous selection.
        /// </remarks>
        public virtual bool SelectProperty(IPropertyGridItem prop, bool focus = false)
        {
            return NativeControl.SelectProperty(prop.Handle, focus);
        }

        /// <summary>
        /// Adds given property into selection.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns></returns>
        /// <remarks>
        /// Multiple selection is not supported for categories. This means that if you have
        /// properties selected, you cannot add category to selection, and also if
        /// you have category selected, you cannot add other properties to
        /// selection. This function will fail silently in these cases, even returning true.
        /// </remarks>
        /// <remarks>
        /// If <see cref="PropertyGridCreateStyleEx.MultipleSelection"/> extra style is not used,
        /// then this has same effect as calling <see cref="SelectProperty"/>.
        /// </remarks>
        public virtual bool AddToSelection(IPropertyGridItem prop)
        {
            return NativeControl.AddToSelection(prop.Handle);
        }

        /// <summary>
        /// Removes given property from selection.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <remarks>
        /// If property is not selected, an assertion failure will occur.
        /// </remarks>
        public virtual bool RemoveFromSelection(IPropertyGridItem prop)
        {
            return NativeControl.RemoveFromSelection(prop.Handle);
        }

        /// <summary>
        /// Sets the 'current' category - <see cref="Add"/> will add non-category
        /// properties under it.
        /// </summary>
        /// <param name="prop">Property category item.</param>
        public virtual void SetCurrentCategory(IPropertyGridItem prop)
        {
            NativeControl.SetCurrentCategory(prop.Handle);
        }

        /// <summary>
        /// Returns rectangle of custom paint image.
        /// </summary>
        /// <param name="prop">Return image rectangle for this property.</param>
        /// <param name="item">Which choice of property to use (each choice may
        /// have different image).</param>
        public virtual Int32Rect GetImageRect(IPropertyGridItem prop, int item)
        {
            return NativeControl.GetImageRect(prop.Handle, item);
        }

        /// <summary>
        /// Sets validator of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="validator">Value validator.</param>
        public void SetPropertyValidator(IPropertyGridItem prop, IValueValidator validator)
        {
            IntPtr ptr = default;
            if (validator != null)
                ptr = validator.Handle;
            NativeControl.SetPropertyValidator(prop.Handle, ptr);
        }

        /// <summary>
        /// Returns size of the custom paint image in front of property.
        /// </summary>
        /// <param name="prop">Return image rectangle for this property. If this argument
        /// is <c>null</c>, then preferred size is returned.</param>
        /// <param name="item">Which choice of property to use (each choice may have
        /// different image).</param>
        public virtual Int32Size GetImageSize(IPropertyGridItem? prop, int item)
        {
            IntPtr ptr;
            if (prop is null)
                ptr = default;
            else
                ptr = prop.Handle;
            return NativeControl.GetImageSize(ptr, item);
        }

        /// <summary>
        /// Gets <see cref="IPropertyGridItem"/> added to the control filtered by
        /// <see cref="IPropertyGridItem.Instance"/> (<paramref name="instance"/> param) and
        /// <see cref="IPropertyGridItem.PropInfo"/> (<paramref name="propInfo"/> param).
        /// </summary>
        /// <param name="instance">Instance filter parameter. Ignored if <c>null</c>.</param>
        /// <param name="propInfo">Property information filter parameter.
        /// Ignored if <c>null</c></param>
        public IEnumerable<IPropertyGridItem> GetItemsFiltered(
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
        /// Reloads values of all <see cref="IPropertyGridItem"/> items collected with
        /// <see cref="GetItemsFiltered"/>.
        /// </summary>
        /// <param name="instance">Instance filter parameter. Ignored if <c>null</c>.</param>
        /// <param name="propInfo">Property information filter parameter.
        /// Ignored if <c>null</c></param>
        public virtual void ReloadValues(object? instance = null, PropertyInfo? propInfo = null)
        {
            IPropertyGridVariant variant = CreateVariant();

            var filteredItems = GetItemsFiltered(instance, propInfo);
            if (filteredItems.First() == null)
                return;
            BeginUpdate();
            try
            {
                foreach (var item in filteredItems)
                {
                    ReloadItem(item);
                }
            }
            finally
            {
                EndUpdate();
            }

            void ReloadItem(IPropertyGridItem item)
            {
                var p = item.PropInfo;
                var instance = item.Instance;
                if (instance == null || p == null)
                    return;
                object? propValue = p.GetValue(instance, null);
                variant.AsObject = propValue;
                SetPropertyValueAsVariant(item, variant);
            }
        }

        internal static IntPtr GetEditorByName(string editorName)
        {
            return Native.PropertyGrid.GetEditorByName(editorName);
        }

        internal IntPtr GetPropertyValidator(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyValidator(prop.Handle);
        }

        internal Color GetCaptionBackgroundColor()
        {
            return NativeControl.GetCaptionBackgroundColor();
        }

        internal Color GetCaptionForegroundColor()
        {
            return NativeControl.GetCaptionForegroundColor();
        }

        internal void SetEmptySpaceColor(Color col)
        {
            NativeControl.SetEmptySpaceColor(col);
        }

        internal void SetLineColor(Color col)
        {
            NativeControl.SetLineColor(col);
        }

        internal void SetMarginColor(Color col)
        {
            NativeControl.SetMarginColor(col);
        }

        internal void SetSelectionBackgroundColor(Color col)
        {
            NativeControl.SetSelectionBackgroundColor(col);
        }

        internal void SetSelectionTextColor(Color col)
        {
            NativeControl.SetSelectionTextColor(col);
        }

        internal Color GetCellBackgroundColor()
        {
            return NativeControl.GetCellBackgroundColor();
        }

        internal Color GetCellDisabledTextColor()
        {
            return NativeControl.GetCellDisabledTextColor();
        }

        internal Color GetCellTextColor()
        {
            return NativeControl.GetCellTextColor();
        }

        internal bool CommitChangesFromEditor()
        {
            return NativeControl.CommitChangesFromEditor(0);
        }

        internal IntPtr GetPropertyImage(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyImage(prop.Handle);
        }

        internal void SetPropertyEditor(IPropertyGridItem prop, IntPtr editor)
        {
            NativeControl.SetPropertyEditor(prop.Handle, editor);
        }

        internal virtual void OnPropertyCreated(
            IPropertyGridItem item,
            object instance,
            PropertyInfo p)
        {
            if (!p.CanWrite)
                SetPropertyReadOnly(item, true);
            if (AssemblyUtils.GetNullable(p))
                SetPropertyValueUnspecified(item);
        }

        internal virtual void OnPropertyCreated(IPropertyGridItem item)
        {
        }

        internal IPropertyGridItem? GetPropertyParent(IPropertyGridItem prop)
        {
            var result = NativeControl.GetPropertyParent(prop.Handle);
            return PtrToItem(result);
        }

        internal IPropertyGridItem? GetFirstChild(IPropertyGridItem prop)
        {
            var result = NativeControl.GetFirstChild(prop.Handle);
            return PtrToItem(result);
        }

        internal IPropertyGridItem? GetPropertyCategory(IPropertyGridItem prop)
        {
            var result = NativeControl.GetPropertyCategory(prop.Handle);
            return PtrToItem(result);
        }

        internal IPropertyGridItem? GetFirst(PropertyGridIteratorFlags flags)
        {
            var result = NativeControl.GetFirst((int)flags);
            return PtrToItem(result);
        }

        internal IPropertyGridItem? GetProperty(string name)
        {
            var result = NativeControl.GetProperty(name);
            return PtrToItem(result);
        }

        internal IPropertyGridItem? GetPropertyByLabel(string label)
        {
            var result = NativeControl.GetPropertyByLabel(label);
            return PtrToItem(result);
        }

        internal IPropertyGridItem? GetPropertyByName(string name)
        {
            var result = NativeControl.GetPropertyByName(name);
            return PtrToItem(result);
        }

        internal IPropertyGridItem? GetPropertyByNameAndSubName(string name, string subname)
        {
            var result = NativeControl.GetPropertyByNameAndSubName(name, subname);
            return PtrToItem(result);
        }

        internal IPropertyGridItem? GetSelection()
        {
            var result = NativeControl.GetSelection();
            return PtrToItem(result);
        }

        internal void DeleteProperty(IPropertyGridItem prop)
        {
            NativeControl.DeleteProperty(prop.Handle);
            items.Remove(prop.Handle);
        }

        internal void RaiseProcessException(PropertyGridExceptionEventArgs e)
        {
            OnProcessException(e);
            ProcessException?.Invoke(this, e);
        }

        internal void RaisePropertySelected(EventArgs e)
        {
            OnPropertySelected(e);
            PropertySelected?.Invoke(this, e);
        }

        internal void RaisePropertyChanged(EventArgs e)
        {
            OnPropertyChanged(e);
            PropertyChanged?.Invoke(this, e);
        }

        internal void SetupTextCtrlValue(string text)
        {
            NativeControl.SetupTextCtrlValue(text);
        }

        internal void RaisePropertyChanging(CancelEventArgs e)
        {
            OnPropertyChanging(e);
            PropertyChanging?.Invoke(this, e);
        }

        internal void RaisePropertyHighlighted(EventArgs e)
        {
            OnPropertyHighlighted(e);
            PropertyHighlighted?.Invoke(this, e);
        }

        internal void RaisePropertyRightClick(EventArgs e)
        {
            OnPropertyRightClick(e);
            PropertyRightClick?.Invoke(this, e);
        }

        internal void RaisePropertyDoubleClick(EventArgs e)
        {
            OnPropertyDoubleClick(e);
            PropertyDoubleClick?.Invoke(this, e);
        }

        internal void RaiseItemCollapsed(EventArgs e)
        {
            OnItemCollapsed(e);
            ItemCollapsed?.Invoke(this, e);
        }

        internal void SetCaptionBackgroundColor(Color col)
        {
            NativeControl.SetCaptionBackgroundColor(col);
        }

        internal void SetCaptionTextColor(Color col)
        {
            NativeControl.SetCaptionTextColor(col);
        }

        internal void SetCellBackgroundColor(Color col)
        {
            NativeControl.SetCellBackgroundColor(col);
        }

        internal void SetCellDisabledTextColor(Color col)
        {
            NativeControl.SetCellDisabledTextColor(col);
        }

        internal void SetCellTextColor(Color col)
        {
            NativeControl.SetCellTextColor(col);
        }

        internal void RaiseItemExpanded(EventArgs e)
        {
            OnItemExpanded(e);
            ItemExpanded?.Invoke(this, e);
        }

        internal void RaiseColEndDrag(EventArgs e)
        {
            OnColEndDrag(e);
            ColEndDrag?.Invoke(this, e);
        }

        internal void RaiseButtonClick(EventArgs e)
        {
            OnButtonClick(e);
            ButtonClick?.Invoke(this, e);
        }

        internal void RaiseColDragging(EventArgs e)
        {
            OnColDragging(e);
            ColDragging?.Invoke(this, e);
        }

        internal Color GetEmptySpaceColor()
        {
            return NativeControl.GetEmptySpaceColor();
        }

        internal Color GetLineColor()
        {
            return NativeControl.GetLineColor();
        }

        internal Color GetMarginColor()
        {
            return NativeControl.GetMarginColor();
        }

        internal Color GetSelectionBackgroundColor()
        {
            return NativeControl.GetSelectionBackgroundColor();
        }

        internal Color GetSelectionForegroundColor()
        {
            return NativeControl.GetSelectionForegroundColor();
        }

        internal void EndAddChildren(IPropertyGridItem prop)
        {
            NativeControl.EndAddChildren(prop.Handle);
        }

        internal IPropertyGridItem CreateCursorProperty(
           string label,
           string? name = null,
           int value = 0)
        {
            var handle = NativeControl.CreateCursorProperty(label, CorrectPropName(name), value);
            var result = new PropertyGridItem(this, handle, label, name, value);
            OnPropertyCreated(result);
            return result;
        }

        internal void RaiseColBeginDrag(CancelEventArgs e)
        {
            OnColBeginDrag(e);
            ColBeginDrag?.Invoke(this, e);
        }

        internal void RaiseLabelEditEnding(CancelEventArgs e)
        {
            OnLabelEditEnding(e);
            LabelEditEnding?.Invoke(this, e);
        }

        internal void RaiseLabelEditBegin(CancelEventArgs e)
        {
            OnLabelEditBegin(e);
            LabelEditBegin?.Invoke(this, e);
        }

        internal PropertyGridVariant ToVariant(object? value)
        {
            variant.AsObject = value;
            return variant;
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
            var prop = EventProperty;
            if (prop == null)
                return;

            var setValue = ApplyFlags.HasFlag(PropertyGridApplyFlags.PropInfoSetValue);

            if (setValue && prop.Instance != null && prop.PropInfo != null)
            {
                AvoidException(() =>
                {
                    var variant = EventPropValueAsVariant;
                    var newValue = variant.GetCompatibleValue(prop.PropInfo);
                    prop.PropInfo.SetValue(prop.Instance, newValue);

                    var reload = ApplyFlags.HasFlag(PropertyGridApplyFlags.ReloadAfterSetValue);
                    var reloadAll = ApplyFlags.HasFlag(PropertyGridApplyFlags.ReloadAllAfterSetValue);

                    if (reloadAll)
                        ReloadValues();
                    else
                        if (reload)
                        ReloadValues(prop.Instance, prop.PropInfo);
                });
            }

            var propEvent = ApplyFlags.HasFlag(PropertyGridApplyFlags.PropEvent);
            if (propEvent)
            {
                AvoidException(() => { prop.RaisePropertyChanged(); });
            }
        }

        /// <summary>
        /// Executes <see cref="Action"/> and calls <see cref="ProcessException"/>
        /// event if exception was raised during execution.
        /// </summary>
        /// <param name="action"></param>
        protected virtual void AvoidException(Action action)
        {
            try
            {
                action();
            }
            catch (Exception exception)
            {
                PropertyGridExceptionEventArgs data = new(exception);
                RaiseProcessException(data);
                if (data.ThrowIt)
                    throw;
            }
        }

        /// <summary>
        /// Called when an exception need to be processed.
        /// </summary>
        /// <param name="e">An <see cref="PropertyGridExceptionEventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnProcessException(PropertyGridExceptionEventArgs e)
        {
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

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreatePropertyGridHandler(this);
        }

        private static string CorrectPropName(string? name)
        {
            if (name is null)
                return NameAsLabel;
            return name;
        }

        private IPropertyGridItem? PtrToItem(IntPtr ptr)
        {
            if (items.TryGetValue(ptr, out IPropertyGridItem? result))
                return result;
            return null;
        }
    }
}