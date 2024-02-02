using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Draws control parts using native operating system visual style.
    /// </summary>
    public class NativeControlPainter : CustomControlPainter
    {
        public static NativeControlPainter Default = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeControlPainter"/> class.
        /// </summary>
        public NativeControlPainter()
            : base()
        {
        }

        /// <summary>
        /// Control state flags used for control painting.
        /// </summary>
        [Flags]
        public enum DrawFlags
        {
            /// <summary>
            /// Absence of any other flags.
            /// </summary>
            None = 0x00000000,

            /// <summary>
            /// Control is disabled.
            /// </summary>
            Disabled = 0x00000001,

            /// <summary>
            /// Currently has keyboard focus.
            /// </summary>
            Focused = 0x00000002,

            /// <summary>
            /// Button is pressed.
            /// </summary>
            Pressed = 0x00000004,

            /// <summary>
            /// Control-specific bit.
            /// </summary>
            Special = 0x00000008,

            /// <summary>
            /// Only for the buttons.
            /// </summary>
            IsDefault = Special,

            /// <summary>
            /// Only for the menu items.
            /// </summary>
            IsSubMenu = Special,

            /// <summary>
            /// Only for the tree items.
            /// </summary>
            Expanded = Special,

            /// <summary>
            /// Only for the status bar panes.
            /// </summary>
            SizeGrip = Special,

            /// <summary>
            /// Checkboxes only: flat border.
            /// </summary>
            Flat = Special,

            /// <summary>
            /// Only for item selection rect.
            /// </summary>
            Cell = Special,

            /// <summary>
            /// Mouse is currently over the control.
            /// </summary>
            Current = 0x00000010,

            /// <summary>
            /// Selected item in e.g. ListBox.
            /// </summary>
            Selected = 0x00000020,

            /// <summary>
            /// Check or radio button is checked.
            /// </summary>
            Checked = 0x00000040,

            /// <summary>
            /// Menu item can be checked.
            /// </summary>
            Checkable = 0x00000080,

            /// <summary>
            /// Check undetermined state.
            /// </summary>
            Undetermined = Checkable,
        }

        /// <summary>
        /// Defines buttons that can be shown in the title bar.
        /// </summary>
        [Flags]
        public enum TitleBarButtonFlags
        {
            /// <summary>
            /// Button 'Close' is shown in the title bar.
            /// </summary>
            Close = 0x01000000,

            /// <summary>
            /// Button 'Maximize' is shown in the title bar.
            /// </summary>
            Maximize = 0x02000000,

            /// <summary>
            /// Button 'Iconize' is shown in the title bar.
            /// </summary>
            Iconize = 0x04000000,

            /// <summary>
            /// Button 'Restore' is shown in the title bar.
            /// </summary>
            Restore = 0x08000000,

            /// <summary>
            /// Button 'Help' is shown in the title bar.
            /// </summary>
            Help = 0x10000000,
        }

        /// <summary>
        /// Defines header sort icon types.
        /// </summary>
        public enum HeaderSortIconType
        {
            /// <summary>
            /// Header button has no sort arrow.
            /// </summary>
            None,

            /// <summary>
            /// Header button an up sort arrow icon.
            /// </summary>
            Up,

            /// <summary>
            /// Header button a down sort arrow icon.
            /// </summary>
            Down,
        }

        // Draw the header control button(used, for example, by wxListCtrl).
        // Depending on platforms the flags parameter may support the
        // wxCONTROL_SELECTED wxCONTROL_DISABLED and wxCONTROL_CURRENT bits.
        // Returns the optimal width to contain the unabbreviated label text or bitmap,
        // the sort arrow if present, and internal margins.
        public int DrawHeaderButton(
            Control control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0,
            HeaderSortIconType sortArrow = HeaderSortIconType.None,
            object? headerButtonParams = null)
        {
            return Alternet.UI.Native.WxOtherFactory.RendererDrawHeaderButton(
                default,
                control.WxWidget,
                dc.NativeDrawingContext,
                control.PixelFromDip(rect),
                (int)flags,
                (int)sortArrow,
                default);
        }

        // Draw the contents of a header control button (label, sort arrows, etc.)
        // Normally only called by DrawHeaderButton.
        public int DrawHeaderButtonContents(
            Control control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0,
            HeaderSortIconType sortArrow = HeaderSortIconType.None,
            object? headerButtonParams = null)
        {
            return Alternet.UI.Native.WxOtherFactory.RendererDrawHeaderButtonContents(
                default,
                control.WxWidget,
                dc.NativeDrawingContext,
                control.PixelFromDip(rect),
                (int)flags,
                (int)sortArrow,
                default);
        }

        // draw the expanded/collapsed icon for a tree control item
        public void DrawTreeItemButton(
            Control control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawTreeItemButton(
                default,
                control.WxWidget,
                dc.NativeDrawingContext,
                control.PixelFromDip(rect),
                (int)flags);
        }

        // draw the border for sash window: this border must be such that the sash
        // drawn by DrawSash() blends into it well
        public void DrawSplitterBorder(
            Control control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawSplitterBorder(
                default,
                control.WxWidget,
                dc.NativeDrawingContext,
                control.PixelFromDip(rect),
                (int)flags);
        }

        // draw a combobox dropdown button
        // flags may use wxCONTROL_PRESSED and wxCONTROL_CURRENT
        public void DrawComboBoxDropButton(
            Control control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawComboBoxDropButton(
                default,
                control.WxWidget,
                dc.NativeDrawingContext,
                control.PixelFromDip(rect),
                (int)flags);
        }

        // draw a dropdown arrow
        // flags may use wxCONTROL_PRESSED and wxCONTROL_CURRENT
        public void DrawDropArrow(
            Control control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawDropArrow(
                default,
                control.WxWidget,
                dc.NativeDrawingContext,
                control.PixelFromDip(rect),
                (int)flags);
        }

        // draw check button
        // flags may use wxCONTROL_CHECKED, wxCONTROL_UNDETERMINED and wxCONTROL_CURRENT
        public void DrawCheckBox(
            Control control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawCheckBox(
                default,
                control.WxWidget,
                dc.NativeDrawingContext,
                control.PixelFromDip(rect),
                (int)flags);
        }

        // draw check mark
        // flags may use wxCONTROL_DISABLED
        public void DrawCheckMark(
            Control control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawCheckMark(
                default,
                control.WxWidget,
                dc.NativeDrawingContext,
                control.PixelFromDip(rect),
                (int)flags);
        }

        // draw blank button
        // flags may use wxCONTROL_PRESSED, wxCONTROL_CURRENT and wxCONTROL_ISDEFAULT
        public void DrawPushButton(
            Control control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawPushButton(
                default,
                control.WxWidget,
                dc.NativeDrawingContext,
                control.PixelFromDip(rect),
                (int)flags);
        }

        // draw collapse button
        // flags may use wxCONTROL_CHECKED, wxCONTROL_UNDETERMINED and wxCONTROL_CURRENT
        public void DrawCollapseButton(
            Control control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawCollapseButton(
                default,
                control.WxWidget,
                dc.NativeDrawingContext,
                control.PixelFromDip(rect),
                (int)flags);
        }

        // draw rectangle indicating that an item in e.g. a list control
        // has been selected or focused
        // flags may use
        // wxCONTROL_SELECTED (item is selected, e.g. draw background)
        // wxCONTROL_CURRENT (item is the current item, e.g. dotted border)
        // wxCONTROL_FOCUSED (the whole control has focus, e.g. blue background vs. grey otherwise)
        public void DrawItemSelectionRect(
            Control control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawItemSelectionRect(
                default,
                control.WxWidget,
                dc.NativeDrawingContext,
                control.PixelFromDip(rect),
                (int)flags);
        }

        // draw the focus rectangle around the label contained in the given rect
        // only wxCONTROL_SELECTED makes sense in flags here
        public void DrawFocusRect(
            Control control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawFocusRect(
                default,
                control.WxWidget,
                dc.NativeDrawingContext,
                control.PixelFromDip(rect),
                (int)flags);
        }

        // Draw a native wxChoice
        public void DrawChoice(
            Control control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawChoice(
                default,
                control.WxWidget,
                dc.NativeDrawingContext,
                control.PixelFromDip(rect),
                (int)flags);
        }

        // Draw a native wxComboBox
        public void DrawComboBox(
            Control control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawComboBox(
                default,
                control.WxWidget,
                dc.NativeDrawingContext,
                control.PixelFromDip(rect),
                (int)flags);
        }

        // Draw a native wxTextCtrl frame
        public void DrawTextCtrl(
            Control control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawTextCtrl(
                default,
                control.WxWidget,
                dc.NativeDrawingContext,
                control.PixelFromDip(rect),
                (int)flags);
        }

        // Draw a native wxRadioButton bitmap
        public void DrawRadioBitmap(
            Control control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawRadioBitmap(
                default,
                control.WxWidget,
                dc.NativeDrawingContext,
                control.PixelFromDip(rect),
                (int)flags);
        }

        // Draw a gauge with native style like a wxGauge would display.
        // wxCONTROL_SPECIAL flag must be used for drawing vertical gauges.
        public void DrawGauge(
            Control control,
            Graphics dc,
            RectD rect,
            int value,
            int max,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawGauge(
                default,
                control.WxWidget,
                dc.NativeDrawingContext,
                control.PixelFromDip(rect),
                value,
                max,
                (int)flags);
        }

        // Draw text using the appropriate color for normal and selected states.
        public void DrawItemText(
            Control control,
            Graphics dc,
            string text,
            RectD rect,
            GenericAlignment align = GenericAlignment.Left | GenericAlignment.Top,
            DrawFlags flags = 0,
            TextEllipsizeType ellipsizeMode = TextEllipsizeType.End)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawItemText(
                default,
                control.WxWidget,
                dc.NativeDrawingContext,
                text,
                control.PixelFromDip(rect),
                (int)align,
                (int)flags,
                (int)ellipsizeMode);
        }

        // Returns the default size of a check box.
        internal SizeI GetCheckBoxSize(Control control, DrawFlags flags = 0)
        {
            return Alternet.UI.Native.WxOtherFactory.RendererGetCheckBoxSize(
                default,
                control.WxWidget,
                (int)flags);
        }

        // Returns the default size of a check mark.
        internal SizeI GetCheckMarkSize(Control control)
        {
            return Alternet.UI.Native.WxOtherFactory.RendererGetCheckMarkSize(
                default,
                control.WxWidget);
        }

        // Returns the default size of a expander.
        internal SizeI GetExpanderSize(Control control)
        {
            return Alternet.UI.Native.WxOtherFactory.RendererGetExpanderSize(
                default,
                control.WxWidget);
        }

        // Returns the default height of a header button, either a fixed platform
        // height if available, or a generic height based on the window's font.
        internal int GetHeaderButtonHeight(Control control)
        {
            return Alternet.UI.Native.WxOtherFactory.RendererGetHeaderButtonHeight(
                default,
                control.WxWidget);
        }

        // Returns the margin on left and right sides of header button's label
        internal int GetHeaderButtonMargin(Control control)
        {
            return Alternet.UI.Native.WxOtherFactory.RendererGetHeaderButtonMargin(
                default,
                control.WxWidget);
        }

        // draw a (vertical) sash
        internal void DrawSplitterSash(
            Control control,
            Graphics dcReal,
            SizeI sizeReal,
            int position,
            int orientation,
            DrawFlags flags = 0)
        {
        }

        // Returns the default size of a collapse button
        internal SizeI GetCollapseButtonSize(Control control, Graphics dc)
        {
            return Alternet.UI.Native.WxOtherFactory.RendererGetCollapseButtonSize(
                default,
                control.WxWidget,
                dc.NativeDrawingContext);
        }

        internal string GetVersion()
        {
            return string.Empty;
        }
    }
}
