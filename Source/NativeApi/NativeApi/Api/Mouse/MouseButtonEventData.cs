
using Alternet.Drawing;
using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class MouseButtonEventData : NativeEventData
    {
        public long timestamp;
        public IntPtr targetControl;
        public MouseButton changedButton;
    }
}