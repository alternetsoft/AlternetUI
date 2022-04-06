using System;

namespace Alternet.UI
{
    internal class KeyboardInputProvider : IDisposable
    {
        Native.Application nativeApplication;
        private bool isDisposed;

        public KeyboardInputProvider(Native.Application nativeApplication)
        {
            this.nativeApplication = nativeApplication;
            nativeApplication.KeyDown += NativeApplication_KeyDown;
            nativeApplication.KeyUp += NativeApplication_KeyUp;
        }

        private void NativeApplication_KeyDown(object? sender, Native.NativeEventArgs<Native.KeyEventData> e)
        {
            InputManager.Current.ReportKeyDown(e.Data.timestamp, (Key)e.Data.key, e.Data.isRepeat);
        }

        private void NativeApplication_KeyUp(object? sender, Native.NativeEventArgs<Native.KeyEventData> e)
        {
            InputManager.Current.ReportKeyUp(e.Data.timestamp, (Key)e.Data.key, e.Data.isRepeat);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    nativeApplication.KeyDown -= NativeApplication_KeyDown;
                    nativeApplication.KeyUp -= NativeApplication_KeyUp;
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


