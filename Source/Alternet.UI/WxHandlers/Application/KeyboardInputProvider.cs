using System;

namespace Alternet.UI
{
    internal class KeyboardInputProvider : DisposableObject
    {
        private readonly Native.Keyboard nativeKeyboard;

        public KeyboardInputProvider(Native.Keyboard nativeKeyboard)
        {
            this.nativeKeyboard = nativeKeyboard;
            nativeKeyboard.KeyDown += NativeKeyboard_KeyDown;
            nativeKeyboard.KeyUp += NativeKeyboard_KeyUp;
            nativeKeyboard.TextInput += NativeKeyboard_TextInput;
        }

        protected override void DisposeManaged()
        {
            nativeKeyboard.KeyDown -= NativeKeyboard_KeyDown;
            nativeKeyboard.KeyUp -= NativeKeyboard_KeyUp;
            nativeKeyboard.TextInput -= NativeKeyboard_TextInput;
        }

        private void NativeKeyboard_TextInput(
            object? sender,
            Native.NativeEventArgs<Native.TextInputEventData> e)
        {
            if (e.Data.keyChar == 0)
                return;
            Control.BubbleTextInput(e.Data.keyChar, out var handled);
            e.Handled = handled;
        }

        private void NativeKeyboard_KeyDown(
            object? sender,
            Native.NativeEventArgs<Native.KeyEventData> e)
        {
            Control.BubbleKeyDown(
                (Key)e.Data.key,
                Keyboard.IsRepeatToRepeatCount(e.Data.isRepeat),
                out var handled);
            e.Handled = handled;
        }

        private void NativeKeyboard_KeyUp(
            object? sender,
            Native.NativeEventArgs<Native.KeyEventData> e)
        {
            Control.BubbleKeyUp(
                (Key)e.Data.key,
                Keyboard.IsRepeatToRepeatCount(e.Data.isRepeat),
                out var handled);
            e.Handled = handled;
        }
    }
}