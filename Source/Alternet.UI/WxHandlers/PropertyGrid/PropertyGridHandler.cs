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

        bool IPropertyGridHandler.HasBorder
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
            if (prop is null)
                return null;
            var result = NativeControl.GetPropertyCategory(ItemToPtr(prop));
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
            if (prop is null)
                return null;
            var result = NativeControl.GetPropertyParent(ItemToPtr(prop));
            return PtrToItem(result);
        }

        /// <summary>
        /// Gets first child of the property item.
        /// </summary>
        /// <param name="prop">Property item.</param>
        public virtual IPropertyGridItem? GetFirstChild(IPropertyGridItem? prop)
        {
            if (prop is null)
                return null;
            var result = NativeControl.GetFirstChild(ItemToPtr(prop));
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
            var handle = NativeControl.GetPropertyValueAsVariant(ItemToPtr(prop));
            PropertyGridVariant propValue = new(handle);
            return propValue;
        }

        /// <summary>
        /// Sets validator of a property.
        /// </summary>
        /// <param name="prop">Property item.</param>
        /// <param name="validator">Value validator.</param>
        public virtual void SetPropertyValidator(IPropertyGridItem prop, IValueValidator validator)
        {
            IntPtr ptr = default;
            if (validator != null)
                ptr = validator.Handle;
            NativeControl.SetPropertyValidator(ItemToPtr(prop), ptr);
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
            NativeControl.SetPropertyImage(ItemToPtr(prop), (UI.Native.ImageSet?)bmp?.Handler);
        }

        public bool IsSmallScreen()
        {
            return Native.PropertyGrid.IsSmallScreen();
        }

        void IPropertyGridHandler.RefreshProperty(IPropertyGridItem p)
        {
            NativeControl.RefreshProperty(ItemToPtr(p));
        }

        void IPropertyGridHandler.SetPropertyReadOnly(
            IPropertyGridItem id,
            bool set,
            PropertyGridItemValueFlags flags)
        {
            NativeControl.SetPropertyReadOnly(ItemToPtr(id), set, (int)flags);
        }

        void IPropertyGridHandler.SetPropertyValueUnspecified(IPropertyGridItem id)
        {
            NativeControl.SetPropertyValueUnspecified(ItemToPtr(id));
        }

        void IPropertyGridHandler.AppendIn(IPropertyGridItem id, IPropertyGridItem newproperty)
        {
            NativeControl.AppendIn(ItemToPtr(id), ItemToPtr(newproperty));
        }

        void IPropertyGridHandler.BeginAddChildren(IPropertyGridItem id)
        {
            NativeControl.BeginAddChildren(ItemToPtr(id));
        }

        bool IPropertyGridHandler.Collapse(IPropertyGridItem id)
        {
            return NativeControl.Collapse(ItemToPtr(id));
        }

        void IPropertyGridHandler.DeleteProperty(IPropertyGridItem id)
        {
            NativeControl.DeleteProperty(ItemToPtr(id));
        }

        void IPropertyGridHandler.RemoveProperty(IPropertyGridItem id)
        {
            NativeControl.RemoveProperty(ItemToPtr(id));
        }

        bool IPropertyGridHandler.DisableProperty(IPropertyGridItem id)
        {
            return NativeControl.DisableProperty(ItemToPtr(id));
        }

        bool IPropertyGridHandler.EnableProperty(IPropertyGridItem id, bool enable)
        {
            return NativeControl.EnableProperty(ItemToPtr(id), enable);
        }

        void IPropertyGridHandler.EndAddChildren(IPropertyGridItem id)
        {
            NativeControl.EndAddChildren(ItemToPtr(id));
        }

        bool IPropertyGridHandler.Expand(IPropertyGridItem id)
        {
            return NativeControl.Expand(ItemToPtr(id));
        }

        nint IPropertyGridHandler.GetPropertyClientData(IPropertyGridItem id)
        {
            return NativeControl.GetPropertyClientData(ItemToPtr(id));
        }

        string IPropertyGridHandler.GetPropertyHelpString(IPropertyGridItem id)
        {
            return NativeControl.GetPropertyHelpString(ItemToPtr(id));
        }

        string IPropertyGridHandler.GetPropertyLabel(IPropertyGridItem id)
        {
            return NativeControl.GetPropertyLabel(ItemToPtr(id));
        }

        string IPropertyGridHandler.GetPropertyValueAsString(IPropertyGridItem id)
        {
            return NativeControl.GetPropertyValueAsString(ItemToPtr(id));
        }

        long IPropertyGridHandler.GetPropertyValueAsLong(IPropertyGridItem id)
        {
            return NativeControl.GetPropertyValueAsLong(ItemToPtr(id));
        }

        string IPropertyGridHandler.GetPropNameAsLabel()
        {
            return Native.PropertyGrid.NameAsLabel;
        }

        ulong IPropertyGridHandler.GetPropertyValueAsULong(IPropertyGridItem id)
        {
            return NativeControl.GetPropertyValueAsULong(ItemToPtr(id));
        }

        int IPropertyGridHandler.GetPropertyValueAsInt(IPropertyGridItem id)
        {
            return NativeControl.GetPropertyValueAsInt(ItemToPtr(id));
        }

        bool IPropertyGridHandler.GetPropertyValueAsBool(IPropertyGridItem id)
        {
            return NativeControl.GetPropertyValueAsBool(ItemToPtr(id));
        }

        double IPropertyGridHandler.GetPropertyValueAsDouble(IPropertyGridItem id)
        {
            return NativeControl.GetPropertyValueAsDouble(ItemToPtr(id));
        }

        DateTime IPropertyGridHandler.GetPropertyValueAsDateTime(IPropertyGridItem id)
        {
            return NativeControl.GetPropertyValueAsDateTime(ItemToPtr(id));
        }

        bool IPropertyGridHandler.HideProperty(
            IPropertyGridItem id,
            bool hide,
            PropertyGridItemValueFlags flags)
        {
            return NativeControl.HideProperty(ItemToPtr(id), hide, (int)flags);
        }

        void IPropertyGridHandler.Insert(
            IPropertyGridItem priorThis, IPropertyGridItem newproperty)
        {
            NativeControl.Insert(ItemToPtr(priorThis), ItemToPtr(newproperty));
        }

        void IPropertyGridHandler.InsertByIndex(IPropertyGridItem parent, int index, IPropertyGridItem newproperty)
        {
            NativeControl.InsertByIndex(ItemToPtr(parent), index, ItemToPtr(newproperty));
        }

        bool IPropertyGridHandler.IsPropertyCategory(IPropertyGridItem id)
        {
            return NativeControl.IsPropertyCategory(ItemToPtr(id));
        }

        bool IPropertyGridHandler.IsPropertyEnabled(IPropertyGridItem id)
        {
            return NativeControl.IsPropertyEnabled(ItemToPtr(id));
        }

        bool IPropertyGridHandler.IsPropertyExpanded(IPropertyGridItem id)
        {
            return NativeControl.IsPropertyExpanded(ItemToPtr(id));
        }

        bool IPropertyGridHandler.IsPropertyModified(IPropertyGridItem id)
        {
            return NativeControl.IsPropertyModified(ItemToPtr(id));
        }

        bool IPropertyGridHandler.IsPropertySelected(IPropertyGridItem id)
        {
            return NativeControl.IsPropertySelected(ItemToPtr(id));
        }

        bool IPropertyGridHandler.IsPropertyShown(IPropertyGridItem id)
        {
            return NativeControl.IsPropertyShown(ItemToPtr(id));
        }

        bool IPropertyGridHandler.IsPropertyValueUnspecified(IPropertyGridItem id)
        {
            return NativeControl.IsPropertyValueUnspecified(ItemToPtr(id));
        }

        void IPropertyGridHandler.LimitPropertyEditing(IPropertyGridItem id, bool limit)
        {
            NativeControl.LimitPropertyEditing(ItemToPtr(id), limit);
        }

        void IPropertyGridHandler.ReplaceProperty(IPropertyGridItem id, IPropertyGridItem property)
        {
            NativeControl.ReplaceProperty(ItemToPtr(id), ItemToPtr(property));
        }

        void IPropertyGridHandler.SetPropertyBackgroundColor(
            IPropertyGridItem id,
            Color color,
            PropertyGridItemValueFlags flags)
        {
            NativeControl.SetPropertyBackgroundColor(ItemToPtr(id), color, (int)flags);
        }

        void IPropertyGridHandler.SetPropertyColorsToDefault(
            IPropertyGridItem id,
            PropertyGridItemValueFlags flags)
        {
            NativeControl.SetPropertyColorsToDefault(ItemToPtr(id), (int)flags);
        }

        void IPropertyGridHandler.SetPropertyTextColor(
            IPropertyGridItem id,
            Color col,
            PropertyGridItemValueFlags flags)
        {
            NativeControl.SetPropertyTextColor(ItemToPtr(id), col, (int)flags);
        }

        Color IPropertyGridHandler.GetPropertyBackgroundColor(IPropertyGridItem id)
        {
            return NativeControl.GetPropertyBackgroundColor(ItemToPtr(id));
        }

        Color IPropertyGridHandler.GetPropertyTextColor(IPropertyGridItem id)
        {
            return NativeControl.GetPropertyTextColor(ItemToPtr(id));
        }

        void IPropertyGridHandler.SetPropertyEditorByName(IPropertyGridItem id, string editorName)
        {
            NativeControl.SetPropertyEditorByName(ItemToPtr(id), editorName);
        }

        void IPropertyGridHandler.SetPropertyLabel(IPropertyGridItem id, string newproplabel)
        {
            NativeControl.SetPropertyLabel(ItemToPtr(id), newproplabel);
        }

        void IPropertyGridHandler.SetPropertyName(IPropertyGridItem id, string newName)
        {
            NativeControl.SetPropertyName(ItemToPtr(id), newName);
        }

        void IPropertyGridHandler.SetPropertyHelpString(IPropertyGridItem id, string helpString)
        {
            NativeControl.SetPropertyHelpString(ItemToPtr(id), helpString);
        }

        bool IPropertyGridHandler.SetPropertyMaxLength(IPropertyGridItem id, int maxLen)
        {
            return NativeControl.SetPropertyMaxLength(ItemToPtr(id), maxLen);
        }

        void IPropertyGridHandler.SetPropertyValueAsLong(IPropertyGridItem id, long value)
        {
            NativeControl.SetPropertyValueAsLong(ItemToPtr(id), value);
        }

        void IPropertyGridHandler.SetPropertyValueAsInt(IPropertyGridItem id, int value)
        {
            NativeControl.SetPropertyValueAsInt(ItemToPtr(id), value);
        }

        void IPropertyGridHandler.SetPropertyValueAsDouble(IPropertyGridItem id, double value)
        {
            NativeControl.SetPropertyValueAsDouble(ItemToPtr(id), value);
        }

        void IPropertyGridHandler.SetPropertyValueAsBool(IPropertyGridItem id, bool value)
        {
            NativeControl.SetPropertyValueAsBool(ItemToPtr(id), value);
        }

        void IPropertyGridHandler.SetPropertyValueAsStr(IPropertyGridItem id, string value)
        {
            NativeControl.SetPropertyValueAsStr(ItemToPtr(id), value);
        }

        void IPropertyGridHandler.SetPropertyValueAsVariant(IPropertyGridItem id, IPropertyGridVariant variant)
        {
            NativeControl.SetPropertyValueAsVariant(ItemToPtr(id), VariantToHandle(variant));
        }

        public nint VariantToHandle(IPropertyGridVariant variant)
        {
            return ((PropertyGridVariant)variant).Handle;
        }

        void IPropertyGridHandler.SetPropertyValueAsDateTime(IPropertyGridItem id, DateTime value)
        {
            NativeControl.SetPropertyValueAsDateTime(ItemToPtr(id), value);
        }

        void IPropertyGridHandler.SetValidationFailureBehavior(PropertyGridValidationFailure vfbFlags)
        {
            NativeControl.SetValidationFailureBehavior((int)vfbFlags);
        }

        void IPropertyGridHandler.SortChildren(IPropertyGridItem id, PropertyGridItemValueFlags flags)
        {
            NativeControl.SortChildren(ItemToPtr(id), (int)flags);
        }

        bool IPropertyGridHandler.ChangePropertyValue(IPropertyGridItem id, IPropertyGridVariant variant)
        {
            return NativeControl.ChangePropertyValue(ItemToPtr(id), VariantToHandle(variant));
        }

        void IPropertyGridHandler.SetPropertyClientData(IPropertyGridItem prop, nint clientData)
        {
            NativeControl.SetPropertyClientData(ItemToPtr(prop), clientData);
        }

        void IPropertyGridHandler.SetPropertyAttribute(
            IPropertyGridItem id,
            string attrName,
            IPropertyGridVariant variant,
            PropertyGridItemValueFlags argFlags)
        {
            NativeControl.SetPropertyAttribute(
                ItemToPtr(id),
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
            return NativeControl.EnsureVisible(ItemToPtr(propArg));
        }

        bool IPropertyGridHandler.SelectProperty(IPropertyGridItem propArg, bool focus)
        {
            return NativeControl.SelectProperty(ItemToPtr(propArg), focus);
        }

        bool IPropertyGridHandler.AddToSelection(IPropertyGridItem propArg)
        {
            return NativeControl.AddToSelection(ItemToPtr(propArg));
        }

        bool IPropertyGridHandler.RemoveFromSelection(IPropertyGridItem propArg)
        {
            return NativeControl.RemoveFromSelection(ItemToPtr(propArg));
        }

        void IPropertyGridHandler.SetCurrentCategory(IPropertyGridItem propArg)
        {
            NativeControl.SetCurrentCategory(ItemToPtr(propArg));
        }

        RectI IPropertyGridHandler.GetImageRect(IPropertyGridItem p, int item)
        {
            return NativeControl.GetImageRect(ItemToPtr(p), item);
        }

        SizeI IPropertyGridHandler.GetImageSize(IPropertyGridItem? p, int item)
        {
            return NativeControl.GetImageSize(ItemToPtr(p), item);
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateStringProperty(string label, string name, string value)
        {
            return CreateHandle(NativeControl.CreateStringProperty(label, name, value));
        }

        PropertyGridItemHandle IPropertyGridHandler.CreateFilenameProperty(string label, string name, string value)
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
            NativeControl.Append(ItemToPtr(property));
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
            return NativeControl.GetPropertyName(ItemToPtr(property));
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
            NativeControl.SetPropertyFlag(ItemToPtr(prop), (int)flag, value);
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

            var items = ColorUtils.GetColorInfos();

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

        internal IntPtr GetPropertyValidator(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyValidator(ItemToPtr(prop));
        }

        internal bool CommitChangesFromEditor()
        {
            return NativeControl.CommitChangesFromEditor(0);
        }

        internal IntPtr GetPropertyImage(IPropertyGridItem prop)
        {
            return NativeControl.GetPropertyImage(ItemToPtr(prop));
        }

        internal void SetPropertyEditor(IPropertyGridItem prop, IntPtr editor)
        {
            NativeControl.SetPropertyEditor(ItemToPtr(prop), editor);
        }

        internal IntPtr ItemToPtr(IPropertyGridItem? prop)
        {
            if (prop is null)
                return default;
            var handle = ((WxPropertyGridItemHandle)prop.Handle).Handle;
            return handle;
        }

        internal void DeleteProperty(IPropertyGridItem prop)
        {
            NativeControl.DeleteProperty(ItemToPtr(prop));
        }

        internal void SetupTextCtrlValue(string text)
        {
            NativeControl.SetupTextCtrlValue(text);
        }

        internal void EndAddChildren(IPropertyGridItem prop)
        {
            NativeControl.EndAddChildren(ItemToPtr(prop));
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            NativeControl.ButtonClick = null;
            NativeControl.Selected = null;
            NativeControl.Changed = null;
            NativeControl.Changing -= NativeControl_Changing;
            NativeControl.Highlighted = null;
            NativeControl.RightClick = null;
            NativeControl.DoubleClick = null;
            NativeControl.ItemCollapsed = null;
            NativeControl.ItemExpanded = null;
            NativeControl.LabelEditBegin -= NativeControl_LabelEditBegin;
            NativeControl.LabelEditEnding -= NativeControl_LabelEditEnding;
            NativeControl.ColBeginDrag -= NativeControl_ColBeginDrag;
            NativeControl.ColDragging = null;
            NativeControl.ColEndDrag = null;
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.ButtonClick += NativeControl_ButtonClick;
            NativeControl.Selected += NativeControl_Selected;
            NativeControl.Changed += NativeControl_Changed;
            NativeControl.Changing += NativeControl_Changing;
            NativeControl.Highlighted += NativeControl_Highlighted;
            NativeControl.RightClick += NativeControl_RightClick;
            NativeControl.DoubleClick += NativeControl_DoubleClick;
            NativeControl.ItemCollapsed += NativeControl_ItemCollapsed;
            NativeControl.ItemExpanded += NativeControl_ItemExpanded;
            NativeControl.LabelEditBegin += NativeControl_LabelEditBegin;
            NativeControl.LabelEditEnding += NativeControl_LabelEditEnding;
            NativeControl.ColBeginDrag += NativeControl_ColBeginDrag;
            NativeControl.ColDragging += NativeControl_ColDragging;
            NativeControl.ColEndDrag += NativeControl_ColEndDrag;
        }

        private void NativeControl_ButtonClick()
        {
            Control.RaiseButtonClick(EventArgs.Empty);
        }

        private void NativeControl_ColEndDrag()
        {
            Control.RaiseColEndDrag(EventArgs.Empty);
        }

        private void NativeControl_ColDragging()
        {
            Control.RaiseColDragging(EventArgs.Empty);
        }

        private void NativeControl_ColBeginDrag(object? sender, CancelEventArgs e)
        {
            Control.RaiseColBeginDrag(e);
        }

        private void NativeControl_LabelEditEnding(object? sender, CancelEventArgs e)
        {
            Control.RaiseLabelEditEnding(e);
        }

        private void NativeControl_LabelEditBegin(object? sender, CancelEventArgs e)
        {
            Control.RaiseLabelEditBegin(e);
        }

        private void NativeControl_ItemExpanded()
        {
            Control.RaiseItemExpanded(EventArgs.Empty);
        }

        private void NativeControl_ItemCollapsed()
        {
            Control.RaiseItemCollapsed(EventArgs.Empty);
        }

        private void NativeControl_DoubleClick()
        {
            Control.RaisePropertyDoubleClick(EventArgs.Empty);
        }

        private void NativeControl_RightClick()
        {
            Control.RaisePropertyRightClick(EventArgs.Empty);
        }

        private void NativeControl_Highlighted()
        {
            Control.RaisePropertyHighlighted(EventArgs.Empty);
        }

        private void NativeControl_Changing(object? sender, CancelEventArgs e)
        {
            Control.RaisePropertyChanging(e);
        }

        private void NativeControl_Changed()
        {
            Control.RaisePropertyChanged(EventArgs.Empty);
        }

        private IPropertyGridItem? PtrToItem(IntPtr ptr)
        {
            return Control.HandleToItem(new WxPropertyGridItemHandle(ptr));
        }

        private void NativeControl_Selected()
        {
            Control.RaisePropertySelected(EventArgs.Empty);
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