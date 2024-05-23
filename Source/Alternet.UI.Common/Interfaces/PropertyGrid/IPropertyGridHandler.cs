using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public interface IPropertyGridHandler : IControlHandler
    {
        PropertyGridValidationFailure EventValidationFailureBehavior { get; set; }

        IPropertyGridVariant EventPropValueAsVariant { get; }

        int EventColumn { get; }

        IPropertyGridItem? EventProperty { get; }

        string EventPropertyName { get; }

        string EventValidationFailureMessage { get; set; }

        bool HasBorder { get; set; }

        PropertyGridCreateStyle CreateStyle { get; set; }

        PropertyGridCreateStyleEx CreateStyleEx { get; set; }

        void RefreshProperty(IPropertyGridItem p);

        bool IsSmallScreen();

        void SetPropertyReadOnly(IPropertyGridItem id, bool set, PropertyGridItemValueFlags flags);

        void SetPropertyValueUnspecified(IPropertyGridItem id);

        void AppendIn(IPropertyGridItem id, IPropertyGridItem newproperty);

        void AutoGetTranslation(bool enable);

        void InitAllTypeHandlers();

        void RegisterAdditionalEditors();

        void SetBoolChoices(string trueChoice, string falseChoice);

        IPropertyGridVariant CreateVariant();

        void BeginAddChildren(IPropertyGridItem id);

        bool Collapse(IPropertyGridItem id);

        void DeleteProperty(IPropertyGridItem id);

        void RemoveProperty(IPropertyGridItem id);

        bool DisableProperty(IPropertyGridItem id);

        bool EnableProperty(IPropertyGridItem id, bool enable);

        void EndAddChildren(IPropertyGridItem id);

        bool Expand(IPropertyGridItem id);

        IPropertyGridItem? GetFirstChild(IPropertyGridItem? id);

        IPropertyGridItem? GetPropertyCategory(IPropertyGridItem? id);

        IPropertyGridVariant ToVariant(object? value);

        IntPtr GetPropertyClientData(IPropertyGridItem id);

        string GetPropertyHelpString(IPropertyGridItem id);

        string GetPropertyLabel(IPropertyGridItem id);

        IPropertyGridItem? GetPropertyParent(IPropertyGridItem? id);

        IPropertyGridVariant GetPropertyValueAsVariant(IPropertyGridItem id);

        string GetPropertyValueAsString(IPropertyGridItem id);

        long GetPropertyValueAsLong(IPropertyGridItem id);

        string GetPropNameAsLabel();

        IPropertyGridVariant GetTempVariant();

        ulong GetPropertyValueAsULong(IPropertyGridItem id);

        int GetPropertyValueAsInt(IPropertyGridItem id);

        bool GetPropertyValueAsBool(IPropertyGridItem id);

        double GetPropertyValueAsDouble(IPropertyGridItem id);

        DateTime GetPropertyValueAsDateTime(IPropertyGridItem id);

        bool HideProperty(IPropertyGridItem id, bool hide, PropertyGridItemValueFlags flags);

        void Insert(IPropertyGridItem priorThis, IPropertyGridItem newproperty);

        void InsertByIndex(IPropertyGridItem parent, int index, IPropertyGridItem newproperty);

        bool IsPropertyCategory(IPropertyGridItem id);

        bool IsPropertyEnabled(IPropertyGridItem id);

        bool IsPropertyExpanded(IPropertyGridItem id);

        bool IsPropertyModified(IPropertyGridItem id);

        bool IsPropertySelected(IPropertyGridItem id);

        bool IsPropertyShown(IPropertyGridItem id);

        bool IsPropertyValueUnspecified(IPropertyGridItem id);

        void LimitPropertyEditing(IPropertyGridItem id, bool limit);

        void ReplaceProperty(IPropertyGridItem id, IPropertyGridItem property);

        void SetPropertyBackgroundColor(IPropertyGridItem id, Color color, PropertyGridItemValueFlags flags);

        void SetPropertyColorsToDefault(IPropertyGridItem id, PropertyGridItemValueFlags flags);

        void SetPropertyTextColor(IPropertyGridItem id, Color col, PropertyGridItemValueFlags flags);

        Color GetPropertyBackgroundColor(IPropertyGridItem id);

        Color GetPropertyTextColor(IPropertyGridItem id);

        void SetPropertyEditorByName(IPropertyGridItem id, string editorName);

        void SetPropertyLabel(IPropertyGridItem id, string newproplabel);

        void SetPropertyName(IPropertyGridItem id, string newName);

        void SetPropertyHelpString(IPropertyGridItem id, string helpString);

        bool SetPropertyMaxLength(IPropertyGridItem id, int maxLen);

        void SetPropertyValueAsLong(IPropertyGridItem id, long value);

        void SetPropertyValueAsInt(IPropertyGridItem id, int value);

        void SetPropertyValueAsDouble(IPropertyGridItem id, double value);

        void SetPropertyValueAsBool(IPropertyGridItem id, bool value);

        void SetPropertyValueAsStr(IPropertyGridItem id, string value);

        void SetPropertyValueAsVariant(IPropertyGridItem id, IPropertyGridVariant variant);

        void SetPropertyValueAsDateTime(IPropertyGridItem id, DateTime value);

        void SetValidationFailureBehavior(PropertyGridValidationFailure vfbFlags);

        void SortChildren(IPropertyGridItem id, PropertyGridItemValueFlags flags);

        bool ChangePropertyValue(IPropertyGridItem id, IPropertyGridVariant variant);

        void SetPropertyImage(IPropertyGridItem prop, ImageSet? bmp);

        void SetPropertyClientData(IPropertyGridItem prop, IntPtr clientData);

        void SetPropertyAttribute(
            IPropertyGridItem id,
            string attrName,
            IPropertyGridVariant variant,
            PropertyGridItemValueFlags argFlags);

        void SetPropertyAttributeAll(string attrName, IPropertyGridVariant variant);

        int GetSplitterPosition(int splitterIndex);

        int GetVerticalSpacing();

        bool IsEditorFocused();

        bool IsEditorsValueModified();

        bool IsAnyModified();

        void ResetColors();

        void ResetColumnSizes(bool enableAutoResizing);

        void MakeColumnEditable(int column, bool editable);

        void BeginLabelEdit(int column);

        void EndLabelEdit(bool commit);

        void SetCaptionBackgroundColor(Color col);

        void SetCaptionTextColor(Color col);

        void SetCellBackgroundColor(Color col);

        void SetCellDisabledTextColor(Color col);

        void SetCellTextColor(Color col);

        void SetColumnCount(int colCount);

        void SetEmptySpaceColor(Color col);

        void SetLineColor(Color col);

        void SetMarginColor(Color col);

        void SetSelectionBackgroundColor(Color col);

        void SetSelectionTextColor(Color col);

        void SetSplitterPosition(int newXPos, int col);

        string GetUnspecifiedValueText(PropertyGridValueFormatFlags argFlags = 0);

        void SetVirtualWidth(int width);

        void SetSplitterLeft(bool privateChildrenToo);

        void SetVerticalSpacing(int vspacing);

        bool HasVirtualWidth();

        int GetCommonValueCount();

        string GetCommonValueLabel(int i);

        int GetUnspecifiedCommonValue();

        void SetUnspecifiedCommonValue(int index);

        void RefreshEditor();

        bool WasValueChangedInEvent();

        int GetSpacingY();

        void SetupTextCtrlValue(string text);

        bool UnfocusEditor();

        IPropertyGridItem? GetLastItem(PropertyGridIteratorFlags flags);

        IPropertyGridItem? GetRoot();

        IPropertyGridItem? GetSelectedProperty();

        bool EnsureVisible(IPropertyGridItem propArg);

        bool SelectProperty(IPropertyGridItem propArg, bool focus);

        bool AddToSelection(IPropertyGridItem propArg);

        bool RemoveFromSelection(IPropertyGridItem propArg);

        void SetCurrentCategory(IPropertyGridItem propArg);

        RectI GetImageRect(IPropertyGridItem p, int item);

        SizeI GetImageSize(IPropertyGridItem? p, int item);

        void SetPropertyValidator(IPropertyGridItem prop, IValueValidator validator);

        PropertyGridItemHandle CreateColorProperty(string label, string name, Color value);

        PropertyGridItemHandle CreateStringProperty(string label, string name, string value);

        PropertyGridItemHandle CreateFilenameProperty(string label, string name, string value);

        PropertyGridItemHandle CreateDirProperty(string label, string name, string value);

        PropertyGridItemHandle CreateImageFilenameProperty(string label, string name, string value);

        PropertyGridItemHandle CreateSystemColorProperty(string label, string name, Color value);

        PropertyGridItemHandle CreateCursorProperty(string label, string name, int value);

        PropertyGridItemHandle CreateBoolProperty(string label, string name, bool value);

        PropertyGridItemHandle CreateIntProperty(string label, string name, long value);

        PropertyGridItemHandle CreateFloatProperty(string label, string name, double value);

        PropertyGridItemHandle CreateUIntProperty(string label, string name, ulong value);

        PropertyGridItemHandle CreateLongStringProperty(string label, string name, string value);

        PropertyGridItemHandle CreateDateProperty(string label, string name, DateTime value);

        PropertyGridItemHandle CreateEditEnumProperty(string label, string name, IPropertyGridChoices choices, string value);

        PropertyGridItemHandle CreateEnumProperty(string label, string name, IPropertyGridChoices choices, int value);

        PropertyGridItemHandle CreateFlagsProperty(string label, string name, IPropertyGridChoices choices, int value);

        PropertyGridItemHandle CreatePropCategory(string label, string name);

        void Clear();

        void Append(IPropertyGridItem property);

        bool ClearSelection(bool validation);

        void ClearModifiedStatus();

        bool CollapseAll();

        bool EditorValidate();

        bool ExpandAll(bool expand);

        IPropertyGridItem? GetFirst(PropertyGridIteratorFlags flags);

        IPropertyGridItem? GetProperty(string? name);

        IPropertyGridItem? GetPropertyByLabel(string? label);

        IPropertyGridItem? GetPropertyByName(string? name);

        IPropertyGridItem? GetPropertyByNameAndSubName(string? name, string? subname);

        IPropertyGridItem? GetSelection();

        string GetPropertyName(IPropertyGridItem property);

        bool RestoreEditableState(string src, PropertyGridEditableState restoreStates);

        string SaveEditableState(PropertyGridEditableState includedStates);

        bool SetColumnProportion(int column, int proportion);

        int GetColumnProportion(int column);

        void Sort(PropertyGridItemValueFlags flags);

        PointI CalcScrolledPosition(PointI point);

        PointI CalcUnscrolledPosition(PointI point);

        int GetHitTestColumn(PointI point);

        IPropertyGridItem? GetHitTestProp(PointD point);

        void SetPropertyFlag(IPropertyGridItem prop, PropertyGridItemFlags flag, bool value);

        void AddActionTrigger(
            PropertyGridKeyboardAction action,
            Key keycode,
            ModifierKeys modifiers = 0);

        void DedicateKey(Key keycode);

        void CenterSplitter(bool enableAutoResizing);

        void ClearActionTriggers(PropertyGridKeyboardAction action);

        bool CommitChangesFromEditor(PropertyGridSelectPropFlags flags);

        void EditorsValueWasModified();

        void EditorsValueWasNotModified();

        bool EnableCategories(bool enable);

        SizeD FitColumns();

        Color GetCaptionBackgroundColor();

        Color GetCaptionForegroundColor();

        Color GetCellBackgroundColor();

        Color GetCellDisabledTextColor();

        Color GetCellTextColor();

        int GetColumnCount();

        Color GetEmptySpaceColor();

        int GetFontHeight();

        Color GetLineColor();

        Color GetMarginColor();

        int GetMarginWidth();

        int GetRowHeight();

        Color GetSelectionBackgroundColor();

        Color GetSelectionForegroundColor();
    }
}
