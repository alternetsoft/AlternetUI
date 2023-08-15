#pragma warning disable
using ApiCommon;
using System;
using System.Collections.Generic;
using System.Text;
using Alternet.Drawing;

namespace NativeApi.Api
{
	//https://docs.wxwidgets.org/3.2/classwx_aui_tool_bar.html
	public class AuiToolBar	: Control
	{
        public void SetArtProvider(IntPtr art) {}
        public IntPtr GetArtProvider() => throw new Exception();

        public IntPtr AddTool(int toolId,
                     string label,
                     IntPtr bitmapBundle,
                     string shortHelpString,
                     int itemKind) => throw new Exception();

        public IntPtr AddTool2(int toolId,
                     string label,
                     IntPtr bitmapBundle,
                     IntPtr disabledBitmapBundle,
                     int itemKind,
                     string shortHelpString,
                     string longHelpString,
                     IntPtr clientData) => throw new Exception();

        public IntPtr AddTool3(int toolId,
                     IntPtr bitmapBundle,
                     IntPtr disabledBitmapBundle,
                     bool toggle,
                     IntPtr clientData,
                     string shortHelpString,
                     string longHelpString) => throw new Exception();

        public IntPtr AddLabel(int toolId, string label, int width = -1) =>
            throw new Exception();

        public IntPtr AddControl(IntPtr control, string label) => throw new Exception();

        public IntPtr AddSeparator() => throw new Exception();

        public IntPtr AddSpacer(int pixels) => throw new Exception();

        public IntPtr AddStretchSpacer(int proportion = 1) => throw new Exception();

        public bool Realize() => throw new Exception();

        public IntPtr FindControl(int windowId) => throw new Exception();
        public IntPtr FindToolByPosition(int x, int y) => throw new Exception();
        public IntPtr FindToolByIndex(int idx) => throw new Exception();
        public IntPtr FindTool(int toolId) => throw new Exception();

        public void Clear() => throw new Exception();

        public bool DestroyTool(int toolId) => throw new Exception();
        public bool DestroyToolByIndex(int idx) => throw new Exception();

        // Note that these methods do _not_ delete the associated control, if any.
        // Use DestroyTool() or DestroyToolByIndex() if this is wanted.
        public bool DeleteTool(int toolId) => throw new Exception();

        public bool DeleteByIndex(int toolId) => throw new Exception();

        public int GetToolIndex(int toolId) => throw new Exception();
        public bool GetToolFits(int toolId) => throw new Exception();
        public Rect GetToolRect(int toolId) => throw new Exception();

        public bool GetToolFitsByIndex(int toolId) => throw new Exception();
        public bool GetToolBarFits() => throw new Exception();

        public void SetToolBitmapSize(Size size) => throw new Exception();
        public Size GetToolBitmapSize() => throw new Exception();

        public bool GetOverflowVisible() => throw new Exception();
        public void SetOverflowVisible(bool visible) => throw new Exception();

        public bool GetGripperVisible() => throw new Exception();
        public void SetGripperVisible(bool visible) => throw new Exception();

        public void ToggleTool(int toolId, bool state) => throw new Exception();
        public bool GetToolToggled(int toolId) => throw new Exception();

        public void SetMargins(int left, int right, int top, int bottom)
            => throw new Exception();

        public void EnableTool(int toolId, bool state) => throw new Exception();
        public bool GetToolEnabled(int toolId) => throw new Exception();

        public void SetToolDropDown(int toolId, bool dropdown) => throw new Exception();
        public bool GetToolDropDown(int toolId) => throw new Exception();

        public void SetToolBorderPadding(int padding) => throw new Exception();
        public int GetToolBorderPadding() => throw new Exception();

        public void SetToolTextOrientation(int orientation) => throw new Exception();
        public int GetToolTextOrientation() => throw new Exception();

        public void SetToolPacking(int packing) => throw new Exception();
        public int GetToolPacking() => throw new Exception();

        public void SetToolProportion(int toolId, int proportion) =>
            throw new Exception();
        public int GetToolProportion(int toolId) => throw new Exception();

        public void SetToolSeparation(int separation) => throw new Exception();
        public int GetToolSeparation() => throw new Exception();

        public void SetToolSticky(int toolId, bool sticky) => throw new Exception();
        public bool GetToolSticky(int toolId) => throw new Exception();
        public string GetToolLabel(int toolId) => throw new Exception();
        public void SetToolLabel(int toolId, string label) => throw new Exception();

        public IntPtr GetToolBitmap(int toolId) => throw new Exception();
        public void SetToolBitmap(int toolId, IntPtr bitmapBundle) =>
            throw new Exception();

        public string GetToolShortHelp(int toolId) => throw new Exception();
        public void SetToolShortHelp(int toolId, string helpString) =>
            throw new Exception();

        public string GetToolLongHelp(int toolId) => throw new Exception();
        public void SetToolLongHelp(int toolId, string helpString) =>
            throw new Exception();
        public ulong GetToolCount() => throw new Exception();
    }
}