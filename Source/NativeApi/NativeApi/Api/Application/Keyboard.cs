#pragma warning disable
using System;
using System.ComponentModel;
using ApiCommon;

namespace NativeApi.Api
{
    public class Keyboard
    {
        public char InputChar { get; }

        /*
            0 - key down
            1 - key up
            2 - char press
        */
        public byte InputEventCode { get; }

        public bool InputHandled { get; set; }

        public Key InputKey { get; }

        public bool InputIsRepeat { get; }

        public event EventHandler? KeyPress;

        public KeyStates GetKeyState(Key key) => throw new Exception();
    }
}