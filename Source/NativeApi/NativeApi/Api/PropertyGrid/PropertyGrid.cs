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
        public int EventValidationFailureBehavior { get; set; }
        public int EventColumn { get;}
        public IntPtr EventProperty { get;}
        public string EventPropertyName { get; }
        public string EventValidationFailureMessage { get; set; }

        public event EventHandler? Selected;
        
        public event EventHandler? Changed;

        [NativeEvent(cancellable: true)]
        public event EventHandler? Changing;
        
        public event EventHandler? Highlighted;
        
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

        public static IntPtr CreateEx(long styles) => throw new Exception();

        public long CreateStyle { get; set; }

        /*
    public void eeAddActionTrigger(int action, int keycode, int modifiers = 0);

    public void eeDedicateKey(int keycode);

    public static void eeAutoGetTranslation(bool enable);

    public void eeCenterSplitter(bool enableAutoResizing = false);

    public void eeClearActionTriggers(int action);

    public bool eeCommitChangesFromEditor(wxUint32 flags = 0);

    public void eeEditorsValueWasModified();

    public void eeEditorsValueWasNotModified();

    public bool eeEnableCategories(bool enable);

    public Size eeFitColumns();

    public Color eeGetCaptionBackgroundColor();

    public Color eeGetCaptionForegroundColor();

    public Color eeGetCellBackgroundColor();

    public Color eeGetCellDisabledTextColor();

    public Color eeGetCellTextColor();

    public uint eeGetColumnCount();

    public Color eeGetEmptySpaceColor();

    public int eeGetFontHeight();

    public Color eeGetLineColor();

    public Color eeGetMarginColor()

    public int eeGetMarginWidth(); 

    public int eeGetRowHeight();

    public Color eeGetSelectionBackgroundColor();

    public Color eeGetSelectionForegroundColor();

    public int eeGetSplitterPosition(uint splitterIndex = 0);

    public int eeGetVerticalSpacing();

    public bool eeIsEditorFocused();

    public bool eeIsEditorsValueModified();

    public bool eeIsAnyModified();

    public void eeResetColors();

    public void eeResetColumnSizes(bool enableAutoResizing = false);

    public void eeMakeColumnEditable(uint column, bool editable = true);

    public void eeBeginLabelEdit(uint column = 0);

    public void eeEndLabelEdit(bool commit = true);

    public void eeSetCaptionBackgroundColor(Color col);

    public void eeSetCaptionTextColor(Color col);

    public void eeSetCellBackgroundColor(Color col);

    public void eeSetCellDisabledTextColor(Color col);

    public void eeSetCellTextColor(Color col);

    public void eeSetColumnCount(int colCount);

    public void eeSetEmptySpaceColor(Color col);

    public void eeSetLineColor(Color col);

    public void eeSetMarginColor(Color col);

    public void eeSetSelectionBackgroundColor(Color col);

    public void eeSetSelectionTextColor(Color col);

    public void eeSetSplitterPosition(int newXPos, int col = 0);

    public string eeGetUnspecifiedValueText(int argFlags = 0);

    public void eeSetVirtualWidth(int width);

    public void eeSetSplitterLeft(bool privateChildrenToo = false);

    public void eeSetVerticalSpacing(int vspacing);

    public bool eeHasVirtualWidth();

    public uint eeGetCommonValueCount();

    public string eeGetCommonValueLabel(uint i);

    public int eeGetUnspecifiedCommonValue();

    public void eeSetUnspecifiedCommonValue(int index);

    public static bool eeIsSmallScreen();

    public void eeRefreshEditor();

    public bool eeWasValueChangedInEvent();

    public int eeGetSpacingY();

    public void eeSetupTextCtrlValue(string text);

    public bool eeUnfocusEditor();

    public IntPtr eeGetLastItem(int flags);

    public IntPtr eeGetRoot();

    public IntPtr eeGetSelectedProperty();

    public void eeRefreshProperty(IntPtr p);

    public bool eeEnsureVisible(IntPtr propArg id);

    public bool eeSelectProperty(IntPtr propArg, bool focus = false);

    public bool eeAddToSelection(IntPtr propArg);

    public bool eeRemoveFromSelection(IntPtr propArg);

    public void eeSetCurrentCategory(IntPtr propArg);

    public Int32Rect eeGetImageRect(IntPtr p, int item);

    public Int32Size eeGetImageSize(IntPtr p = null, int item = -1);
        */
    }
}