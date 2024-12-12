#pragma warning disable
using System;
using System.ComponentModel;
using ApiCommon;

namespace NativeApi.Api
{
    public class Keyboard
    {
        public static char InputChar { get; }

        /*
            0 - key down
            1 - key up
            2 - char press
        */
        public static byte InputEventCode { get; }

        public static bool InputHandled { get; set; }

        public static Key InputKey { get; }

        public static bool InputIsRepeat { get; }

        public event EventHandler? KeyPress;

        public static KeyStates GetKeyState(Key key) => default;
    }
}