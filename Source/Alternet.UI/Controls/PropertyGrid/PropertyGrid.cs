using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

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
    public class PropertyGrid : Control
    {
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
            var result = new PropertyGridItem(handle, label, name, value);
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
            var result = new PropertyGridItem(handle, label, name, value);
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
            var result = new PropertyGridItem(handle, label, name, value);
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
            var result = new PropertyGridItem(handle, label, name, value);
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
            var result = new PropertyGridItem(handle, label, name, value);
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
            var result = new PropertyGridItem(handle, label, name, value);
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
            var result = new PropertyGridItem(handle, label, name, value);
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
            double value = 0.0)
        {
            var handle = NativeControl.CreateFloatProperty(label, CorrectPropName(name), value);
            var result = new PropertyGridItem(handle, label, name, value);
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
            var result = new PropertyGridItem(handle, label, name, value);
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
            var result = new PropertyGridItem(handle, label, name, value);
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
            var result = new PropertyGridItem(handle, label, name, value);
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
            var result = new PropertyGridItem(handle, label, name, value);
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
            var result = new PropertyGridItem(handle, label, name, value);
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
            var result = new PropertyGridItem(handle, label, name, value);
            OnPropertyCreated(result);
            return result;
        }

        /// <summary>
        /// Creates property choices list for use with <see cref="CreateFlagsProperty"/> and
        /// <see cref="CreateEnumProperty"/>.
        /// </summary>
#pragma warning disable
        public IPropertyGridChoices CreateChoices()
#pragma warning restore
        {
            return new PropertyGridChoices();
        }

        /// <summary>
        /// Creates property choices list for the given enumeration type or returns it from
        /// the internal cache if it was previously created.
        /// </summary>
        /// <remarks>
        /// Result can be used in <see cref="CreateFlagsProperty"/> and
        /// <see cref="CreateEnumProperty"/>.
        /// </remarks>
        public IPropertyGridChoices CreateChoicesOnce(Type enumType)
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
        public IPropertyGridChoices CreateChoices(Type enumType)
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
        public void Add(IPropertyGridItem prop)
        {
            if (prop == null)
                return;
            NativeControl.Append(prop.Handle);
            items.Add(prop.Handle, prop);
            if (!prop.HasChildren)
                return;
            foreach (IPropertyGridItem child in prop.Children)
            {
                items.Add(child.Handle, child);
                AppendIn(prop, child);
            }

            Collapse(prop);
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

        public string GetPropertyName(IPropertyGridItem property)
        {
            return NativeControl.GetPropertyName(property.Handle);
        }

        public bool RestoreEditableState(
            string src,
            PropertyGridEditableStateFlags restoreStates = PropertyGridEditableStateFlags.AllStates)
        {
            return NativeControl.RestoreEditableState(src, (int)restoreStates);
        }

        public string SaveEditableState(
            PropertyGridEditableStateFlags includedStates =
                PropertyGridEditableStateFlags.AllStates)
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

        public void Sort(bool topLevelOnly = false)
        {
            var flags = topLevelOnly ? PGSORTTOPLEVELONLY : 0;
            NativeControl.Sort(flags);
        }

        public void RefreshProperty(IPropertyGridItem p)
        {
            NativeControl.RefreshProperty(p.Handle);
        }

        public void SetPropertyReadOnly(IPropertyGridItem prop, bool isSet, bool recurse = true)
        {
            var flags = recurse ? PGRECURSE : PGDONTRECURSE;

            NativeControl.SetPropertyReadOnly(prop.Handle, isSet, flags);
        }

        public void SetPropertyValueUnspecified(IPropertyGridItem prop)
        {
            NativeControl.SetPropertyValueUnspecified(prop.Handle);
        }

        public void AppendIn(IPropertyGridItem prop, IPropertyGridItem newproperty)
        {
            /*var result = */
            NativeControl.AppendIn(prop.Handle, newproperty.Handle);
        }

        public void BeginAddChildren(IPropertyGridItem prop)
        {
            NativeControl.BeginAddChildren(prop.Handle);
        }

        public bool Collapse(IPropertyGridItem prop)
        {
            return NativeControl.Collapse(prop.Handle);
        }

        public void RemoveProperty(IPropertyGridItem prop)
        {
            /*var result = */
            NativeControl.RemoveProperty(prop.Handle);
            items.Remove(prop.Handle);
        }

        public bool DisableProperty(IPropertyGridItem prop)
        {
            return NativeControl.DisableProperty(prop.Handle);
        }

        public bool EnableProperty(IPropertyGridItem prop, bool enable = true)
        {
            return NativeControl.EnableProperty(prop.Handle, enable);
        }

        public void EndAddChildren(IPropertyGridItem prop)
        {
            NativeControl.EndAddChildren(prop.Handle);
        }

        public bool Expand(IPropertyGridItem prop)
        {
            return NativeControl.Expand(prop.Handle);
        }

        public IntPtr GetPropertyClientData(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyClientData(prop.Handle);
        }

        public string GetPropertyHelpString(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyHelpString(prop.Handle);
        }

        public string GetPropertyLabel(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyLabel(prop.Handle);
        }

        public string GetPropertyValueAsString(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyValueAsString(prop.Handle);
        }

        public long GetPropertyValueAsLong(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyValueAsLong(prop.Handle);
        }

        public ulong GetPropertyValueAsULong(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyValueAsULong(prop.Handle);
        }

        public int GetPropertyValueAsInt(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyValueAsInt(prop.Handle);
        }

        public bool GetPropertyValueAsBool(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyValueAsBool(prop.Handle);
        }

        public double GetPropertyValueAsDouble(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyValueAsDouble(prop.Handle);
        }

        public DateTime GetPropertyValueAsDateTime(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyValueAsDateTime(prop.Handle);
        }

        public bool HideProperty(IPropertyGridItem prop, bool hide, bool recurse = true)
        {
            var flags = recurse ? PGRECURSE : PGDONTRECURSE;
            return NativeControl.HideProperty(prop.Handle, hide, flags);
        }

        public void Insert(IPropertyGridItem priorThis, IPropertyGridItem newproperty)
        {
            /*var result = */
            NativeControl.Insert(priorThis.Handle, newproperty.Handle);
        }

        public void Insert(IPropertyGridItem parent, int index, IPropertyGridItem newproperty)
        {
            /*var result = */
            NativeControl.InsertByIndex(parent.Handle, index, newproperty.Handle);
        }

        public bool IsPropertyCategory(IPropertyGridItem prop)
        {
            return NativeControl.IsPropertyCategory(prop.Handle);
        }

        public bool IsPropertyEnabled(IPropertyGridItem prop)
        {
            return NativeControl.IsPropertyEnabled(prop.Handle);
        }

        public bool IsPropertyExpanded(IPropertyGridItem prop)
        {
            return NativeControl.IsPropertyExpanded(prop.Handle);
        }

        public bool IsPropertyModified(IPropertyGridItem prop)
        {
            return NativeControl.IsPropertyModified(prop.Handle);
        }

        public bool IsPropertySelected(IPropertyGridItem prop)
        {
            return NativeControl.IsPropertySelected(prop.Handle);
        }

        public bool IsPropertyShown(IPropertyGridItem prop)
        {
            return NativeControl.IsPropertyShown(prop.Handle);
        }

        public bool IsPropertyValueUnspecified(IPropertyGridItem prop)
        {
            return NativeControl.IsPropertyValueUnspecified(prop.Handle);
        }

        public void LimitPropertyEditing(IPropertyGridItem prop, bool limit = true)
        {
            NativeControl.LimitPropertyEditing(prop.Handle, limit);
        }

        public void ReplaceProperty(IPropertyGridItem prop, IPropertyGridItem newProp)
        {
            /*var result = */
            NativeControl.ReplaceProperty(prop.Handle, newProp.Handle);
        }

        public void SetPropertyBackgroundColor(
            IPropertyGridItem prop,
            Color color,
            bool recurse = true)
        {
            var flags = recurse ? PGRECURSE : PGDONTRECURSE;
            NativeControl.SetPropertyBackgroundColor(prop.Handle, color, flags);
        }

        public void SetPropertyColorsToDefault(IPropertyGridItem prop, bool recurse = true)
        {
            var flags = recurse ? PGRECURSE : PGDONTRECURSE;
            NativeControl.SetPropertyColorsToDefault(prop.Handle, flags);
        }

        public void SetPropertyTextColor(IPropertyGridItem prop, Color col, bool recurse = true)
        {
            var flags = recurse ? PGRECURSE : PGDONTRECURSE;
            NativeControl.SetPropertyTextColor(prop.Handle, col, flags);
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

        public void AddActionTrigger(
            PropertyGridKeyboardAction action,
            Key keycode,
            ModifierKeys modifiers = 0)
        {
            NativeControl.AddActionTrigger((int)action, (int)keycode, (int)modifiers);
        }

        public void ClearActionTriggers(PropertyGridKeyboardAction action)
        {
            NativeControl.ClearActionTriggers((int)action);
        }

        internal void DedicateKey(int keycode)
        {
            NativeControl.DedicateKey(keycode);
        }

        public static void AutoGetTranslation(bool enable)
        {
            Native.PropertyGrid.AutoGetTranslation(enable);
        }

        public void CenterSplitter(bool enableAutoResizing = false)
        {
            NativeControl.CenterSplitter(enableAutoResizing);
        }

        internal bool CommitChangesFromEditor()
        {
            return NativeControl.CommitChangesFromEditor(0);
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

        internal Color GetCaptionBackgroundColor()
        {
            return NativeControl.GetCaptionBackgroundColor();
        }

        internal Color GetCaptionForegroundColor()
        {
            return NativeControl.GetCaptionForegroundColor();
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

        public uint GetColumnCount()
        {
            return NativeControl.GetColumnCount();
        }

        internal Color GetEmptySpaceColor()
        {
            return NativeControl.GetEmptySpaceColor();
        }

        public int GetFontHeight()
        {
            return NativeControl.GetFontHeight();
        }

        internal Color GetLineColor()
        {
            return NativeControl.GetLineColor();
        }

        internal Color GetMarginColor()
        {
            return NativeControl.GetMarginColor();
        }

        public int GetMarginWidth()
        {
            return NativeControl.GetMarginWidth();
        }

        public int GetRowHeight()
        {
            return NativeControl.GetRowHeight();
        }

        internal Color GetSelectionBackgroundColor()
        {
            return NativeControl.GetSelectionBackgroundColor();
        }

        internal Color GetSelectionForegroundColor()
        {
            return NativeControl.GetSelectionForegroundColor();
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

        public void SetColumnCount(int colCount)
        {
            NativeControl.SetColumnCount(colCount);
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

        public static bool IsSmallScreen()
        {
            return Native.PropertyGrid.IsSmallScreen();
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

        internal static IntPtr GetEditorByName(string editorName)
        {
            return Native.PropertyGrid.GetEditorByName(editorName);
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

        internal void RaiseColDragging(EventArgs e)
        {
            OnColDragging(e);
            ColDragging?.Invoke(this, e);
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