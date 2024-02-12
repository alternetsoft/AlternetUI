using System;

namespace Alternet.UI
{
    internal class KeyboardInputProvider : IDisposable
    {
        private readonly Native.Keyboard nativeKeyboard;
        private bool isDisposed;

        public KeyboardInputProvider(Native.Keyboard nativeKeyboard)
        {
            this.nativeKeyboard = nativeKeyboard;
            nativeKeyboard.KeyDown += NativeKeyboard_KeyDown;
            nativeKeyboard.KeyUp += NativeKeyboard_KeyUp;
            nativeKeyboard.TextInput += NativeKeyboard_TextInput;
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
                    nativeKeyboard.KeyDown -= NativeKeyboard_KeyDown;
                    nativeKeyboard.KeyUp -= NativeKeyboard_KeyUp;
                    nativeKeyboard.TextInput -= NativeKeyboard_TextInput;
                }

                isDisposed = true;
            }
        }

        private void NativeKeyboard_TextInput(
            object? sender,
            Native.NativeEventArgs<Native.TextInputEventData> e)
        {
            if (e.Data.keyChar == 0)
                return;
            InputManager.Current.ReportTextInput(e.Data.keyChar, out var handled);
            e.Handled = handled;
        }

        private void NativeKeyboard_KeyDown(
            object? sender,
            Native.NativeEventArgs<Native.KeyEventData> e)
        {
            InputManager.Current.ReportKeyDown(
                (Key)e.Data.key,
                e.Data.isRepeat,
                out var handled);
            e.Handled = handled;
        }

        private void NativeKeyboard_KeyUp(
            object? sender,
            Native.NativeEventArgs<Native.KeyEventData> e)
        {
            InputManager.Current.ReportKeyUp(
                (Key)e.Data.key,
                e.Data.isRepeat,
                out var handled);
            e.Handled = handled;
        }
    }
}