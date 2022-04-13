
using Alternet.Drawing;
using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class MouseWheelEventData : NativeEventData
    {
        public long timestamp;
        public IntPtr targetControl;
        public int delta;
    }
}