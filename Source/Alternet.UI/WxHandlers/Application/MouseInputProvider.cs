using System;
using Alternet.UI.Native;

namespace Alternet.UI
{
    internal class MouseInputProvider : DisposableObject
    {
        private readonly Native.Mouse nativeMouse;

        public MouseInputProvider(Native.Mouse nativeMouse)
        {
            this.nativeMouse = nativeMouse;
            nativeMouse.MouseMove += NativeMouse_MouseMove;
            nativeMouse.MouseDown += NativeMouse_MouseDown;
            nativeMouse.MouseUp += NativeMouse_MouseUp;
            nativeMouse.MouseWheel += NativeMouse_MouseWheel;
            nativeMouse.MouseDoubleClick += NativeMouse_MouseDoubleClick;
        }

        protected override void DisposeManaged()
        {
            nativeMouse.MouseMove -= NativeMouse_MouseMove;
            nativeMouse.MouseDown -= NativeMouse_MouseDown;
            nativeMouse.MouseUp -= NativeMouse_MouseUp;
            nativeMouse.MouseWheel -= NativeMouse_MouseWheel;
            nativeMouse.MouseDoubleClick -= NativeMouse_MouseDoubleClick;
        }

        private static Control? GetTargetControl(IntPtr targetControlPointer, bool setHoveredControl)
        {
            if (targetControlPointer == IntPtr.Zero)
                return null;

            var hoveredControl = Control.GetHoveredControl();

            var nativeHoveredHandler = hoveredControl?.Handler as WxControlHandler;

            if(nativeHoveredHandler is not null)
            {
                if (nativeHoveredHandler.NativeControl.NativePointer == targetControlPointer)
                    return hoveredControl;
            }

            var nobj = NativeObject.GetFromNativePointer<NativeObject>(
                targetControlPointer,
                null);
            if (nobj is not Native.Control c)
                return null;

            var result = WxControlHandler.NativeControlToHandler(c)?.Control;
            if(setHoveredControl && result is not null)
                Control.HoveredControl = result;

            return result;
        }

        private void NativeMouse_MouseDoubleClick(
            object? sender,
            NativeEventArgs<MouseButtonEventData> e)
        {
            Mouse.ReportMouseDoubleClick(
                GetTargetControl(e.Data.targetControl, true),
                e.Data.timestamp,
                (MouseButton)e.Data.changedButton,
                out _);

            e.Handled = false;
        }

        private void NativeMouse_MouseWheel(object? sender, NativeEventArgs<MouseWheelEventData> e)
        {
            Mouse.ReportMouseWheel(
                GetTargetControl(e.Data.targetControl, true),
                e.Data.timestamp,
                e.Data.delta,
                out _);
            e.Handled = false;
        }

        private void NativeMouse_MouseUp(object? sender, NativeEventArgs<MouseButtonEventData> e)
        {
            Mouse.ReportMouseUp(
                GetTargetControl(e.Data.targetControl, true),
                e.Data.timestamp,
                (MouseButton)e.Data.changedButton,
                out _);

            e.Handled = false;
        }

        private void NativeMouse_MouseDown(object? sender, NativeEventArgs<MouseButtonEventData> e)
        {
            Mouse.ReportMouseDown(
                GetTargetControl(e.Data.targetControl, true),
                e.Data.timestamp,
                (MouseButton)e.Data.changedButton,
                out _);

            e.Handled = false;
        }

        private void NativeMouse_MouseMove(
            object? sender,
            Native.NativeEventArgs<Native.MouseEventData> e)
        {
            Mouse.ReportMouseMove(
                GetTargetControl(e.Data.targetControl, true),
                e.Data.timestamp,
                out _);

            e.Handled = false;
        }
     }
}