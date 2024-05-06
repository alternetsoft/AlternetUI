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
        public static IntPtr WxWidget(IControl? control)
        {
            if (control is null)
                return default;
            return ((UI.Native.Control)control.NativeControl).WxWidget;
        }

        /// <inheritdoc/>
        public override bool BeginRepositioningChildren(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).BeginRepositioningChildren();
        }

        /// <inheritdoc/>
        public override void AlwaysShowScrollbars(IControl control, bool hflag = true, bool vflag = true)
        {
            ((UI.Native.Control)control.NativeControl).AlwaysShowScrollbars(hflag, vflag);
        }

        /// <inheritdoc/>
        public override void EndRepositioningChildren(IControl control)
        {
            ((UI.Native.Control)control.NativeControl).EndRepositioningChildren();
        }

        /// <inheritdoc/>
        public override void SendMouseDownEvent(IControl control, int x, int y)
        {
            ((UI.Native.Control)control.NativeControl).SendMouseDownEvent(x, y);
        }

        /// <inheritdoc/>
        public override void SendMouseUpEvent(IControl control, int x, int y)
        {
            ((UI.Native.Control)control.NativeControl).SendMouseUpEvent(x, y);
        }

        /// <inheritdoc/>
        public override Color GetDefaultAttributesBgColor(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).GetDefaultAttributesBgColor();
        }

        /// <inheritdoc/>
        public override Color GetDefaultAttributesFgColor(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).GetDefaultAttributesFgColor();
        }

        /// <inheritdoc/>
        public override Font? GetDefaultAttributesFont(IControl control)
        {
            return Font.FromInternal(((UI.Native.Control)control.NativeControl).GetDefaultAttributesFont());
        }

        /// <inheritdoc/>
        public override bool GetProcessUIUpdates(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).ProcessUIUpdates;
        }

        /// <inheritdoc/>
        public override void SetProcessUIUpdates(IControl control, bool value)
        {
            ((UI.Native.Control)control.NativeControl).ProcessUIUpdates = value;
        }

        /// <inheritdoc/>
        public override void SetEnabled(IControl control, bool value)
        {
            ((UI.Native.Control)control.NativeControl).Enabled = value;
        }

        /// <inheritdoc/>
        public override void ResetBackgroundColor(IControl control)
        {
            ((UI.Native.Control)control.NativeControl).ResetBackgroundColor();
        }

        /// <inheritdoc/>
        public override void ResetForegroundColor(IControl control)
        {
            ((UI.Native.Control)control.NativeControl).ResetForegroundColor();
        }

        /// <inheritdoc/>
        public override int PixelFromDip(IControl control, double value)
        {
            var wxWidget = ((UI.Native.Control)control.NativeControl).WxWidget;
            return Native.Control.DrawingFromDip(value, wxWidget);
        }

        /// <inheritdoc/>
        public override void SetScrollBar(
            IControl control,
            bool isVertical,
            bool visible,
            int value,
            int largeChange,
            int maximum)
        {
            Native.ScrollBarOrientation orientation = isVertical
                ? Native.ScrollBarOrientation.Vertical : Native.ScrollBarOrientation.Horizontal;
            ((UI.Native.Control)control.NativeControl)
                .SetScrollBar(orientation, visible, value, largeChange, maximum);
        }

        /// <inheritdoc/>
        public override bool IsScrollBarVisible(IControl control, bool isVertical)
        {
            Native.ScrollBarOrientation orientation = isVertical
                ? Native.ScrollBarOrientation.Vertical : Native.ScrollBarOrientation.Horizontal;
            return ((UI.Native.Control)control.NativeControl).IsScrollBarVisible(orientation);
        }

        /// <inheritdoc/>
        public override int GetScrollBarValue(IControl control, bool isVertical)
        {
            Native.ScrollBarOrientation orientation = isVertical
                ? Native.ScrollBarOrientation.Vertical : Native.ScrollBarOrientation.Horizontal;
            return ((UI.Native.Control)control.NativeControl).GetScrollBarValue(orientation);
        }

        /// <inheritdoc/>
        public override int GetScrollBarLargeChange(IControl control, bool isVertical)
        {
            Native.ScrollBarOrientation orientation = isVertical
                ? Native.ScrollBarOrientation.Vertical : Native.ScrollBarOrientation.Horizontal;
            return ((UI.Native.Control)control.NativeControl).GetScrollBarLargeChange(orientation);
        }

        /// <inheritdoc/>
        public override int GetScrollBarMaximum(IControl control, bool isVertical)
        {
            Native.ScrollBarOrientation orientation = isVertical
                ? Native.ScrollBarOrientation.Vertical : Native.ScrollBarOrientation.Horizontal;
            return ((UI.Native.Control)control.NativeControl).GetScrollBarMaximum(orientation);
        }

        /// <inheritdoc/>
        public override double PixelToDip(IControl control, int value)
        {
            var wxWidget = ((UI.Native.Control)control.NativeControl).WxWidget;
            return Native.Control.DrawingToDip(value, wxWidget);
        }

        /// <inheritdoc/>
        public override double PixelFromDipF(IControl control, double value)
        {
            var wxWidget = ((UI.Native.Control)control.NativeControl).WxWidget;
            return Native.Control.DrawingFromDipF(value, wxWidget);
        }

        /// <inheritdoc/>
        public override RectI GetUpdateClientRectI(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).GetUpdateClientRect();
        }

        /// <inheritdoc/>
        public override double GetPixelScaleFactor(IControl control)
        {
            var wxWidget = ((UI.Native.Control)control.NativeControl).WxWidget;
            return Native.Control.DrawingDPIScaleFactor(wxWidget);
        }

        /// <inheritdoc/>
        public override void EndIgnoreRecreate(IControl control)
        {
            ((UI.Native.Control)control.NativeControl).EndIgnoreRecreate();
        }

        /// <inheritdoc/>
        public override void BeginIgnoreRecreate(IControl control)
        {
            ((UI.Native.Control)control.NativeControl).BeginIgnoreRecreate();
        }

        /// <inheritdoc/>
        public override SizeD GetDPI(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).GetDPI();
        }

        /// <inheritdoc/>
        public override bool IsTransparentBackgroundSupported(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).IsTransparentBackgroundSupported();
        }

        /// <inheritdoc/>
        public override void BeginUpdate(IControl control)
        {
            ((UI.Native.Control)control.NativeControl).BeginUpdate();
        }

        /// <inheritdoc/>
        public override void EndUpdate(IControl control)
        {
            ((UI.Native.Control)control.NativeControl).EndUpdate();
        }

        /// <inheritdoc/>
        public override void SetBounds(IControl control, RectD rect, SetBoundsFlags flags)
        {
            ((UI.Native.Control)control.NativeControl).SetBoundsEx(rect, (int)flags);
        }

        /// <inheritdoc/>
        public override void BeginInit(IControl control)
        {
            ((UI.Native.Control)control.NativeControl).BeginInit();
        }

        /// <inheritdoc/>
        public override void EndInit(IControl control)
        {
            ((UI.Native.Control)control.NativeControl).EndInit();
        }

        /// <inheritdoc/>
        public override bool SetFocus(IControl control)
        {
            return ((UI.Native.Control)control.NativeControl).SetFocus();
        }

        /// <inheritdoc/>
        public override void SaveScreenshot(IControl control, string fileName)
        {
            ((UI.Native.Control)control.NativeControl).SaveScreenshot(fileName);
        }

        /// <inheritdoc/>
        public override void FocusNextControl(IControl control, bool forward = true, bool nested = true)
        {
            ((UI.Native.Control)control.NativeControl).FocusNextControl(forward, nested);
        }

        /// <inheritdoc/>
        public override DragDropEffects DoDragDrop(
            IControl control,
            object data,
            DragDropEffects allowedEffects)
        {
            allowedEffects &= ~DragDropEffects.Scroll;
            return (DragDropEffects)((UI.Native.Control)control.NativeControl).DoDragDrop(
                UnmanagedDataObjectService.GetUnmanagedDataObject(data),
                (Native.DragDropEffects)allowedEffects);
        }

        /// <inheritdoc/>
        public override void RecreateWindow(IControl control)
        {
            ((UI.Native.Control)control.NativeControl).RecreateWindow();
        }

        /// <inheritdoc/>
        public override PointD ScreenToClient(IControl control, PointD point)
        {
            return ((UI.Native.Control)control.NativeControl).ScreenToClient(point);
        }

        /// <inheritdoc/>
        public override PointD ClientToScreen(IControl control, PointD point)
        {
            return ((UI.Native.Control)control.NativeControl).ClientToScreen(point);
        }

        /// <inheritdoc/>
        public override PointI ScreenToDevice(IControl control, PointD point)
        {
            return ((UI.Native.Control)control.NativeControl).ScreenToDevice(point);
        }

        /// <inheritdoc/>
        public override PointD DeviceToScreen(IControl control, PointI point)
        {
            return ((UI.Native.Control)control.NativeControl).DeviceToScreen(point);
        }

        /// <inheritdoc/>
        public override void DisableRecreate(IControl control)
        {
            ((UI.Native.Control)control.NativeControl).DisableRecreate();
        }

        /// <inheritdoc/>
        public override void EnableRecreate(IControl control)
        {
            ((UI.Native.Control)control.NativeControl).EnableRecreate();
        }

        /// <inheritdoc/>
        public override Graphics CreateDrawingContext(IControl control)
        {
            return new WxGraphics(((UI.Native.Control)control.NativeControl).OpenClientDrawingContext());
        }

        /// <inheritdoc/>
        public override void CaptureMouse(IControl control)
        {
            ((UI.Native.Control)control.NativeControl).SetMouseCapture(true);
        }

        /// <inheritdoc/>
        public override void ReleaseMouseCapture(IControl control)
        {
            ((UI.Native.Control)control.NativeControl).SetMouseCapture(false);
        }

        /// <inheritdoc/>
        public override void RefreshRect(IControl control, RectD rect, bool eraseBackground = true)
        {
            ((UI.Native.Control)control.NativeControl).RefreshRect(rect, eraseBackground);
        }

        /// <inheritdoc/>
        public override void HandleNeeded(IControl control)
        {
            ((UI.Native.Control)control.NativeControl).Required();
        }

        /// <inheritdoc/>
        public override void Raise(IControl control)
        {
            ((UI.Native.Control)control.NativeControl).Raise();
        }

        /// <inheritdoc/>
        public override void CenterOnParent(IControl control, GenericOrientation direction)
        {
            ((UI.Native.Control)control.NativeControl).CenterOnParent((int)direction);
        }

        /// <inheritdoc/>
        public override void Lower(IControl control)
        {
            ((UI.Native.Control)control.NativeControl).Lower();
        }

        /// <inheritdoc/>
        public override void SendSizeEvent(IControl control)
        {
            ((UI.Native.Control)control.NativeControl).SendSizeEvent();
        }

        /// <inheritdoc/>
        public override void UnsetToolTip(IControl control)
        {
            ((UI.Native.Control)control.NativeControl).UnsetToolTip();
        }

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
