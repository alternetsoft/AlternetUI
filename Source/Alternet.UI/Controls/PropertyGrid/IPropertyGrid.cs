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
    /// <inheritdoc cref="PropertyGrid"/>
    public interface IPropertyGrid
    {
        /// <inheritdoc cref="PropertyGrid.ProcessException"/>
        event EventHandler<PropertyGridExceptionEventArgs>? ProcessException;

        /// <inheritdoc cref="PropertyGrid.PropertySelected"/>
        event EventHandler? PropertySelected;

        /// <inheritdoc cref="PropertyGrid.PropertyChanged"/>
        event EventHandler? PropertyChanged;

        /// <inheritdoc cref="PropertyGrid.ButtonClick"/>
        event EventHandler? ButtonClick;

        /// <inheritdoc cref="PropertyGrid.PropertyChanging"/>
        event EventHandler<CancelEventArgs>? PropertyChanging;

        /// <inheritdoc cref="PropertyGrid.PropertyHighlighted"/>
        event EventHandler? PropertyHighlighted;

        /// <inheritdoc cref="PropertyGrid.PropertyRightClick"/>
        event EventHandler? PropertyRightClick;

        /// <inheritdoc cref="PropertyGrid.PropertyDoubleClick"/>
        event EventHandler? PropertyDoubleClick;

        /// <inheritdoc cref="PropertyGrid.ItemCollapsed"/>
        event EventHandler? ItemCollapsed;

        /// <inheritdoc cref="PropertyGrid.ItemExpanded"/>
        event EventHandler? ItemExpanded;

        /// <inheritdoc cref="PropertyGrid.LabelEditBegin"/>
        event EventHandler<CancelEventArgs>? LabelEditBegin;

        /// <inheritdoc cref="PropertyGrid.LabelEditEnding"/>
        event EventHandler<CancelEventArgs>? LabelEditEnding;

        /// <inheritdoc cref="PropertyGrid.ColBeginDrag"/>
        event EventHandler<CancelEventArgs>? ColBeginDrag;

        /// <inheritdoc cref="PropertyGrid.ColDragging"/>
        event EventHandler? ColDragging;

        /// <inheritdoc cref="PropertyGrid.ColEndDrag"/>
        event EventHandler? ColEndDrag;

        /// <inheritdoc cref="PropertyGrid.Factory"/>
        IPropertyGridFactory Factory { get; }

        /// <inheritdoc cref="PropertyGrid.Items"/>
        IEnumerable<IPropertyGridItem> Items { get; }

        /// <inheritdoc cref="PropertyGrid.IgnorePropNames"/>
        ICollection<string> IgnorePropNames { get; }

        /// <inheritdoc cref="PropertyGrid.BoolAsCheckBox"/>
        bool BoolAsCheckBox { get; set; }

        /// <inheritdoc cref="PropertyGrid.ColorHasAlpha"/>
        bool ColorHasAlpha { get; set; }

        /// <inheritdoc cref="PropertyGrid.EventPropValue"/>
        object? EventPropValue { get; }

        /// <inheritdoc cref="PropertyGrid.EventPropValueAsVariant"/>
        IPropertyGridVariant EventPropValueAsVariant { get; }

        /// <inheritdoc cref="PropertyGrid.ApplyFlags"/>
        PropertyGridApplyFlags ApplyFlags { get; set; }

        /// <inheritdoc cref="PropertyGrid.EventValidationFailureBehavior"/>
        PropertyGridValidationFailure EventValidationFailureBehavior { get; set; }

        /// <inheritdoc cref="PropertyGrid.EventColumn"/>
        int EventColumn { get; }

        /// <inheritdoc cref="PropertyGrid.EventProperty"/>
        IPropertyGridItem? EventProperty { get; }

        /// <inheritdoc cref="PropertyGrid.EventPropName"/>
        string EventPropName { get; }

        /// <inheritdoc cref="PropertyGrid.EventValidationFailureMessage"/>
        string EventValidationFailureMessage { get; set; }

        /// <inheritdoc cref="PropertyGrid.CreateStyle"/>
        PropertyGridCreateStyle CreateStyle { get; set; }

        /// <inheritdoc cref="PropertyGrid.CreateStyleEx"/>
        PropertyGridCreateStyleEx CreateStyleEx { get; set; }

        /// <inheritdoc cref="PropertyGrid.HasBorder"/>
        bool HasBorder { get; set; }

        /// <inheritdoc cref="PropertyGrid.CreateFilenameProperty"/>
        IPropertyGridItem CreateFilenameProperty(
            string label,
            string? name = null,
            string? value = null,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.CreateDirProperty"/>
        IPropertyGridItem CreateDirProperty(
            string label,
            string? name = null,
            string? value = null,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.CreateImageFilenameProperty"/>
        IPropertyGridItem CreateImageFilenameProperty(
            string label,
            string? name = null,
            string? value = null,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.CreateSystemColorProperty"/>
        IPropertyGridItem CreateSystemColorProperty(
            string label,
            string? name,
            Color value,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.CreateStringProperty"/>
        IPropertyGridItem CreateStringProperty(
            string label,
            string? name = null,
            string? value = null,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.CreateCharProperty"/>
        IPropertyGridItem CreateCharProperty(
            string label,
            string? name = null,
            char? value = null,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.CreateBoolProperty"/>
        IPropertyGridItem CreateBoolProperty(
            string label,
            string? name = null,
            bool value = false,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.CreateLongProperty"/>
        IPropertyGridItem CreateLongProperty(
            string label,
            string? name = null,
            long value = 0,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.CreateDoubleProperty"/>
        IPropertyGridItem CreateDoubleProperty(
            string label,
            string? name = null,
            double value = default,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.CreateFloatProperty"/>
        IPropertyGridItem CreateFloatProperty(
            string label,
            string? name = null,
            double value = default,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.CreateColorProperty"/>
        IPropertyGridItem CreateColorProperty(
            string label,
            string? name,
            Color value,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.CreateULongProperty"/>
        IPropertyGridItem CreateULongProperty(
            string label,
            string? name = null,
            ulong value = 0,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.CreateLongStringProperty"/>
        IPropertyGridItem CreateLongStringProperty(
            string label,
            string? name = null,
            string? value = null,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.CreateDateProperty"/>
        IPropertyGridItem CreateDateProperty(
            string label,
            string? name = null,
            DateTime? value = null,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.CreatePropertyAsStruct"/>
        IPropertyGridItem CreatePropertyAsStruct(
            string label,
            string? name,
            object instance,
            PropertyInfo propInfo);

        /// <inheritdoc cref="PropertyGrid.CreatePropertyAsFont"/>
        IPropertyGridItem CreatePropertyAsFont(
            string label,
            string? name,
            object instance,
            PropertyInfo propInfo);

        /// <inheritdoc cref="PropertyGrid.CreatePropertyAsBrush"/>
        IPropertyGridItem CreatePropertyAsBrush(
            string label,
            string? name,
            object instance,
            PropertyInfo propInfo);

        /// <inheritdoc cref="PropertyGrid.CreatePropertyAsPen"/>
        IPropertyGridItem CreatePropertyAsPen(
            string label,
            string? name,
            object instance,
            PropertyInfo propInfo);

        /// <inheritdoc cref="PropertyGrid.CreateDecimalProperty"/>
        IPropertyGridItem CreateDecimalProperty(
            string label,
            string? name = null,
            decimal value = default,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.CreateSByteProperty"/>
        IPropertyGridItem CreateSByteProperty(
            string label,
            string? name = null,
            sbyte value = 0,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.SetPropertyMinMax(IPropertyGridItem, TypeCode)"/>
        void SetPropertyMinMax(IPropertyGridItem prop, TypeCode code);

        /// <inheritdoc cref="PropertyGrid.SetPropertyMinMax(IPropertyGridItem, object?, object?)"/>
        void SetPropertyMinMax(IPropertyGridItem prop, object? min, object? max = null);

        /// <inheritdoc cref="PropertyGrid.CreateInt16Property"/>
        IPropertyGridItem CreateInt16Property(
            string label,
            string? name = null,
            short value = 0,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.CreateIntProperty"/>
        IPropertyGridItem CreateIntProperty(
            string label,
            string? name = null,
            int value = 0,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.CreateByteProperty"/>
        IPropertyGridItem CreateByteProperty(
            string label,
            string? name = null,
            byte value = 0,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.CreateUIntProperty"/>
        IPropertyGridItem CreateUIntProperty(
            string label,
            string? name = null,
            uint value = 0,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.CreateUInt16Property"/>
        IPropertyGridItem CreateUInt16Property(
            string label,
            string? name = null,
            ushort value = 0,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.CreateProperty(string?, string?, object, string)"/>
        IPropertyGridItem? CreateProperty(
            string? label,
            string? name,
            object instance,
            string nameInInstance);

        /// <inheritdoc cref="PropertyGrid.CreateProperty(object, string)"/>
        IPropertyGridItem? CreateProperty(object instance, string nameInInstance);

        /// <inheritdoc cref="PropertyGrid.CreateProperty(object, PropertyInfo)"/>
        IPropertyGridItem? CreateProperty(object instance, PropertyInfo p);

        /// <inheritdoc cref="PropertyGrid.CreateProperty(string?, string?, object, PropertyInfo)"/>
        IPropertyGridItem? CreateProperty(
            string? label,
            string? propName,
            object instance,
            PropertyInfo p);

        /// <inheritdoc cref="PropertyGrid.CreatePropertyAsBool"/>
        IPropertyGridItem CreatePropertyAsBool(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo);

        /// <inheritdoc cref="PropertyGrid.CreatePropertyAsSByte"/>
        IPropertyGridItem CreatePropertyAsSByte(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo);

        /// <inheritdoc cref="PropertyGrid.CreatePropertyAsInt16"/>
        IPropertyGridItem CreatePropertyAsInt16(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo);

        /// <inheritdoc cref="PropertyGrid.CreatePropertyAsInt"/>
        IPropertyGridItem CreatePropertyAsInt(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo);

        /// <inheritdoc cref="PropertyGrid.CreatePropertyAsLong"/>
        IPropertyGridItem CreatePropertyAsLong(
                    string label,
                    string name,
                    object instance,
                    PropertyInfo propInfo);

        /// <inheritdoc cref="PropertyGrid.CreatePropertyAsByte"/>
        IPropertyGridItem CreatePropertyAsByte(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo);

        /// <inheritdoc cref="PropertyGrid.CreatePropertyAsUInt"/>
        IPropertyGridItem CreatePropertyAsUInt(
            string label,
            string? name,
            object instance,
            PropertyInfo propInfo);

        /// <inheritdoc cref="PropertyGrid.CreatePropertyAsUInt16"/>
        IPropertyGridItem CreatePropertyAsUInt16(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo);

        /// <inheritdoc cref="PropertyGrid.CreatePropertyAsULong"/>
        IPropertyGridItem CreatePropertyAsULong(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo);

        /// <inheritdoc cref="PropertyGrid.CreatePropertyAsFloat"/>
        IPropertyGridItem CreatePropertyAsFloat(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo);

        /// <inheritdoc cref="PropertyGrid.CreatePropertyAsDouble"/>
        IPropertyGridItem CreatePropertyAsDouble(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo);

        /// <inheritdoc cref="PropertyGrid.CreatePropertyAsDecimal"/>
        IPropertyGridItem CreatePropertyAsDecimal(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo);

        /// <inheritdoc cref="PropertyGrid.CreatePropertyAsDate"/>
        IPropertyGridItem CreatePropertyAsDate(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo);

        /// <inheritdoc cref="PropertyGrid.CreatePropertyAsChar"/>
        IPropertyGridItem CreatePropertyAsChar(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo);

        /// <inheritdoc cref="PropertyGrid.CreatePropertyAsString"/>
        IPropertyGridItem CreatePropertyAsString(
                    string label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo);

        /// <inheritdoc cref="PropertyGrid.CreateProps"/>
        IEnumerable<IPropertyGridItem> CreateProps(object instance, bool sort = false);

        /// <inheritdoc cref="PropertyGrid.AddProps"/>
        void AddProps(
            object instance,
            IPropertyGridItem? parent = null,
            bool sort = false);

        /// <inheritdoc cref="PropertyGrid.SetProps"/>
        void SetProps(object? instance, bool sort = false);

        /// <inheritdoc cref="PropertyGrid.CreatePropertyAsColor"/>
        IPropertyGridItem CreatePropertyAsColor(
            string? label,
            string? name,
            object instance,
            PropertyInfo propInfo);

        /// <inheritdoc cref="PropertyGrid.CreatePropertyAsEnum"/>
        IPropertyGridItem CreatePropertyAsEnum(
            string? label,
            string? name,
            object instance,
            PropertyInfo propInfo);

        /// <inheritdoc cref="PropertyGrid.CreateChoicesProperty"/>
        IPropertyGridItem CreateChoicesProperty(
            string label,
            string? name,
            IPropertyGridChoices choices,
            object? value = null,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.CreateEditEnumProperty"/>
        IPropertyGridItem CreateEditEnumProperty(
            string label,
            string? name,
            IPropertyGridChoices choices,
            string? value = null,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.CreateFlagsProperty"/>
        IPropertyGridItem CreateFlagsProperty(
            string label,
            string? name,
            IPropertyGridChoices choices,
            object? value = null,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.Clear"/>
        void Clear();

        /// <inheritdoc cref="PropertyGrid.AddRange"/>
        void AddRange(
            IEnumerable<IPropertyGridItem> props,
            IPropertyGridItem? parent = null);

        /// <inheritdoc cref="PropertyGrid.Add"/>
        void Add(IPropertyGridItem prop, IPropertyGridItem? parent = null);

        /// <inheritdoc cref="PropertyGrid.CreatePropCategory"/>
        IPropertyGridItem CreatePropCategory(
            string label,
            string? name = null,
            IPropertyGridNewItemParams? prm = null);

        /// <inheritdoc cref="PropertyGrid.GetPropertyName"/>
        string GetPropertyName(IPropertyGridItem property);

        /// <inheritdoc cref="PropertyGrid.Sort"/>
        void Sort(bool topLevelOnly = false);

        /// <inheritdoc cref="PropertyGrid.SetPropertyReadOnly"/>
        void SetPropertyReadOnly(
            IPropertyGridItem prop,
            bool isSet,
            bool recurse = true);

        /// <inheritdoc cref="PropertyGrid.SetPropertyValueUnspecified"/>
        void SetPropertyValueUnspecified(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.AppendIn"/>
        void AppendIn(IPropertyGridItem prop, IPropertyGridItem newproperty);

        /// <inheritdoc cref="PropertyGrid.Collapse"/>
        bool Collapse(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.RemoveProperty"/>
        void RemoveProperty(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.DisableProperty"/>
        bool DisableProperty(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.EnableProperty"/>
        bool EnableProperty(IPropertyGridItem prop, bool enable = true);

        /// <inheritdoc cref="PropertyGrid.Expand"/>
        bool Expand(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.GetPropertyClientData"/>
        IntPtr GetPropertyClientData(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.GetPropertyHelpString"/>
        string GetPropertyHelpString(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.GetPropertyLabel"/>
        string GetPropertyLabel(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.GetPropertyValueAsString"/>
        string GetPropertyValueAsString(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.GetPropertyValueAsLong"/>
        long GetPropertyValueAsLong(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.GetPropertyValueAsULong"/>
        ulong GetPropertyValueAsULong(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.GetPropertyValueAsInt"/>
        int GetPropertyValueAsInt(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.GetPropertyValueAsVariant"/>
        IPropertyGridVariant GetPropertyValueAsVariant(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.GetPropertyValueAsBool"/>
        bool GetPropertyValueAsBool(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.GetPropertyValueAsDouble"/>
        double GetPropertyValueAsDouble(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.GetPropertyValueAsDateTime"/>
        DateTime GetPropertyValueAsDateTime(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.HideProperty"/>
        bool HideProperty(IPropertyGridItem prop, bool hide, bool recurse = true);

        /// <inheritdoc cref="PropertyGrid.Insert"/>
        void Insert(IPropertyGridItem priorThis, IPropertyGridItem newproperty);

        /// <inheritdoc cref="PropertyGrid.InsertAt"/>
        void InsertAt(IPropertyGridItem parent, int index, IPropertyGridItem newproperty);

        /// <inheritdoc cref="PropertyGrid.IsPropertyCategory"/>
        bool IsPropertyCategory(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.IsPropertyEnabled"/>
        bool IsPropertyEnabled(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.IsPropertyExpanded"/>
        bool IsPropertyExpanded(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.IsPropertyModified"/>
        bool IsPropertyModified(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.IsPropertySelected"/>
        bool IsPropertySelected(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.IsPropertyShown"/>
        bool IsPropertyShown(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.IsPropertyValueUnspecified"/>
        bool IsPropertyValueUnspecified(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.LimitPropertyEditing"/>
        void LimitPropertyEditing(IPropertyGridItem prop, bool limit = true);

        /// <inheritdoc cref="PropertyGrid.ReplaceProperty"/>
        void ReplaceProperty(IPropertyGridItem prop, IPropertyGridItem newProp);

        /// <inheritdoc cref="PropertyGrid.SetPropertyBackgroundColor"/>
        void SetPropertyBackgroundColor(
            IPropertyGridItem prop,
            Color color,
            bool recurse = true);

        /// <inheritdoc cref="PropertyGrid.SetPropertyColorsToDefault"/>
        void SetPropertyColorsToDefault(IPropertyGridItem prop, bool recurse = true);

        /// <inheritdoc cref="PropertyGrid.SetPropertyTextColor"/>
        void SetPropertyTextColor(
            IPropertyGridItem prop,
            Color color,
            bool recurse = true);

        /// <inheritdoc cref="PropertyGrid.RestoreEditableState"/>
        bool RestoreEditableState(
            string src,
            PropertyGridEditableState restoreStates = PropertyGridEditableState.AllStates);

        /// <inheritdoc cref="PropertyGrid.RefreshProperty"/>
        void RefreshProperty(IPropertyGridItem p);

        /// <inheritdoc cref="PropertyGrid.SaveEditableState"/>
        string SaveEditableState(
            PropertyGridEditableState includedStates =
                PropertyGridEditableState.AllStates);

        /// <inheritdoc cref="PropertyGrid.SetColumnProportion"/>
        bool SetColumnProportion(uint column, int proportion);

        /// <inheritdoc cref="PropertyGrid.GetColumnProportion"/>
        int GetColumnProportion(uint column);

        /// <inheritdoc cref="PropertyGrid.GetPropertyBackgroundColor"/>
        Color GetPropertyBackgroundColor(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.GetPropertyTextColor"/>
        Color GetPropertyTextColor(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.SetPropertyClientData"/>
        void SetPropertyClientData(IPropertyGridItem prop, IntPtr clientData);

        /// <inheritdoc cref="PropertyGrid.SetPropertyLabel"/>
        void SetPropertyLabel(IPropertyGridItem prop, string newproplabel);

        /// <inheritdoc cref="PropertyGrid.SetPropertyHelpString"/>
        void SetPropertyHelpString(IPropertyGridItem prop, string helpString);

        /// <inheritdoc cref="PropertyGrid.SetPropertyMaxLength"/>
        bool SetPropertyMaxLength(IPropertyGridItem prop, int maxLen);

        /// <inheritdoc cref="PropertyGrid.SetPropertyValueAsLong"/>
        void SetPropertyValueAsLong(IPropertyGridItem prop, long value);

        /// <inheritdoc cref="PropertyGrid.SetPropertyValueAsInt"/>
        void SetPropertyValueAsInt(IPropertyGridItem prop, int value);

        /// <inheritdoc cref="PropertyGrid.SetPropertyValueAsDouble"/>
        void SetPropertyValueAsDouble(IPropertyGridItem prop, double value);

        /// <inheritdoc cref="PropertyGrid.SetPropertyValueAsBool"/>
        void SetPropertyValueAsBool(IPropertyGridItem prop, bool value);

        /// <inheritdoc cref="PropertyGrid.SetPropertyValueAsStr"/>
        void SetPropertyValueAsStr(IPropertyGridItem prop, string value);

        /// <inheritdoc cref="PropertyGrid.SetPropertyValueAsDateTime"/>
        void SetPropertyValueAsDateTime(IPropertyGridItem prop, DateTime value);

        /// <inheritdoc cref="PropertyGrid.SetValidationFailureBehavior"/>
        void SetValidationFailureBehavior(PropertyGridValidationFailure vfbFlags);

        /// <inheritdoc cref="PropertyGrid.SortChildren"/>
        void SortChildren(IPropertyGridItem prop, bool recurse = false);

        /// <inheritdoc cref="PropertyGrid.SetPropertyEditorByName"/>
        void SetPropertyEditorByName(IPropertyGridItem prop, string editorName);

        /// <inheritdoc cref="PropertyGrid.ApplyKnownColors"/>
        void ApplyKnownColors(PropertyGridKnownColors colors);

        /// <inheritdoc cref="PropertyGrid.BackgroundToLineColor"/>
        void BackgroundToLineColor();

        /// <inheritdoc cref="PropertyGrid.ApplyColors"/>
        void ApplyColors(IPropertyGridColors? colors = null);

        /// <inheritdoc cref="PropertyGrid.AddActionTrigger"/>
        void AddActionTrigger(
            PropertyGridKeyboardAction action,
            Key keycode,
            ModifierKeys modifiers = 0);

        /// <inheritdoc cref="PropertyGrid.RemoveFromSelection"/>
        bool RemoveFromSelection(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.SetCurrentCategory"/>
        void SetCurrentCategory(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.GetImageRect"/>
        Int32Rect GetImageRect(IPropertyGridItem prop, int item);

        /// <inheritdoc cref="PropertyGrid.SetPropertyValidator"/>
        void SetPropertyValidator(IPropertyGridItem prop, IValueValidator validator);

        /// <inheritdoc cref="PropertyGrid.GetImageSize"/>
        Int32Size GetImageSize(IPropertyGridItem? prop, int item);

        /// <inheritdoc cref="PropertyGrid.GetItemsFiltered"/>
        IEnumerable<IPropertyGridItem> GetItemsFiltered(
            object? instance = null,
            PropertyInfo? propInfo = null);

        /// <inheritdoc cref="PropertyGrid.ReloadPropertyValues"/>
        void ReloadPropertyValues(object? instance = null, PropertyInfo? propInfo = null);

        /// <inheritdoc cref="PropertyGrid.ReloadPropertyValue"/>
        void ReloadPropertyValue(IPropertyGridItem item);

        /// <inheritdoc cref="PropertyGrid.ClearActionTriggers"/>
        void ClearActionTriggers(PropertyGridKeyboardAction action);

        /// <inheritdoc cref="PropertyGrid.DedicateKey"/>
        void DedicateKey(Key keycode);

        /// <inheritdoc cref="PropertyGrid.CenterSplitter"/>
        void CenterSplitter(bool enableAutoResizing = false);

        /// <inheritdoc cref="PropertyGrid.EditorsValueWasModified"/>
        void EditorsValueWasModified();

        /// <inheritdoc cref="PropertyGrid.EditorsValueWasNotModified"/>
        void EditorsValueWasNotModified();

        /// <inheritdoc cref="PropertyGrid.EnableCategories"/>
        bool EnableCategories(bool enable);

        /// <inheritdoc cref="PropertyGrid.FitColumns"/>
        Size FitColumns();

        /// <inheritdoc cref="PropertyGrid.GetColumnCount"/>
        uint GetColumnCount();

        /// <inheritdoc cref="PropertyGrid.GetFontHeight"/>
        int GetFontHeight();

        /// <inheritdoc cref="PropertyGrid.GetMarginWidth"/>
        int GetMarginWidth();

        /// <inheritdoc cref="PropertyGrid.GetRowHeight"/>
        int GetRowHeight();

        /// <inheritdoc cref="PropertyGrid.GetSplitterPosition"/>
        int GetSplitterPosition(uint splitterIndex = 0);

        /// <inheritdoc cref="PropertyGrid.GetVerticalSpacing"/>
        int GetVerticalSpacing();

        /// <inheritdoc cref="PropertyGrid.IsEditorFocused"/>
        bool IsEditorFocused();

        /// <inheritdoc cref="PropertyGrid.IsEditorsValueModified"/>
        bool IsEditorsValueModified();

        /// <inheritdoc cref="PropertyGrid.IsAnyModified"/>
        bool IsAnyModified();

        /// <inheritdoc cref="PropertyGrid.ResetColors"/>
        void ResetColors();

        /// <inheritdoc cref="PropertyGrid.ResetColumnSizes"/>
        void ResetColumnSizes(bool enableAutoResizing = false);

        /// <inheritdoc cref="PropertyGrid.MakeColumnEditable"/>
        void MakeColumnEditable(uint column, bool editable = true);

        /// <inheritdoc cref="PropertyGrid.BeginLabelEdit"/>
        void BeginLabelEdit(uint column = 0);

        /// <inheritdoc cref="PropertyGrid.EndLabelEdit"/>
        void EndLabelEdit(bool commit = true);

        /// <inheritdoc cref="PropertyGrid.SetColumnCount"/>
        void SetColumnCount(int colCount);

        /// <inheritdoc cref="PropertyGrid.SetSplitterPosition"/>
        void SetSplitterPosition(int newXPos, int col = 0);

        /// <inheritdoc cref="PropertyGrid.GetUnspecifiedValueText"/>
        string GetUnspecifiedValueText();

        /// <inheritdoc cref="PropertyGrid.SetVirtualWidth"/>
        void SetVirtualWidth(int width);

        /// <inheritdoc cref="PropertyGrid.SetSplitterLeft"/>
        void SetSplitterLeft(bool privateChildrenToo = false);

        /// <inheritdoc cref="PropertyGrid.SetVerticalSpacing"/>
        void SetVerticalSpacing(int? vspacing = null);

        /// <inheritdoc cref="PropertyGrid.HasVirtualWidth"/>
        bool HasVirtualWidth();

        /// <inheritdoc cref="PropertyGrid.GetCommonValueCount"/>
        uint GetCommonValueCount();

        /// <inheritdoc cref="PropertyGrid.GetCommonValueLabel"/>
        string GetCommonValueLabel(uint i);

        /// <inheritdoc cref="PropertyGrid.GetUnspecifiedCommonValue"/>
        int GetUnspecifiedCommonValue();

        /// <inheritdoc cref="PropertyGrid.SetUnspecifiedCommonValue"/>
        void SetUnspecifiedCommonValue(int index);

        /// <inheritdoc cref="PropertyGrid.RefreshEditor"/>
        void RefreshEditor();

        /// <inheritdoc cref="PropertyGrid.WasValueChangedInEvent"/>
        bool WasValueChangedInEvent();

        /// <inheritdoc cref="PropertyGrid.GetSpacingY"/>
        int GetSpacingY();

        /// <inheritdoc cref="PropertyGrid.UnfocusEditor"/>
        bool UnfocusEditor();

        /// <inheritdoc cref="PropertyGrid.GetLastItem"/>
        IPropertyGridItem? GetLastItem(PropertyGridIteratorFlags flags);

        /// <inheritdoc cref="PropertyGrid.GetRoot"/>
        IPropertyGridItem? GetRoot();

        /// <inheritdoc cref="PropertyGrid.GetSelectedProperty"/>
        IPropertyGridItem? GetSelectedProperty();

        /// <inheritdoc cref="PropertyGrid.ChangePropertyValue"/>
        bool ChangePropertyValue(IPropertyGridItem prop, object value);

        /// <inheritdoc cref="PropertyGrid.ChangePropertyValueAsVariant"/>
        bool ChangePropertyValueAsVariant(
                    IPropertyGridItem prop,
                    IPropertyGridVariant value);

        /// <inheritdoc cref="PropertyGrid.SetPropertyValueAsVariant"/>
        void SetPropertyValueAsVariant(
                    IPropertyGridItem prop,
                    IPropertyGridVariant value);

        /// <inheritdoc cref="PropertyGrid.SetPropertyImage"/>
        void SetPropertyImage(IPropertyGridItem prop, ImageSet? bmp);

        /// <inheritdoc cref="PropertyGrid.SetPropertyAttribute"/>
        void SetPropertyAttribute(
                    IPropertyGridItem prop,
                    string attrName,
                    object? value = null,
                    PropertyGridItemValueFlags argFlags = 0);

        /// <inheritdoc cref="PropertyGrid.SetPropertyAttributeAsVariant"/>
        void SetPropertyAttributeAsVariant(
            IPropertyGridItem prop,
            string attrName,
            IPropertyGridVariant value,
            PropertyGridItemValueFlags argFlags = 0);

        /// <inheritdoc cref="PropertyGrid.SetPropertyKnownAttribute"/>
        void SetPropertyKnownAttribute(
            IPropertyGridItem prop,
            PropertyGridItemAttrId attrName,
            object? value,
            PropertyGridItemValueFlags argFlags = 0);

        /// <inheritdoc cref="PropertyGrid.SetPropertyAttributeAll(string, object)"/>
        void SetPropertyAttributeAll(string attrName, object value);

        /// <inheritdoc cref="PropertyGrid.SetPropertyAttributeAll(string, IPropertyGridVariant)"/>
        void SetPropertyAttributeAll(string attrName, IPropertyGridVariant value);

        /// <inheritdoc cref="PropertyGrid.EnsureVisible"/>
        bool EnsureVisible(IPropertyGridItem prop);

        /// <inheritdoc cref="PropertyGrid.SelectProperty"/>
        bool SelectProperty(IPropertyGridItem prop, bool focus = false);

        /// <inheritdoc cref="PropertyGrid.AddToSelection"/>
        bool AddToSelection(IPropertyGridItem prop);
    }
}
