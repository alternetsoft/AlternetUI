using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Alternet.Base.Collections;

using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    internal class PropertyGridHandler : WxControlHandler<PropertyGrid>, IPropertyGridHandler
    {
        private readonly PropertyGridVariant variant = new();

        static PropertyGridHandler()
        {
            PropertyGrid.EditWithListEdit += DialogFactory.EditWithListEdit;

            KnownColorStrings.CustomChanged += (s, e) =>
            {
                Native.PropertyGrid.KnownColorsSetCustomColorTitle(KnownColorStrings.Default.Custom);
            };
        }

        public PropertyGridHandler()
        {
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
        /// Gets property value used in the event handler as <see cref="IPropertyGridVariant"/>.
        /// </summary>
        [Browsable(false)]
        public virtual IPropertyGridVariant EventPropValueAsVariant
        {
            get
            {
                IntPtr handle = NativeControl.EventPropValue;
                PropertyGridVariant propValue = new(handle);
                return propValue;
            }
        }

        public new Native.PropertyGrid NativeControl =>
            (Native.PropertyGrid)base.NativeControl!;

        PropertyGridValidationFailure IPropertyGridHandler.EventValidationFailureBehavior
        {
            get => (PropertyGridValidationFailure)NativeControl.EventValidationFailureBehavior;
            set => NativeControl.EventValidationFailureBehavior = (int)value;
        }

        IPropertyGridItem? IPropertyGridHandler.EventProperty
        {
            get => PtrToItem(NativeControl.EventProperty);
        }

        string IPropertyGridHandler.EventValidationFailureMessage
        {
            get => NativeControl.EventValidationFailureMessage;
            set => NativeControl.EventValidationFailureMessage = value;
        }

        public override bool HasBorder
        {
            get => NativeControl.HasBorder;
            set => NativeControl.HasBorder = value;
        }

        PropertyGridCreateStyle IPropertyGridHandler.CreateStyle
        {
            get => (PropertyGridCreateStyle)NativeControl.CreateStyle;
            set => NativeControl.CreateStyle = (int)value;
        }

        PropertyGridCreateStyleEx IPropertyGridHandler.CreateStyleEx
        {
            get => (PropertyGridCreateStyleEx)NativeControl.CreateStyleEx;
            set => NativeControl.CreateStyleEx = (int)value;
        }

        int IPropertyGridHandler.EventColumn
        {
            get => NativeControl.EventColumn;
        }

        string IPropertyGridHandler.EventPropertyName
        {
            get => NativeControl.EventPropertyName;
        }

        /// <summary>
        /// Creates new variant instance for use with <see cref="PropertyGrid"/>
        /// </summary>
        public IPropertyGridVariant CreateVariant()
        {
            return new PropertyGridVariant();
        }

        public PropertyGridItemHandle CreateColorProperty(string label, string name, Color value)
        {
            KnownColorsAdd();
            return CreateHandle(NativeControl.CreateColorProperty(label, name, value));
        }

        public PropertyGridItemHandle CreateHandle(IntPtr ptr)
        {
            return new WxPropertyGridItemHandle(ptr);
        }

        public PropertyGridItemHandle CreateSystemColorProperty(
            string label,
            string name,
            Color value)
        {
            KnownColorsAdd();
            uint kind = GetColorKind(value);
            return CreateHandle(NativeControl.CreateSystemColorProperty(label, name, value, kind));
        }

        /// <summary>
        /// Enables or disables automatic translation for enum list labels and
        /// flags child property labels.
        /// </summary>
        /// <param name="enable"><c>true</c> enables automatic translation, <c>false</c>
        /// disables it.</param>
        public void AutoGetTranslation(bool enable)
        {
            Native.PropertyGrid.AutoGetTranslation(enable);
        }

        /// <summary>
        /// Registers all type handlers for use in <see cref="PropertyGrid"/>.
        /// </summary>
        public void InitAllTypeHandlers()
        {
            Native.PropertyGrid.InitAllTypeHandlers();
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
        /// Gets category of the property item.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual IPropertyGridItem? GetPropertyCategory(IPropertyGridItem? prop)
        {
            var ptr = ItemToPtr(prop);
            if (ptr is null)
                return null;
            var result = NativeControl.GetPropertyCategory(ptr.Value);
            return PtrToItem(result);
        }

        /// <summary>
        /// Gets first property item which satisfies search criteria specified by
        /// <paramref name="flags"/>.
        /// </summary>
        /// <param name="flags">Filter flags.</param>
        public virtual IPropertyGridItem? GetFirst(PropertyGridIteratorFlags flags)
        {
            var result = NativeControl.GetFirst((int)flags);
            return PtrToItem(result);
        }

        /// <summary>
        /// Gets parent of the property item.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual IPropertyGridItem? GetPropertyParent(IPropertyGridItem? prop)
        {
            var ptr = ItemToPtr(prop);
            if (ptr is null)
                return null;
            var result = NativeControl.GetPropertyParent(ptr.Value);
            return PtrToItem(result);
        }

        /// <summary>
        /// Gets first child of the property item.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual IPropertyGridItem? GetFirstChild(IPropertyGridItem? prop)
        {
            var ptr = ItemToPtr(prop);
            if (ptr is null)
                return null;
            var result = NativeControl.GetFirstChild(ptr.Value);
            return PtrToItem(result);
        }

        /// <summary>
        /// Gets selected property item.
        /// </summary>
        /// <returns></returns>
        public virtual IPropertyGridItem? GetSelection()
        {
            var result = NativeControl.GetSelection();
            return PtrToItem(result);
        }

        /// <summary>
        /// Gets property item with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name of the property item.</param>
        /// <returns></returns>
        public virtual IPropertyGridItem? GetProperty(string? name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            var result = NativeControl.GetProperty(name!);
            return PtrToItem(result);
        }

        /// <summary>
        /// Gets property item with the specified <paramref name="label"/>.
        /// </summary>
        /// <param name="label">label of the property item.</param>
        /// <returns></returns>
        public virtual IPropertyGridItem? GetPropertyByLabel(string? label)
        {
            if (string.IsNullOrEmpty(label))
                return null;
            var result = NativeControl.GetPropertyByLabel(label!);
            return PtrToItem(result);
        }

        /// <summary>
        /// Gets property item with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name of the property item.</param>
        /// <returns></returns>
        public virtual IPropertyGridItem? GetPropertyByName(string? name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            var result = NativeControl.GetPropertyByName(name!);
            return PtrToItem(result);
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
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(subname))
                return null;
            var result = NativeControl.GetPropertyByNameAndSubName(name!, subname!);
            return PtrToItem(result);
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

        public IPropertyGridVariant GetTempVariant()
        {
            return variant;
        }

        public virtual IPropertyGridItem? GetHitTestProp(PointD point)
        {
            if (Control is null)
                return null;
            var pointI = Control.PixelFromDip(point);
            var ptr = NativeControl.GetHitTestProp(pointI);
            var item = PtrToItem(ptr);
            return item;
        }

        public IPropertyGridVariant ToVariant(object? value)
        {
            variant.AsObject = value;
            return variant;
        }

        /// <summary>
        /// Gets property value as <see cref="IPropertyGridVariant"/>.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual IPropertyGridVariant GetPropertyValueAsVariant(IPropertyGridItem prop)
        {
            var ptr = ItemToPtr(prop);
            if (ptr is null)
                return CreateVariant();
            var handle = NativeControl.GetPropertyValueAsVariant(ptr.Value);
            PropertyGridVariant propValue = new(handle);
            return propValue;
        }

        /// <summary>
        /// Registers additional editors for use in <see cref="PropertyGrid"/>.
        /// </summary>
        public void RegisterAdditionalEditors()
        {
            Native.PropertyGrid.RegisterAdditionalEditors();
        }

        /// <summary>
        /// Sets string constants for <c>true</c> and <c>false</c> words
        /// used in <see cref="bool"/> properties.
        /// </summary>
        /// <param name="trueChoice"></param>
        /// <param name="falseChoice"></param>
        public void SetBoolChoices(string trueChoice, string falseChoice)
        {
            Native.PropertyGrid.SetBoolChoices(trueChoice, falseChoice);
        }

        /// <summary>
        /// Sets image associated with the property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="bmp">Image.</param>
        public void SetPropertyImage(IPropertyGridItem prop, ImageSet? bmp)
        {
            var ptr = ItemToPtr(prop);
            if (ptr is null)
                return;
            NativeControl.SetPropertyImage(ptr.Value, (UI.Native.ImageSet?)bmp?.Handler);
        }

        public bool IsSmallScreen()
        {
            return Native.PropertyGrid.IsSmallScreen();
        }

        void IPropertyGridHandler.RefreshProperty(IPropertyGridItem prop)
        {
            var ptr = ItemToPtr(prop);
            if (ptr is null)
                return;
            NativeControl.RefreshProperty(ptr.Value);
        }

        void IPropertyGridHandler.SetPropertyReadOnly(
            IPropertyGridItem prop,
            bool set,
            PropertyGridItemValueFlags flags)
        {
            var ptr = ItemToPtr(prop);
            if (ptr is null)
                return;
            NativeControl.SetPropertyReadOnly(ptr.Value, set, (int)flags);
        }

        void IPropertyGridHandler.SetPropertyValueUnspecified(IPropertyGridItem prop)
        {
            var ptr = ItemToPtr(prop);
            if (ptr is null)
                return;
            NativeControl.SetPropertyValueUnspecified(ptr.Value);
        }

        void IPropertyGridHandler.AppendIn(IPropertyGridItem id, IPropertyGridItem newproperty)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return;
            var ptr2 = ItemToPtr(newproperty);
            if (ptr2 is null)
                return;
            NativeControl.AppendIn(ptr.Value, ptr2.Value);
        }

        void IPropertyGridHandler.BeginAddChildren(IPropertyGridItem prop)
        {
            var ptr = ItemToPtr(prop);
            if (ptr is null)
                return;
            NativeControl.BeginAddChildren(ptr.Value);
        }

        bool IPropertyGridHandler.Collapse(IPropertyGridItem prop)
        {
            var ptr = ItemToPtr(prop);
            if (ptr is null)
                return false;
            return NativeControl.Collapse(ptr.Value);
        }

        void IPropertyGridHandler.DeleteProperty(IPropertyGridItem prop)
        {
            var ptr = ItemToPtr(prop);
            if (ptr is null)
                return;
            NativeControl.DeleteProperty(ptr.Value);
        }

        void IPropertyGridHandler.RemoveProperty(IPropertyGridItem id)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return;
            NativeControl.RemoveProperty(ptr.Value);
        }

        bool IPropertyGridHandler.DisableProperty(IPropertyGridItem id)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return false;
            return NativeControl.DisableProperty(ptr.Value);
        }

        bool IPropertyGridHandler.EnableProperty(IPropertyGridItem id, bool enable)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return false;
            return NativeControl.EnableProperty(ptr.Value, enable);
        }

        void IPropertyGridHandler.EndAddChildren(IPropertyGridItem id)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return;
            NativeControl.EndAddChildren(ptr.Value);
        }

        bool IPropertyGridHandler.Expand(IPropertyGridItem id)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return default;
            return NativeControl.Expand(ptr.Value);
        }

        nint IPropertyGridHandler.GetPropertyClientData(IPropertyGridItem id)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return default;
            return NativeControl.GetPropertyClientData(ptr.Value);
        }

        string IPropertyGridHandler.GetPropertyHelpString(IPropertyGridItem id)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return string.Empty;
            return NativeControl.GetPropertyHelpString(ptr.Value);
        }

        string IPropertyGridHandler.GetPropertyLabel(IPropertyGridItem id)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return string.Empty;
            return NativeControl.GetPropertyLabel(ptr.Value);
        }

        string IPropertyGridHandler.GetPropertyValueAsString(IPropertyGridItem id)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return string.Empty;
            return NativeControl.GetPropertyValueAsString(ptr.Value);
        }

        long IPropertyGridHandler.GetPropertyValueAsLong(IPropertyGridItem id)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return default;
            return NativeControl.GetPropertyValueAsLong(ptr.Value);
        }

        string IPropertyGridHandler.GetPropNameAsLabel()
        {
            return Native.PropertyGrid.NameAsLabel;
        }

        ulong IPropertyGridHandler.GetPropertyValueAsULong(IPropertyGridItem id)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return default;
            return NativeControl.GetPropertyValueAsULong(ptr.Value);
        }

        int IPropertyGridHandler.GetPropertyValueAsInt(IPropertyGridItem id)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return default;
            return NativeControl.GetPropertyValueAsInt(ptr.Value);
        }

        bool IPropertyGridHandler.GetPropertyValueAsBool(IPropertyGridItem id)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return default;
            return NativeControl.GetPropertyValueAsBool(ptr.Value);
        }

        double IPropertyGridHandler.GetPropertyValueAsDouble(IPropertyGridItem id)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return default;
            return NativeControl.GetPropertyValueAsDouble(ptr.Value);
        }

        DateTime IPropertyGridHandler.GetPropertyValueAsDateTime(IPropertyGridItem id)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return default;
            return NativeControl.GetPropertyValueAsDateTime(ptr.Value);
        }

        bool IPropertyGridHandler.HideProperty(
            IPropertyGridItem id,
            bool hide,
            PropertyGridItemValueFlags flags)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return default;
            return NativeControl.HideProperty(ptr.Value, hide, (int)flags);
        }

        void IPropertyGridHandler.Insert(
            IPropertyGridItem priorThis, IPropertyGridItem newproperty)
        {
            var ptr = ItemToPtr(priorThis);
            if (ptr is null)
                return;
            var ptr2 = ItemToPtr(newproperty);
            if (ptr2 is null)
                return;
            NativeControl.Insert(ptr.Value, ptr2.Value);
        }

        void IPropertyGridHandler.InsertByIndex(
            IPropertyGridItem parent,
            int index,
            IPropertyGridItem newproperty)
        {
            var ptr = ItemToPtr(parent);
            if (ptr is null)
                return;
            var ptr2 = ItemToPtr(newproperty);
            if (ptr2 is null)
                return;
            NativeControl.InsertByIndex(ptr.Value, index, ptr2.Value);
        }

        bool IPropertyGridHandler.IsPropertyCategory(IPropertyGridItem id)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return default;
            return NativeControl.IsPropertyCategory(ptr.Value);
        }

        bool IPropertyGridHandler.IsPropertyEnabled(IPropertyGridItem id)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return default;
            return NativeControl.IsPropertyEnabled(ptr.Value);
        }

        bool IPropertyGridHandler.IsPropertyExpanded(IPropertyGridItem id)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return default;
            return NativeControl.IsPropertyExpanded(ptr.Value);
        }

        bool IPropertyGridHandler.IsPropertyModified(IPropertyGridItem id)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return default;
            return NativeControl.IsPropertyModified(ptr.Value);
        }

        bool IPropertyGridHandler.IsPropertySelected(IPropertyGridItem id)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return default;
            return NativeControl.IsPropertySelected(ptr.Value);
        }

        bool IPropertyGridHandler.IsPropertyShown(IPropertyGridItem id)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return default;
            return NativeControl.IsPropertyShown(ptr.Value);
        }

        bool IPropertyGridHandler.IsPropertyValueUnspecified(IPropertyGridItem id)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return default;
            return NativeControl.IsPropertyValueUnspecified(ptr.Value);
        }

        void IPropertyGridHandler.LimitPropertyEditing(IPropertyGridItem id, bool limit)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return;
            NativeControl.LimitPropertyEditing(ptr.Value, limit);
        }

        void IPropertyGridHandler.ReplaceProperty(IPropertyGridItem id, IPropertyGridItem property)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return;
            var ptr2 = ItemToPtr(property);
            if (ptr2 is null)
                return;
            NativeControl.ReplaceProperty(ptr.Value, ptr2.Value);
        }

        void IPropertyGridHandler.SetPropertyBackgroundColor(
            IPropertyGridItem id,
            Color color,
            PropertyGridItemValueFlags flags)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return;
            NativeControl.SetPropertyBackgroundColor(ptr.Value, color, (int)flags);
        }

        void IPropertyGridHandler.SetPropertyColorsToDefault(
            IPropertyGridItem id,
            PropertyGridItemValueFlags flags)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return;
            NativeControl.SetPropertyColorsToDefault(ptr.Value, (int)flags);
        }

        void IPropertyGridHandler.SetPropertyTextColor(
            IPropertyGridItem id,
            Color col,
            PropertyGridItemValueFlags flags)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return;
            NativeControl.SetPropertyTextColor(ptr.Value, col, (int)flags);
        }

        Color IPropertyGridHandler.GetPropertyBackgroundColor(IPropertyGridItem id)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return Color.Empty;
            return NativeControl.GetPropertyBackgroundColor(ptr.Value);
        }

        Color IPropertyGridHandler.GetPropertyTextColor(IPropertyGridItem id)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return Color.Empty;
            return NativeControl.GetPropertyTextColor(ptr.Value);
        }

        void IPropertyGridHandler.SetPropertyEditorByName(IPropertyGridItem id, string editorName)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return;
            NativeControl.SetPropertyEditorByName(ptr.Value, editorName);
        }

        void IPropertyGridHandler.SetPropertyLabel(IPropertyGridItem id, string newproplabel)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return;
            NativeControl.SetPropertyLabel(ptr.Value, newproplabel);
        }

        void IPropertyGridHandler.SetPropertyName(IPropertyGridItem id, string newName)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return;
            NativeControl.SetPropertyName(ptr.Value, newName);
        }

        void IPropertyGridHandler.SetPropertyHelpString(IPropertyGridItem id, string helpString)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return;
            NativeControl.SetPropertyHelpString(ptr.Value, helpString);
        }

        bool IPropertyGridHandler.SetPropertyMaxLength(IPropertyGridItem id, int maxLen)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return default;
            return NativeControl.SetPropertyMaxLength(ptr.Value, maxLen);
        }

        void IPropertyGridHandler.SetPropertyValueAsLong(IPropertyGridItem id, long value)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return;
            NativeControl.SetPropertyValueAsLong(ptr.Value, value);
        }

        void IPropertyGridHandler.SetPropertyValueAsInt(IPropertyGridItem id, int value)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return;
            NativeControl.SetPropertyValueAsInt(ptr.Value, value);
        }

        void IPropertyGridHandler.SetPropertyValueAsDouble(IPropertyGridItem id, double value)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return;
            NativeControl.SetPropertyValueAsDouble(ptr.Value, value);
        }

        void IPropertyGridHandler.SetPropertyValueAsBool(IPropertyGridItem id, bool value)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return;
            NativeControl.SetPropertyValueAsBool(ptr.Value, value);
        }

        void IPropertyGridHandler.SetPropertyValueAsStr(IPropertyGridItem id, string value)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return;
            NativeControl.SetPropertyValueAsStr(ptr.Value, value);
        }

        void IPropertyGridHandler.SetPropertyValueAsVariant(
            IPropertyGridItem id,
            IPropertyGridVariant variant)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return;
            NativeControl.SetPropertyValueAsVariant(ptr.Value, VariantToHandle(variant));
        }

        public nint VariantToHandle(IPropertyGridVariant variant)
        {
            return ((PropertyGridVariant)variant).Handle;
        }

        void IPropertyGridHandler.SetPropertyValueAsDateTime(IPropertyGridItem id, DateTime value)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return;
            NativeControl.SetPropertyValueAsDateTime(ptr.Value, value);
        }

        void IPropertyGridHandler.SetValidationFailureBehavior(PropertyGridValidationFailure vfbFlags)
        {
            NativeControl.SetValidationFailureBehavior((int)vfbFlags);
        }

        void IPropertyGridHandler.SortChildren(IPropertyGridItem id, PropertyGridItemValueFlags flags)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return;
            NativeControl.SortChildren(ptr.Value, (int)flags);
        }

        bool IPropertyGridHandler.ChangePropertyValue(
            IPropertyGridItem id,
            IPropertyGridVariant variant)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return false;
            return NativeControl.ChangePropertyValue(ptr.Value, VariantToHandle(variant));
        }

        void IPropertyGridHandler.SetPropertyClientData(IPropertyGridItem prop, nint clientData)
        {
            var ptr = ItemToPtr(prop);
            if (ptr is null)
                return;
            NativeControl.SetPropertyClientData(ptr.Value, clientData);
        }

        void IPropertyGridHandler.SetPropertyAttribute(
            IPropertyGridItem id,
            string attrName,
            IPropertyGridVariant variant,
            PropertyGridItemValueFlags argFlags)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return;
            NativeControl.SetPropertyAttribute(
                ptr.Value,
                attrName,
                VariantToHandle(variant),
                (int)argFlags);
        }

        void IPropertyGridHandler.SetPropertyAttributeAll(string attrName, IPropertyGridVariant variant)
        {
            NativeControl.SetPropertyAttributeAll(attrName, VariantToHandle(variant));
        }

        int IPropertyGridHandler.GetSplitterPosition(int splitterIndex)
        {
            return NativeControl.GetSplitterPosition((uint)splitterIndex);
        }

        int IPropertyGridHandler.GetVerticalSpacing()
        {
            return NativeControl.GetVerticalSpacing();
        }

        bool IPropertyGridHandler.IsEditorFocused()
        {
            return NativeControl.IsEditorFocused();
        }

        bool IPropertyGridHandler.IsEditorsValueModified()
        {
            return NativeControl.IsEditorsValueModified();
        }

        bool IPropertyGridHandler.IsAnyModified()
        {
            return NativeControl.IsAnyModified();
        }

        void IPropertyGridHandler.ResetColors()
        {
            NativeControl.ResetColors();
        }

        void IPropertyGridHandler.ResetColumnSizes(bool enableAutoResizing)
        {
            NativeControl.ResetColumnSizes(enableAutoResizing);
        }

        void IPropertyGridHandler.MakeColumnEditable(int column, bool editable)
        {
            NativeControl.MakeColumnEditable((uint)column, editable);
        }

        void IPropertyGridHandler.BeginLabelEdit(int column)
        {
            NativeControl.BeginLabelEdit((uint)column);
        }

        void IPropertyGridHandler.EndLabelEdit(bool commit)
        {
            NativeControl.EndLabelEdit(commit);
        }

        void IPropertyGridHandler.SetCaptionBackgroundColor(Color col)
        {
            NativeControl.SetCaptionBackgroundColor(col);
        }

        void IPropertyGridHandler.SetCaptionTextColor(Color col)
        {
            NativeControl.SetCaptionTextColor(col);
        }

        void IPropertyGridHandler.SetCellBackgroundColor(Color col)
        {
            NativeControl.SetCellBackgroundColor(col);
        }

        void IPropertyGridHandler.SetCellDisabledTextColor(Color col)
        {
            NativeControl.SetCellDisabledTextColor(col);
        }

        void IPropertyGridHandler.SetCellTextColor(Color col)
        {
            NativeControl.SetCellTextColor(col);
        }

        void IPropertyGridHandler.SetColumnCount(int colCount)
        {
            NativeControl.SetColumnCount(colCount);
        }

        void IPropertyGridHandler.SetEmptySpaceColor(Color col)
        {
            NativeControl.SetEmptySpaceColor(col);
        }

        void IPropertyGridHandler.SetLineColor(Color col)
        {
            NativeControl.SetLineColor(col);
        }

        void IPropertyGridHandler.SetMarginColor(Color col)
        {
            NativeControl.SetMarginColor(col);
        }

        void IPropertyGridHandler.SetSelectionBackgroundColor(Color col)
        {
            NativeControl.SetSelectionBackgroundColor(col);
        }

        void IPropertyGridHandler.SetSelectionTextColor(Color col)
        {
            NativeControl.SetSelectionTextColor(col);
        }

        void IPropertyGridHandler.SetSplitterPosition(int newXPos, int col)
        {
            NativeControl.SetSplitterPosition(newXPos, col);
        }

        string IPropertyGridHandler.GetUnspecifiedValueText(PropertyGridValueFormatFlags argFlags)
        {
            return NativeControl.GetUnspecifiedValueText((int)argFlags);
        }

        void IPropertyGridHandler.SetVirtualWidth(int width)
        {
            NativeControl.SetVirtualWidth(width);
        }

        void IPropertyGridHandler.SetSplitterLeft(bool privateChildrenToo)
        {
            NativeControl.SetSplitterLeft(privateChildrenToo);
        }

        void IPropertyGridHandler.SetVerticalSpacing(int vspacing)
        {
            NativeControl.SetVerticalSpacing(vspacing);
        }

        bool IPropertyGridHandler.HasVirtualWidth()
        {
            return NativeControl.HasVirtualWidth();
        }

        int IPropertyGridHandler.GetCommonValueCount()
        {
            return (int)NativeControl.GetCommonValueCount();
        }

        string IPropertyGridHandler.GetCommonValueLabel(int i)
        {
            return NativeControl.GetCommonValueLabel((uint)i);
        }

        int IPropertyGridHandler.GetUnspecifiedCommonValue()
        {
            return NativeControl.GetUnspecifiedCommonValue();
        }

        void IPropertyGridHandler.SetUnspecifiedCommonValue(int index)
        {
            NativeControl.SetUnspecifiedCommonValue(index);
        }

        void IPropertyGridHandler.RefreshEditor()
        {
            NativeControl.RefreshEditor();
        }

        bool IPropertyGridHandler.WasValueChangedInEvent()
        {
            return NativeControl.WasValueChangedInEvent();
        }

        int IPropertyGridHandler.GetSpacingY()
        {
            return NativeControl.GetSpacingY();
        }

        void IPropertyGridHandler.SetupTextCtrlValue(string text)
        {
            NativeControl.SetupTextCtrlValue(text);
        }

        bool IPropertyGridHandler.UnfocusEditor()
        {
            return NativeControl.UnfocusEditor();
        }

        bool IPropertyGridHandler.EnsureVisible(IPropertyGridItem propArg)
        {
            var ptr = ItemToPtr(propArg);
            if (ptr is null)
                return false;
            return NativeControl.EnsureVisible(ptr.Value);
        }

        bool IPropertyGridHandler.SelectProperty(IPropertyGridItem propArg, bool focus)
        {
            var ptr = ItemToPtr(propArg);
            if (ptr is null)
                return false;
            return NativeControl.SelectProperty(ptr.Value, focus);
        }

        bool IPropertyGridHandler.AddToSelection(IPropertyGridItem propArg)
        {
            var ptr = ItemToPtr(propArg);
            if (ptr is null)
                return false;
            return NativeControl.AddToSelection(ptr.Value);
        }

        bool IPropertyGridHandler.RemoveFromSelection(IPropertyGridItem propArg)
        {
            var ptr = ItemToPtr(propArg);
            if (ptr is null)
                return false;
            return NativeControl.RemoveFromSelection(ptr.Value);
        }

        void IPropertyGridHandler.SetCurrentCategory(IPropertyGridItem id)
        {
            var ptr = ItemToPtr(id);
            if (ptr is null)
                return;
            NativeControl.SetCurrentCategory(ptr.Value);
        }

        RectI IPropertyGridHandler.GetImageRect(IPropertyGridItem p, int item)
        {
            var ptr = ItemToPtr(p);
            if (ptr is null)
                return default;
            return NativeControl.GetImageRect(ptr.Value, item);
        }

        SizeI IPropertyGridHandler.GetImageSize(IPropertyGridItem? p, int item)
        {
            var ptr = ItemToPtr(p);
            if (ptr is null)
                return default;
            return NativeControl.GetImageSize(ptr.Value, item);
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateStringProperty(
            string label,
            string name,
            string value)
        {
            return CreateHandle(NativeControl.CreateStringProperty(label, name, value));
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateFilenameProperty(
            string label,
            string name,
            string value)
        {
            return CreateHandle(NativeControl.CreateFilenameProperty(label, name, value));
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateDirProperty(string label, string name, string value)
        {
            return CreateHandle(NativeControl.CreateDirProperty(label, name, value));
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateImageFilenameProperty(string label, string name, string value)
        {
            return CreateHandle(NativeControl.CreateImageFilenameProperty(label, name, value));
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateCursorProperty(string label, string name, int value)
        {
            return CreateHandle(NativeControl.CreateCursorProperty(label, name, value));
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateBoolProperty(string label, string name, bool value)
        {
            return CreateHandle(NativeControl.CreateBoolProperty(label, name, value));
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateIntProperty(string label, string name, long value)
        {
            return CreateHandle(NativeControl.CreateIntProperty(label, name, value));
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateFloatProperty(string label, string name, double value)
        {
            return CreateHandle(NativeControl.CreateFloatProperty(label, name, value));
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateUIntProperty(string label, string name, ulong value)
        {
            return CreateHandle(NativeControl.CreateUIntProperty(label, name, value));
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateLongStringProperty(string label, string name, string value)
        {
            return CreateHandle(NativeControl.CreateLongStringProperty(label, name, value));
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateDateProperty(string label, string name, DateTime value)
        {
            return CreateHandle(NativeControl.CreateDateProperty(label, name, value));
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateEditEnumProperty(string label, string name, IPropertyGridChoices choices, string value)
        {
            return CreateHandle(NativeControl.CreateEditEnumProperty(label, name, ChoicesToPtr(choices), value));
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateEnumProperty(string label, string name, IPropertyGridChoices choices, int value)
        {
            return CreateHandle(NativeControl.CreateEnumProperty(label, name, ChoicesToPtr(choices), value));
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateFlagsProperty(string label, string name, IPropertyGridChoices choices, int value)
        {
            return CreateHandle(NativeControl.CreateFlagsProperty(label, name, ChoicesToPtr(choices), value));
        }

        public nint ChoicesToPtr(IPropertyGridChoices choices)
        {
            return ((PropertyGridChoices)choices).Handle;
        }

        PropertyGridItemHandle IPropertyGridHandler.CreatePropCategory(string label, string name)
        {
            return CreateHandle(NativeControl.CreatePropCategory(label, name));
        }

        void IPropertyGridHandler.Clear()
        {
            NativeControl.Clear();
        }

        void IPropertyGridHandler.Append(IPropertyGridItem property)
        {
            var ptr = ItemToPtr(property);
            if (ptr is null)
                return;
            NativeControl.Append(ptr.Value);
        }

        bool IPropertyGridHandler.ClearSelection(bool validation)
        {
            return NativeControl.ClearSelection(validation);
        }

        void IPropertyGridHandler.ClearModifiedStatus()
        {
            NativeControl.ClearModifiedStatus();
        }

        bool IPropertyGridHandler.CollapseAll()
        {
            return NativeControl.CollapseAll();
        }

        bool IPropertyGridHandler.EditorValidate()
        {
            return NativeControl.EditorValidate();
        }

        bool IPropertyGridHandler.ExpandAll(bool expand)
        {
            return NativeControl.ExpandAll(expand);
        }

        string IPropertyGridHandler.GetPropertyName(IPropertyGridItem property)
        {
            var ptr = ItemToPtr(property);
            if (ptr is null)
                return string.Empty;
            return NativeControl.GetPropertyName(ptr.Value);
        }

        bool IPropertyGridHandler.RestoreEditableState(string src, PropertyGridEditableState restoreStates)
        {
            return NativeControl.RestoreEditableState(src, (int)restoreStates);
        }

        string IPropertyGridHandler.SaveEditableState(PropertyGridEditableState includedStates)
        {
            return NativeControl.SaveEditableState((int)includedStates);
        }

        bool IPropertyGridHandler.SetColumnProportion(int column, int proportion)
        {
            return NativeControl.SetColumnProportion((uint)column, proportion);
        }

        int IPropertyGridHandler.GetColumnProportion(int column)
        {
            return NativeControl.GetColumnProportion((uint)column);
        }

        void IPropertyGridHandler.Sort(PropertyGridItemValueFlags flags)
        {
            NativeControl.Sort((int)flags);
        }

        PointI IPropertyGridHandler.CalcScrolledPosition(PointI point)
        {
            return NativeControl.CalcScrolledPosition(point);
        }

        PointI IPropertyGridHandler.CalcUnscrolledPosition(PointI point)
        {
            return NativeControl.CalcUnscrolledPosition(point);
        }

        int IPropertyGridHandler.GetHitTestColumn(PointI point)
        {
            return NativeControl.GetHitTestColumn(point);
        }

        void IPropertyGridHandler.SetPropertyFlag(
            IPropertyGridItem prop,
            PropertyGridItemFlags flag,
            bool value)
        {
            var ptr = ItemToPtr(prop);
            if (ptr is null)
                return;
            NativeControl.SetPropertyFlag(ptr.Value, (int)flag, value);
        }

        void IPropertyGridHandler.AddActionTrigger(
            PropertyGridKeyboardAction action,
            Key keycode,
            ModifierKeys modifiers)
        {
            NativeControl.AddActionTrigger((int)action, (int)keycode, (int)modifiers);
        }

        void IPropertyGridHandler.DedicateKey(Key keycode)
        {
            NativeControl.DedicateKey((int)keycode);
        }

        void IPropertyGridHandler.CenterSplitter(bool enableAutoResizing)
        {
            NativeControl.CenterSplitter(enableAutoResizing);
        }

        void IPropertyGridHandler.ClearActionTriggers(PropertyGridKeyboardAction action)
        {
            NativeControl.ClearActionTriggers((int)action);
        }

        bool IPropertyGridHandler.CommitChangesFromEditor(PropertyGridSelectPropFlags flags)
        {
            return NativeControl.CommitChangesFromEditor((uint)flags);
        }

        void IPropertyGridHandler.EditorsValueWasModified()
        {
            NativeControl.EditorsValueWasModified();
        }

        void IPropertyGridHandler.EditorsValueWasNotModified()
        {
            NativeControl.EditorsValueWasNotModified();
        }

        bool IPropertyGridHandler.EnableCategories(bool enable)
        {
            return NativeControl.EnableCategories(enable);
        }

        SizeD IPropertyGridHandler.FitColumns()
        {
            return NativeControl.FitColumns();
        }

        Color IPropertyGridHandler.GetCaptionBackgroundColor()
        {
            return NativeControl.GetCaptionBackgroundColor();
        }

        Color IPropertyGridHandler.GetCaptionForegroundColor()
        {
            return NativeControl.GetCaptionForegroundColor();
        }

        Color IPropertyGridHandler.GetCellBackgroundColor()
        {
            return NativeControl.GetCellBackgroundColor();
        }

        Color IPropertyGridHandler.GetCellDisabledTextColor()
        {
            return NativeControl.GetCellDisabledTextColor();
        }

        Color IPropertyGridHandler.GetCellTextColor()
        {
            return NativeControl.GetCellTextColor();
        }

        int IPropertyGridHandler.GetColumnCount()
        {
            return (int)NativeControl.GetColumnCount();
        }

        Color IPropertyGridHandler.GetEmptySpaceColor()
        {
            return NativeControl.GetEmptySpaceColor();
        }

        int IPropertyGridHandler.GetFontHeight()
        {
            return NativeControl.GetFontHeight();
        }

        Color IPropertyGridHandler.GetLineColor()
        {
            return NativeControl.GetLineColor();
        }

        Color IPropertyGridHandler.GetMarginColor()
        {
            return NativeControl.GetMarginColor();
        }

        int IPropertyGridHandler.GetMarginWidth()
        {
            return NativeControl.GetMarginWidth();
        }

        int IPropertyGridHandler.GetRowHeight()
        {
            return NativeControl.GetRowHeight();
        }

        Color IPropertyGridHandler.GetSelectionBackgroundColor()
        {
            return NativeControl.GetSelectionBackgroundColor();
        }

        Color IPropertyGridHandler.GetSelectionForegroundColor()
        {
            return NativeControl.GetSelectionForegroundColor();
        }

        internal static Color SetColorKind(Color value, uint kind)
        {
            Color Fn()
            {
                const uint PG_COLOUR_CUSTOM = 0xFFFFFF;

                if (kind == PG_COLOUR_CUSTOM)
                    return value;
                else
                {
                    KnownColor knownColor = WxColorUtils.Convert((SystemSettingsColor)kind);
                    if (knownColor == 0)
                        return value;
                    Color result = Color.FromKnownColor(knownColor);
                    return result;
                }
            }

            var result = Fn();
            if (result.IsKnownColor)
                return result;
            if (!result.IsOpaque)
                return result;
            var knownResult = KnownColorTable.ArgbToKnownColor(result.AsUInt());
            return knownResult;
        }

        internal static uint GetColorKind(Color value)
        {
            const uint PG_COLOUR_CUSTOM = 0xFFFFFF;
            uint kind = PG_COLOUR_CUSTOM;

            if (value.IsKnownColor)
            {
                KnownColor knownColor = value.ToKnownColor();
                SystemSettingsColor converted = WxColorUtils.Convert(knownColor);
                if (converted != SystemSettingsColor.Max)
                    kind = (uint)converted;
            }

            return kind;
        }

        internal static void KnownColorsClear()
        {
            Native.PropertyGrid.KnownColorsClear();
        }

        internal static void KnownColorsAdd()
        {
            if (PropertyGrid.StaticFlags.HasFlag(PropertyGrid.StaticStateFlags.KnownColorsAdded))
                return;
            PropertyGrid.StaticFlags |= PropertyGrid.StaticStateFlags.KnownColorsAdded;

            var items = ColorUtils.GetColorInfoItems();

            KnownColorsClear();

            foreach (var item in items)
            {
                if (!item.Visible)
                    continue;
                KnownColorsAdd(
                            item.Label,
                            item.LabelLocalized,
                            item.Value,
                            item.KnownColor);
            }

            KnownColorsApply();
        }

        internal static void KnownColorsAdd(
            string name,
            string title,
            Color value,
            KnownColor knownColor)
        {
            Native.PropertyGrid.KnownColorsAdd(name, title, value, (int)knownColor);
        }

        internal static void KnownColorsApply()
        {
            Native.PropertyGrid.KnownColorsApply();
        }

        internal static IntPtr GetEditorByName(string editorName)
        {
            return Native.PropertyGrid.GetEditorByName(editorName);
        }

        internal override Native.Control CreateNativeControl()
        {
            return new NativePropertyGrid(PropertyGrid.DefaultCreateStyle);
        }

        internal bool CommitChangesFromEditor()
        {
            return NativeControl.CommitChangesFromEditor(0);
        }

        internal IntPtr GetPropertyImage(IPropertyGridItem prop)
        {
            var ptr = ItemToPtr(prop);
            if (ptr is null)
                return default;
            return NativeControl.GetPropertyImage(ptr.Value);
        }

        internal void SetPropertyEditor(IPropertyGridItem prop, IntPtr editor)
        {
            var ptr = ItemToPtr(prop);
            if (ptr is null)
                return;
            NativeControl.SetPropertyEditor(ptr.Value, editor);
        }

        internal IntPtr? ItemToPtr(IPropertyGridItem? prop)
        {
            if (DisposingOrDisposed)
                return null;
            var handle = (prop?.Handle as WxPropertyGridItemHandle)?.Handle;
            return handle;
        }

        internal void DeleteProperty(IPropertyGridItem prop)
        {
            var ptr = ItemToPtr(prop);
            if (ptr is null)
                return;
            NativeControl.DeleteProperty(ptr.Value);
        }

        internal void SetupTextCtrlValue(string text)
        {
            NativeControl.SetupTextCtrlValue(text);
        }

        internal void EndAddChildren(IPropertyGridItem prop)
        {
            var ptr = ItemToPtr(prop);
            if (ptr is null)
                return;
            NativeControl.EndAddChildren(ptr.Value);
        }

        public override void OnSystemColorsChanged()
        {
            base.OnSystemColorsChanged();
            NativeControl.EndLabelEdit(false);
            NativeControl.ResetColors();
            NativeControl.RefreshEditor();
            if (App.IsWindowsOS)
                RecreateWindow();
        }

        private IPropertyGridItem? PtrToItem(IntPtr ptr)
        {
            return Control?.HandleToItem(new WxPropertyGridItemHandle(ptr));
        }

        public class WxPropertyGridItemHandle : PropertyGridItemHandle
        {
            private readonly IntPtr handle;

            public WxPropertyGridItemHandle(IntPtr handle)
            {
                this.handle = handle;
            }

            public IntPtr Handle => handle;

            public override int GetHashCode()
            {
                return handle.GetHashCode();
            }

            public override bool Equals(object? obj)
            {
                if (obj is not WxPropertyGridItemHandle wx)
                    return false;
                return handle == wx.handle;
            }
        }

        public class NativePropertyGrid : Native.PropertyGrid
        {
            public NativePropertyGrid(PropertyGridCreateStyle style)
                : base()
            {
                SetNativePointer(NativeApi.PropertyGrid_CreateEx_((int)style));
            }
        }
    }
}