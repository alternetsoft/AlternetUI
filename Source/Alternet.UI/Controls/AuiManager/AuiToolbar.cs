using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    public class AuiToolbar : Control
    {
        private int idCounter = 0;

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.AuiToolbar;

        internal new NativeAuiToolbarHandler Handler
        {
            get
            {
                CheckDisposed();
                return (NativeAuiToolbarHandler)base.Handler;
            }
        }

        internal Native.AuiToolBar NativeControl => Handler.NativeControl;

        public int AddTool(
            string label,
            ImageSet? bitmapBundle,
            string? shortHelpString,
            AuiToolbarItemKind itemKind = AuiToolbarItemKind.Normal)
        {
            int toolId = GenNewId();
            NativeControl.AddTool(
                toolId,
                label,
                bitmapBundle?.NativeImageSet,
                shortHelpString!,
                (int)itemKind);
            return toolId;
        }

        public int AddTool(
            string label,
            ImageSet? bitmapBundle,
            ImageSet? disabledBitmapBundle,
            AuiToolbarItemKind itemKind,
            string? shortHelpString,
            string? longHelpString,
            IntPtr clientData)
        {
            int toolId = GenNewId();
            NativeControl.AddTool2(
                toolId,
                label,
                bitmapBundle?.NativeImageSet,
                disabledBitmapBundle?.NativeImageSet,
                (int)itemKind,
                shortHelpString!,
                longHelpString!,
                clientData);
            return toolId;
        }

        public int AddTool(
            ImageSet? bitmapBundle,
            ImageSet? disabledBitmapBundle,
            bool toggle,
            IntPtr clientData,
            string? shortHelpString,
            string? longHelpString)
        {
            int toolId = GenNewId();
            NativeControl.AddTool3(
                toolId,
                bitmapBundle?.NativeImageSet,
                disabledBitmapBundle?.NativeImageSet,
                toggle,
                clientData,
                shortHelpString!,
                longHelpString!);
            return toolId;
        }

        public int AddLabel(string label, int width = -1)
        {
            int toolId = GenNewId();
            NativeControl.AddLabel(toolId, label, width);
            return toolId;
        }

        public void AddControl(Control control, string label)
        {
            NativeControl.AddControl(control.Handler.NativeControl!.WxWidget, label);
        }

        public void AddSeparator()
        {
            NativeControl.AddSeparator();
        }

        public void AddSpacer(int pixels)
        {
            NativeControl.AddSpacer(pixels);
        }

        public void AddStretchSpacer(int proportion = 1)
        {
            NativeControl.AddStretchSpacer(proportion);
        }

        public bool Realize()
        {
            return NativeControl.Realize();
        }

        public void Clear()
        {
            NativeControl.Clear();
        }

        public bool DestroyTool(int toolId)
        {
            return NativeControl.DestroyTool(toolId);
        }

        public bool DestroyToolByIndex(int idx)
        {
            return NativeControl.DestroyToolByIndex(idx);
        }

        // Note that these methods do _not_ delete the associated control, if any.
        // Use DestroyTool() or DestroyToolByIndex() if this is wanted.
        public bool DeleteTool(int toolId)
        {
            return NativeControl.DeleteTool(toolId);
        }

        public bool DeleteByIndex(int index)
        {
            return NativeControl.DeleteByIndex(index);
        }

        public int GetToolIndex(int toolId)
        {
            return NativeControl.GetToolIndex(toolId);
        }

        public bool GetToolFits(int toolId)
        {
            return NativeControl.GetToolFits(toolId);
        }

        public Rect GetToolRect(int toolId)
        {
            return NativeControl.GetToolRect(toolId);
        }

        public bool GetToolFitsByIndex(int toolId)
        {
            return NativeControl.GetToolFitsByIndex(toolId);
        }

        public bool GetToolBarFits()
        {
            return NativeControl.GetToolBarFits();
        }

        internal void SetToolBitmapSize(Size size)
        {
            NativeControl.SetToolBitmapSize(size);
        }

        internal Size GetToolBitmapSize()
        {
            return NativeControl.GetToolBitmapSize();
        }

        internal bool GetOverflowVisible()
        {
            return NativeControl.GetOverflowVisible();
        }

        internal void SetOverflowVisible(bool visible)
        {
            NativeControl.SetOverflowVisible(visible);
        }

        internal bool GetGripperVisible()
        {
            return NativeControl.GetGripperVisible();
        }

        internal void SetGripperVisible(bool visible)
        {
            NativeControl.SetGripperVisible(visible);
        }

        public void ToggleTool(int toolId, bool state)
        {
            NativeControl.ToggleTool(toolId, state);
        }

        public bool GetToolToggled(int toolId)
        {
            return NativeControl.GetToolToggled(toolId);
        }

        public void SetMargins(int left, int right, int top, int bottom)
        {
            NativeControl.SetMargins(left, right, top, bottom);
        }

        public void EnableTool(int toolId, bool state)
        {
            NativeControl.EnableTool(toolId, state);
        }

        public bool GetToolEnabled(int toolId)
        {
            return NativeControl.GetToolEnabled(toolId);
        }

        public void SetToolDropDown(int toolId, bool dropdown)
        {
            NativeControl.SetToolDropDown(toolId, dropdown);
        }

        public bool GetToolDropDown(int toolId)
        {
            return NativeControl.GetToolDropDown(toolId);
        }

        internal void SetToolBorderPadding(int padding)
        {
            NativeControl.SetToolBorderPadding(padding);
        }

        internal int GetToolBorderPadding()
        {
            return NativeControl.GetToolBorderPadding();
        }

        internal void SetToolTextOrientation(AuiToolbarTextOrientation orientation)
        {
            NativeControl.SetToolTextOrientation((int)orientation);
        }

        internal AuiToolbarTextOrientation GetToolTextOrientation()
        {
            return (AuiToolbarTextOrientation)NativeControl.GetToolTextOrientation();
        }

        internal void SetToolPacking(int packing)
        {
            NativeControl.SetToolPacking(packing);
        }

        internal int GetToolPacking()
        {
            return NativeControl.GetToolPacking();
        }

        public void SetToolProportion(int toolId, int proportion)
        {
            NativeControl.SetToolProportion(toolId, proportion);
        }

        public int GetToolProportion(int toolId)
        {
            return NativeControl.GetToolProportion(toolId);
        }

        internal void SetToolSeparation(int separation)
        {
            NativeControl.SetToolSeparation(separation);
        }

        internal int GetToolSeparation()
        {
            return NativeControl.GetToolSeparation();
        }

        public void SetToolSticky(int toolId, bool sticky)
        {
            NativeControl.SetToolSticky(toolId, sticky);
        }

        public bool GetToolSticky(int toolId)
        {
            return NativeControl.GetToolSticky(toolId);
        }

        public string GetToolLabel(int toolId)
        {
            return NativeControl.GetToolLabel(toolId);
        }

        public void SetToolLabel(int toolId, string label)
        {
            NativeControl.SetToolLabel(toolId, label);
        }

        public void SetToolBitmap(int toolId, ImageSet? bitmapBundle)
        {
            NativeControl.SetToolBitmap(toolId, bitmapBundle?.NativeImageSet);
        }

        public string GetToolShortHelp(int toolId)
        {
            return NativeControl.GetToolShortHelp(toolId);
        }

        public void SetToolShortHelp(int toolId, string helpString)
        {
            NativeControl.SetToolShortHelp(toolId, helpString);
        }

        public string GetToolLongHelp(int toolId)
        {
            return NativeControl.GetToolLongHelp(toolId);
        }

        public void SetToolLongHelp(int toolId, string helpString)
        {
            NativeControl.SetToolLongHelp(toolId, helpString);
        }

        public ulong GetToolCount()
        {
            return NativeControl.GetToolCount();
        }

        internal IntPtr FindControl(int windowId)
        {
            return NativeControl.FindControl(windowId);
        }

        internal IntPtr FindToolByPosition(int x, int y)
        {
            return NativeControl.FindToolByPosition(x, y);
        }

        internal IntPtr FindToolByIndex(int idx)
        {
            return NativeControl.FindToolByIndex(idx);
        }

        internal IntPtr FindTool(int toolId)
        {
            return NativeControl.FindTool(toolId);
        }

        internal void SetArtProvider(IntPtr art)
        {
            NativeControl.SetArtProvider(art);
        }

        internal IntPtr GetArtProvider()
        {
            return NativeControl.GetArtProvider();
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreateAuiToolbarHandler(this);
        }

        private int GenNewId()
        {
            int result = idCounter;
            idCounter++;
            return result;
        }
    }
}
