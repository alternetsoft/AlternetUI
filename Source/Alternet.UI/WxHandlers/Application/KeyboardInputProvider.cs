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

            var inputChar = nativeKeyboard.InputChar;
            bool handled = false;
            var repeatCount = Keyboard.IsRepeatToRepeatCount(nativeKeyboard.InputIsRepeat);
            var key = nativeKeyboard.InputKey;

            switch (nativeKeyboard.InputEventCode)
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

            nativeKeyboard.InputHandled = handled;
        }
    }
}