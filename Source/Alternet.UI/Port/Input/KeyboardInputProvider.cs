using System;

namespace Alternet.UI
{
    internal class KeyboardInputProvider : IDisposable
    {
        Native.Keyboard nativeKeyboard;
        private bool isDisposed;

        public KeyboardInputProvider(Native.Keyboard nativeKeyboard)
        {
            this.nativeKeyboard = nativeKeyboard;
            nativeKeyboard.KeyDown += NativeApplication_KeyDown;
            nativeKeyboard.KeyUp += NativeApplication_KeyUp;
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
                    nativeKeyboard.KeyDown -= NativeApplication_KeyDown;
                    nativeKeyboard.KeyUp -= NativeApplication_KeyUp;
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


