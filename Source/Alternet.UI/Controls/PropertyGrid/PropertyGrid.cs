using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /*
    - multibuttons in prop editor   https://docs.wxwidgets.org/3.2/classwx_p_g_multi_button.html
    - How to make cells bigger in height? So DateEdit will look ok?
    - How to make cell spacing vertical?
    - How to hide lines? (set their color to bk color)
    - ClickButton event in property intf
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
        private static Dictionary<Type, IPropertyGridChoices>? choicesCache = null;
        private readonly Dictionary<IntPtr, IPropertyGridItem> items = new();
        private readonly PropertyGridVariant variant = new();

        static PropertyGrid()
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
        /// Defines default style for the newly created
        /// <see cref="PropertyGrid"/> controls.
        /// </summary>
        public static PropertyGridCreateStyle DefaultCreateStyle { get; set; }
            = PropertyGridCreateStyle.DefaultStyle;

        /// <summary>
        /// Defines default extended style for the newly created
        /// <see cref="PropertyGrid"/> controls.
        /// </summary>
        public static PropertyGridCreateStyleEx DefaultCreateStyleEx { get; set; }
            = PropertyGridCreateStyleEx.DefaultStyle;

        /// <summary>
        /// Gets or sets whether boolean properties will be shown as checkboxes.
        /// </summary>
        public bool BoolAsCheckBox { get; set; } = true;

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
        /// <see cref="CreateEnumProperty"/>.
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
        /// <see cref="CreateEnumProperty"/>.
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
        /// <see cref="CreateEnumProperty"/>.
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
        public IPropertyGridItem CreateFilenameProperty(
            string label,
            string? name = null,
            string? value = null)
        {
            value ??= string.Empty;
            var handle = NativeControl.CreateFilenameProperty(label, CorrectPropName(name), value!);
            var result = new PropertyGridItem(handle, label, name, value)
            {
                PropertyEditorKind = "Filename",
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
        public IPropertyGridItem CreateDirProperty(
            string label,
            string? name = null,
            string? value = null)
        {
            value ??= string.Empty;
            var handle = NativeControl.CreateDirProperty(label, CorrectPropName(name), value!);
            var result = new PropertyGridItem(handle, label, name, value)
            {
                PropertyEditorKind = "Dir",
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
        public IPropertyGridItem CreateImageFilenameProperty(
            string label,
            string? name = null,
            string? value = null)
        {
            value ??= string.Empty;
            var handle = NativeControl.CreateImageFilenameProperty(
                label,
                CorrectPropName(name),
                value!);
            var result = new PropertyGridItem(handle, label, name, value)
            {
                PropertyEditorKind = "ImageFilename",
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
        public IPropertyGridItem CreateSystemColorProperty(
            string label,
            string? name,
            Color value)
        {
            var handle = NativeControl.CreateSystemColorProperty(label, CorrectPropName(name), value);
            var result = new PropertyGridItem(handle, label, name, value)
            {
                PropertyEditorKind = "SystemColor",
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
        public IPropertyGridItem CreateStringProperty(
            string label,
            string? name = null,
            string? value = null)
        {
            value ??= string.Empty;
            var handle = NativeControl.CreateStringProperty(label, CorrectPropName(name), value!);
            var result = new PropertyGridItem(handle, label, name, value)
            {
                PropertyEditorKind = "String",
            };
            OnPropertyCreated(result);
            return result;
        }

        /// <summary>
        /// Creates <see cref="bool"/> property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public IPropertyGridItem CreateBoolProperty(
            string label,
            string? name = null,
            bool value = false)
        {
            var handle = NativeControl.CreateBoolProperty(label, CorrectPropName(name), value);
            var result = new PropertyGridItem(handle, label, name, value)
            {
                PropertyEditorKind = "Bool",
            };
            OnPropertyCreated(result);
            return result;
        }

        /// <summary>
        /// Creates <see cref="int"/> property. Supports also <see cref="long"/> values.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public IPropertyGridItem CreateIntProperty(
            string label,
            string? name = null,
            long value = 0)
        {
            var handle = NativeControl.CreateIntProperty(label, CorrectPropName(name), value);
            var result = new PropertyGridItem(handle, label, name, value)
            {
                PropertyEditorKind = "Int",
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
        public IPropertyGridItem CreateFloatProperty(
            string label,
            string? name = null,
            double value = default(double))
        {
            var handle = NativeControl.CreateFloatProperty(label, CorrectPropName(name), value);
            var result = new PropertyGridItem(handle, label, name, value)
            {
                PropertyEditorKind = "Float",
            };
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
        public IPropertyGridItem CreateColorProperty(
            string label,
            string? name,
            Color value)
        {
            var handle = NativeControl.CreateColorProperty(label, CorrectPropName(name), value);
            var result = new PropertyGridItem(handle, label, name, value)
            {
                PropertyEditorKind = "Color",
            };
            OnPropertyCreated(result);
            return result;
        }

        /// <summary>
        /// Creates <see cref="uint"/> property. Supports also <see cref="ulong"/> values.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public IPropertyGridItem CreateUIntProperty(
            string label,
            string? name = null,
            ulong value = 0)
        {
            var handle = NativeControl.CreateUIntProperty(label, CorrectPropName(name), value);
            var result = new PropertyGridItem(handle, label, name, value)
            {
                PropertyEditorKind = "UInt",
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
        public IPropertyGridItem CreateLongStringProperty(
            string label,
            string? name = null,
            string? value = null)
        {
            value ??= string.Empty;
            var handle = NativeControl.CreateLongStringProperty(
                label,
                CorrectPropName(name),
                value);
            var result = new PropertyGridItem(handle, label, name, value)
            {
                PropertyEditorKind = "LongString",
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
        public IPropertyGridItem CreateDateProperty(
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
            var result = new PropertyGridItem(handle, label, name, value)
            {
                PropertyEditorKind = "Date",
            };
            OnPropertyCreated(result);
            return result;
        }

        /// <summary>
        /// Creates enumeration property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="choices">Enumeration elements.</param>
        /// <param name="value">Default property value.</param>
        /// <returns>Property declaration for use with <see cref="PropertyGrid.Add"/>.</returns>
        public IPropertyGridItem CreateEnumProperty(
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
            var result = new PropertyGridItem(handle, label, name, value)
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
        public IPropertyGridItem CreateEditEnumProperty(
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
            var result = new PropertyGridItem(handle, label, name, value)
            {
                PropertyEditorKind = "EditEnum",
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
        public IPropertyGridItem CreateFlagsProperty(
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
            var result = new PropertyGridItem(handle, label, name, value)
            {
                PropertyEditorKind = "Flags",
            };
            OnPropertyCreated(result);
            return result;
        }

        /// <summary>
        /// Deletes all items from the property grid.
        /// </summary>
        public void Clear()
        {
            items.Clear();
            NativeControl.Clear();
        }

        /// <summary>
        /// Adds item to the property grid.
        /// </summary>
        /// <param name="prop">Item to add.</param>
        /// <param name="parent">Parent item or null.</param>
        public void Add(IPropertyGridItem prop, IPropertyGridItem? parent = null)
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

                if (BoolAsCheckBox && (kind == "Bool" || kind == "Flags"))
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
        public IPropertyGridItem CreatePropCategory(string label, string? name = null)
        {
            var handle = NativeControl.CreatePropCategory(
                label,
                CorrectPropName(name));
            var result = new PropertyGridItem(handle, label, name, null)
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
        public void AppendIn(IPropertyGridItem prop, IPropertyGridItem newproperty)
        {
            NativeControl.AppendIn(prop.Handle, newproperty.Handle);
        }

        /// <summary>
        /// Collapses (hides) all sub properties of the given property.
        /// </summary>
        /// <param name="prop">Property item to collapse.</param>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        public bool Collapse(IPropertyGridItem prop)
        {
            return NativeControl.Collapse(prop.Handle);
        }

        /// <summary>
        /// Removes property from the <see cref="PropertyGrid"/>.
        /// </summary>
        /// <param name="prop">Property item to remove.</param>
        public void RemoveProperty(IPropertyGridItem prop)
        {
            NativeControl.RemoveProperty(prop.Handle);
            items.Remove(prop.Handle);
        }

        /// <summary>
        /// Disables property.
        /// </summary>
        /// <param name="prop">Property item to disable.</param>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        public bool DisableProperty(IPropertyGridItem prop)
        {
            return NativeControl.DisableProperty(prop.Handle);
        }

        /// <summary>
        /// Changes enabled state of the property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="enable">New enabled state value.</param>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        public bool EnableProperty(IPropertyGridItem prop, bool enable = true)
        {
            return NativeControl.EnableProperty(prop.Handle, enable);
        }

        /// <summary>
        /// Expands (shows) all sub properties of the given property.
        /// </summary>
        /// <param name="prop">Property item to expand.</param>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        public bool Expand(IPropertyGridItem prop)
        {
            return NativeControl.Expand(prop.Handle);
        }

        /// <summary>
        /// Gets client data associated with the property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public IntPtr GetPropertyClientData(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyClientData(prop.Handle);
        }

        /// <summary>
        /// Gets help string associated with the property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public string GetPropertyHelpString(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyHelpString(prop.Handle);
        }

        /// <summary>
        /// Gets label associated with the property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public string GetPropertyLabel(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyLabel(prop.Handle);
        }

        /// <summary>
        /// Gets property value as <see cref="string"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public string GetPropertyValueAsString(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyValueAsString(prop.Handle);
        }

        /// <summary>
        /// Gets property value as <see cref="long"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public long GetPropertyValueAsLong(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyValueAsLong(prop.Handle);
        }

        /// <summary>
        /// Gets property value as <see cref="ulong"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public ulong GetPropertyValueAsULong(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyValueAsULong(prop.Handle);
        }

        /// <summary>
        /// Gets property value as <see cref="int"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public int GetPropertyValueAsInt(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyValueAsInt(prop.Handle);
        }

        /// <summary>
        /// Gets property value as <see cref="bool"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public bool GetPropertyValueAsBool(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyValueAsBool(prop.Handle);
        }

        /// <summary>
        /// Gets property value as <see cref="double"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public double GetPropertyValueAsDouble(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyValueAsDouble(prop.Handle);
        }

        /// <summary>
        /// Gets property value as <see cref="DateTime"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public DateTime GetPropertyValueAsDateTime(IPropertyGridItem prop)
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
        public bool HideProperty(IPropertyGridItem prop, bool hide, bool recurse = true)
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
        public void Insert(IPropertyGridItem priorThis, IPropertyGridItem newproperty)
        {
            NativeControl.Insert(priorThis.Handle, newproperty.Handle);
        }

        /// <summary>
        /// Inserts property in parent property at specified index.
        /// </summary>
        /// <param name="parent">Parent property item.</param>
        /// <param name="index">Insert position.</param>
        /// <param name="newproperty">Property item to insert.</param>
        public void Insert(IPropertyGridItem parent, int index, IPropertyGridItem newproperty)
        {
            NativeControl.InsertByIndex(parent.Handle, index, newproperty.Handle);
        }

        /// <summary>
        /// Gets whether property is category.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property is category, <c>false</c> otherwise.</returns>
        public bool IsPropertyCategory(IPropertyGridItem prop)
        {
            return NativeControl.IsPropertyCategory(prop.Handle);
        }

        /// <summary>
        /// Gets whether property is enabled.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property is enabled, <c>false</c> otherwise.</returns>
        public bool IsPropertyEnabled(IPropertyGridItem prop)
        {
            return NativeControl.IsPropertyEnabled(prop.Handle);
        }

        /// <summary>
        /// Gets whether property is expanded.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property is expanded, <c>false</c> otherwise.</returns>
        public bool IsPropertyExpanded(IPropertyGridItem prop)
        {
            return NativeControl.IsPropertyExpanded(prop.Handle);
        }

        /// <summary>
        /// Gets whether property is modified.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property is modified, <c>false</c> otherwise.</returns>
        public bool IsPropertyModified(IPropertyGridItem prop)
        {
            return NativeControl.IsPropertyModified(prop.Handle);
        }

        /// <summary>
        /// Gets whether property is selected.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property is selected, <c>false</c> otherwise.</returns>
        public bool IsPropertySelected(IPropertyGridItem prop)
        {
            return NativeControl.IsPropertySelected(prop.Handle);
        }

        /// <summary>
        /// Gets whether property is shown.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property is shown, <c>false</c> otherwise.</returns>
        public bool IsPropertyShown(IPropertyGridItem prop)
        {
            return NativeControl.IsPropertyShown(prop.Handle);
        }

        /// <summary>
        /// Gets whether property value is unspecified.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <returns><c>true</c> if property value is unspecified, <c>false</c> otherwise.</returns>
        public bool IsPropertyValueUnspecified(IPropertyGridItem prop)
        {
            return NativeControl.IsPropertyValueUnspecified(prop.Handle);
        }

        /// <summary>
        /// Disables (limit = true) or enables (limit = false) text editor of a property,
        /// if it is not the sole mean to edit the value.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="limit"><c>true</c> to disable text editor, <c>false</c>< otherwise./param>
        public void LimitPropertyEditing(IPropertyGridItem prop, bool limit = true)
        {
            NativeControl.LimitPropertyEditing(prop.Handle, limit);
        }

        /// <summary>
        /// Replaces existing property with newly created property.
        /// </summary>
        /// <param name="prop">Property item to be replaced.</param>
        /// <param name="newProp">New property item.</param>
        public void ReplaceProperty(IPropertyGridItem prop, IPropertyGridItem newProp)
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
        public void SetPropertyBackgroundColor(
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
        public void SetPropertyColorsToDefault(IPropertyGridItem prop, bool recurse = true)
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
        public void SetPropertyTextColor(IPropertyGridItem prop, Color color, bool recurse = true)
        {
            var flags = recurse ? PGRECURSE : PGDONTRECURSE;
            NativeControl.SetPropertyTextColor(prop.Handle, color, flags);
        }

        public bool RestoreEditableState(
            string src,
            PropertyGridEditableState restoreStates = PropertyGridEditableState.AllStates)
        {
            return NativeControl.RestoreEditableState(src, (int)restoreStates);
        }

        public void RefreshProperty(IPropertyGridItem p)
        {
            NativeControl.RefreshProperty(p.Handle);
        }

        public string SaveEditableState(
            PropertyGridEditableState includedStates =
                PropertyGridEditableState.AllStates)
        {
            return NativeControl.SaveEditableState((int)includedStates);
        }

        public bool SetColumnProportion(uint column, int proportion)
        {
            return NativeControl.SetColumnProportion(column, proportion);
        }

        public int GetColumnProportion(uint column)
        {
            return NativeControl.GetColumnProportion(column);
        }

        public Color GetPropertyBackgroundColor(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyBackgroundColor(prop.Handle);
        }

        public Color GetPropertyTextColor(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyTextColor(prop.Handle);
        }

        public void SetPropertyClientData(IPropertyGridItem prop, IntPtr clientData)
        {
            NativeControl.SetPropertyClientData(prop.Handle, clientData);
        }

        public void SetPropertyLabel(IPropertyGridItem prop, string newproplabel)
        {
            NativeControl.SetPropertyLabel(prop.Handle, newproplabel);
        }

        public void SetPropertyHelpString(IPropertyGridItem prop, string helpString)
        {
            NativeControl.SetPropertyHelpString(prop.Handle, helpString);
        }

        public bool SetPropertyMaxLength(IPropertyGridItem prop, int maxLen)
        {
            return NativeControl.SetPropertyMaxLength(prop.Handle, maxLen);
        }

        public void SetPropertyValueAsLong(IPropertyGridItem prop, long value)
        {
            NativeControl.SetPropertyValueAsLong(prop.Handle, value);
        }

        public void SetPropertyValueAsInt(IPropertyGridItem prop, int value)
        {
            NativeControl.SetPropertyValueAsInt(prop.Handle, value);
        }

        public void SetPropertyValueAsDouble(IPropertyGridItem prop, double value)
        {
            NativeControl.SetPropertyValueAsDouble(prop.Handle, value);
        }

        public void SetPropertyValueAsBool(IPropertyGridItem prop, bool value)
        {
            NativeControl.SetPropertyValueAsBool(prop.Handle, value);
        }

        public void SetPropertyValueAsStr(IPropertyGridItem prop, string value)
        {
            NativeControl.SetPropertyValueAsStr(prop.Handle, value);
        }

        public void SetPropertyValueAsDateTime(IPropertyGridItem prop, DateTime value)
        {
            NativeControl.SetPropertyValueAsDateTime(prop.Handle, value);
        }

        public void SetValidationFailureBehavior(PropertyGridValidationFailure vfbFlags)
        {
            NativeControl.SetValidationFailureBehavior((int)vfbFlags);
        }

        public void SortChildren(IPropertyGridItem prop, bool recurse = false)
        {
            var flags = recurse ? PGRECURSE : PGDONTRECURSE;
            NativeControl.SortChildren(prop.Handle, flags);
        }

        public void SetPropertyEditorByName(IPropertyGridItem prop, string editorName)
        {
            NativeControl.SetPropertyEditorByName(prop.Handle, editorName);
        }

        /// <summary>
        /// Sets all <see cref="PropertyGrid"/> colors.
        /// </summary>
        /// <param name="colors">New color settings.</param>
        public void ApplyColors(PropertyGridColors? colors = null)
        {
            if(colors == null)
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
        public void AddActionTrigger(
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
        public void ClearActionTriggers(PropertyGridKeyboardAction action)
        {
            NativeControl.ClearActionTriggers((int)action);
        }

        public void DedicateKey(Key keycode)
        {
            NativeControl.DedicateKey((int)keycode);
        }

        public static void AutoGetTranslation(bool enable)
        {
            Native.PropertyGrid.AutoGetTranslation(enable);
        }

        public void CenterSplitter(bool enableAutoResizing = false)
        {
            NativeControl.CenterSplitter(enableAutoResizing);
        }

        public void EditorsValueWasModified()
        {
            NativeControl.EditorsValueWasModified();
        }

        public void EditorsValueWasNotModified()
        {
            NativeControl.EditorsValueWasNotModified();
        }

        public bool EnableCategories(bool enable)
        {
            return NativeControl.EnableCategories(enable);
        }

        public Size FitColumns()
        {
            return NativeControl.FitColumns();
        }

        public uint GetColumnCount()
        {
            return NativeControl.GetColumnCount();
        }

        public int GetFontHeight()
        {
            return NativeControl.GetFontHeight();
        }

        public int GetMarginWidth()
        {
            return NativeControl.GetMarginWidth();
        }

        public int GetRowHeight()
        {
            return NativeControl.GetRowHeight();
        }

        public int GetSplitterPosition(uint splitterIndex = 0)
        {
            return NativeControl.GetSplitterPosition(splitterIndex);
        }

        public int GetVerticalSpacing()
        {
            return NativeControl.GetVerticalSpacing();
        }

        public bool IsEditorFocused()
        {
            return NativeControl.IsEditorFocused();
        }

        public bool IsEditorsValueModified()
        {
            return NativeControl.IsEditorsValueModified();
        }

        public bool IsAnyModified()
        {
            return NativeControl.IsAnyModified();
        }

        public void ResetColors()
        {
            NativeControl.ResetColors();
        }

        public void ResetColumnSizes(bool enableAutoResizing = false)
        {
            NativeControl.ResetColumnSizes(enableAutoResizing);
        }

        public void MakeColumnEditable(uint column, bool editable = true)
        {
            NativeControl.MakeColumnEditable(column, editable);
        }

        public void BeginLabelEdit(uint column = 0)
        {
            NativeControl.BeginLabelEdit(column);
        }

        public void EndLabelEdit(bool commit = true)
        {
            NativeControl.EndLabelEdit(commit);
        }

        public void SetColumnCount(int colCount)
        {
            NativeControl.SetColumnCount(colCount);
        }

        public void SetSplitterPosition(int newXPos, int col = 0)
        {
            NativeControl.SetSplitterPosition(newXPos, col);
        }

        public string GetUnspecifiedValueText()
        {
            return NativeControl.GetUnspecifiedValueText(0);
        }

        public void SetVirtualWidth(int width)
        {
            NativeControl.SetVirtualWidth(width);
        }

        public void SetSplitterLeft(bool privateChildrenToo = false)
        {
            NativeControl.SetSplitterLeft(privateChildrenToo);
        }

        public void SetVerticalSpacing(int vspacing)
        {
            NativeControl.SetVerticalSpacing(vspacing);
        }

        public bool HasVirtualWidth()
        {
            return NativeControl.HasVirtualWidth();
        }

        public uint GetCommonValueCount()
        {
            return NativeControl.GetCommonValueCount();
        }

        public string GetCommonValueLabel(uint i)
        {
            return NativeControl.GetCommonValueLabel(i);
        }

        public int GetUnspecifiedCommonValue()
        {
            return NativeControl.GetUnspecifiedCommonValue();
        }

        public void SetUnspecifiedCommonValue(int index)
        {
            NativeControl.SetUnspecifiedCommonValue(index);
        }

        public void RefreshEditor()
        {
            NativeControl.RefreshEditor();
        }

        public bool WasValueChangedInEvent()
        {
            return NativeControl.WasValueChangedInEvent();
        }

        public int GetSpacingY()
        {
            return NativeControl.GetSpacingY();
        }

        public void SetupTextCtrlValue(string text)
        {
            NativeControl.SetupTextCtrlValue(text);
        }

        public bool UnfocusEditor()
        {
            return NativeControl.UnfocusEditor();
        }

        public IPropertyGridItem? GetLastItem(PropertyGridIteratorFlags flags)
        {
            return PtrToItem(NativeControl.GetLastItem((int)flags));
        }

        public IPropertyGridItem? GetRoot()
        {
            return PtrToItem(NativeControl.GetRoot());
        }

        public IPropertyGridItem? GetSelectedProperty()
        {
            return PtrToItem(NativeControl.GetSelectedProperty());
        }

        public bool ChangePropertyValue(IPropertyGridItem id, object value)
        {
            return NativeControl.ChangePropertyValue(id.Handle, ToVariant(value).Handle);
        }

        public void SetPropertyImage(IPropertyGridItem id, ImageSet? bmp)
        {
            NativeControl.SetPropertyImage(id.Handle, bmp?.NativeImageSet);
        }

        public void SetPropertyAttribute(
            IPropertyGridItem id,
            string attrName,
            object value,
            PropertyGridItemValueFlags argFlags = 0)
        {
            NativeControl.SetPropertyAttribute(
                id.Handle,
                attrName,
                ToVariant(value).Handle,
                (int)argFlags);
        }

        public void SetPropertyKnownAttribute(
            IPropertyGridItem id,
            PropertyGridItemAttrId attrName,
            object value,
            PropertyGridItemValueFlags argFlags = 0)
        {
            SetPropertyAttribute(id, attrName.ToString(), value, argFlags);
        }

        public void SetPropertyAttributeAll(string attrName, object value)
        {
            NativeControl.SetPropertyAttributeAll(attrName, ToVariant(value).Handle);
        }

        public bool EnsureVisible(IPropertyGridItem prop)
        {
            return NativeControl.EnsureVisible(prop.Handle);
        }

        public bool SelectProperty(IPropertyGridItem prop, bool focus = false)
        {
            return NativeControl.SelectProperty(prop.Handle, focus);
        }

        public bool AddToSelection(IPropertyGridItem prop)
        {
            return NativeControl.AddToSelection(prop.Handle);
        }

        public bool RemoveFromSelection(IPropertyGridItem prop)
        {
            return NativeControl.RemoveFromSelection(prop.Handle);
        }

        public void SetCurrentCategory(IPropertyGridItem prop)
        {
            NativeControl.SetCurrentCategory(prop.Handle);
        }

        public Int32Rect GetImageRect(IPropertyGridItem prop, int item)
        {
            return NativeControl.GetImageRect(prop.Handle, item);
        }

        public Int32Size GetImageSize(IPropertyGridItem prop, int item)
        {
            return NativeControl.GetImageSize(prop.Handle, item);
        }

        internal static void CreateTestVariant()
        {
#pragma warning disable
            PropertyGridVariant variant = new();
#pragma warning restore

            variant.AsBool = true;

            variant.AsLong = 150;

            variant.AsDateTime = DateTime.Now;

            variant.AsDouble = 18;

            variant.AsString = "hello";
        }

        internal static IntPtr GetEditorByName(string editorName)
        {
            return Native.PropertyGrid.GetEditorByName(editorName);
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
            var result = new PropertyGridItem(handle, label, name, value);
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

        internal PropertyGridVariant ToVariant(object value)
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