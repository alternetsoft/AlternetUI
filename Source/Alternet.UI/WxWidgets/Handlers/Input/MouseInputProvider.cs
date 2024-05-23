using System;
using Alternet.UI.Native;

namespace Alternet.UI
{
    internal class MouseInputProvider : IDisposable
    {
        private readonly Native.Mouse nativeMouse;
        private bool isDisposed;

        public MouseInputProvider(Native.Mouse nativeMouse)
        {
            this.nativeMouse = nativeMouse;
            nativeMouse.MouseMove += NativeMouse_MouseMove;
            nativeMouse.MouseDown += NativeMouse_MouseDown;
            nativeMouse.MouseUp += NativeMouse_MouseUp;
            nativeMouse.MouseWheel += NativeMouse_MouseWheel;
            nativeMouse.MouseDoubleClick += NativeMouse_MouseDoubleClick;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    nativeMouse.MouseMove -= NativeMouse_MouseMove;
                    nativeMouse.MouseDown -= NativeMouse_MouseDown;
                    nativeMouse.MouseUp -= NativeMouse_MouseUp;
                    nativeMouse.MouseWheel -= NativeMouse_MouseWheel;
                    nativeMouse.MouseDoubleClick -= NativeMouse_MouseDoubleClick;
                }

                isDisposed = true;
            }
        }

        private static Control? GetTargetControl(IntPtr targetControlPointer)
        {
            if (targetControlPointer == IntPtr.Zero)
                return null;

            var nobj = NativeObject.GetFromNativePointer<NativeObject>(
                targetControlPointer,
                null);
            if (nobj is not Native.Control c)
                return null;

            /*
            Do not uncomment this. For what reason it was here?
            If uncommented, under Linux, mouse events will be send to wrong windows
            as wxFindWindowAtPoint gets wrong window.
            */
            /*if (!c.IsMouseCaptured)
                return null;*/

            return WxControlHandler.NativeControlToHandler(c)?.Control;
        }

        private void NativeMouse_MouseDoubleClick(
            object? sender,
            NativeEventArgs<MouseButtonEventData> e)
        {
            InputManager.Current.ReportMouseDoubleClick(
                GetTargetControl(e.Data.targetControl),
                e.Data.timestamp,
                (MouseButton)e.Data.changedButton,
                out _);

            e.Handled = false;
        }

        private void NativeMouse_MouseWheel(object? sender, NativeEventArgs<MouseWheelEventData> e)
        {
            InputManager.Current.ReportMouseWheel(
                GetTargetControl(e.Data.targetControl),
                e.Data.timestamp,
                e.Data.delta,
                out _);
            e.Handled = false;
        }

        private void NativeMouse_MouseUp(object? sender, NativeEventArgs<MouseButtonEventData> e)
        {
            InputManager.Current.ReportMouseUp(
                GetTargetControl(e.Data.targetControl),
                e.Data.timestamp,
                (MouseButton)e.Data.changedButton,
                out _);

            e.Handled = false;
        }

        private void NativeMouse_MouseDown(object? sender, NativeEventArgs<MouseButtonEventData> e)
        {
            InputManager.Current.ReportMouseDown(
                GetTargetControl(e.Data.targetControl),
                e.Data.timestamp,
                (MouseButton)e.Data.changedButton,
                out _);

            e.Handled = false;
        }

        private void NativeMouse_MouseMove(
            object? sender,
            Native.NativeEventArgs<Native.MouseEventData> e)
        {
            InputManager.Current.ReportMouseMove(
                GetTargetControl(e.Data.targetControl),
                e.Data.timestamp,
                out _);

            e.Handled = false;
        }
     }
}