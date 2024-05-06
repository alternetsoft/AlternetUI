using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WxPlatformControl : NativeControl
    {
        /// <inheritdoc/>
        public override bool GetBindScrollEvents(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).BindScrollEvents;
        }

        /// <inheritdoc/>
        public override void SetBindScrollEvents(IControl control, bool value)
        {
            ((UI.Native.Control)control.NativeControl).BindScrollEvents = value;
        }

        /// <inheritdoc/>
        public override bool GetAcceptsFocus(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).AcceptsFocus;
        }

        /// <inheritdoc/>
        public override void SetAcceptsFocus(IControl control, bool value)
        {
            ((UI.Native.Control)control.NativeControl).AcceptsFocus = value;
        }

        /// <inheritdoc/>
        public override bool GetAcceptsFocusFromKeyboard(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).AcceptsFocusFromKeyboard;
        }

        /// <inheritdoc/>
        public override void SetAcceptsFocusFromKeyboard(IControl control, bool value)
        {
            ((UI.Native.Control)control.NativeControl).AcceptsFocusFromKeyboard = value;
        }

        /// <inheritdoc/>
        public override bool GetAcceptsFocusRecursively(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).AcceptsFocusRecursively;
        }

        /// <inheritdoc/>
        public override void SetAcceptsFocusRecursively(IControl control, bool value)
        {
            ((UI.Native.Control)control.NativeControl).AcceptsFocusRecursively = value;
        }

        /// <inheritdoc/>
        public override bool GetAcceptsFocusAll(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).AcceptsFocusAll;
        }

        /// <inheritdoc/>
        public override void SetAcceptsFocusAll(IControl control, bool value)
        {
            ((UI.Native.Control)control.NativeControl).AcceptsFocusAll = value;
        }

        /// <inheritdoc/>
        public override bool GetProcessIdle(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).ProcessIdle;
        }

        /// <inheritdoc/>
        public override void SetProcessIdle(IControl control, bool value)
        {
            ((UI.Native.Control)control.NativeControl).ProcessIdle = value;
        }

        /// <inheritdoc/>
        public override bool IsFocused(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).IsFocused;
        }

        /// <inheritdoc/>
        public override bool GetAllowDrop(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).AllowDrop;
        }

        /// <inheritdoc/>
        public override void SetAllowDrop(IControl control, bool value)
        {
            ((UI.Native.Control)control.NativeControl).AllowDrop = value;
        }

        /// <inheritdoc/>
        public override ControlBackgroundStyle GetBackgroundStyle(IControl control)
        {
            var result = ((UI.Native.Control)control.NativeControl).GetBackgroundStyle();
            return (ControlBackgroundStyle?)result ?? ControlBackgroundStyle.System;
        }

        /// <inheritdoc/>
        public override void SetBackgroundStyle(IControl control, ControlBackgroundStyle value)
        {
            ((UI.Native.Control)control.NativeControl).SetBackgroundStyle((int)value);
        }

        /// <inheritdoc/>
        public override bool GetTabStop(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).TabStop;
        }

        /// <inheritdoc/>
        public override void SetTabStop(IControl control, bool value)
        {
            ((UI.Native.Control)control.NativeControl).TabStop = value;
        }

        /// <inheritdoc/>
        public override bool GetIsBold(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).IsBold;
        }

        /// <inheritdoc/>
        public override void SetIsBold(IControl control, bool value)
        {
            ((UI.Native.Control)control.NativeControl).IsBold = value;
        }

        /// <inheritdoc/>
        public override Font? GetFont(IControl control)
        {
            var font = ((UI.Native.Control)control.NativeControl).Font;
            return Font.FromInternal(font);
        }

        /// <inheritdoc/>
        public override void SetFont(IControl control, Font? value)
        {
            ((UI.Native.Control)control.NativeControl).Font = (UI.Native.Font?)value?.NativeObject;
        }

        /// <inheritdoc/>
        public override Thickness GetIntrinsicLayoutPadding(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).IntrinsicLayoutPadding;
        }

        /// <inheritdoc/>
        public override Thickness GetIntrinsicPreferredSizePadding(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).IntrinsicPreferredSizePadding;
        }

        /// <inheritdoc/>
        public override Color GetForegroundColor(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).ForegroundColor;
        }

        /// <inheritdoc/>
        public override void SetForegroundColor(IControl control, Color value)
        {
            ((UI.Native.Control)control.NativeControl).ForegroundColor = value;
        }

        /// <inheritdoc/>
        public override Color GetBackgroundColor(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).BackgroundColor;
        }

        /// <inheritdoc/>
        public override void SetBackgroundColor(IControl control, Color value)
        {
            ((UI.Native.Control)control.NativeControl).BackgroundColor = value;
        }

        /// <inheritdoc/>
        public override SizeD GetMinimumSize(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).MinimumSize;
        }

        /// <inheritdoc/>
        public override void SetMinimumSize(IControl control, SizeD value)
        {
            ((UI.Native.Control)control.NativeControl).MinimumSize = value;
        }

        /// <inheritdoc/>
        public override SizeD GetMaximumSize(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).MaximumSize;
        }

        /// <inheritdoc/>
        public override void SetMaximumSize(IControl control, SizeD value)
        {
            ((UI.Native.Control)control.NativeControl).MaximumSize = value;
        }

        /// <inheritdoc/>
        public override bool GetUserPaint(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).UserPaint;
        }

        /// <inheritdoc/>
        public override void SetUserPaint(IControl control, bool value)
        {
            ((UI.Native.Control)control.NativeControl).UserPaint = value;
        }

        /// <inheritdoc/>
        public override bool IsHandleCreated(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).IsHandleCreated;
        }

        /// <inheritdoc/>
        public override bool GetVisible(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).Visible;
        }

        /// <inheritdoc/>
        public override void SetVisible(IControl control, bool value)
        {
            ((UI.Native.Control)control.NativeControl).Visible = value;
        }

        /// <inheritdoc/>
        public override void SetToolTip(IControl control, string? value)
        {
            ((UI.Native.Control)control.NativeControl).ToolTip = value;
        }

        /// <inheritdoc/>
        public override RectD GetBounds(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).Bounds;
        }

        /// <inheritdoc/>
        public override void SetBounds(IControl control, RectD value)
        {
            ((UI.Native.Control)control.NativeControl).Bounds = value;
        }

        /// <inheritdoc/>
        public override bool CanAcceptFocus(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).CanAcceptFocus;
        }

        /// <inheritdoc/>
        public override bool IsFocusable(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).IsFocusable;
        }

        /// <inheritdoc/>
        public override bool IsMouseOver(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).IsMouseOver;
        }

        /// <inheritdoc/>
        public override bool IsMouseCaptured(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).IsMouseCaptured;
        }

        /// <inheritdoc/>
        public override SizeD GetClientSize(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).ClientSize;
        }

        /// <inheritdoc/>
        public override void SetClientSize(IControl control, SizeD value)
        {
            ((UI.Native.Control)control.NativeControl).ClientSize = value;
        }

        /// <inheritdoc/>
        public override void SetCursor(IControl control, Cursor? value)
        {
            if (value is null)
                ((UI.Native.Control)control.NativeControl).SetCursor(default);
            else
                ((UI.Native.Control)control.NativeControl).SetCursor((IntPtr)value.NativeObject);
        }

        /// <inheritdoc/>
        public override bool GetIsScrollable(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).IsScrollable;
        }

        /// <inheritdoc/>
        public override void SetIsScrollable(IControl control, bool value)
        {
            ((UI.Native.Control)control.NativeControl).IsScrollable = value;
        }
    }
}
