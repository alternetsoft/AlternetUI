
using Alternet.Drawing;
using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class MouseEventData : NativeEventData
    {
        public long timestamp;
        public IntPtr targetControl;
    }
}