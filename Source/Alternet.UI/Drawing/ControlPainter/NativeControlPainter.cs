using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace Alternet.Drawing
{
    internal class NativeControlPainter
    {
        public static NativeControlPainter Default = new();

        public int DrawHeaderButton(
            Control control,
            DrawingContext dc,
            Rect rect,
            NativeControlPainterFlags flags = 0,
            HeaderSortIconType sortArrow = HeaderSortIconType.None,
            object? headerButtonParams = null)
        {
            return 0;
        }

        // Draw the contents of a header control button (label, sort arrows, etc.)
        // Normally only called by DrawHeaderButton.
        public int DrawHeaderButtonContents(
            Control control,
            DrawingContext dc,
            Rect rect,
            NativeControlPainterFlags flags = 0,
            HeaderSortIconType sortArrow = HeaderSortIconType.None,
            object? headerButtonParams = null)
        {
            return 0;
        }

        // Returns the default height of a header button, either a fixed platform
        // height if available, or a generic height based on the window's font.
        public int GetHeaderButtonHeight(Control control)
        {
            return 0;
        }

        // Returns the margin on left and right sides of header button's label
        public int GetHeaderButtonMargin(Control control) => default;

        // draw the expanded/collapsed icon for a tree control item
        public void DrawTreeItemButton(
            Control control,
            DrawingContext dc,
            Rect rect,
            NativeControlPainterFlags flags = 0)
        {
        }

        // draw the border for sash window: this border must be such that the sash
        // drawn by DrawSash() blends into it well
        public void DrawSplitterBorder(
            Control control,
            DrawingContext dc,
            Rect rect,
            NativeControlPainterFlags flags = 0)
        {
        }

        // draw a (vertical) sash
        public void DrawSplitterSash(
            Control control,
            DrawingContext dcReal,
            Int32Size sizeReal,
            int position,
            int orientation,
            NativeControlPainterFlags flags = 0)
        {
        }

        // draw a combobox dropdown button
        // flags may use wxCONTROL_PRESSED and wxCONTROL_CURRENT
        public void DrawComboBoxDropButton(
            Control control,
            DrawingContext dc,
            Rect rect,
            NativeControlPainterFlags flags = 0)
        {
        }

        // draw a dropdown arrow
        // flags may use wxCONTROL_PRESSED and wxCONTROL_CURRENT
        public void DrawDropArrow(
            Control control,
            DrawingContext dc,
            Rect rect,
            NativeControlPainterFlags flags = 0)
        {
        }

        // draw check button
        // flags may use wxCONTROL_CHECKED, wxCONTROL_UNDETERMINED and wxCONTROL_CURRENT
        public void DrawCheckBox(
            Control control,
            DrawingContext dc,
            Rect rect,
            NativeControlPainterFlags flags = 0)
        {
        }

        // draw check mark
        // flags may use wxCONTROL_DISABLED
        public void DrawCheckMark(
            Control control,
            DrawingContext dc,
            Rect rect,
            NativeControlPainterFlags flags = 0)
        {
        }

        // Returns the default size of a check box.
        public Int32Size GetCheckBoxSize(Control control, NativeControlPainterFlags flags = 0)
        {
            return default;
        }

        // Returns the default size of a check mark.
        public Int32Size GetCheckMarkSize(Control control)
        {
            return default;
        }

        // Returns the default size of a expander.
        public Int32Size GetExpanderSize(Control control)
        {
            return default;
        }

        // draw blank button
        // flags may use wxCONTROL_PRESSED, wxCONTROL_CURRENT and wxCONTROL_ISDEFAULT
        public void DrawPushButton(
            Control control,
            DrawingContext dc,
            Rect rect,
            NativeControlPainterFlags flags = 0)
        {
        }

        // draw collapse button
        // flags may use wxCONTROL_CHECKED, wxCONTROL_UNDETERMINED and wxCONTROL_CURRENT
        public void DrawCollapseButton(
            Control control,
            DrawingContext dc,
            Rect rect,
            NativeControlPainterFlags flags = 0)
        {
        }

        // Returns the default size of a collapse button
        public Int32Size GetCollapseButtonSize(Control control, DrawingContext dc) => default;

        // draw rectangle indicating that an item in e.g. a list control
        // has been selected or focused
        // flags may use
        // wxCONTROL_SELECTED (item is selected, e.g. draw background)
        // wxCONTROL_CURRENT (item is the current item, e.g. dotted border)
        // wxCONTROL_FOCUSED (the whole control has focus, e.g. blue background vs. grey otherwise)
        public void DrawItemSelectionRect(
            Control control,
            DrawingContext dc,
            Rect rect,
            NativeControlPainterFlags flags = 0)
        {
        }

        // draw the focus rectangle around the label contained in the given rect
        // only wxCONTROL_SELECTED makes sense in flags here
        public void DrawFocusRect(
            Control control,
            DrawingContext dc,
            Rect rect,
            NativeControlPainterFlags flags = 0)
        {
        }

        // Draw a native wxChoice
        public void DrawChoice(
            Control control,
            DrawingContext dc,
            Rect rect,
            NativeControlPainterFlags flags = 0)
        {
        }

        // Draw a native wxComboBox
        public void DrawComboBox(
            Control control,
            DrawingContext dc,
            Rect rect,
            NativeControlPainterFlags flags = 0)
        {
        }

        // Draw a native wxTextCtrl frame
        public void DrawTextCtrl(
            Control control,
            DrawingContext dc,
            Rect rect,
            NativeControlPainterFlags flags = 0)
        {
        }

        // Draw a native wxRadioButton bitmap
        public void DrawRadioBitmap(
            Control control,
            DrawingContext dc,
            Rect rect,
            NativeControlPainterFlags flags = 0)
        {
        }

        // Draw a gauge with native style like a wxGauge would display.
        // wxCONTROL_SPECIAL flag must be used for drawing vertical gauges.
        public void DrawGauge(
            Control control,
            DrawingContext dc,
            Rect rect,
            int value,
            int max,
            NativeControlPainterFlags flags = 0)
        {
        }

        // Draw text using the appropriate color for normal and selected states.
        public void DrawItemText(
            Control control,
            DrawingContext dc,
            string text,
            Rect rect,
            GenericAlignment align = GenericAlignment.Left | GenericAlignment.Top,
            NativeControlPainterFlags flags = 0,
            TextEllipsizeType ellipsizeMode = TextEllipsizeType.End)
        {
        }

        public string GetVersion()
        {
            return string.Empty;
        }
    }
}
