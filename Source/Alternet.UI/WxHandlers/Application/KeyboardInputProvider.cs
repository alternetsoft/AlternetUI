using System;
using System.Runtime.InteropServices;

namespace Alternet.UI
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void CdeclActionDelegate();

    internal class KeyboardInputProvider : DisposableObject
    {
        private readonly Native.Keyboard nativeKeyboard;

        public KeyboardInputProvider(Native.Keyboard nativeKeyboard)
        {
            this.nativeKeyboard = nativeKeyboard;
            Native.Keyboard.GlobalObject = nativeKeyboard;
            nativeKeyboard.KeyPress = NativeKeyboard_KeyPress;
        }

        protected override void DisposeManaged()
        {
            nativeKeyboard.KeyPress = null;
            Native.Keyboard.GlobalObject = null;
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
                        out handled,
                        inputChar);
                    break;
                case InputEventCodeKeyUp:
                    AbstractControl.BubbleKeyUp(
                        key,
                        Keyboard.Modifiers,
                        repeatCount,
                        out handled,
                        inputChar);
                    break;
                case InputEventCodeChar:
                    if (inputChar != 0)
                        AbstractControl.BubbleTextInput(inputChar, out handled, key);
                    break;
            }

            Native.Keyboard.InputHandled = handled;
        }
    }
}