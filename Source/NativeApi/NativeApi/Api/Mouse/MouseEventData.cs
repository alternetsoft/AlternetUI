
using Alternet.Drawing;
using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class MouseEventData : NativeEventData
    {
        public int mouseEventKind;
        public long timestamp;
        public IntPtr targetControl;
        public int delta;
        public int numClicks;
    }
}