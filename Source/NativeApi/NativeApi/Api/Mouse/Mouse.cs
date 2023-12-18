using System;
using System.ComponentModel;
using Alternet.Drawing;
using ApiCommon;

namespace NativeApi.Api
{
    public class Mouse
    {
        public event NativeEventHandler<MouseEventData>? MouseMove { add => throw new Exception(); remove => throw new Exception(); }

        public event NativeEventHandler<MouseButtonEventData>? MouseDown { add => throw new Exception(); remove => throw new Exception(); }
        public event NativeEventHandler<MouseButtonEventData>? MouseUp { add => throw new Exception(); remove => throw new Exception(); }

        public event NativeEventHandler<MouseButtonEventData>? MouseDoubleClick { add => throw new Exception(); remove => throw new Exception(); }

        public event NativeEventHandler<MouseWheelEventData>? MouseWheel { add => throw new Exception(); remove => throw new Exception(); }

        public PointD GetPosition() => throw new Exception();

        public MouseButtonState GetButtonState(MouseButton button) => throw new Exception();
    }
}