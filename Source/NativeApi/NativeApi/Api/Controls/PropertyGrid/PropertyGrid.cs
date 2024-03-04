#pragma warning disable
using ApiCommon;
using System;
using System.Collections.Generic;
using System.Text;
using Alternet.Drawing;

namespace NativeApi.Api
{
    //https://docs.wxwidgets.org/3.2/overview_propgrid.html
    //https://docs.wxwidgets.org/3.2/classwx_property_grid.html
    public partial class PropertyGrid : Control
    {
        public PointI CalcScrolledPosition(PointI point) => default;
        public PointI CalcUnscrolledPosition(PointI point) => default;
        public int GetHitTestColumn(PointI point) => default;
        public IntPtr GetHitTestProp(PointI point) => default;

        // https://docs.wxwidgets.org/3.2/classwx_colour_database.html
        public static IntPtr ColorDatabaseCreate() => default;
        public static void ColorDatabaseDelete(IntPtr handle) { }
        public static void ColorDatabaseSetGlobal(IntPtr handle) { }
        public static void ColorDatabaseAdd(IntPtr handle, string name, Color color) { }
        public static Color ColorDatabaseFind(IntPtr handle, string name) => default;
        public static string ColorDatabaseFindName(IntPtr handle, Color color) => default;

        public static void KnownColorsClear() { }
        public static void KnownColorsAdd(string name, string title, Color value, int knownColor) { }
        public static void KnownColorsApply() { }
        public static void KnownColorsSetCustomColorTitle(string value) { }

        public IntPtr GetPropertyValidator(IntPtr prop) => default;
        public void SetPropertyValidator(IntPtr prop, IntPtr validator) { }

        public void SetPropertyFlag(IntPtr prop, int flag, bool value) { }

        public int EventValidationFailureBehavior { get; set; }
        public IntPtr EventPropValue { get; }
        public int EventColumn { get; }
        public IntPtr EventProperty { get; }
        public string EventPropertyName { get; }
        public string EventValidationFailureMessage { get; set; }

        public event EventHandler? Selected;

        public event EventHandler? Changed;

        [NativeEvent(cancellable: true)]
        public event EventHandler? Changing;

        public event EventHandler? Highlighted;

        public event EventHandler? ButtonClick;

        public event EventHandler? RightClick;

        public event EventHandler? DoubleClick;

        public event EventHandler? ItemCollapsed;

        public event EventHandler? ItemExpanded;

        [NativeEvent(cancellable: true)]
        public event EventHandler? LabelEditBegin;

        [NativeEvent(cancellable: true)]
        public event EventHandler? LabelEditEnding;

        [NativeEvent(cancellable: true)]
        public event EventHandler? ColBeginDrag;

        public event EventHandler? ColDragging;

        public event EventHandler? ColEndDrag;

        public static string NameAsLabel { get; }
        public bool HasBorder { get; set; }

        public static IntPtr CreateEx(long styles) => default;

        public long CreateStyle { get; set; }

        public long CreateStyleEx { get; set; }

        public void AddActionTrigger(int action, int keycode, int modifiers = 0) { }

        public void DedicateKey(int keycode) { }

        public static void AutoGetTranslation(bool enable) => throw new Exception();

        public void CenterSplitter(bool enableAutoResizing = false) => throw new Exception();

        public void ClearActionTriggers(int action) => throw new Exception();

        public bool CommitChangesFromEditor(UInt32 flags = 0) => throw new Exception();

        public void EditorsValueWasModified() => throw new Exception();

        public void EditorsValueWasNotModified() => throw new Exception();

        public bool EnableCategories(bool enable) => throw new Exception();

        public SizeD FitColumns() => throw new Exception();

        public Color GetCaptionBackgroundColor() => throw new Exception();

        public Color GetCaptionForegroundColor() => throw new Exception();

        public Color GetCellBackgroundColor() => throw new Exception();

        public Color GetCellDisabledTextColor() => throw new Exception();

        public Color GetCellTextColor() => throw new Exception();

        public uint GetColumnCount() => throw new Exception();

        public Color GetEmptySpaceColor() => throw new Exception();

        public int GetFontHeight() => throw new Exception();

        public Color GetLineColor() => throw new Exception();

        public Color GetMarginColor() => throw new Exception();

        public int GetMarginWidth() => throw new Exception();

        public int GetRowHeight() => throw new Exception();

        public Color GetSelectionBackgroundColor() => default;

        public Color GetSelectionForegroundColor() => default;

        public int GetSplitterPosition(uint splitterIndex = 0) => default;

        public int GetVerticalSpacing() => throw new Exception();

        public bool IsEditorFocused() => throw new Exception();

        public bool IsEditorsValueModified() => throw new Exception();

        public bool IsAnyModified() => throw new Exception();

        public void ResetColors() => throw new Exception();

        public void ResetColumnSizes(bool enableAutoResizing = false) => throw new Exception();

        public void MakeColumnEditable(uint column, bool editable = true) => throw new Exception();

        public void BeginLabelEdit(uint column = 0) => throw new Exception();

        public void EndLabelEdit(bool commit = true) => throw new Exception();

        public void SetCaptionBackgroundColor(Color col) => throw new Exception();

        public void SetCaptionTextColor(Color col) => throw new Exception();

        public void SetCellBackgroundColor(Color col) => throw new Exception();

        public void SetCellDisabledTextColor(Color col) => throw new Exception();

        public void SetCellTextColor(Color col) => throw new Exception();

        public void SetColumnCount(int colCount) => throw new Exception();

        public void SetEmptySpaceColor(Color col) => throw new Exception();

        public void SetLineColor(Color col) => throw new Exception();

        public void SetMarginColor(Color col) => throw new Exception();

        public void SetSelectionBackgroundColor(Color col) => throw new Exception();

        public void SetSelectionTextColor(Color col) => throw new Exception();

        public void SetSplitterPosition(int newXPos, int col = 0) => throw new Exception();

        public string GetUnspecifiedValueText(int argFlags = 0) => throw new Exception();

        public void SetVirtualWidth(int width) => throw new Exception();

        public void SetSplitterLeft(bool privateChildrenToo = false) => throw new Exception();

        public void SetVerticalSpacing(int vspacing) => throw new Exception();

        public bool HasVirtualWidth() => throw new Exception();

        public uint GetCommonValueCount() => throw new Exception();

        public string GetCommonValueLabel(uint i) => throw new Exception();

        public int GetUnspecifiedCommonValue() => throw new Exception();

        public void SetUnspecifiedCommonValue(int index) => throw new Exception();

        public static bool IsSmallScreen() => throw new Exception();

        public void RefreshEditor() => throw new Exception();

        public bool WasValueChangedInEvent() => throw new Exception();

        public int GetSpacingY() => throw new Exception();

        public void SetupTextCtrlValue(string text) => throw new Exception();

        public bool UnfocusEditor() => throw new Exception();

        public IntPtr GetLastItem(int flags) => throw new Exception();

        public IntPtr GetRoot() => throw new Exception();

        public IntPtr GetSelectedProperty() => throw new Exception();

        public bool EnsureVisible(IntPtr propArg) => throw new Exception();

        public bool SelectProperty(IntPtr propArg, bool focus = false) => throw new Exception();

        public bool AddToSelection(IntPtr propArg) => throw new Exception();

        public bool RemoveFromSelection(IntPtr propArg) => throw new Exception();

        public void SetCurrentCategory(IntPtr propArg) => throw new Exception();

        public RectI GetImageRect(IntPtr p, int item) => throw new Exception();

        public SizeI GetImageSize(IntPtr p, int item) => throw new Exception();
    }
}