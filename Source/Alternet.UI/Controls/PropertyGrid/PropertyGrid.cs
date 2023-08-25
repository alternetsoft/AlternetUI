using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Defines default visual style for the newly created
        /// <see cref="PropertyGrid"/> controls.
        /// </summary>
        public static PropertyGridCreateStyle DefaultCreateStyle { get; set; }
            = PropertyGridCreateStyle.DefaultStyle;

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
            /*var result = */NativeControl.AppendIn(prop.Handle, newproperty.Handle);
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
            /*var result = */NativeControl.RemoveProperty(prop.Handle);
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
            /*var result = */NativeControl.Insert(priorThis.Handle, newproperty.Handle);
        }

        public void Insert(IPropertyGridItem parent, int index, IPropertyGridItem newproperty)
        {
            /*var result = */NativeControl.InsertByIndex(parent.Handle, index, newproperty.Handle);
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
            /*var result = */NativeControl.ReplaceProperty(prop.Handle, newProp.Handle);
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

        internal static IntPtr GetEditorByName(string editorName)
        {
            return Native.PropertyGrid.GetEditorByName(editorName);
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

#pragma warning disable
        private IPropertyGridItem? PtrToItem(IntPtr ptr)
#pragma warning restore
        {
            return null;
        }
    }
}