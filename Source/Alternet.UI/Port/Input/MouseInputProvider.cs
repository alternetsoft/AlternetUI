using Alternet.UI.Native;
using System;

namespace Alternet.UI
{
    internal class MouseInputProvider : IDisposable
    {
        Native.Mouse nativeMouse;
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

        private void NativeMouse_MouseDoubleClick(object? sender, NativeEventArgs<MouseButtonEventData> e)
        {
            InputManager.Current.ReportMouseDoubleClick(e.Data.timestamp, (MouseButton)e.Data.changedButton, out var handled);
            e.Handled = handled;
        }

        private void NativeMouse_MouseWheel(object? sender, NativeEventArgs<MouseWheelEventData> e)
        {
            InputManager.Current.ReportMouseWheel(e.Data.timestamp, e.Data.delta, out var handled);
            e.Handled = handled;
        }

        private void NativeMouse_MouseUp(object? sender, NativeEventArgs<MouseButtonEventData> e)
        {
            InputManager.Current.ReportMouseUp(e.Data.timestamp, (MouseButton)e.Data.changedButton, out var handled);
            e.Handled = handled;
        }

        private void NativeMouse_MouseDown(object? sender, NativeEventArgs<MouseButtonEventData> e)
        {
            InputManager.Current.ReportMouseDown(e.Data.timestamp, (MouseButton)e.Data.changedButton, out var handled);
            e.Handled = handled;
        }

        private void NativeMouse_MouseMove(object? sender, Native.NativeEventArgs<Native.MouseEventData> e)
        {
            InputManager.Current.ReportMouseMove(e.Data.timestamp, out var handled);
            e.Handled = handled;
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

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}


