using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private readonly AdvDictionary<PropertyGridItemHandle, IPropertyGridItem> items = new();
        private readonly HashSet<string> ignorePropNames = new();

        private int supressIgnoreProps;

        static PropertyGrid()
        {
            RegisterPropCreateFunc(typeof(Color), FuncCreatePropertyAsColor);
            RegisterPropCreateFunc(typeof(Font), FuncCreatePropertyAsFont);
            RegisterPropCreateFunc(typeof(Brush), FuncCreatePropertyAsBrush);
            RegisterPropCreateFunc(typeof(Pen), FuncCreatePropertyAsPen);
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
        /// Gets or sets whether <see cref="Color"/> properties have alpha chanel.
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
        /// Gets property value used in the event handler as <see cref="IPropertyGridVariant"/>.
        /// </summary>
        [Browsable(false)]
        public virtual IPropertyGridVariant EventPropValueAsVariant
        {
            get
            {
                return Handler.EventPropValueAsVariant;
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
        /// Gets or sets validation failure behavior flags used in the event handler.
        /// </summary>
        [Browsable(false)]
        public virtual PropertyGridValidationFailure EventValidationFailureBehavior
        {
            get
            {
                return Handler.EventValidationFailureBehavior;
            }

            set
            {
                Handler.EventValidationFailureBehavior = value;
            }
        }

        /// <inheritdoc cref="PropertyGridColors.CaptionBackgroundColor"/>
        public Color CaptionBackgroundColor
        {
            get => Handler.GetCaptionBackgroundColor();
            set => Handler.SetCaptionBackgroundColor(value);
        }

        /// <inheritdoc cref="PropertyGridColors.CaptionForegroundColor"/>
        public Color CaptionForegroundColor
        {
            get => Handler.GetCaptionForegroundColor();
            set => Handler.SetCaptionTextColor(value);
        }

        /// <inheritdoc cref="PropertyGridColors.SelectionForegroundColor"/>
        public Color SelectionForegroundColor
        {
            get => Handler.GetSelectionForegroundColor();
            set => Handler.SetSelectionTextColor(value);
        }

        /// <inheritdoc cref="PropertyGridColors.CellBackgroundColor"/>
        public Color CellBackgroundColor
        {
            get => Handler.GetCellBackgroundColor();
            set => Handler.SetCellBackgroundColor(value);
        }

        /// <inheritdoc cref="PropertyGridColors.CellDisabledTextColor"/>
        public Color CellDisabledTextColor
        {
            get => Handler.GetCellDisabledTextColor();
            set => Handler.SetCellDisabledTextColor(value);
        }

        /// <inheritdoc cref="PropertyGridColors.CellTextColor"/>
        public Color CellTextColor
        {
            get => Handler.GetCellTextColor();
            set => Handler.SetCellTextColor(value);
        }

        /// <inheritdoc cref="PropertyGridColors.EmptySpaceColor"/>
        public Color EmptySpaceColor
        {
            get => Handler.GetEmptySpaceColor();
            set => Handler.SetEmptySpaceColor(value);
        }

        /// <inheritdoc cref="PropertyGridColors.LineColor"/>
        public Color LineColor
        {
            get => Handler.GetLineColor();
            set => Handler.SetLineColor(value);
        }

        /// <inheritdoc cref="PropertyGridColors.MarginColor"/>
        public Color MarginColor
        {
            get => Handler.GetMarginColor();
            set => Handler.SetMarginColor(value);
        }

        /// <inheritdoc cref="PropertyGridColors.SelectionBackgroundColor"/>
        public Color SelectionBackgroundColor
        {
            get => Handler.GetSelectionBackgroundColor();
            set => Handler.SetSelectionBackgroundColor(value);
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
                return Handler.EventColumn;
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
                return Handler.EventProperty;
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
                return Handler.EventPropertyName;
            }
        }

        /// <summary>
        /// Gets or sets validation failure message used in the event handler.
        /// </summary>
        [Browsable(false)]
        public virtual string EventValidationFailureMessage
        {
            get
            {
                return Handler.EventValidationFailureMessage;
            }

            set
            {
                Handler.EventValidationFailureMessage = value;
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
                return Handler.CreateStyle;
            }

            set
            {
                Handler.CreateStyle = value;
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
                return Handler.CreateStyleEx;
            }

            set
            {
                Handler.CreateStyleEx = value;
            }
        }

        /// <summary>
        /// Returns <see cref="IPropertyGridFactory"/> instance.
        /// </summary>
        [Browsable(false)]
        public IPropertyGridFactory Factory => DefaultFactory;

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public bool HasBorder
        {
            get
            {
                return Handler.HasBorder;
            }

            set
            {
                Handler.HasBorder = value;
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.PropertyGrid;

        /// <summary>
        /// Gets control handler.
        /// </summary>
        [Browsable(false)]
        public new IPropertyGridHandler Handler
        {
            get
            {
                CheckDisposed();
                return (IPropertyGridHandler)base.Handler;
            }
        }

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
        /// <returns><c>true</c> if operation successfull, <c>false</c> otherwise.</returns>
        public static bool SetCustomLabel<T>(string propName, string label)
            where T : class
        {
            var propInfo = typeof(T).GetProperty(propName);
            if (propInfo == null)
                return false;
            var propRegistry = GetPropRegistry(typeof(T), propInfo);
            propRegistry.NewItemParams.Label = label;
            return true;
        }

        /// <summary>
        /// Checks system screen design used for laying out various dialogs.
        /// </summary>
        public bool IsSmallScreen()
        {
            return Handler.IsSmallScreen();
        }

        /// <summary>
        /// Creates new variant instance for use with <see cref="PropertyGrid"/>
        /// </summary>
        public virtual IPropertyGridVariant CreateVar()
        {
            return Handler.CreateVariant();
        }

        /// <summary>
        /// Enables or disables automatic translation for enum list labels and
        /// flags child property labels.
        /// </summary>
        /// <param name="enable"><c>true</c> enables automatic translation, <c>false</c>
        /// disables it.</param>
        public void AutoGetTranslation(bool enable)
        {
            Handler.AutoGetTranslation(enable);
        }

        /// <summary>
        /// Registers all type handlers for use in <see cref="PropertyGrid"/>.
        /// </summary>
        public void InitAllTypeHandlers()
        {
            Handler.InitAllTypeHandlers();
        }

        /// <summary>
        /// Registers additional editors for use in <see cref="PropertyGrid"/>.
        /// </summary>
        public void RegisterAdditionalEditors()
        {
            Handler.RegisterAdditionalEditors();
        }

        /// <summary>
        /// Sets string constants for <c>true</c> and <c>false</c> words
        /// used in <see cref="bool"/> properties.
        /// </summary>
        /// <param name="trueChoice"></param>
        /// <param name="falseChoice"></param>
        public void SetBoolChoices(string trueChoice, string falseChoice)
        {
            Handler.SetBoolChoices(trueChoice, falseChoice);
        }

        /// <summary>
        /// Creates <see cref="string"/> property with ellipsis button which opens
        /// <see cref="FileDialog"/> when pressed.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        /// <remarks>
        /// In order to setup filename and attached <see cref="FileDialog"/>, you can use
        /// <see cref="PropertyGrid.SetPropertyKnownAttribute"/> for
        /// <see cref="PropertyGridItemAttrId.DialogTitle"/>,
        /// <see cref="PropertyGridItemAttrId.InitialPath"/>,
        /// <see cref="PropertyGridItemAttrId.ShowFullPath"/>,
        /// <see cref="PropertyGridItemAttrId.Wildcard"/> attributes.
        /// </remarks>
        public virtual IPropertyGridItem CreateFilenameItem(
            string label,
            string? name = null,
            string? value = null,
            IPropertyGridNewItemParams? prm = null)
        {
            value ??= string.Empty;
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateFilenameProperty(
                label,
                CorrectPropName(name),
                value!);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.StringFilename;
            result.CanHaveCustomEllipsis = false;
            OnPropertyCreated(result, prm);
            return result;
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
        /// Creates <see cref="string"/> property with ellipsis button which opens
        /// <see cref="SelectDirectoryDialog"/> when pressed.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        /// <remarks>
        /// In order to setup folder name and attached <see cref="SelectDirectoryDialog"/>,
        /// you can use
        /// <see cref="PropertyGrid.SetPropertyKnownAttribute"/> for
        /// <see cref="PropertyGridItemAttrId.DialogTitle"/>,
        /// <see cref="PropertyGridItemAttrId.InitialPath"/>,
        /// <see cref="PropertyGridItemAttrId.ShowFullPath"/> attributes.
        /// </remarks>
        public virtual IPropertyGridItem CreateDirItem(
            string label,
            string? name = null,
            string? value = null,
            IPropertyGridNewItemParams? prm = null)
        {
            value ??= string.Empty;
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateDirProperty(
                label,
                CorrectPropName(name),
                value!);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.StringDirectory;
            result.CanHaveCustomEllipsis = false;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates <see cref="string"/> property with ellipsis button which opens
        /// <see cref="FileDialog"/> when pressed.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        /// <remarks>
        /// In order to setup filename and attached <see cref="FileDialog"/>, you can use
        /// <see cref="PropertyGrid.SetPropertyKnownAttribute"/> for
        /// <see cref="PropertyGridItemAttrId.DialogTitle"/>,
        /// <see cref="PropertyGridItemAttrId.InitialPath"/>,
        /// <see cref="PropertyGridItemAttrId.ShowFullPath"/> attributes.
        /// </remarks>
        /// <remarks>
        /// This function is similar to <see cref="CreateFilenameItem"/> but wildcards
        /// are limited to supported image file extensions.
        /// </remarks>
        public virtual IPropertyGridItem CreateImageFilenameItem(
            string label,
            string? name = null,
            string? value = null,
            IPropertyGridNewItemParams? prm = null)
        {
            value ??= string.Empty;
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateImageFilenameProperty(
                label,
                CorrectPropName(name),
                value!);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.StringImageFilename;
            result.CanHaveCustomEllipsis = false;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates <see cref="string"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateStringItem(
            string label,
            string? name = null,
            string? value = null,
            IPropertyGridNewItemParams? prm = null)
        {
            value ??= string.Empty;
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateStringProperty(
                label,
                CorrectPropName(name),
                value!);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.String;
            OnPropertyCreated(result, prm);
            return result;
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

        /// <summary>
        /// Creates <see cref="bool"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateBoolItem(
            string label,
            string? name = null,
            bool value = false,
            IPropertyGridNewItemParams? prm = null)
        {
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateBoolProperty(
                label,
                CorrectPropName(name),
                value);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.Bool;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates <see cref="long"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateLongItem(
            string label,
            string? name = null,
            long value = 0,
            IPropertyGridNewItemParams? prm = null)
        {
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateIntProperty(
                label,
                CorrectPropName(name),
                value);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.Int64;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <inheritdoc/>
        public override void OnLayout()
        {
        }

        /// <summary>
        /// Creates <see cref="double"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateDoubleItem(
            string label,
            string? name = null,
            double value = default,
            IPropertyGridNewItemParams? prm = null)
        {
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateFloatProperty(
                label,
                CorrectPropName(name),
                value);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.Double;
            SetPropertyMinMax(result, TypeCode.Double);
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates <see cref="float"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateFloatItem(
            string label,
            string? name = null,
            float value = default,
            IPropertyGridNewItemParams? prm = null)
        {
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateFloatProperty(
                label,
                CorrectPropName(name),
                value);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.Single;
            SetPropertyMinMax(result, TypeCode.Single);
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates <see cref="Color"/> property with system colors.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateSystemColorItem(
            string label,
            string? name,
            Color value,
            IPropertyGridNewItemParams? prm = null)
        {
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateSystemColorProperty(
                label,
                CorrectPropName(name),
                value);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.ColorSystem;
            result.CanHaveCustomEllipsis = false;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates <see cref="Color"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateColorItem(
            string label,
            string? name,
            Color value,
            IPropertyGridNewItemParams? prm = null)
        {
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateColorProperty(
                label,
                CorrectPropName(name),
                value);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.Color;
            result.CanHaveCustomEllipsis = false;
            OnPropertyCreated(result, prm);
            return result;
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
        /// Creates <see cref="ulong"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateULongItem(
            string label,
            string? name = null,
            ulong value = 0,
            IPropertyGridNewItemParams? prm = null)
        {
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateUIntProperty(
                label,
                CorrectPropName(name),
                value);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.UInt64;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates <see cref="string"/> property with additional edit dialog for
        /// entering long values.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateLongStringItem(
            string label,
            string? name = null,
            string? value = null,
            IPropertyGridNewItemParams? prm = null)
        {
            value ??= string.Empty;
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateLongStringProperty(
                label,
                CorrectPropName(name),
                value);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.StringLong;
            result.CanHaveCustomEllipsis = false;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates <see cref="DateTime"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateDateItem(
            string label,
            string? name = null,
            DateTime? value = null,
            IPropertyGridNewItemParams? prm = null)
        {
            DateTime dt;

            if (value == null)
                dt = DateTime.Now;
            else
                dt = value.Value;

            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateDateProperty(
                label,
                CorrectPropName(name),
                dt);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.Date;
            result.CanHaveCustomEllipsis = false;
            OnPropertyCreated(result, prm);
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
                    AddChildren();

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
                        supressIgnoreProps++;
                        try
                        {
                            var children = CreateProps(value, true);
                            result.AddChildren(children);
                        }
                        finally
                        {
                            supressIgnoreProps--;
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
                    if(args.PropName is not null)
                        propName = args.PropName;
                    if(args.Label is not null)
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
                        prop = CreatePropertyAsSByte(label, propName, instance, p);
                        break;
                    case TypeCode.Int16:
                        prop = CreatePropertyAsInt16(label, propName, instance, p);
                        break;
                    case TypeCode.Int32:
                        prop = CreatePropertyAsInt(label, propName, instance, p);
                        break;
                    case TypeCode.Int64:
                        prop = CreatePropertyAsLong(label, propName, instance, p);
                        break;
                    case TypeCode.Byte:
                        prop = CreatePropertyAsByte(label, propName, instance, p);
                        break;
                    case TypeCode.UInt32:
                        prop = CreatePropertyAsUInt(label, propName, instance, p);
                        break;
                    case TypeCode.UInt16:
                        prop = CreatePropertyAsUInt16(label, propName, instance, p);
                        break;
                    case TypeCode.UInt64:
                        prop = CreatePropertyAsULong(label, propName, instance, p);
                        break;
                    case TypeCode.Single:
                        prop = CreatePropertyAsFloat(label, propName, instance, p);
                        break;
                    case TypeCode.Double:
                        prop = CreatePropertyAsDouble(label, propName, instance, p);
                        break;
                    case TypeCode.Decimal:
                        prop = CreatePropertyAsDecimal(label, propName, instance, p);
                        break;
                    case TypeCode.DateTime:
                        prop = CreatePropertyAsDate(label, propName, instance, p);
                        break;
                    case TypeCode.Char:
                        prop = CreatePropertyAsChar(label, propName, instance, p);
                        break;
                    case TypeCode.String:
                        prop = CreatePropertyAsString(label, propName, instance, p);
                        break;
                }
            }

            prop!.Instance = instance;
            prop!.PropInfo = p;
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
        /// Creates <see cref="sbyte"/> property.
        /// </summary>
        /// <inheritdoc cref="CreatePropertyAsDummy"/>
        public virtual IPropertyGridItem CreatePropertyAsSByte(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo)
        {
            var value = AssemblyUtils.GetPropValue<sbyte>(instance, propInfo, default);
            var prm = ConstructNewItemParams(instance, propInfo);
            var prop = CreateSByteItem(label, name, value, prm);
            OnPropertyCreated(prop, instance, propInfo, prm);
            return prop;
        }

        /// <summary>
        /// Creates <see cref="short"/> property.
        /// </summary>
        /// <inheritdoc cref="CreatePropertyAsDummy"/>
        public virtual IPropertyGridItem CreatePropertyAsInt16(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo)
        {
            var value = AssemblyUtils.GetPropValue<short>(instance, propInfo, default);
            var prm = ConstructNewItemParams(instance, propInfo);
            var prop = CreateInt16Item(label, name, value, prm);
            OnPropertyCreated(prop, instance, propInfo, prm);
            return prop;
        }

        /// <summary>
        /// Creates <see cref="int"/> property.
        /// </summary>
        /// <inheritdoc cref="CreatePropertyAsDummy"/>
        public virtual IPropertyGridItem CreatePropertyAsInt(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo)
        {
            var value = AssemblyUtils.GetPropValue<int>(instance, propInfo, default);
            var prm = ConstructNewItemParams(instance, propInfo);
            var prop = CreateIntItem(label, name, value, prm);
            OnPropertyCreated(prop, instance, propInfo, prm);
            return prop;
        }

        /// <summary>
        /// Creates <see cref="long"/> property.
        /// </summary>
        /// <inheritdoc cref="CreatePropertyAsDummy"/>
        public virtual IPropertyGridItem CreatePropertyAsLong(
                    string label,
                    string name,
                    object instance,
                    PropertyInfo propInfo)
        {
            var value = AssemblyUtils.GetPropValue<long>(instance, propInfo, default);
            var prm = ConstructNewItemParams(instance, propInfo);
            var prop = CreateLongItem(label, name, value, prm);
            OnPropertyCreated(prop, instance, propInfo, prm);
            return prop;
        }

        /// <summary>
        /// Creates <see cref="byte"/> property.
        /// </summary>
        /// <inheritdoc cref="CreatePropertyAsDummy"/>
        public virtual IPropertyGridItem CreatePropertyAsByte(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo)
        {
            var value = AssemblyUtils.GetPropValue<byte>(instance, propInfo, default);
            var prm = ConstructNewItemParams(instance, propInfo);
            var prop = CreateByteItem(label, name, value, prm);
            OnPropertyCreated(prop, instance, propInfo, prm);
            return prop;
        }

        /// <summary>
        /// Creates <see cref="uint"/> property.
        /// </summary>
        /// <inheritdoc cref="CreatePropertyAsDummy"/>
        public virtual IPropertyGridItem CreatePropertyAsUInt(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo)
        {
            var value = AssemblyUtils.GetPropValue<uint>(instance, propInfo, default);
            var prm = ConstructNewItemParams(instance, propInfo);
            var prop = CreateUIntItem(label, name, value, prm);
            OnPropertyCreated(prop, instance, propInfo, prm);
            return prop;
        }

        /// <summary>
        /// Creates <see cref="ushort"/> property.
        /// </summary>
        /// <inheritdoc cref="CreatePropertyAsDummy"/>
        public virtual IPropertyGridItem CreatePropertyAsUInt16(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo)
        {
            var value = AssemblyUtils.GetPropValue<ushort>(instance, propInfo, default);
            var prm = ConstructNewItemParams(instance, propInfo);
            var prop = CreateUInt16Item(label, name, value, prm);
            OnPropertyCreated(prop, instance, propInfo, prm);
            return prop;
        }

        /// <summary>
        /// Creates <see cref="ulong"/> property.
        /// </summary>
        /// <inheritdoc cref="CreatePropertyAsDummy"/>
        public virtual IPropertyGridItem CreatePropertyAsULong(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo)
        {
            var value = AssemblyUtils.GetPropValue<ulong>(instance, propInfo, default);
            var prm = ConstructNewItemParams(instance, propInfo);
            var prop = CreateULongItem(label, name, value, prm);
            OnPropertyCreated(prop, instance, propInfo, prm);
            return prop;
        }

        /// <summary>
        /// Creates <see cref="float"/> property.
        /// </summary>
        /// <inheritdoc cref="CreatePropertyAsDummy"/>
        public virtual IPropertyGridItem CreatePropertyAsFloat(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo)
        {
            var value = AssemblyUtils.GetPropValue<float>(instance, propInfo, default);
            var prm = ConstructNewItemParams(instance, propInfo);
            var prop = CreateFloatItem(label, name, value, prm);
            OnPropertyCreated(prop, instance, propInfo, prm);
            return prop;
        }

        /// <summary>
        /// Creates <see cref="double"/> property.
        /// </summary>
        /// <inheritdoc cref="CreatePropertyAsDummy"/>
        public virtual IPropertyGridItem CreatePropertyAsDouble(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo)
        {
            var value = AssemblyUtils.GetPropValue<double>(instance, propInfo, default);
            var prm = ConstructNewItemParams(instance, propInfo);
            var prop = CreateDoubleItem(label, name, value, prm);
            OnPropertyCreated(prop, instance, propInfo, prm);
            return prop;
        }

        /// <summary>
        /// Creates <see cref="decimal"/> property.
        /// </summary>
        /// <inheritdoc cref="CreatePropertyAsDummy"/>
        public virtual IPropertyGridItem CreatePropertyAsDecimal(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo)
        {
            var value = AssemblyUtils.GetPropValue<decimal>(instance, propInfo, default);
            var prm = ConstructNewItemParams(instance, propInfo);
            var prop = CreateDecimalItem(label, name, value, prm);
            OnPropertyCreated(prop, instance, propInfo, prm);
            return prop;
        }

        /// <summary>
        /// Creates <see cref="DateTime"/> property.
        /// </summary>
        /// <inheritdoc cref="CreatePropertyAsDummy"/>
        public virtual IPropertyGridItem CreatePropertyAsDate(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo)
        {
            var value = AssemblyUtils.GetPropValue<DateTime>(instance, propInfo, default);
            var prm = ConstructNewItemParams(instance, propInfo);
            var prop = CreateDateItem(label, name, value, prm);
            OnPropertyCreated(prop, instance, propInfo, prm);
            return prop;
        }

        /// <summary>
        /// Creates <see cref="char"/> property.
        /// </summary>
        /// <inheritdoc cref="CreatePropertyAsDummy"/>
        public virtual IPropertyGridItem CreatePropertyAsChar(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo)
        {
            var value = AssemblyUtils.GetPropValue<char>(instance, propInfo, default);
            var prm = ConstructNewItemParams(instance, propInfo);
            var prop = CreateCharItem(label, name, value, prm);
            OnPropertyCreated(prop, instance, propInfo, prm);
            return prop;
        }

        /// <summary>
        /// Creates <see cref="string"/> property.
        /// </summary>
        /// <inheritdoc cref="CreatePropertyAsDummy"/>
        public virtual IPropertyGridItem CreatePropertyAsString(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo)
        {
            var value = AssemblyUtils.GetPropValue<string>(instance, propInfo, string.Empty);
            var prm = ConstructNewItemParams(instance, propInfo);
            var prop = CreateStringItemWithKind(label, name, value, prm);
            OnPropertyCreated(prop, instance, propInfo, prm);
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
                if (supressIgnoreProps == 0 && ignorePropNames.Contains(propName))
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
        /// Creates enumeration property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="choices">Enumeration elements.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateChoicesItem(
            string label,
            string? name,
            IPropertyGridChoices choices,
            object? value = null,
            IPropertyGridNewItemParams? prm = null)
        {
            value ??= 0;
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateEnumProperty(
                label,
                CorrectPropName(name),
                choices,
                (int)value);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.Enum;
            result.Choices = choices;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates editable enumeration property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="choices">Enumeration elements.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateEditEnumItem(
            string label,
            string? name,
            IPropertyGridChoices choices,
            string? value = null,
            IPropertyGridNewItemParams? prm = null)
        {
            value ??= string.Empty;
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateEditEnumProperty(
                label,
                CorrectPropName(name),
                choices,
                value);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.EnumEditable;
            result.Choices = choices;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Creates flags property (like enumeration with Flags attribute).
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="choices">Elements.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreateFlagsItem(
            string label,
            string? name,
            IPropertyGridChoices choices,
            object? value = null,
            IPropertyGridNewItemParams? prm = null)
        {
            value ??= 0;
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateFlagsProperty(
                label,
                CorrectPropName(name),
                choices,
                (int)value);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);

            result.PropertyEditorKind = PropertyGridEditKindAll.EnumFlags;
            result.Choices = choices;
            OnPropertyCreated(result, prm);
            return result;
        }

        /// <summary>
        /// Deletes all items from the property grid.
        /// </summary>
        public virtual void Clear()
        {
            items.Clear();
            Handler.Clear();
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
        /// Adds item to the property grid.
        /// </summary>
        /// <param name="prop">Item to add.</param>
        /// <param name="parent">Parent item or null.</param>
        public virtual void Add(IPropertyGridItem? prop, IPropertyGridItem? parent = null)
        {
            if (prop == null)
                return;
            SetAsCheckBox(prop);

            if (parent == null)
                Handler.Append(prop);
            else
                Handler.AppendIn(parent, prop);

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
                var kindIsOk = kind == PropertyGridEditKindAll.Bool || p.IsFlags;

                if (BoolAsCheckBox && kindIsOk)
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
        /// <param name="prm">Item create params.</param>
        /// <returns>Category property for use with <see cref="PropertyGrid.Add"/>.</returns>
        public virtual IPropertyGridItem CreatePropCategory(
            string label,
            string? name = null,
            IPropertyGridNewItemParams? prm = null)
        {
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreatePropCategory(
                label,
                CorrectPropName(name));
            var result = new PropertyGridItem(this, handle, label, name, null, prm);

            result.IsCategory = true;
            OnPropertyCreated(result, prm);
            return result;
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
        /// Gets property name of the <see cref="IPropertyGridItem"/>.
        /// </summary>
        /// <param name="property">Property Item.</param>
        public virtual string GetPropertyName(IPropertyGridItem property)
        {
            return Handler.GetPropertyName(property);
        }

        /// <summary>
        /// Sorts properties.
        /// </summary>
        /// <param name="topLevelOnly"><c>true</c> to sort only top level properties,
        /// <c>false</c> otherwise.</param>
        public virtual void Sort(bool topLevelOnly = false)
        {
            var flags = topLevelOnly ? PropertyGridItemValueFlags.SortTopLevelOnly : 0;
            Handler.Sort(flags);
        }

        /// <summary>
        /// Sets property readonly flag.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="isSet">New Readonly flag value.</param>
        /// <param name="recurse"><c>true</c> to change readonly flag recursively
        /// for child propereties, <c>false</c> otherwise.</param>
        public virtual void SetPropertyReadOnly(
            IPropertyGridItem prop,
            bool isSet,
            bool recurse = true)
        {
            var flags = recurse
                ? PropertyGridItemValueFlags.Recurse : PropertyGridItemValueFlags.DontRecurse;

            Handler.SetPropertyReadOnly(prop, isSet, flags);
        }

        /// <summary>
        /// Allows property value to be unspecified.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual void SetPropertyValueUnspecified(IPropertyGridItem prop)
        {
            Handler.SetPropertyValueUnspecified(prop);
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
            Handler.AppendIn(prop, newproperty);
        }

        /// <summary>
        /// Collapses (hides) all sub properties of the given property.
        /// </summary>
        /// <param name="prop">Property item to collapse.</param>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        public virtual bool Collapse(IPropertyGridItem? prop)
        {
            if (prop is null)
                return false;
            return Handler.Collapse(prop);
        }

        /// <summary>
        /// Removes property from the <see cref="PropertyGrid"/>.
        /// </summary>
        /// <param name="prop">Property item to remove.</param>
        public virtual void RemoveProperty(IPropertyGridItem? prop)
        {
            if (prop is null)
                return;
            Handler.RemoveProperty(prop);
            items.Remove(prop.Handle);
        }

        /// <summary>
        /// Disables property.
        /// </summary>
        /// <param name="prop">Property item to disable.</param>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        public virtual bool DisableProperty(IPropertyGridItem prop)
        {
            return Handler.DisableProperty(prop);
        }

        /// <summary>
        /// Changes enabled state of the property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="enable">New enabled state value.</param>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        public virtual bool EnableProperty(IPropertyGridItem? prop, bool enable = true)
        {
            if (prop is null)
                return false;
            return Handler.EnableProperty(prop, enable);
        }

        /// <summary>
        /// Expands (shows) all sub properties of the given property.
        /// </summary>
        /// <param name="prop">Property item to expand.</param>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        public virtual bool Expand(IPropertyGridItem? prop)
        {
            if (prop is null)
                return false;
            return Handler.Expand(prop);
        }

        /// <summary>
        /// Gets client data associated with the property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual IntPtr GetPropertyClientData(IPropertyGridItem prop)
        {
            return Handler.GetPropertyClientData(prop);
        }

        /// <summary>
        /// Gets help string associated with the property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual string GetPropertyHelpString(IPropertyGridItem prop)
        {
            return Handler.GetPropertyHelpString(prop);
        }

        /// <summary>
        /// Gets label associated with the property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual string GetPropertyLabel(IPropertyGridItem prop)
        {
            return Handler.GetPropertyLabel(prop);
        }

        /// <summary>
        /// Gets property value as <see cref="string"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual string GetPropertyValueAsString(IPropertyGridItem prop)
        {
            return Handler.GetPropertyValueAsString(prop);
        }

        /// <summary>
        /// Gets property value as <see cref="long"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual long GetPropertyValueAsLong(IPropertyGridItem prop)
        {
            return Handler.GetPropertyValueAsLong(prop);
        }

        /// <summary>
        /// Gets property value as <see cref="ulong"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual ulong GetPropertyValueAsULong(IPropertyGridItem prop)
        {
            return Handler.GetPropertyValueAsULong(prop);
        }

        /// <summary>
        /// Gets property value as <see cref="int"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual int GetPropertyValueAsInt(IPropertyGridItem prop)
        {
            return Handler.GetPropertyValueAsInt(prop);
        }

        /// <summary>
        /// Gets property value as <see cref="IPropertyGridVariant"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual IPropertyGridVariant GetPropertyValueAsVariant(IPropertyGridItem prop)
        {
            return Handler.GetPropertyValueAsVariant(prop);
        }

        /// <summary>
        /// Gets property value as <see cref="bool"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual bool GetPropertyValueAsBool(IPropertyGridItem prop)
        {
            return Handler.GetPropertyValueAsBool(prop);
        }

        /// <summary>
        /// Gets property value as <see cref="double"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual double GetPropertyValueAsDouble(IPropertyGridItem prop)
        {
            return Handler.GetPropertyValueAsDouble(prop);
        }

        /// <summary>
        /// Gets property value as <see cref="DateTime"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual DateTime GetPropertyValueAsDateTime(IPropertyGridItem prop)
        {
            return Handler.GetPropertyValueAsDateTime(prop);
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
            var flags = recurse
                ? PropertyGridItemValueFlags.Recurse : PropertyGridItemValueFlags.DontRecurse;
            return Handler.HideProperty(prop, hide, flags);
        }

        /// <summary>
        /// Inserts property before another property.
        /// </summary>
        /// <param name="priorThis">Property item before which other property
        /// will be inserted.</param>
        /// <param name="newproperty">Property item to insert.</param>
        public virtual void Insert(IPropertyGridItem priorThis, IPropertyGridItem newproperty)
        {
            Handler.Insert(priorThis, newproperty);
        }

        /// <summary>
        /// Inserts property in parent property at specified index.
        /// </summary>
        /// <param name="parent">Parent property item.</param>
        /// <param name="index">Insert position.</param>
        /// <param name="newproperty">Property item to insert.</param>
        public virtual void InsertAt(
            IPropertyGridItem parent,
            int index,
            IPropertyGridItem newproperty)
        {
            Handler.InsertByIndex(parent, index, newproperty);
        }

        /// <summary>
        /// Gets whether property is category.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property is category, <c>false</c> otherwise.</returns>
        public virtual bool IsPropertyCategory(IPropertyGridItem prop)
        {
            return Handler.IsPropertyCategory(prop);
        }

        /// <summary>
        /// Gets whether property is enabled.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property is enabled, <c>false</c> otherwise.</returns>
        public virtual bool IsPropertyEnabled(IPropertyGridItem prop)
        {
            return Handler.IsPropertyEnabled(prop);
        }

        /// <summary>
        /// Gets whether property is expanded.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property is expanded, <c>false</c> otherwise.</returns>
        public virtual bool IsPropertyExpanded(IPropertyGridItem prop)
        {
            return Handler.IsPropertyExpanded(prop);
        }

        /// <summary>
        /// Gets whether property is modified.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property is modified, <c>false</c> otherwise.</returns>
        public virtual bool IsPropertyModified(IPropertyGridItem prop)
        {
            return Handler.IsPropertyModified(prop);
        }

        /// <summary>
        /// Gets whether property is selected.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property is selected, <c>false</c> otherwise.</returns>
        public virtual bool IsPropertySelected(IPropertyGridItem prop)
        {
            return Handler.IsPropertySelected(prop);
        }

        /// <summary>
        /// Gets whether property is shown.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property is shown, <c>false</c> otherwise.</returns>
        public virtual bool IsPropertyShown(IPropertyGridItem prop)
        {
            return Handler.IsPropertyShown(prop);
        }

        /// <summary>
        /// Gets whether property value is unspecified.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property value is unspecified, <c>false</c> otherwise.</returns>
        public virtual bool IsPropertyValueUnspecified(IPropertyGridItem prop)
        {
            return Handler.IsPropertyValueUnspecified(prop);
        }

        /// <summary>
        /// Disables (limit = true) or enables (limit = false) text editor of a property,
        /// if it is not the sole mean to edit the value.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="limit"><c>true</c> to disable text editor, <c>false</c> otherwise.</param>
        public virtual void LimitPropertyEditing(IPropertyGridItem prop, bool limit = true)
        {
            Handler.LimitPropertyEditing(prop, limit);
        }

        /// <summary>
        /// Replaces existing property with newly created property.
        /// </summary>
        /// <param name="prop">Property item to be replaced.</param>
        /// <param name="newProp">New property item.</param>
        public virtual void ReplaceProperty(IPropertyGridItem prop, IPropertyGridItem newProp)
        {
            Handler.ReplaceProperty(prop, newProp);
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
            var flags = recurse
                ? PropertyGridItemValueFlags.Recurse : PropertyGridItemValueFlags.DontRecurse;
            Handler.SetPropertyBackgroundColor(prop, color, flags);
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
            var flags = recurse
                ? PropertyGridItemValueFlags.Recurse : PropertyGridItemValueFlags.DontRecurse;
            Handler.SetPropertyColorsToDefault(prop, flags);
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
            var flags = recurse
                ? PropertyGridItemValueFlags.Recurse : PropertyGridItemValueFlags.DontRecurse;
            Handler.SetPropertyTextColor(prop, color, flags);
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
            return Handler.RestoreEditableState(src, restoreStates);
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
            Handler.RefreshProperty(p);
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
            return Handler.SaveEditableState(includedStates);
        }

        /// <summary>
        /// Sets proportion of an auto-stretchable column.
        /// </summary>
        /// <param name="column">Column index.</param>
        /// <param name="proportion"> Column proportion (must be 1 or higher).</param>
        /// <returns><c>true</c> on success, <c>false</c> on failure.</returns>
        /// <remarks>
        /// <see cref="PropertyGridCreateStyle.SplitterAutoCenter"/> style needs to be used
        /// to indicate that columns are auto-resizable.
        /// </remarks>
        public virtual bool SetColumnProportion(int column, int proportion)
        {
            return Handler.SetColumnProportion(column, proportion);
        }

        /// <summary>
        /// Gets auto-resize proportion of the given column.
        /// </summary>
        /// <param name="column">Column index.</param>
        public virtual int GetColumnProportion(int column)
        {
            return Handler.GetColumnProportion(column);
        }

        /// <summary>
        /// Gets background color of first cell of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual Color GetPropertyBackgroundColor(IPropertyGridItem prop)
        {
            return Handler.GetPropertyBackgroundColor(prop);
        }

        /// <summary>
        /// Returns text color of first cell of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual Color GetPropertyTextColor(IPropertyGridItem prop)
        {
            return Handler.GetPropertyTextColor(prop);
        }

        /// <summary>
        /// Sets client data of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="clientData">Client data associated with the property.</param>
        public virtual void SetPropertyClientData(IPropertyGridItem prop, IntPtr clientData)
        {
            Handler.SetPropertyClientData(prop, clientData);
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
            Handler.SetPropertyLabel(prop, newproplabel);
        }

        /// <summary>
        /// Sets help string of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="helpString">Help string associated with the property.</param>
        public virtual void SetPropertyHelpString(IPropertyGridItem prop, string helpString)
        {
            Handler.SetPropertyHelpString(prop, helpString);
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
            return Handler.SetPropertyMaxLength(prop, maxLen);
        }

        /// <summary>
        /// Sets property value as <see cref="long"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetPropertyValueAsLong(IPropertyGridItem prop, long value)
        {
            Handler.SetPropertyValueAsLong(prop, value);
        }

        /// <summary>
        /// Sets property value as <see cref="int"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetPropertyValueAsInt(IPropertyGridItem prop, int value)
        {
            Handler.SetPropertyValueAsInt(prop, value);
        }

        /// <summary>
        /// Sets property value as <see cref="double"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetPropertyValueAsDouble(IPropertyGridItem prop, double value)
        {
            Handler.SetPropertyValueAsDouble(prop, value);
        }

        /// <summary>
        /// Sets property value as <see cref="bool"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetPropertyValueAsBool(IPropertyGridItem prop, bool value)
        {
            Handler.SetPropertyValueAsBool(prop, value);
        }

        /// <summary>
        /// Sets property value as <see cref="string"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetPropertyValueAsStr(IPropertyGridItem prop, string value)
        {
            Handler.SetPropertyValueAsStr(prop, value);
        }

        /// <summary>
        /// Sets property value as <see cref="DateTime"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="value">New property value.</param>
        public virtual void SetPropertyValueAsDateTime(IPropertyGridItem prop, DateTime value)
        {
            Handler.SetPropertyValueAsDateTime(prop, value);
        }

        /// <summary>
        /// Adjusts how <see cref="PropertyGrid"/> behaves when invalid value is
        /// entered in a property.
        /// </summary>
        /// <param name="vfbFlags">Validation failure flags.</param>
        public virtual void SetValidationFailureBehavior(PropertyGridValidationFailure vfbFlags)
        {
            Handler.SetValidationFailureBehavior(vfbFlags);
        }

        /// <summary>
        /// Sorts children of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="recurse"><c>true</c> to perform recursive sorting, <c>false</c>
        /// otherwise.</param>
        public virtual void SortChildren(IPropertyGridItem prop, bool recurse = false)
        {
            var flags = recurse
                ? PropertyGridItemValueFlags.Recurse : PropertyGridItemValueFlags.DontRecurse;
            Handler.SortChildren(prop, flags);
        }

        /// <summary>
        /// Sets editor control of a property using its name.
        /// </summary>
        /// <param name="prop">Property Item.</param>
        /// <param name="editorName">Name of the editor.</param>
        /// <remarks>
        /// Names of built-in editors are: TextCtrl, Choice, ComboBox, CheckBox,
        /// TextCtrlAndButton, ChoiceAndButton, SpinCtrl and DatePickerCtrl.
        /// </remarks>
        public virtual void SetPropertyEditorByName(IPropertyGridItem prop, string editorName)
        {
            Handler.SetPropertyEditorByName(prop, editorName);
        }

        /// <summary>
        /// Sets editor control of a property using its known name.
        /// </summary>
        /// <param name="prop">Property Item.</param>
        /// <param name="editorName">Knwon name of the editor.</param>
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
            Handler.AddActionTrigger(action, keycode, modifiers);
        }

        /// <summary>
        /// Removes added action triggers for the given action.
        /// </summary>
        /// <param name="action">Action for which triggers (keyboard key associations) will
        /// be removed.</param>
        public virtual void ClearActionTriggers(PropertyGridKeyboardAction action)
        {
            Handler.ClearActionTriggers(action);
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
            Handler.DedicateKey(keycode);
        }

        /// <summary>
        /// Centers the splitter.
        /// </summary>
        /// <param name="enableAutoResizing">If <c>true</c>, automatic column resizing is
        /// enabled (only applicable if window style
        /// <see cref="PropertyGridCreateStyle.SplitterAutoCenter"/> is used).</param>
        public virtual void CenterSplitter(bool enableAutoResizing = false)
        {
            Handler.CenterSplitter(enableAutoResizing);
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
            Handler.EditorsValueWasModified();
        }

        /// <summary>
        /// Reverse of <see cref="EditorsValueWasModified"/>.
        /// </summary>
        /// <remarks>
        /// This function should only be called by custom properties.
        /// </remarks>
        public virtual void EditorsValueWasNotModified()
        {
            Handler.EditorsValueWasNotModified();
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
            return Handler.EnableCategories(enable);
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
        public virtual SizeD FitColumns()
        {
            return Handler.FitColumns();
        }

        /// <summary>
        /// Gets number of columns currently on grid.
        /// </summary>
        public virtual int GetColumnCount()
        {
            return Handler.GetColumnCount();
        }

        /// <summary>
        /// Gets height of highest characters of used font.
        /// </summary>
        public virtual int GetFontHeight()
        {
            return Handler.GetFontHeight();
        }

        /// <summary>
        /// Gets margin width.
        /// </summary>
        public virtual int GetMarginWidth()
        {
            return Handler.GetMarginWidth();
        }

        /// <summary>
        /// Gets height of a single grid row (in pixels).
        /// </summary>
        public virtual int GetRowHeight()
        {
            return Handler.GetRowHeight();
        }

        /// <summary>
        /// Gets current splitter x position.
        /// </summary>
        /// <param name="splitterIndex">Splitter index (starting from 0).</param>
        public virtual int GetSplitterPosition(int splitterIndex = 0)
        {
            return Handler.GetSplitterPosition(splitterIndex);
        }

        /// <summary>
        /// Gets current vertical spacing.
        /// </summary>
        public virtual int GetVerticalSpacing()
        {
            return Handler.GetVerticalSpacing();
        }

        /// <summary>
        /// Gets whether a property editor control has focus.
        /// </summary>
        /// <returns><c>true</c> if a property editor control has focus, <c>false</c>
        /// otherwise.</returns>
        public virtual bool IsEditorFocused()
        {
            return Handler.IsEditorFocused();
        }

        /// <summary>
        /// Gets whether editor's value was marked modified.
        /// </summary>
        /// <returns><c>true</c> if editor's value was marked modified, <c>false</c>
        /// otherwise.</returns>
        public virtual bool IsEditorsValueModified()
        {
            return Handler.IsEditorsValueModified();
        }

        /// <summary>
        /// Gets whether any property has been modified by the user.
        /// </summary>
        /// <returns><c>true</c> if any property has been modified by the user, <c>false</c>
        /// otherwise.</returns>
        public virtual bool IsAnyModified()
        {
            return Handler.IsAnyModified();
        }

        /// <summary>
        /// Resets all colors used in <see cref="PropertyGrid"/> to default values.
        /// </summary>
        public virtual void ResetColors()
        {
            Handler.ResetColors();
        }

        /// <summary>
        /// Resets column sizes and splitter positions, based on proportions.
        /// </summary>
        /// <param name="enableAutoResizing">If <c>true</c>, automatic column resizing
        /// is enabled (only applicable if control style
        /// <see cref="PropertyGridCreateStyle.SplitterAutoCenter"/> is used).</param>
        public virtual void ResetColumnSizes(bool enableAutoResizing = false)
        {
            Handler.ResetColumnSizes(enableAutoResizing);
        }

        /// <summary>
        /// Makes given column editable by user.
        /// </summary>
        /// <param name="column">The index of the column to make editable.</param>
        /// <param name="editable">Using <c>false</c> here will disable column
        /// from being editable.</param>
        public virtual void MakeColumnEditable(int column, bool editable = true)
        {
            Handler.MakeColumnEditable(column, editable);
        }

        /// <summary>
        /// Creates label editor for given column, for property that is currently selected.
        /// </summary>
        /// <param name="column">Which column's label to edit. Note that you should not use
        /// value 1, which is reserved for property value column.</param>
        /// <remarks>
        /// When multiple selection is enabled, this applies to all selected properties.
        /// </remarks>
        public virtual void BeginLabelEdit(int column = 0)
        {
            Handler.BeginLabelEdit(column);
        }

        /// <summary>
        /// Ends label editing, if any.
        /// </summary>
        /// <param name="commit">Use <c>true</c> (default) to store edited label text in
        /// property cell data.</param>
        public virtual void EndLabelEdit(bool commit = true)
        {
            Handler.EndLabelEdit(commit);
        }

        /// <summary>
        /// Sets number of columns (2 or more).
        /// </summary>
        /// <param name="colCount">Number of columns.</param>
        public virtual void SetColumnCount(int colCount)
        {
            Handler.SetColumnCount(colCount);
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
            Handler.SetSplitterPosition(newXPos, col);
        }

        /// <summary>
        /// Returns (visual) text representation of the unspecified property value.
        /// </summary>
        public virtual string GetUnspecifiedValueText(PropertyGridValueFormatFlags flags = 0)
        {
            return Handler.GetUnspecifiedValueText(flags);
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
            Handler.SetVirtualWidth(width);
        }

        /// <summary>
        /// Moves splitter as left as possible, while still allowing all labels to be shown in full.
        /// </summary>
        /// <param name="privateChildrenToo">If <c>false</c>, will still allow private children
        /// to be cropped.</param>
        public virtual void SetSplitterLeft(bool privateChildrenToo = false)
        {
            Handler.SetSplitterLeft(privateChildrenToo);
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

            Handler.SetVerticalSpacing(v);
        }

        /// <summary>
        /// Gets whether control has virtual width specified with
        /// <see cref="SetVirtualWidth"/>.
        /// </summary>
        public virtual bool HasVirtualWidth()
        {
            return Handler.HasVirtualWidth();
        }

        /// <summary>
        /// Gets number of common values.
        /// </summary>
        public virtual int GetCommonValueCount()
        {
            return Handler.GetCommonValueCount();
        }

        /// <summary>
        /// Gets label of given common value.
        /// </summary>
        /// <param name="i">Index of the commo nvalue.</param>
        public virtual string GetCommonValueLabel(int i)
        {
            return Handler.GetCommonValueLabel(i);
        }

        /// <summary>
        /// Gets index of common value that will truly change value to unspecified.
        /// </summary>
        public virtual int GetUnspecifiedCommonValue()
        {
            return Handler.GetUnspecifiedCommonValue();
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
            Handler.SetUnspecifiedCommonValue(index);
        }

        /// <summary>
        /// Refreshes any active editor control.
        /// </summary>
        public virtual void RefreshEditor()
        {
            Handler.RefreshEditor();
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
            return Handler.WasValueChangedInEvent();
        }

        /// <summary>
        /// Gets current Y spacing.
        /// </summary>
        public virtual int GetSpacingY()
        {
            return Handler.GetSpacingY();
        }

        /// <summary>
        /// Unfocuses or closes editor if one was open, but does not deselect property.
        /// </summary>
        public virtual bool UnfocusEditor()
        {
            return Handler.UnfocusEditor();
        }

        /// <summary>
        /// Returns last item which could be iterated using given flags.
        /// </summary>
        /// <param name="flags">Flags to limit returned properties.</param>
        /// <returns></returns>
        public virtual IPropertyGridItem? GetLastItem(PropertyGridIteratorFlags flags)
        {
            return Handler.GetLastItem(flags);
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
            return Handler.GetRoot();
        }

        /// <summary>
        /// Gets currently selected property.
        /// </summary>
        public virtual IPropertyGridItem? GetSelectedProperty()
        {
            return Handler.GetSelectedProperty();
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
            return Handler.ChangePropertyValue(prop, Handler.ToVariant(value));
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
            return Handler.ChangePropertyValue(prop, value);
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
            Handler.SetPropertyValueAsVariant(prop, value);
        }

        /// <summary>
        /// Sets image associated with the property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="bmp">Image.</param>
        public virtual void SetPropertyImage(IPropertyGridItem prop, ImageSet? bmp)
        {
            Handler.SetPropertyImage(prop, bmp);
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
            Handler.SetPropertyAttribute(
                prop,
                attrName,
                Handler.ToVariant(value),
                argFlags);
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
            Handler.SetPropertyAttribute(
                prop,
                attrName,
                value,
                argFlags);
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
            Handler.SetPropertyAttributeAll(attrName, Handler.ToVariant(value));
        }

        /// <summary>
        /// Sets an attribute for all the properties.
        /// </summary>
        /// <param name="attrName">Text identifier of attribute. See
        /// <see cref="PropertyGridItemAttrId"/> for the known attribute names.</param>
        /// <param name="value">Value of attribute.</param>
        public virtual void SetPropertyAttributeAll(string attrName, IPropertyGridVariant value)
        {
            Handler.SetPropertyAttributeAll(attrName, value);
        }

        /// <summary>
        /// Scrolls and/or expands items to ensure that the given item is visible.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if something was actually done.</returns>
        public virtual bool EnsureVisible(IPropertyGridItem prop)
        {
            return Handler.EnsureVisible(prop);
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
            if (prop == null)
                return false;
            else
                return Handler.SelectProperty(prop, focus);
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
            return Handler.AddToSelection(prop);
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
            return Handler.RemoveFromSelection(prop);
        }

        /// <summary>
        /// Sets the 'current' category - <see cref="Add"/> will add non-category
        /// properties under it.
        /// </summary>
        /// <param name="prop">Property category item.</param>
        public virtual void SetCurrentCategory(IPropertyGridItem prop)
        {
            Handler.SetCurrentCategory(prop);
        }

        /// <summary>
        /// Returns rectangle of custom paint image.
        /// </summary>
        /// <param name="prop">Return image rectangle for this property.</param>
        /// <param name="item">Which choice of property to use (each choice may
        /// have different image).</param>
        public virtual RectI GetImageRect(IPropertyGridItem prop, int item)
        {
            return Handler.GetImageRect(prop, item);
        }

        /// <summary>
        /// Sets flag value for the specified property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="flag">Flag to set.</param>
        /// <param name="value">New value of the flag.</param>
        public virtual void SetPropertyFlag(
            IPropertyGridItem prop,
            PropertyGridItemFlags flag,
            bool value)
        {
            Handler.SetPropertyFlag(prop, flag, value);
        }

        /// <summary>
        /// Returns size of the custom paint image in front of property.
        /// </summary>
        /// <param name="prop">Return image rectangle for this property. If this argument
        /// is <c>null</c>, then preferred size is returned.</param>
        /// <param name="item">Which choice of property to use (each choice may have
        /// different image).</param>
        public virtual SizeI GetImageSize(IPropertyGridItem? prop, int item)
        {
            return Handler.GetImageSize(prop, item);
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
        public virtual void ReloadPropertyValues(
            object? instance = null,
            PropertyInfo? propInfo = null)
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
        /// Gets parent of the property item.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual IPropertyGridItem? GetPropertyParent(IPropertyGridItem? prop)
        {
            return Handler.GetPropertyParent(prop);
        }

        /// <summary>
        /// Gets first child of the property item.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual IPropertyGridItem? GetFirstChild(IPropertyGridItem? prop)
        {
            return Handler.GetFirstChild(prop);
        }

        /// <summary>
        /// Gets special property name which means to use label as property name.
        /// </summary>
        /// <returns></returns>
        public string GetPropNameAsLabel() => Handler.GetPropNameAsLabel();

        /// <summary>
        /// Gets category of the property item.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual IPropertyGridItem? GetPropertyCategory(IPropertyGridItem? prop)
        {
            return Handler.GetPropertyCategory(prop);
        }

        /// <summary>
        /// Gets first property item which satisfies search criteria specified by
        /// <paramref name="flags"/>.
        /// </summary>
        /// <param name="flags">Filter flags.</param>
        public virtual IPropertyGridItem? GetFirst(PropertyGridIteratorFlags flags)
        {
            return Handler.GetFirst(flags);
        }

        /// <summary>
        /// Gets property item with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name of the property item.</param>
        /// <returns></returns>
        public virtual IPropertyGridItem? GetProperty(string? name)
        {
            return Handler.GetProperty(name);
        }

        /// <summary>
        /// Gets property item with the specified <paramref name="label"/>.
        /// </summary>
        /// <param name="label">label of the property item.</param>
        /// <returns></returns>
        public virtual IPropertyGridItem? GetPropertyByLabel(string? label)
        {
            return Handler.GetPropertyByLabel(label);
        }

        /// <summary>
        /// Gets property item with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name of the property item.</param>
        /// <returns></returns>
        public virtual IPropertyGridItem? GetPropertyByName(string? name)
        {
            return Handler.GetPropertyByName(name);
        }

        /// <summary>
        /// Gets property item with the specified <paramref name="name"/>
        /// and <paramref name="subname"/>.
        /// </summary>
        /// <param name="name">Name of the property item.</param>
        /// <param name="subname">Subname of the property item.</param>
        /// <returns></returns>
        public virtual IPropertyGridItem? GetPropertyByNameAndSubName(string? name, string? subname)
        {
            return Handler.GetPropertyByNameAndSubName(name, subname);
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
        /// Gets selected property item.
        /// </summary>
        /// <returns></returns>
        public virtual IPropertyGridItem? GetSelection()
        {
            return Handler.GetSelection();
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
            OnPropertySelected(e);
            PropertySelected?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="PropertyChanged"/> event and <see cref="OnPropertyChanged"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaisePropertyChanged(EventArgs e)
        {
            OnPropertyChanged(e);
            PropertyChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="PropertyChanging"/> event and <see cref="OnPropertyChanging"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaisePropertyChanging(CancelEventArgs e)
        {
            OnPropertyChanging(e);
            PropertyChanging?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="PropertyHighlighted"/> event and <see cref="OnPropertyHighlighted"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaisePropertyHighlighted(EventArgs e)
        {
            OnPropertyHighlighted(e);
            PropertyHighlighted?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="PropertyRightClick"/> event and <see cref="OnPropertyRightClick"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaisePropertyRightClick(EventArgs e)
        {
            OnPropertyRightClick(e);
            PropertyRightClick?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="PropertyDoubleClick"/> event and <see cref="OnPropertyDoubleClick"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaisePropertyDoubleClick(EventArgs e)
        {
            OnPropertyDoubleClick(e);
            PropertyDoubleClick?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="ItemCollapsed"/> event and <see cref="OnItemCollapsed"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaiseItemCollapsed(EventArgs e)
        {
            OnItemCollapsed(e);
            ItemCollapsed?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="ItemExpanded"/> event and <see cref="OnItemExpanded"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaiseItemExpanded(EventArgs e)
        {
            OnItemExpanded(e);
            ItemExpanded?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="ColEndDrag"/> event and <see cref="OnColEndDrag"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaiseColEndDrag(EventArgs e)
        {
            OnColEndDrag(e);
            ColEndDrag?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="ButtonClick"/> event and <see cref="OnButtonClick"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaiseButtonClick(EventArgs e)
        {
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
        /// Reloads value of the <see cref="IPropertyGridItem"/> item if it is attached
        /// to the external object (<see cref="IPropInfoAndInstance.Instance"/> and
        /// <see cref="IPropInfoAndInstance.PropInfo"/>) are not null.
        /// </summary>
        public virtual void ReloadPropertyValue(IPropertyGridItem item)
        {
            var p = item.PropInfo;
            var instance = item.Instance;
            if (instance == null || p == null)
                return;

            AvoidException(() =>
            {
                var variant = Handler.GetTempVariant();

                object? propValue;
                var reloadFunc = item.GetValueFuncForReload;
                var realInstance = GetRealInstance(item);
                realInstance ??= instance;
                if (reloadFunc == null)
                {
                    propValue = p.GetValue(realInstance);
                    variant.SetCompatibleValue(propValue, p);
                }
                else
                {
                    propValue = reloadFunc(item, realInstance, p);
                    variant.AsObject = propValue;
                }

                SetPropertyValueAsVariant(item, variant);
            });

            if (item.Parent != null)
                ReloadPropertyValue(item.Parent);
        }

        /// <summary>
        /// Raises <see cref="ColBeginDrag"/> event and <see cref="OnColBeginDrag"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaiseColBeginDrag(CancelEventArgs e)
        {
            OnColBeginDrag(e);
            ColBeginDrag?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="LabelEditEnding"/> event and <see cref="OnLabelEditEnding"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaiseLabelEditEnding(CancelEventArgs e)
        {
            OnLabelEditEnding(e);
            LabelEditEnding?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="LabelEditBegin"/> event and <see cref="OnLabelEditBegin"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaiseLabelEditBegin(CancelEventArgs e)
        {
            OnLabelEditBegin(e);
            LabelEditBegin?.Invoke(this, e);
        }

        /// <summary>
        /// Creates cursor property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <param name="prm">Property item create parameters.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        internal IPropertyGridItem CreateCursorItem(
           string label,
           string? name = null,
           int value = 0,
           IPropertyGridNewItemParams? prm = null)
        {
            label = CorrectPropLabel(label, prm);
            var handle = Handler.CreateCursorProperty(
                label,
                CorrectPropName(name),
                value);
            var result = new PropertyGridItem(this, handle, label, name, value, prm);
            OnPropertyCreated(result, prm);
            return result;
        }

        internal void DeleteProperty(IPropertyGridItem prop)
        {
            Handler.DeleteProperty(prop);
            items.Remove(prop.Handle);
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreatePropertyGridHandler(this);
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
            var prop = EventProperty;
            if (prop == null)
                return;

            var setValue = ApplyFlags.HasFlag(PropertyGridApplyFlags.PropInfoSetValue);

            var propInfo = prop.PropInfo;
            var instance = prop.Instance;

            if (setValue && instance != null && propInfo != null && propInfo.CanWrite)
            {
                AvoidException(() =>
                {
                    var variant = EventPropValueAsVariant;
                    var newValue = variant.GetCompatibleValue(prop);
                    propInfo.SetValue(instance, newValue);
                    UpdateStruct();
                });

                AvoidException(() =>
                {
                    var reload = ApplyFlags.HasFlag(PropertyGridApplyFlags.ReloadAfterSetValue);
                    var reloadAll = ApplyFlags.HasFlag(PropertyGridApplyFlags.ReloadAllAfterSetValue);

                    if (reloadAll)
                        ReloadPropertyValues();
                    else
                        if (reload)
                        ReloadPropertyValues(instance, propInfo);
                });
            }

            var propEvent = ApplyFlags.HasFlag(PropertyGridApplyFlags.PropEvent);
            if (propEvent)
            {
                AvoidException(() => { prop.RaisePropertyChanged(); });
            }

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