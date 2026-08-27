using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Alternet.UI
{
    /// <summary>
    /// Defines properties and methods for working with property grid.
    /// </summary>
    public interface IPropertyGrid
    {
        event EventHandler<ThrowExceptionEventArgs>? ProcessException;

        event EventHandler? PropertySelected;

        event EventHandler? PropertyChanged;

        event EventHandler? ButtonClick;

        event EventHandler<CancelEventArgs>? PropertyChanging;

        event EventHandler? PropertyHighlighted;

        event EventHandler? PropertyRightClick;

        event EventHandler? PropertyDoubleClick;

        event EventHandler? ItemCollapsed;

        event EventHandler? ItemExpanded;

        event EventHandler<CancelEventArgs>? LabelEditBegin;

        event EventHandler<CancelEventArgs>? LabelEditEnding;

        event EventHandler<CancelEventArgs>? ColBeginDrag;

        event EventHandler? ColDragging;

        event EventHandler? ColEndDrag;

        bool HasItems { get; }

        object? FirstItemInstance { get; }

        ICollection<IPropertyGridItem> Items { get; }

        ICollection<string> IgnorePropNames { get; }

        bool BoolAsCheckBox { get; set; }

        bool ColorHasAlpha { get; set; }

        object? EventPropValue { get; }

        IPropertyGridVariant EventPropValueAsVariant { get; }

        PropertyGridApplyFlags ApplyFlags { get; set; }

        IPropertyGridItem? EventProperty { get; }

        string EventPropName { get; }

        PropertyGridCreateStyle CreateStyle { get; set; }

        PropertyGridCreateStyleEx CreateStyleEx { get; set; }

        bool HasBorder { get; set; }

        IPropertyGridItem CreateFilenameItem(
            string label,
            string? name = null,
            string? value = null,
            IPropertyGridNewItemParams? prm = null);

        IPropertyGridItem CreateDirItem(
            string label,
            string? name = null,
            string? value = null,
            IPropertyGridNewItemParams? prm = null);

        IPropertyGridItem CreateImageFilenameItem(
            string label,
            string? name = null,
            string? value = null,
            IPropertyGridNewItemParams? prm = null);

        IPropertyGridItem CreateSystemColorItem(
            string label,
            string? name,
            Color value,
            IPropertyGridNewItemParams? prm = null);

        IPropertyGridItem CreateStringItem(
            string label,
            string? name = null,
            string? value = null,
            IPropertyGridNewItemParams? prm = null);

        string GetPropNameAsLabel();

        IPropertyGridItem CreateBoolItem(
            string label,
            string? name = null,
            bool value = false,
            IPropertyGridNewItemParams? prm = null);

        IPropertyGridItem CreateLongItem(
            string label,
            string? name = null,
            long value = 0,
            IPropertyGridNewItemParams? prm = null);

        IPropertyGridItem CreateColorItem(
            string label,
            string? name,
            Color value,
            IPropertyGridNewItemParams? prm = null);

        IPropertyGridItem CreateULongItem(
            string label,
            string? name = null,
            ulong value = 0,
            IPropertyGridNewItemParams? prm = null);

        IPropertyGridItem CreateLongStringItem(
            string label,
            string? name = null,
            string? value = null,
            IPropertyGridNewItemParams? prm = null);

        IPropertyGridItem CreateDateItem(
            string label,
            string? name = null,
            DateTime? value = null,
            IPropertyGridNewItemParams? prm = null);

        IPropertyGridItem CreatePropertyAsStruct(
            string label,
            string? name,
            object instance,
            PropertyInfo propInfo);

        IPropertyGridItem CreatePropertyAsFont(
            string label,
            string? name,
            object instance,
            PropertyInfo propInfo);

        IPropertyGridItem CreatePropertyAsBrush(
            string label,
            string? name,
            object instance,
            PropertyInfo propInfo);

        IPropertyGridItem CreatePropertyAsPen(
            string label,
            string? name,
            object instance,
            PropertyInfo propInfo);

        void SetPropertyMinMax(IPropertyGridItem prop, TypeCode code);

        void SetPropertyMinMax(IPropertyGridItem prop, object? min, object? max = null);

        IPropertyGridItem? CreateProperty(
            string? label,
            string? name,
            object instance,
            string nameInInstance);

        IPropertyGridItem? CreateProperty(object instance, string nameInInstance);

        IPropertyGridItem? CreateProperty(object instance, PropertyInfo p);

        IPropertyGridItem? CreateProperty(
            string? label,
            string? propName,
            object instance,
            PropertyInfo p);

        IPropertyGridItem CreatePropertyAsBool(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo);

        IPropertyGridItem CreatePropertyAsString(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo,
                    TypeConverter? typeConverter = null);

        IEnumerable<IPropertyGridItem> CreateProps(object instance, bool sort = false);

        void AddProps(
            object instance,
            IPropertyGridItem? parent = null,
            bool sort = false);

        void SetProps(object? instance, bool sort = false);

        IPropertyGridItem CreatePropertyAsColor(
            string? label,
            string? name,
            object instance,
            PropertyInfo propInfo);

        IPropertyGridItem CreatePropertyAsEnum(
            string? label,
            string? name,
            object instance,
            PropertyInfo propInfo);

        IPropertyGridItem CreateChoicesItem(
            string label,
            string? name,
            IPropertyGridChoices choices,
            object? value = null,
            IPropertyGridNewItemParams? prm = null);

        IPropertyGridItem CreateEditEnumItem(
            string label,
            string? name,
            IPropertyGridChoices choices,
            string? value = null,
            IPropertyGridNewItemParams? prm = null);

        IPropertyGridItem CreateFlagsItem(
            string label,
            string? name,
            IPropertyGridChoices choices,
            object? value = null,
            IPropertyGridNewItemParams? prm = null);

        void Clear();

        void Add(IPropertyGridItem prop, IPropertyGridItem? parent = null);

        IPropertyGridItem CreatePropCategory(
            string label,
            string? name = null,
            IPropertyGridNewItemParams? prm = null);

        void SetPropertyReadOnly(
            IPropertyGridItem prop,
            bool isSet,
            bool recurse = true);

        void SetPropertyValueUnspecified(IPropertyGridItem prop);

        void AppendIn(IPropertyGridItem prop, IPropertyGridItem newproperty);

        bool Collapse(IPropertyGridItem prop);

        void RemoveProperty(IPropertyGridItem prop);

        bool DisableProperty(IPropertyGridItem prop);

        bool EnableProperty(IPropertyGridItem prop, bool enable = true);

        bool Expand(IPropertyGridItem prop);

        IntPtr GetPropertyClientData(IPropertyGridItem prop);

        string GetPropertyHelpString(IPropertyGridItem prop);

        string GetPropertyLabel(IPropertyGridItem prop);

        string GetPropertyValueAsString(IPropertyGridItem prop);

        long GetPropertyValueAsLong(IPropertyGridItem prop);

        ulong GetPropertyValueAsULong(IPropertyGridItem prop);

        int GetPropertyValueAsInt(IPropertyGridItem prop);

        IPropertyGridVariant GetPropertyValueAsVariant(IPropertyGridItem prop);

        bool GetPropertyValueAsBool(IPropertyGridItem prop);

        double GetPropertyValueAsDouble(IPropertyGridItem prop);

        DateTime GetPropertyValueAsDateTime(IPropertyGridItem prop);

        bool HideProperty(IPropertyGridItem prop, bool hide, bool recurse = true);

        void Insert(IPropertyGridItem priorThis, IPropertyGridItem newproperty);

        void InsertAt(IPropertyGridItem parent, int index, IPropertyGridItem newproperty);

        bool IsPropertyCategory(IPropertyGridItem prop);

        bool IsPropertyEnabled(IPropertyGridItem prop);

        bool IsPropertyExpanded(IPropertyGridItem prop);

        bool IsPropertyModified(IPropertyGridItem prop);

        bool IsPropertySelected(IPropertyGridItem prop);

        bool IsPropertyShown(IPropertyGridItem prop);

        bool IsPropertyValueUnspecified(IPropertyGridItem prop);

        void LimitPropertyEditing(IPropertyGridItem prop, bool limit = true);

        void ReplaceProperty(IPropertyGridItem prop, IPropertyGridItem newProp);

        void SetPropertyBackgroundColor(
            IPropertyGridItem prop,
            Color color,
            bool recurse = true);

        void SetPropertyColorsToDefault(IPropertyGridItem prop, bool recurse = true);

        void SetPropertyTextColor(
            IPropertyGridItem prop,
            Color color,
            bool recurse = true);

        bool RestoreEditableState(
            string src,
            PropertyGridEditableState restoreStates = PropertyGridEditableState.AllStates);

        void RefreshProperty(IPropertyGridItem p);

        string SaveEditableState(
            PropertyGridEditableState includedStates =
                PropertyGridEditableState.AllStates);

        bool SetColumnProportion(int column, int proportion);

        int GetColumnProportion(int column);

        Color GetPropertyBackgroundColor(IPropertyGridItem prop);

        Color GetPropertyTextColor(IPropertyGridItem prop);

        void SetPropertyClientData(IPropertyGridItem prop, IntPtr clientData);

        void SetPropertyLabel(IPropertyGridItem prop, string newproplabel);

        void SetPropertyHelpString(IPropertyGridItem prop, string helpString);

        bool SetPropertyMaxLength(IPropertyGridItem prop, int maxLen);

        void SetPropertyValueAsLong(IPropertyGridItem prop, long value);

        void SetPropertyValueAsInt(IPropertyGridItem prop, int value);

        void SetPropertyValueAsDouble(IPropertyGridItem prop, double value);

        void SetPropertyValueAsBool(IPropertyGridItem prop, bool value);

        void SetPropertyValueAsStr(IPropertyGridItem prop, string value);

        void SetPropertyValueAsDateTime(IPropertyGridItem prop, DateTime value);

        void SetValidationFailureBehavior(PropertyGridValidationFailure vfbFlags);

        void SortChildren(IPropertyGridItem prop, bool recurse = false);

        void SetPropertyEditorByName(IPropertyGridItem prop, string editorName);

        void ApplyKnownColors(PropertyGridKnownColors colors);

        void BackgroundToLineColor();

        void ApplyColors(IPropertyGridColors? colors = null);

        void AddActionTrigger(
            PropertyGridKeyboardAction action,
            Key keycode,
            ModifierKeys modifiers = 0);

        bool RemoveFromSelection(IPropertyGridItem prop);

        void SetCurrentCategory(IPropertyGridItem prop);

        RectI GetImageRect(IPropertyGridItem prop, int item);

        SizeI GetImageSize(IPropertyGridItem? prop, int item);

        IEnumerable<IPropertyGridItem> GetItemsFiltered(
            object? instance = null,
            PropertyInfo? propInfo = null);

        bool ReloadPropertyValues(object? instance = null, PropertyInfo? propInfo = null);

        void ReloadPropertyValue(IPropertyGridItem item);

        void ClearActionTriggers(PropertyGridKeyboardAction action);

        void DedicateKey(Key keycode);

        void CenterSplitter(bool enableAutoResizing = false);

        void EditorsValueWasModified();

        void EditorsValueWasNotModified();

        bool EnableCategories(bool enable);

        SizeD FitColumns();

        int GetColumnCount();

        int GetFontHeight();

        int GetMarginWidth();

        int GetRowHeight();

        int GetSplitterPosition(int splitterIndex = 0);

        int GetVerticalSpacing();

        bool IsEditorFocused();

        bool IsEditorsValueModified();

        bool IsAnyModified();

        void ResetColors();

        void ResetColumnSizes(bool enableAutoResizing = false);

        void MakeColumnEditable(int column, bool editable = true);

        void BeginLabelEdit(int column = 0);

        void EndLabelEdit(bool commit = true);

        void SetColumnCount(int colCount);

        void SetSplitterPosition(int newXPos, int col = 0);

        string GetUnspecifiedValueText(PropertyGridValueFormatFlags flags = 0);

        void SetVirtualWidth(int width);

        void SetSplitterLeft(bool privateChildrenToo = false);

        void SetVerticalSpacing(int? vspacing = null);

        bool HasVirtualWidth();

        int GetCommonValueCount();

        string GetCommonValueLabel(int i);

        int GetUnspecifiedCommonValue();

        void SetUnspecifiedCommonValue(int index);

        void RefreshEditor();

        bool WasValueChangedInEvent();

        int GetSpacingY();

        bool UnfocusEditor();

        IPropertyGridItem? GetLastItem(PropertyGridIteratorFlags flags);

        IPropertyGridItem? GetRoot();

        IPropertyGridItem? GetSelectedProperty();

        bool ChangePropertyValue(IPropertyGridItem prop, object value);

        bool ChangePropertyValueAsVariant(
                    IPropertyGridItem prop,
                    IPropertyGridVariant value);

        void SetPropertyValueAsVariant(
                    IPropertyGridItem prop,
                    IPropertyGridVariant value);

        void SetPropertyAttribute(
                    IPropertyGridItem prop,
                    string attrName,
                    object? value = null,
                    PropertyGridItemValueFlags argFlags = 0);

        void SetPropertyAttributeAsVariant(
            IPropertyGridItem prop,
            string attrName,
            IPropertyGridVariant value,
            PropertyGridItemValueFlags argFlags = 0);

        void SetPropertyKnownAttribute(
            IPropertyGridItem prop,
            PropertyGridItemAttrId attrName,
            object? value,
            PropertyGridItemValueFlags argFlags = 0);

        void SetPropertyAttributeAll(string attrName, object value);

        void SetPropertyAttributeAll(string attrName, IPropertyGridVariant value);

        bool EnsureVisible(IPropertyGridItem prop);

        bool SelectProperty(IPropertyGridItem prop, bool focus = false);

        bool AddToSelection(IPropertyGridItem prop);
    }
}
