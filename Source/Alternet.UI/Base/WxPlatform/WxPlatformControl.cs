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
        public override bool GetTabStop(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).TabStop;
        }

        public override void SetTabStop(IControl control, bool value)
        {
            ((UI.Native.Control)control.NativeControl).TabStop = value;
        }

        public override bool GetIsBold(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).IsBold;
        }

        public override void SetIsBold(IControl control, bool value)
        {
            ((UI.Native.Control)control.NativeControl).IsBold = value;
        }

        public override Font? GetFont(IControl control)
        {
            var font = ((UI.Native.Control)control.NativeControl).Font;
            return Font.FromInternal(font);
        }

        public override void SetFont(IControl control, Font? value)
        {
            ((UI.Native.Control)control.NativeControl).Font = (UI.Native.Font?)value?.NativeObject;
        }

        public override Thickness GetIntrinsicLayoutPadding(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).IntrinsicLayoutPadding;
        }

        public override Thickness GetIntrinsicPreferredSizePadding(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).IntrinsicPreferredSizePadding;
        }

        public override Color GetForegroundColor(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).ForegroundColor;
        }

        public override void SetForegroundColor(IControl control, Color value)
        {
            ((UI.Native.Control)control.NativeControl).ForegroundColor = value;
        }

        public override Color GetBackgroundColor(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).BackgroundColor;
        }

        public override void SetBackgroundColor(IControl control, Color value)
        {
            ((UI.Native.Control)control.NativeControl).BackgroundColor = value;
        }

        public override SizeD GetMinimumSize(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).MinimumSize;
        }

        public override void SetMinimumSize(IControl control, SizeD value)
        {
            ((UI.Native.Control)control.NativeControl).MinimumSize = value;
        }

        public override SizeD GetMaximumSize(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).MaximumSize;
        }

        public override void SetMaximumSize(IControl control, SizeD value)
        {
            ((UI.Native.Control)control.NativeControl).MaximumSize = value;
        }

        public override bool GetUserPaint(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).UserPaint;
        }

        public override void SetUserPaint(IControl control, bool value)
        {
            ((UI.Native.Control)control.NativeControl).UserPaint = value;
        }

        public override bool IsHandleCreated(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).IsHandleCreated;
        }

        public override bool GetVisible(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).Visible;
        }

        public override void SetVisible(IControl control, bool value)
        {
            ((UI.Native.Control)control.NativeControl).Visible = value;
        }

        public override void SetToolTip(IControl control, string? value)
        {
            ((UI.Native.Control)control.NativeControl).ToolTip = value;
        }

        public override RectD GetBounds(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).Bounds;
        }

        public override void SetBounds(IControl control, RectD value)
        {
            ((UI.Native.Control)control.NativeControl).Bounds = value;
        }

        public override bool CanAcceptFocus(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).CanAcceptFocus;
        }

        public override bool IsFocusable(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).IsFocusable;
        }

        public override bool IsMouseOver(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).IsMouseOver;
        }

        public override bool IsMouseCaptured(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).IsMouseCaptured;
        }

        public override SizeD GetClientSize(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).ClientSize;
        }

        public override void SetClientSize(IControl control, SizeD value)
        {
            ((UI.Native.Control)control.NativeControl).ClientSize = value;
        }

        public override void SetCursor(IControl control, Cursor? value)
        {
            if (value is null)
                ((UI.Native.Control)control.NativeControl).SetCursor(default);
            else
                ((UI.Native.Control)control.NativeControl).SetCursor((IntPtr)value.NativeObject);
        }

        public override bool GetIsScrollable(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).IsScrollable;
        }

        public override void SetIsScrollable(IControl control, bool value)
        {
            ((UI.Native.Control)control.NativeControl).IsScrollable = value;
        }
    }
}
