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


