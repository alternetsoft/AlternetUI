using System;
using System.Runtime.InteropServices;

namespace Alternet.UI
{
    internal class WxKeyboardInputProvider : DisposableObject
    {
        private readonly Native.Keyboard nativeKeyboard;

        public WxKeyboardInputProvider(Native.Keyboard nativeKeyboard)
        {
            this.nativeKeyboard = nativeKeyboard;
            Native.Keyboard.GlobalObject = nativeKeyboard;
            nativeKeyboard.KeyPress = OnNativeKeyboardKeyPress;
        }

        protected override void DisposeManaged()
        {
            nativeKeyboard.KeyPress = null;
            Native.Keyboard.GlobalObject = null;
        }

        private void OnNativeKeyboardKeyPress()
        {
            const byte InputEventCodeKeyDown = 0;
            const byte InputEventCodeKeyUp = 1;
            const byte InputEventCodeChar = 2;

            bool handled = false;
            Key key = Native.Keyboard.InputKey;

            switch (Native.Keyboard.InputEventCode)
            {
                case InputEventCodeKeyDown:
                    var repeatCount = Keyboard.IsRepeatToRepeatCount(Native.Keyboard.InputIsRepeat);
                    AbstractControl.BubbleKeyDown(
                        key,
                        Keyboard.Modifiers,
                        repeatCount,
                        out handled);
                    break;
                case InputEventCodeKeyUp:
                    var repeatCount2 = Keyboard.IsRepeatToRepeatCount(Native.Keyboard.InputIsRepeat);
                    AbstractControl.BubbleKeyUp(
                        key,
                        Keyboard.Modifiers,
                        repeatCount2,
                        out handled);
                    break;
                case InputEventCodeChar:
                    var inputInt = Native.Keyboard.InputChar;
                    string inputStr;

                    if (App.IsWindowsOS)
                    {
                        var ch = (char)inputInt;

                        if (ch != 0)
                            AbstractControl.BubbleTextInput(ch, out handled);
                    }
                    else
                    {
                        inputStr = char.ConvertFromUtf32(inputInt);

                        if (inputStr.Length == 1)
                        {
                            var ch = inputStr[0];

                            if (ch != 0)
                                AbstractControl.BubbleTextInput(ch, out handled);
                        }
                        else
                        {
                            foreach (var ch in inputStr)
                            {
                                AbstractControl.BubbleTextInput(ch, out handled);
                            }
                        }
                    }

                    break;
            }

            Native.Keyboard.InputHandled = handled;
        }
    }
}