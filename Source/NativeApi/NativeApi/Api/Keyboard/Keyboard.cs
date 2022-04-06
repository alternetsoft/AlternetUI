using System;
using System.ComponentModel;
using ApiCommon;

namespace NativeApi.Api
{
    public class Keyboard
    {
        public event NativeEventHandler<KeyEventData>? KeyDown { add => throw new Exception(); remove => throw new Exception(); }
        public event NativeEventHandler<KeyEventData>? KeyUp { add => throw new Exception(); remove => throw new Exception(); }

        public KeyStates GetKeyState(Key key) => throw new Exception();
    }
}