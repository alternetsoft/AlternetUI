#pragma warning disable
using System;
using System.ComponentModel;
using Alternet.Drawing;
using ApiCommon;

namespace NativeApi.Api
{
    public class Mouse
    {
        public event NativeEventHandler<MouseEventData>? MouseChanged;

        public PointI GetPosition() => default;

        public MouseButtonState GetButtonState(MouseButton button) => default;
    }
}