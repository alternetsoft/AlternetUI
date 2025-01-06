using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with property grid control.
    /// </summary>
    public interface IPropertyGridHandler
    {
        /// <summary>
        /// Gets or sets validation failure behavior. Valid only in the event handler.
        /// </summary>
        PropertyGridValidationFailure EventValidationFailureBehavior { get; set; }

        /// <summary>
        /// Gets target property value as variant. Valid only in the event handler.
        /// </summary>
        IPropertyGridVariant EventPropValueAsVariant { get; }

        /// <summary>
        /// Gets target column index. Valid only in the event handler.
        /// </summary>
        int EventColumn { get; }

        /// <summary>
        /// Gets target item. Valid only in the event handler.
        /// </summary>
        IPropertyGridItem? EventProperty { get; }

        /// <summary>
        /// Gets target property name. Valid only in the event handler.
        /// </summary>
        string EventPropertyName { get; }

        /// <summary>
        /// Gets or sets validation failure message. Valid only in the event handler.
        /// </summary>
        string EventValidationFailureMessage { get; set; }

        /// <inheritdoc cref="PropertyGrid.CreateStyle"/>
        PropertyGridCreateStyle CreateStyle { get; set; }

        /// <inheritdoc cref="PropertyGrid.CreateStyleEx"/>
        PropertyGridCreateStyleEx CreateStyleEx { get; set; }

        /// <inheritdoc cref="PropertyGrid.RefreshProperty"/>
        void RefreshProperty(IPropertyGridItem p);

        /// <inheritdoc cref="PropertyGrid.IsSmallScreen"/>
        bool IsSmallScreen();

        /// <inheritdoc cref="PropertyGrid.SetPropertyReadOnly"/>
        void SetPropertyReadOnly(IPropertyGridItem id, bool set, PropertyGridItemValueFlags flags);

        /// <inheritdoc cref="PropertyGrid.SetPropertyValueUnspecified"/>
        void SetPropertyValueUnspecified(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.AppendIn"/>
        void AppendIn(IPropertyGridItem id, IPropertyGridItem newproperty);

        /// <inheritdoc cref="PropertyGrid.AutoGetTranslation"/>
        void AutoGetTranslation(bool enable);

        /// <inheritdoc cref="PropertyGrid.InitAllTypeHandlers"/>
        void InitAllTypeHandlers();

        /// <inheritdoc cref="PropertyGrid.RegisterAdditionalEditors"/>
        void RegisterAdditionalEditors();

        /// <inheritdoc cref="PropertyGrid.SetBoolChoices"/>
        void SetBoolChoices(string trueChoice, string falseChoice);

        /// <inheritdoc cref="PropertyGrid.CreateVariant"/>
        IPropertyGridVariant CreateVariant();

        /// <summary>
        /// Starts adding children for the specified item.
        /// </summary>
        /// <param name="id"></param>
        void BeginAddChildren(IPropertyGridItem id);

        /// <summary>
        /// Ends adding children for the specified item.
        /// </summary>
        /// <param name="id"></param>
        void EndAddChildren(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.Collapse"/>
        bool Collapse(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.DeleteProperty"/>
        void DeleteProperty(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.RemoveProperty"/>
        void RemoveProperty(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.DisableProperty"/>
        bool DisableProperty(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.EnableProperty"/>
        bool EnableProperty(IPropertyGridItem id, bool enable);

        /// <inheritdoc cref="PropertyGrid.Expand"/>
        bool Expand(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.GetFirstChild"/>
        IPropertyGridItem? GetFirstChild(IPropertyGridItem? id);

        /// <inheritdoc cref="PropertyGrid.GetPropertyCategory"/>
        IPropertyGridItem? GetPropertyCategory(IPropertyGridItem? id);

        /// <summary>
        /// Converts object to <see cref="IPropertyGridVariant"/>.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns></returns>
        IPropertyGridVariant ToVariant(object? value);

        /// <inheritdoc cref="PropertyGrid.GetPropertyClientData"/>
        IntPtr GetPropertyClientData(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.GetPropertyHelpString"/>
        string GetPropertyHelpString(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.GetPropertyLabel"/>
        string GetPropertyLabel(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.GetPropertyParent"/>
        IPropertyGridItem? GetPropertyParent(IPropertyGridItem? id);

        /// <inheritdoc cref="PropertyGrid.GetPropertyValueAsVariant"/>
        IPropertyGridVariant GetPropertyValueAsVariant(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.GetPropertyValueAsString"/>
        string GetPropertyValueAsString(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.GetPropertyValueAsLong"/>
        long GetPropertyValueAsLong(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.GetPropNameAsLabel"/>
        string GetPropNameAsLabel();

        /// <summary>
        /// Gets temporary variant.
        /// </summary>
        /// <returns></returns>
        IPropertyGridVariant GetTempVariant();

        /// <inheritdoc cref="PropertyGrid.GetPropertyValueAsULong"/>
        ulong GetPropertyValueAsULong(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.GetPropertyValueAsInt"/>
        int GetPropertyValueAsInt(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.GetPropertyValueAsBool"/>
        bool GetPropertyValueAsBool(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.GetPropertyValueAsDouble"/>
        double GetPropertyValueAsDouble(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.GetPropertyValueAsDateTime"/>
        DateTime GetPropertyValueAsDateTime(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.HideProperty"/>
        bool HideProperty(IPropertyGridItem id, bool hide, PropertyGridItemValueFlags flags);

        /// <inheritdoc cref="PropertyGrid.Insert"/>
        void Insert(IPropertyGridItem priorThis, IPropertyGridItem newproperty);

        /// <summary>
        /// Inserts item into the parent's childs collection at the specified index.
        /// </summary>
        /// <param name="parent">Parent row.</param>
        /// <param name="index">Index of the item where insertion is performed.</param>
        /// <param name="newproperty">Item to insert.</param>
        void InsertByIndex(IPropertyGridItem parent, int index, IPropertyGridItem newproperty);

        /// <inheritdoc cref="PropertyGrid.IsPropertyCategory"/>
        bool IsPropertyCategory(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.IsPropertyEnabled"/>
        bool IsPropertyEnabled(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.IsPropertyExpanded"/>
        bool IsPropertyExpanded(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.IsPropertyModified"/>
        bool IsPropertyModified(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.IsPropertySelected"/>
        bool IsPropertySelected(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.IsPropertyShown"/>
        bool IsPropertyShown(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.IsPropertyValueUnspecified"/>
        bool IsPropertyValueUnspecified(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.LimitPropertyEditing"/>
        void LimitPropertyEditing(IPropertyGridItem id, bool limit);

        /// <inheritdoc cref="PropertyGrid.ReplaceProperty"/>
        void ReplaceProperty(IPropertyGridItem id, IPropertyGridItem property);

        /// <inheritdoc cref="PropertyGrid.SetPropertyBackgroundColor"/>
        void SetPropertyBackgroundColor(IPropertyGridItem id, Color color, PropertyGridItemValueFlags flags);

        /// <inheritdoc cref="PropertyGrid.SetPropertyColorsToDefault"/>
        void SetPropertyColorsToDefault(IPropertyGridItem id, PropertyGridItemValueFlags flags);

        /// <inheritdoc cref="PropertyGrid.SetPropertyTextColor"/>
        void SetPropertyTextColor(IPropertyGridItem id, Color col, PropertyGridItemValueFlags flags);

        /// <inheritdoc cref="PropertyGrid.GetPropertyBackgroundColor"/>
        Color GetPropertyBackgroundColor(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.GetPropertyTextColor"/>
        Color GetPropertyTextColor(IPropertyGridItem id);

        /// <inheritdoc cref="PropertyGrid.SetPropertyEditorByName"/>
        void SetPropertyEditorByName(IPropertyGridItem id, string editorName);

        /// <inheritdoc cref="PropertyGrid.SetPropertyLabel"/>
        void SetPropertyLabel(IPropertyGridItem id, string newproplabel);

        /// <summary>
        /// Sets property name.
        /// </summary>
        /// <param name="id">Item.</param>
        /// <param name="newName">New name.</param>
        void SetPropertyName(IPropertyGridItem id, string newName);

        /// <inheritdoc cref="PropertyGrid.SetPropertyHelpString"/>
        void SetPropertyHelpString(IPropertyGridItem id, string helpString);

        /// <inheritdoc cref="PropertyGrid.SetPropertyMaxLength"/>
        bool SetPropertyMaxLength(IPropertyGridItem id, int maxLen);

        /// <inheritdoc cref="PropertyGrid.SetPropertyValueAsLong"/>
        void SetPropertyValueAsLong(IPropertyGridItem id, long value);

        /// <inheritdoc cref="PropertyGrid.SetPropertyValueAsInt"/>
        void SetPropertyValueAsInt(IPropertyGridItem id, int value);

        /// <inheritdoc cref="PropertyGrid.SetPropertyValueAsDouble"/>
        void SetPropertyValueAsDouble(IPropertyGridItem id, double value);

        /// <inheritdoc cref="PropertyGrid.SetPropertyValueAsBool"/>
        void SetPropertyValueAsBool(IPropertyGridItem id, bool value);

        /// <inheritdoc cref="PropertyGrid.SetPropertyValueAsStr"/>
        void SetPropertyValueAsStr(IPropertyGridItem id, string value);

        /// <inheritdoc cref="PropertyGrid.SetPropertyValueAsVariant"/>
        void SetPropertyValueAsVariant(IPropertyGridItem id, IPropertyGridVariant variant);

        /// <inheritdoc cref="PropertyGrid.SetPropertyValueAsDateTime"/>
        void SetPropertyValueAsDateTime(IPropertyGridItem id, DateTime value);

        /// <inheritdoc cref="PropertyGrid.SetValidationFailureBehavior"/>
        void SetValidationFailureBehavior(PropertyGridValidationFailure vfbFlags);

        /// <inheritdoc cref="PropertyGrid.SortChildren"/>
        void SortChildren(IPropertyGridItem id, PropertyGridItemValueFlags flags);

        /// <inheritdoc cref="PropertyGrid.ChangePropertyValue"/>
        bool ChangePropertyValue(IPropertyGridItem id, IPropertyGridVariant variant);

        /// <inheritdoc cref="PropertyGrid.SetPropertyImage"/>
        void SetPropertyImage(IPropertyGridItem prop, ImageSet? bmp);

        /// <inheritdoc cref="PropertyGrid.SetPropertyClientData"/>
        void SetPropertyClientData(IPropertyGridItem prop, IntPtr clientData);

        /// <inheritdoc cref="PropertyGrid.SetPropertyAttribute"/>
        void SetPropertyAttribute(
            IPropertyGridItem id,
            string attrName,
            IPropertyGridVariant variant,
            PropertyGridItemValueFlags argFlags);

        /// <inheritdoc cref="PropertyGrid.SetPropertyAttributeAll(string, IPropertyGridVariant)"/>
        void SetPropertyAttributeAll(string attrName, IPropertyGridVariant variant);

        /// <inheritdoc cref="PropertyGrid.GetSplitterPosition"/>
        int GetSplitterPosition(int splitterIndex);

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
        void ResetColumnSizes(bool enableAutoResizing);

        /// <inheritdoc cref="PropertyGrid.MakeColumnEditable"/>
        void MakeColumnEditable(int column, bool editable);

        /// <inheritdoc cref="PropertyGrid.BeginLabelEdit"/>
        void BeginLabelEdit(int column);

        /// <inheritdoc cref="PropertyGrid.EndLabelEdit"/>
        void EndLabelEdit(bool commit);

        /// <summary>
        /// Imlements set method for <see cref="PropertyGridColors.CaptionBackgroundColor"/>
        /// </summary>
        /// <param name="col">Color.</param>
        void SetCaptionBackgroundColor(Color col);

        /// <summary>
        /// Imlements set method for <see cref="PropertyGridColors.CaptionForegroundColor"/>.
        /// </summary>
        /// <param name="col">Color.</param>
        void SetCaptionTextColor(Color col);

        /// <summary>
        /// Imlements set method for <see cref="PropertyGridColors.CellBackgroundColor"/>.
        /// </summary>
        /// <param name="col">Color.</param>
        void SetCellBackgroundColor(Color col);

        /// <summary>
        /// Imlements set method for <see cref="PropertyGridColors.CellDisabledTextColor"/>.
        /// </summary>
        /// <param name="col">Color.</param>
        void SetCellDisabledTextColor(Color col);

        /// <summary>
        /// Imlements set method for <see cref="PropertyGridColors.CellTextColor"/>.
        /// </summary>
        /// <param name="col">Color.</param>
        void SetCellTextColor(Color col);

        /// <inheritdoc cref="PropertyGrid.SetColumnCount"/>
        void SetColumnCount(int colCount);

        /// <summary>
        /// Imlements set method for <see cref="PropertyGridColors.EmptySpaceColor"/>.
        /// </summary>
        /// <param name="col">Color.</param>
        void SetEmptySpaceColor(Color col);

        /// <summary>
        /// Imlements set method for <see cref="PropertyGridColors.LineColor"/>.
        /// </summary>
        /// <param name="col">Color.</param>
        void SetLineColor(Color col);

        /// <summary>
        /// Imlements set method for <see cref="PropertyGridColors.MarginColor"/>.
        /// </summary>
        /// <param name="col">Color.</param>
        void SetMarginColor(Color col);

        /// <summary>
        /// Imlements set method for <see cref="PropertyGridColors.SelectionBackgroundColor"/>.
        /// </summary>
        /// <param name="col">Color.</param>
        void SetSelectionBackgroundColor(Color col);

        /// <summary>
        /// Imlements set method for <see cref="PropertyGridColors.SelectionForegroundColor"/>.
        /// </summary>
        /// <param name="col">Color.</param>
        void SetSelectionTextColor(Color col);

        /// <inheritdoc cref="PropertyGrid.SetSplitterPosition"/>
        void SetSplitterPosition(int newXPos, int col);

        /// <inheritdoc cref="PropertyGrid.GetUnspecifiedValueText"/>
        string GetUnspecifiedValueText(PropertyGridValueFormatFlags argFlags = 0);

        /// <inheritdoc cref="PropertyGrid.SetVirtualWidth"/>
        void SetVirtualWidth(int width);

        /// <inheritdoc cref="PropertyGrid.SetSplitterLeft"/>
        void SetSplitterLeft(bool privateChildrenToo);

        /// <inheritdoc cref="PropertyGrid.SetVerticalSpacing"/>
        void SetVerticalSpacing(int vspacing);

        /// <inheritdoc cref="PropertyGrid.HasVirtualWidth"/>
        bool HasVirtualWidth();

        /// <inheritdoc cref="PropertyGrid.GetCommonValueCount"/>
        int GetCommonValueCount();

        /// <inheritdoc cref="PropertyGrid.GetCommonValueLabel"/>
        string GetCommonValueLabel(int i);

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

        /// <summary>
        /// Setups current text control value.
        /// </summary>
        /// <param name="text"></param>
        void SetupTextCtrlValue(string text);

        /// <inheritdoc cref="PropertyGrid.UnfocusEditor"/>
        bool UnfocusEditor();

        /// <inheritdoc cref="PropertyGrid.GetLastItem"/>
        IPropertyGridItem? GetLastItem(PropertyGridIteratorFlags flags);

        /// <inheritdoc cref="PropertyGrid.GetRoot"/>
        IPropertyGridItem? GetRoot();

        /// <inheritdoc cref="PropertyGrid.GetSelectedProperty"/>
        IPropertyGridItem? GetSelectedProperty();

        /// <inheritdoc cref="PropertyGrid.EnsureVisible"/>
        bool EnsureVisible(IPropertyGridItem propArg);

        /// <inheritdoc cref="PropertyGrid.SelectProperty"/>
        bool SelectProperty(IPropertyGridItem propArg, bool focus);

        /// <inheritdoc cref="PropertyGrid.AddToSelection"/>
        bool AddToSelection(IPropertyGridItem propArg);

        /// <inheritdoc cref="PropertyGrid.RemoveFromSelection"/>
        bool RemoveFromSelection(IPropertyGridItem propArg);

        /// <inheritdoc cref="PropertyGrid.SetCurrentCategory"/>
        void SetCurrentCategory(IPropertyGridItem propArg);

        /// <inheritdoc cref="PropertyGrid.GetImageRect"/>
        RectI GetImageRect(IPropertyGridItem p, int item);

        /// <inheritdoc cref="PropertyGrid.GetImageSize"/>
        SizeI GetImageSize(IPropertyGridItem? p, int item);

        /// <summary>
        /// Creates color property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <returns></returns>
        PropertyGridItemHandle CreateColorProperty(string label, string name, Color value);

        /// <summary>
        /// Creates string property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <returns>Handle to the item.</returns>
        PropertyGridItemHandle CreateStringProperty(string label, string name, string value);

        /// <summary>
        /// Creates filename property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <returns>Handle to the item.</returns>
        PropertyGridItemHandle CreateFilenameProperty(string label, string name, string value);

        /// <summary>
        /// Creates directory property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <returns>Handle to the item.</returns>
        PropertyGridItemHandle CreateDirProperty(string label, string name, string value);

        /// <summary>
        /// Creates image filename property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <returns>Handle to the item.</returns>
        PropertyGridItemHandle CreateImageFilenameProperty(string label, string name, string value);

        /// <summary>
        /// Creates system color property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <returns>Handle to the item.</returns>
        PropertyGridItemHandle CreateSystemColorProperty(string label, string name, Color value);

        /// <summary>
        /// Creates cursor property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <returns>Handle to the item.</returns>
        PropertyGridItemHandle CreateCursorProperty(string label, string name, int value);

        /// <summary>
        /// Creates boolean property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <returns>Handle to the item.</returns>
        PropertyGridItemHandle CreateBoolProperty(string label, string name, bool value);

        /// <summary>
        /// Creates integer property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <returns>Handle to the item.</returns>
        PropertyGridItemHandle CreateIntProperty(string label, string name, long value);

        /// <summary>
        /// Creates float property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <returns>Handle to the item.</returns>
        PropertyGridItemHandle CreateFloatProperty(string label, string name, double value);

        /// <summary>
        /// Creates unsigned integer property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <returns>Handle to the item.</returns>
        PropertyGridItemHandle CreateUIntProperty(string label, string name, ulong value);

        /// <summary>
        /// Creates long string property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <returns>Handle to the item.</returns>
        PropertyGridItemHandle CreateLongStringProperty(string label, string name, string value);

        /// <summary>
        /// Creates date/time property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <returns>Handle to the item.</returns>
        PropertyGridItemHandle CreateDateProperty(string label, string name, DateTime value);

        /// <summary>
        /// Creates editable enum property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="choices">Pick list.</param>
        /// <param name="value">Property value.</param>
        /// <returns>Handle to the item.</returns>
        PropertyGridItemHandle CreateEditEnumProperty(
            string label,
            string name,
            IPropertyGridChoices choices,
            string value);

        /// <summary>
        /// Creates enum property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <param name="choices">Pick list.</param>
        /// <returns>Handle to the item.</returns>
        PropertyGridItemHandle CreateEnumProperty(
            string label,
            string name,
            IPropertyGridChoices choices,
            int value);

        /// <summary>
        /// Creates flags property.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <param name="choices">Pick list.</param>
        /// <returns>Handle to the item.</returns>
        PropertyGridItemHandle CreateFlagsProperty(
            string label,
            string name,
            IPropertyGridChoices choices,
            int value);

        /// <summary>
        /// Creates property category.
        /// </summary>
        /// <param name="label">Property label.</param>
        /// <param name="name">Property name.</param>
        /// <returns>Handle to the item.</returns>
        PropertyGridItemHandle CreatePropCategory(string label, string name);

        /// <inheritdoc cref="PropertyGrid.Clear"/>
        void Clear();

        /// <summary>
        /// Appends item to the end of the items.
        /// </summary>
        /// <param name="property">Item.</param>
        void Append(IPropertyGridItem property);

        /// <inheritdoc cref="PropertyGrid.ClearSelection"/>
        bool ClearSelection(bool validation);

        /// <inheritdoc cref="PropertyGrid.ClearModifiedStatus"/>
        void ClearModifiedStatus();

        /// <inheritdoc cref="PropertyGrid.CollapseAll"/>
        bool CollapseAll();

        /// <inheritdoc cref="PropertyGrid.EditorValidate"/>
        bool EditorValidate();

        /// <inheritdoc cref="PropertyGrid.ExpandAll"/>
        bool ExpandAll(bool expand);

        /// <inheritdoc cref="PropertyGrid.GetFirst"/>
        IPropertyGridItem? GetFirst(PropertyGridIteratorFlags flags);

        /// <inheritdoc cref="PropertyGrid.GetProperty"/>
        IPropertyGridItem? GetProperty(string? name);

        /// <inheritdoc cref="PropertyGrid.GetPropertyByLabel"/>
        IPropertyGridItem? GetPropertyByLabel(string? label);

        /// <inheritdoc cref="PropertyGrid.GetPropertyByName"/>
        IPropertyGridItem? GetPropertyByName(string? name);

        /// <inheritdoc cref="PropertyGrid.GetPropertyByNameAndSubName"/>
        IPropertyGridItem? GetPropertyByNameAndSubName(string? name, string? subname);

        /// <inheritdoc cref="PropertyGrid.GetSelection"/>
        IPropertyGridItem? GetSelection();

        /// <inheritdoc cref="PropertyGrid.GetPropertyName"/>
        string GetPropertyName(IPropertyGridItem property);

        /// <inheritdoc cref="PropertyGrid.RestoreEditableState"/>
        bool RestoreEditableState(string src, PropertyGridEditableState restoreStates);

        /// <inheritdoc cref="PropertyGrid.SaveEditableState"/>
        string SaveEditableState(PropertyGridEditableState includedStates);

        /// <inheritdoc cref="PropertyGrid.SetColumnProportion"/>
        bool SetColumnProportion(int column, int proportion);

        /// <inheritdoc cref="PropertyGrid.GetColumnProportion"/>
        int GetColumnProportion(int column);

        /// <inheritdoc cref="PropertyGrid.Sort"/>
        void Sort(PropertyGridItemValueFlags flags);

        /// <summary>
        /// Calculates scrolled position.
        /// </summary>
        /// <param name="point">Position.</param>
        /// <returns></returns>
        PointI CalcScrolledPosition(PointI point);

        /// <summary>
        /// Calculates unscrolled position.
        /// </summary>
        /// <param name="point">Position.</param>
        /// <returns></returns>
        PointI CalcUnscrolledPosition(PointI point);

        /// <inheritdoc cref="PropertyGrid.GetHitTestColumn"/>
        int GetHitTestColumn(PointI point);

        /// <inheritdoc cref="PropertyGrid.GetHitTestProp"/>
        IPropertyGridItem? GetHitTestProp(PointD point);

        /// <inheritdoc cref="PropertyGrid.SetPropertyFlag"/>
        void SetPropertyFlag(IPropertyGridItem prop, PropertyGridItemFlags flag, bool value);

        /// <inheritdoc cref="PropertyGrid.AddActionTrigger"/>
        void AddActionTrigger(
            PropertyGridKeyboardAction action,
            Key keycode,
            ModifierKeys modifiers = 0);

        /// <inheritdoc cref="PropertyGrid.DedicateKey"/>
        void DedicateKey(Key keycode);

        /// <inheritdoc cref="PropertyGrid.CenterSplitter"/>
        void CenterSplitter(bool enableAutoResizing);

        /// <inheritdoc cref="PropertyGrid.ClearActionTriggers"/>
        void ClearActionTriggers(PropertyGridKeyboardAction action);

        /// <summary>
        /// Commin changes from editor.
        /// </summary>
        /// <param name="flags">Flags.</param>
        /// <returns></returns>
        bool CommitChangesFromEditor(PropertyGridSelectPropFlags flags);

        /// <inheritdoc cref="PropertyGrid.EditorsValueWasModified"/>
        void EditorsValueWasModified();

        /// <inheritdoc cref="PropertyGrid.EditorsValueWasNotModified"/>
        void EditorsValueWasNotModified();

        /// <inheritdoc cref="PropertyGrid.EnableCategories"/>
        bool EnableCategories(bool enable);

        /// <inheritdoc cref="PropertyGrid.FitColumns"/>
        SizeD FitColumns();

        /// <summary>
        /// Imlements get method for <see cref="PropertyGridColors.CaptionBackgroundColor"/>.
        /// </summary>
        Color GetCaptionBackgroundColor();

        /// <summary>
        /// Imlements get method for <see cref="PropertyGridColors.CaptionForegroundColor"/>.
        /// </summary>
        Color GetCaptionForegroundColor();

        /// <summary>
        /// Imlements get method for <see cref="PropertyGridColors.CellBackgroundColor"/>.
        /// </summary>
        Color GetCellBackgroundColor();

        /// <summary>
        /// Imlements get method for <see cref="PropertyGridColors.CellDisabledTextColor"/>.
        /// </summary>
        Color GetCellDisabledTextColor();

        /// <summary>
        /// Imlements get method for <see cref="PropertyGridColors.CellTextColor"/>.
        /// </summary>
        Color GetCellTextColor();

        /// <inheritdoc cref="PropertyGrid.GetColumnCount"/>
        int GetColumnCount();

        /// <summary>
        /// Imlements get method for <see cref="PropertyGridColors.EmptySpaceColor"/>.
        /// </summary>
        Color GetEmptySpaceColor();

        /// <inheritdoc cref="PropertyGrid.GetFontHeight"/>
        int GetFontHeight();

        /// <summary>
        /// Imlements get method for <see cref="PropertyGridColors.LineColor"/>.
        /// </summary>
        Color GetLineColor();

        /// <summary>
        /// Imlements get method for <see cref="PropertyGridColors.MarginColor"/>.
        /// </summary>
        Color GetMarginColor();

        /// <inheritdoc cref="PropertyGrid.GetMarginWidth"/>
        int GetMarginWidth();

        /// <inheritdoc cref="PropertyGrid.GetRowHeight"/>
        int GetRowHeight();

        /// <summary>
        /// Imlements get method for <see cref="PropertyGridColors.SelectionBackgroundColor"/>.
        /// </summary>
        Color GetSelectionBackgroundColor();

        /// <summary>
        /// Imlements get method for <see cref="PropertyGridColors.SelectionForegroundColor"/>.
        /// </summary>
        Color GetSelectionForegroundColor();
    }
}
