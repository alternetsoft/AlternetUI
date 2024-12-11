using System;

namespace Alternet.UI
{
    internal class KeyboardInputProvider : DisposableObject
    {
        private readonly Native.Keyboard nativeKeyboard;

        public KeyboardInputProvider(Native.Keyboard nativeKeyboard)
        {
            this.nativeKeyboard = nativeKeyboard;
            nativeKeyboard.KeyPress = NativeKeyboard_KeyPress;
        }

        protected override void DisposeManaged()
        {
            nativeKeyboard.KeyPress = null;
        }

        private void NativeKeyboard_KeyPress()
        {
            const byte InputEventCodeKeyDown = 0;
            const byte InputEventCodeKeyUp = 1;
            const byte InputEventCodeChar = 2;

            var inputChar = Native.Keyboard.InputChar;
            bool handled = false;
            var repeatCount = Keyboard.IsRepeatToRepeatCount(Native.Keyboard.InputIsRepeat);
            Key key = Native.Keyboard.InputKey;

            switch (Native.Keyboard.InputEventCode)
            {
                case InputEventCodeKeyDown:
                    AbstractControl.BubbleKeyDown(
                        key,
                        Keyboard.Modifiers,
                        repeatCount,
                        out handled);
                    break;
                case InputEventCodeKeyUp:
                    AbstractControl.BubbleKeyUp(
                        key,
                        Keyboard.Modifiers,
                        repeatCount,
                        out handled);
                    break;
                case InputEventCodeChar:
                    if (inputChar != 0)
                        AbstractControl.BubbleTextInput(inputChar, out handled);
                    break;
            }

            Native.Keyboard.InputHandled = handled;
        }
    }
}