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
    public class WxControlPainterHandler : DisposableObject, IControlPainterHandler
    {
        static WxControlPainterHandler()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WxControlPainterHandler"/> class.
        /// </summary>
        public WxControlPainterHandler()
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

        /// <summary>
        /// Defines possible controls parts to draw.
        /// </summary>
        public enum ControlPartKind
        {
            /// <summary>
            /// Item is 'TreeItemButton'.
            /// </summary>
            TreeItemButton,

            /// <summary>
            /// Item is 'SplitterBorder'.
            /// </summary>
            SplitterBorder,

            /// <summary>
            /// Item is 'ComboBoxDropButton'.
            /// </summary>
            ComboBoxDropButton,

            /// <summary>
            /// Item is 'DropArrow'.
            /// </summary>
            DropArrow,

            /// <summary>
            /// Item is 'CheckBox'.
            /// </summary>
            CheckBox,

            /// <summary>
            /// Item is 'CheckMark'.
            /// </summary>
            CheckMark,

            /// <summary>
            /// Item is 'PushButton'.
            /// </summary>
            PushButton,

            /// <summary>
            /// Item is 'CollapseButton'.
            /// </summary>
            CollapseButton,

            /// <summary>
            /// Item is .
            /// </summary>
            ItemSelectionRect,

            /// <summary>
            /// Item is 'FocusRect'.
            /// </summary>
            FocusRect,

            /// <summary>
            /// Item is 'Choice'.
            /// </summary>
            Choice,

            /// <summary>
            /// Item is 'ComboBox'.
            /// </summary>
            ComboBox,

            /// <summary>
            /// Item is 'TextCtrl'.
            /// </summary>
            TextCtrl,

            /// <summary>
            /// Item is 'RadioBitmap'.
            /// </summary>
            RadioBitmap,

            /// <summary>
            /// Item is 'HeaderButton'.
            /// </summary>
            HeaderButton,

            /// <summary>
            /// Item is 'HeaderButtonContents'.
            /// </summary>
            HeaderButtonContents,

            /// <summary>
            /// Item is 'Gauge'.
            /// </summary>
            Gauge,

            /// <summary>
            /// Item is 'ItemText'.
            /// </summary>
            ItemText,

            /// <summary>
            /// Item is 'SplitterSash'.
            /// </summary>
            SplitterSash,
        }

        /// <inheritdoc/>
        public SizeD GetCheckBoxSize(
            AbstractControl control,
            CheckState checkState,
            VisualControlState controlState)
        {
            var flags = Convert(checkState, controlState);
            return GetCheckBoxSize(control, flags);
        }

        /// <inheritdoc/>
        public void DrawCheckBox(
            AbstractControl control,
            Graphics canvas,
            RectD rect,
            CheckState checkState,
            VisualControlState controlState)
        {
            var flags = Convert(checkState, controlState);
            DrawCheckBox(control, canvas, rect, flags);
        }

        /// <summary>
        /// Converts <see cref="CheckState"/> and <see cref="VisualControlState"/>
        /// to <see cref="DrawFlags"/>.
        /// </summary>
        /// <param name="checkState"></param>
        /// <param name="controlState"></param>
        /// <returns></returns>
        public virtual DrawFlags Convert(CheckState checkState, VisualControlState controlState)
        {
            DrawFlags flags;
            switch (checkState)
            {
                case CheckState.Checked:
                    flags = DrawFlags.Checked;
                    break;
                case CheckState.Indeterminate:
                    flags = DrawFlags.Undetermined;
                    break;
                default:
                    flags = 0;
                    break;
            }

            switch (controlState)
            {
                case VisualControlState.Hovered:
                    flags |= DrawFlags.Current;
                    break;
                case VisualControlState.Disabled:
                    flags |= DrawFlags.Disabled;
                    break;
            }

            return flags;
        }

        /// <summary>
        /// Draws the header control button (used, for example, by <see cref="ListView"/> like
        /// controls).
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect">Rectangle in which control is painted.</param>
        /// <param name="flags">Drawing flags.</param>
        /// <param name="sortArrow">Type of the sort icon.</param>
        /// <param name="headerButtonParams">Button parameters.</param>
        /// <returns>
        /// The optimal width to contain the unabbreviated label text or bitmap,
        /// the sort arrow if present, and internal margins.
        /// </returns>
        /// <remarks>
        /// Depending on platforms the flags parameter may support the
        /// <see cref="DrawFlags.Selected"/>, <see cref="DrawFlags.Disabled"/>
        /// and <see cref="DrawFlags.Current"/>.
        /// </remarks>
        public int DrawHeaderButton(
            AbstractControl control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0,
            HeaderSortIconType sortArrow = HeaderSortIconType.None,
            HeaderButtonParams? headerButtonParams = null)
        {
            return Alternet.UI.Native.WxOtherFactory.RendererDrawHeaderButton(
                default,
                WxApplicationHandler.WxWidget(control),
                (UI.Native.DrawingContext)dc.NativeObject,
                control.PixelFromDip(rect),
                (int)flags,
                (int)sortArrow,
                default);
        }

        /// <summary>
        /// Draws the contents of a header control button (label, sort arrows, etc.).
        /// Normally only called by <see cref="DrawHeaderButton"/>.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect">Rectangle in which control is painted.</param>
        /// <param name="flags">Drawing flags.</param>
        /// <param name="sortArrow">Type of the sort icon.</param>
        /// <param name="headerButtonParams">Button parameters.</param>
        /// <returns>
        /// The optimal width to contain the unabbreviated label text
        /// or bitmap, the sort arrow if present, and internal margins.
        /// </returns>
        /// <remarks>
        /// The flags parameter may support the
        /// <see cref="DrawFlags.Selected"/>, <see cref="DrawFlags.Disabled"/>
        /// and <see cref="DrawFlags.Current"/>.
        /// </remarks>
        public int DrawHeaderButtonContents(
            AbstractControl control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0,
            HeaderSortIconType sortArrow = HeaderSortIconType.None,
            HeaderButtonParams? headerButtonParams = null)
        {
            return Alternet.UI.Native.WxOtherFactory.RendererDrawHeaderButtonContents(
                default,
                WxApplicationHandler.WxWidget(control),
                (UI.Native.DrawingContext)dc.NativeObject,
                control.PixelFromDip(rect),
                (int)flags,
                (int)sortArrow,
                default);
        }

        /// <summary>
        /// Draws the expanded/collapsed icon for a tree control item.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect">Rectangle in which control is painted.</param>
        /// <param name="flags">Drawing flags.</param>
        public void DrawTreeItemButton(
            AbstractControl control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawTreeItemButton(
                default,
                WxApplicationHandler.WxWidget(control),
                (UI.Native.DrawingContext)dc.NativeObject,
                control.PixelFromDip(rect),
                (int)flags);
        }

        /// <summary>
        /// Draws the border for sash window: this border must be such that the sash
        /// drawn by <see cref="DrawSplitterSash"/> blends into it well.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect">Rectangle in which control is painted.</param>
        /// <param name="flags">Drawing flags.</param>
        public void DrawSplitterBorder(
            AbstractControl control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawSplitterBorder(
                default,
                WxApplicationHandler.WxWidget(control),
                (UI.Native.DrawingContext)dc.NativeObject,
                control.PixelFromDip(rect),
                (int)flags);
        }

        /// <summary>
        /// Draws a combo box dropdown button.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect">Rectangle in which control is painted.</param>
        /// <param name="flags">Drawing flags.</param>
        /// <remarks>
        /// The flags parameter may support the
        /// <see cref="DrawFlags.Pressed"/> and <see cref="DrawFlags.Current"/>.
        /// </remarks>
        public void DrawComboBoxDropButton(
            AbstractControl control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawComboBoxDropButton(
                default,
                WxApplicationHandler.WxWidget(control),
                (UI.Native.DrawingContext)dc.NativeObject,
                control.PixelFromDip(rect),
                (int)flags);
        }

        /// <summary>
        /// Draws a dropdown arrow.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect">Rectangle in which control is painted.</param>
        /// <param name="flags">Drawing flags.</param>
        /// <remarks>
        /// The flags parameter may support the
        /// <see cref="DrawFlags.Pressed"/> and <see cref="DrawFlags.Current"/>.
        /// </remarks>
        public void DrawDropArrow(
            AbstractControl control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawDropArrow(
                default,
                WxApplicationHandler.WxWidget(control),
                (UI.Native.DrawingContext)dc.NativeObject,
                control.PixelFromDip(rect),
                (int)flags);
        }

        /// <summary>
        /// Draws check button.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect">Rectangle in which control is painted.</param>
        /// <param name="flags">Drawing flags.</param>
        /// <remarks>
        /// The flags parameter may support the
        /// <see cref="DrawFlags.Checked"/>, <see cref="DrawFlags.Undetermined"/>
        /// and <see cref="DrawFlags.Current"/>.
        /// </remarks>
        public void DrawCheckBox(
            AbstractControl control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            rect.Offset(dc.TransformDXDY);

            Alternet.UI.Native.WxOtherFactory.RendererDrawCheckBox(
                default,
                WxApplicationHandler.WxWidget(control),
                (UI.Native.DrawingContext)dc.NativeObject,
                control.PixelFromDip(rect),
                (int)flags);
        }

        /// <summary>
        /// Draws check mark.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect">Rectangle in which control is painted.</param>
        /// <param name="flags">Drawing flags.</param>
        /// <remarks>
        /// The flags parameter may support the
        /// <see cref="DrawFlags.Disabled"/>.
        /// </remarks>
        public void DrawCheckMark(
            AbstractControl control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawCheckMark(
                default,
                WxApplicationHandler.WxWidget(control),
                (UI.Native.DrawingContext)dc.NativeObject,
                control.PixelFromDip(rect),
                (int)flags);
        }

        /// <summary>
        /// Draws blank button.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect">Rectangle in which control is painted.</param>
        /// <param name="flags">Drawing flags.</param>
        /// <remarks>
        /// The flags parameter may support the
        /// <see cref="DrawFlags.Pressed"/>, <see cref="DrawFlags.IsDefault"/>
        /// and <see cref="DrawFlags.Current"/>.
        /// </remarks>
        public void DrawPushButton(
            AbstractControl control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawPushButton(
                default,
                WxApplicationHandler.WxWidget(control),
                (UI.Native.DrawingContext)dc.NativeObject,
                control.PixelFromDip(rect),
                (int)flags);
        }

        /// <summary>
        /// Draws collapse button.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect">Rectangle in which control is painted.</param>
        /// <param name="flags">Drawing flags.</param>
        /// <remarks>
        /// The flags parameter may support the
        /// <see cref="DrawFlags.Checked"/>, <see cref="DrawFlags.Undetermined"/>
        /// and <see cref="DrawFlags.Current"/>.
        /// </remarks>
        public void DrawCollapseButton(
            AbstractControl control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawCollapseButton(
                default,
                WxApplicationHandler.WxWidget(control),
                (UI.Native.DrawingContext)dc.NativeObject,
                control.PixelFromDip(rect),
                (int)flags);
        }

        /// <summary>
        /// Draws rectangle indicating that an item in e.g. a list control
        /// has been selected or focused.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect">Rectangle in which control is painted.</param>
        /// <param name="flags">Drawing flags.</param>
        /// <remarks>
        /// The flags parameter may support the
        /// <see cref="DrawFlags.Selected"/> (item is selected, e.g. draw background),
        /// <see cref="DrawFlags.Focused"/> (the whole control has focus,
        /// e.g. blue background vs. grey otherwise)
        /// and <see cref="DrawFlags.Current"/> (item is the current item, e.g. dotted border).
        /// </remarks>
        public void DrawItemSelectionRect(
            AbstractControl control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawItemSelectionRect(
                default,
                WxApplicationHandler.WxWidget(control),
                (UI.Native.DrawingContext)dc.NativeObject,
                control.PixelFromDip(rect),
                (int)flags);
        }

        /// <summary>
        /// Draws the focus rectangle around the label contained in the given rect.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect">Rectangle in which control is painted.</param>
        /// <param name="flags">Drawing flags.</param>
        /// <remarks>
        /// Only <see cref="DrawFlags.Selected"/> makes sense in flags here.
        /// </remarks>
        public void DrawFocusRect(
            AbstractControl control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawFocusRect(
                default,
                WxApplicationHandler.WxWidget(control),
                (UI.Native.DrawingContext)dc.NativeObject,
                control.PixelFromDip(rect),
                (int)flags);
        }

        /// <summary>
        /// Draws a native choice control.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect">Rectangle in which control is painted.</param>
        /// <param name="flags">Drawing flags.</param>
        public void DrawChoice(
            AbstractControl control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawChoice(
                default,
                WxApplicationHandler.WxWidget(control),
                (UI.Native.DrawingContext)dc.NativeObject,
                control.PixelFromDip(rect),
                (int)flags);
        }

        /// <summary>
        /// Draws a native <see cref="ComboBox"/> frame.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect">Rectangle in which control is painted.</param>
        /// <param name="flags">Drawing flags.</param>
        public void DrawComboBox(
            AbstractControl control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawComboBox(
                default,
                WxApplicationHandler.WxWidget(control),
                (UI.Native.DrawingContext)dc.NativeObject,
                control.PixelFromDip(rect),
                (int)flags);
        }

        /// <summary>
        /// Draws a native <see cref="TextBox"/> frame.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect">Rectangle in which control is painted.</param>
        /// <param name="flags">Drawing flags.</param>
        public void DrawTextCtrl(
            AbstractControl control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawTextCtrl(
                default,
                WxApplicationHandler.WxWidget(control),
                (UI.Native.DrawingContext)dc.NativeObject,
                control.PixelFromDip(rect),
                (int)flags);
        }

        /// <summary>
        /// Draws a native <see cref="RadioButton"/> bitmap.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect">Rectangle in which control is painted.</param>
        /// <param name="flags">Drawing flags.</param>
        public void DrawRadioBitmap(
            AbstractControl control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawRadioBitmap(
                default,
                WxApplicationHandler.WxWidget(control),
                (UI.Native.DrawingContext)dc.NativeObject,
                control.PixelFromDip(rect),
                (int)flags);
        }

        /// <summary>
        /// Draws a gauge with native style.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect">Rectangle in which control is painted.</param>
        /// <param name="flags">Drawing flags.</param>
        /// <param name="value">Current value.</param>
        /// <param name="max">Maximal value.</param>
        /// <remarks>
        /// <see cref="DrawFlags.Special"/> flag must be used for drawing vertical gauges.
        /// </remarks>
        public void DrawGauge(
            AbstractControl control,
            Graphics dc,
            RectD rect,
            int value,
            int max,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawGauge(
                default,
                WxApplicationHandler.WxWidget(control),
                (UI.Native.DrawingContext)dc.NativeObject,
                control.PixelFromDip(rect),
                value,
                max,
                (int)flags);
        }

        /// <summary>
        /// Draws text using the appropriate color for normal and selected states.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect">Rectangle in which control is painted.</param>
        /// <param name="flags">Drawing flags.</param>
        /// <param name="text">Text to draw.</param>
        /// <param name="align">Text alignment.</param>
        /// <param name="ellipsizeMode">Text ellipsize mode.</param>
        public void DrawItemText(
            AbstractControl control,
            Graphics dc,
            string text,
            RectD rect,
            GenericAlignment align = GenericAlignment.Left | GenericAlignment.Top,
            DrawFlags flags = 0,
            TextEllipsisType ellipsizeMode = TextEllipsisType.End)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawItemText(
                default,
                WxApplicationHandler.WxWidget(control),
                (UI.Native.DrawingContext)dc.NativeObject,
                text,
                control.PixelFromDip(rect),
                (int)align,
                (int)flags,
                (int)ellipsizeMode);
        }

        /// <summary>
        /// Returns the default size of a check box in dips.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <param name="flags">Drawing flags.</param>
        /// <returns></returns>
        /// <remarks>
        /// The only acceptable flag is <see cref="DrawFlags.Cell"/> which means that just the
        /// size of the checkbox itself is returned, without any margins that are
        /// included by default.
        /// </remarks>
        public SizeD GetCheckBoxSize(AbstractControl control, DrawFlags flags = 0)
        {
            var result = Alternet.UI.Native.WxOtherFactory.RendererGetCheckBoxSize(
                default,
                WxApplicationHandler.WxWidget(control),
                (int)flags);
            return control.PixelToDip(result);
        }

        /// <inheritdoc/>
        public SizeD GetCheckMarkSize(AbstractControl control)
        {
            var result = Alternet.UI.Native.WxOtherFactory.RendererGetCheckMarkSize(
                default,
                WxApplicationHandler.WxWidget(control));
            return control.PixelToDip(result);
        }

        /// <inheritdoc/>
        public SizeD GetExpanderSize(AbstractControl control)
        {
            var result = Alternet.UI.Native.WxOtherFactory.RendererGetExpanderSize(
                default,
                WxApplicationHandler.WxWidget(control));
            return control.PixelToDip(result);
        }

        /// <inheritdoc/>
        public double GetHeaderButtonHeight(AbstractControl control)
        {
            var result = Alternet.UI.Native.WxOtherFactory.RendererGetHeaderButtonHeight(
                default,
                WxApplicationHandler.WxWidget(control));
            return control.PixelToDip(result);
        }

        /// <inheritdoc/>
        public double GetHeaderButtonMargin(AbstractControl control)
        {
            var result = Alternet.UI.Native.WxOtherFactory.RendererGetHeaderButtonMargin(
                default,
                WxApplicationHandler.WxWidget(control));
            return control.PixelToDip(result);
        }

        /// <inheritdoc/>
        public SizeD GetCollapseButtonSize(AbstractControl control, Graphics dc)
        {
            var result = Alternet.UI.Native.WxOtherFactory.RendererGetCollapseButtonSize(
                default,
                WxApplicationHandler.WxWidget(control),
                (UI.Native.DrawingContext)dc.NativeObject);
            return control.PixelToDip(result);
        }

        /// <summary>
        /// Draw a (vertical) splitter sash.
        /// </summary>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <param name="dc">Drawing context.</param>
        /// <param name="flags">Drawing flags.</param>
        /// <param name="size">Splitter sash size.</param>
        /// <param name="position">Splitter position.</param>
        /// <param name="orientation">Defines whether the sash should be vertical or
        /// horizontal and how the position should be interpreted</param>
        public void DrawSplitterSash(
            AbstractControl control,
            Graphics dc,
            SizeD size,
            double position,
            GenericOrientation orientation,
            DrawFlags flags = 0)
        {
            Alternet.UI.Native.WxOtherFactory.RendererDrawSplitterSash(
                default,
                WxApplicationHandler.WxWidget(control),
                (UI.Native.DrawingContext)dc.NativeObject,
                control.PixelFromDip(size),
                control.PixelFromDip(position),
                (int)orientation,
                (int)flags);
        }

        /// <summary>
        /// Gets version of the renderer.
        /// </summary>
        public string GetVersion()
        {
            return Alternet.UI.Native.WxOtherFactory.RendererGetVersion(default);
        }

        /// <summary>
        /// Draws item specified with <paramref name="kind"/>.
        /// </summary>
        /// <param name="kind">Kind of the control part.</param>
        /// <param name="control">Control in which drawing will be performed.</param>
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect">Rectangle in which control is painted.</param>
        /// <param name="flags">Drawing flags.</param>
        /// <param name="prm">Item parameters.</param>
        public int DrawItem(
            ControlPartKind kind,
            AbstractControl control,
            Graphics dc,
            RectD rect,
            DrawFlags flags = 0,
            DrawItemParams? prm = null)
        {
            switch (kind)
            {
                case ControlPartKind.TreeItemButton:
                    DrawTreeItemButton(control, dc, rect, flags);
                    return 0;
                case ControlPartKind.SplitterBorder:
                    DrawSplitterBorder(control, dc, rect, flags);
                    return 0;
                case ControlPartKind.ComboBoxDropButton:
                    DrawComboBoxDropButton(control, dc, rect, flags);
                    return 0;
                case ControlPartKind.DropArrow:
                    DrawDropArrow(control, dc, rect, flags);
                    return 0;
                case ControlPartKind.CheckBox:
                    DrawCheckBox(control, dc, rect, flags);
                    return 0;
                case ControlPartKind.CheckMark:
                    DrawCheckMark(control, dc, rect, flags);
                    return 0;
                case ControlPartKind.PushButton:
                    DrawPushButton(control, dc, rect, flags);
                    return 0;
                case ControlPartKind.CollapseButton:
                    DrawCollapseButton(control, dc, rect, flags);
                    return 0;
                case ControlPartKind.ItemSelectionRect:
                    DrawItemSelectionRect(control, dc, rect, flags);
                    return 0;
                case ControlPartKind.FocusRect:
                    DrawFocusRect(control, dc, rect, flags);
                    return 0;
                case ControlPartKind.Choice:
                    DrawChoice(control, dc, rect, flags);
                    return 0;
                case ControlPartKind.ComboBox:
                    DrawComboBox(control, dc, rect, flags);
                    return 0;
                case ControlPartKind.TextCtrl:
                    DrawTextCtrl(control, dc, rect, flags);
                    return 0;
                case ControlPartKind.RadioBitmap:
                    DrawRadioBitmap(control, dc, rect, flags);
                    return 0;
                case ControlPartKind.HeaderButton:
                    return DrawHeaderButton(
                        control,
                        dc,
                        rect,
                        flags,
                        prm?.SortArrow ?? HeaderSortIconType.None,
                        prm?.HeaderButtonParams);
                case ControlPartKind.HeaderButtonContents:
                    return DrawHeaderButtonContents(
                        control,
                        dc,
                        rect,
                        flags,
                        prm?.SortArrow ?? HeaderSortIconType.None,
                        prm?.HeaderButtonParams);
                case ControlPartKind.Gauge:
                    DrawGauge(
                        control,
                        dc,
                        rect,
                        prm?.GaugeValue ?? 0,
                        prm?.GaugeMaxValue ?? 0,
                        flags);
                    return 0;
                case ControlPartKind.ItemText:
                    DrawItemText(
                        control,
                        dc,
                        prm?.Text ?? string.Empty,
                        rect,
                        prm?.TextAlignment ?? GenericAlignment.Left | GenericAlignment.Top,
                        flags,
                        prm?.TextEllipsizeMode ?? TextEllipsisType.End);
                    return 0;
                case ControlPartKind.SplitterSash:
                    DrawSplitterSash(
                        control,
                        dc,
                        prm?.SashSize ?? 0,
                        prm?.SashPosition ?? 0,
                        prm?.SashOrientation ?? GenericOrientation.Vertical,
                        flags);
                    return 0;
                default:
                    return 0;
            }
        }

        /// <inheritdoc/>
        public void DrawPushButton(
            AbstractControl control,
            Graphics canvas,
            RectD rect,
            VisualControlState controlState)
        {
            var flags = Convert(CheckState.Unchecked, controlState);
            DrawPushButton(control, canvas, rect, flags);
        }

        /// <inheritdoc/>
        public void DrawRadioButton(AbstractControl control,
            Graphics canvas,
            RectD rect,
            bool isChecked,
            VisualControlState controlState)
        {
            CheckState state = isChecked ? CheckState.Checked : CheckState.Unchecked;
            var flags = Convert(state, controlState);
            DrawRadioBitmap(control, canvas, rect, flags);
        }

        /// <summary>
        /// Defines parameters for the <see cref="DrawHeaderButton"/>.
        /// </summary>
        public class HeaderButtonParams
        {
            /// <summary>
            /// Gets or sets arrow color.
            /// </summary>
            public Color? ArrowColor;

            /// <summary>
            /// Gets or sets selection color.
            /// </summary>
            public Color? SelectionColor;

            /// <summary>
            /// Gets or sets label text.
            /// </summary>
            public string LabelText = string.Empty;

            /// <summary>
            /// Gets or sets label font.
            /// </summary>
            public Font? LabelFont;

            /// <summary>
            /// Gets or sets label color.
            /// </summary>
            public Color? LabelColor;

            /// <summary>
            /// Gets or sets label image.
            /// </summary>
            public Bitmap? LabelBitmap;

            /// <summary>
            /// Gets or sets label alignment.
            /// </summary>
            public GenericAlignment LabelAlignment;
        }

        /// <summary>
        /// Defines additional parameters for the <see cref="DrawItem"/> method.
        /// </summary>
        public class DrawItemParams
        {
            /// <summary>
            /// Gets or sets text alignment for the <see cref="DrawItemText"/>.
            /// </summary>
            public GenericAlignment TextAlignment = GenericAlignment.Left | GenericAlignment.Top;

            /// <summary>
            /// Gets or sets text for the <see cref="DrawItemText"/>.
            /// </summary>
            public string? Text = null;

            /// <summary>
            /// Gets or sets text ellipsize mode for the <see cref="DrawItemText"/>.
            /// </summary>
            public TextEllipsisType TextEllipsizeMode = TextEllipsisType.End;

            /// <summary>
            /// Gets or sets value for the <see cref="DrawGauge"/>.
            /// </summary>
            public int GaugeValue;

            /// <summary>
            /// Gets or sets max value for the <see cref="DrawGauge"/>.
            /// </summary>
            public int GaugeMaxValue;

            /// <summary>
            /// Gets or sets parameters for the <see cref="DrawHeaderButton"/>.
            /// </summary>
            public HeaderButtonParams? HeaderButtonParams;

            /// <summary>
            /// Gets or sets sort arrow for the <see cref="DrawHeaderButton"/>.
            /// </summary>
            public HeaderSortIconType SortArrow = HeaderSortIconType.None;

            /// <summary>
            /// Gets or sets sash size for the <see cref="DrawSplitterSash"/>.
            /// </summary>
            public SizeD SashSize;

            /// <summary>
            /// Gets or sets sash position for the <see cref="DrawSplitterSash"/>.
            /// </summary>
            public double SashPosition;

            /// <summary>
            /// Gets or sets sash orientation for the <see cref="DrawSplitterSash"/>.
            /// </summary>
            public GenericOrientation SashOrientation;
        }
    }
}