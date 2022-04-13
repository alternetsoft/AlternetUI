
using Alternet.Drawing;
using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class MouseButtonEventData : NativeEventData
    {
        public long timestamp;
        public MouseButton changedButton;
    }
}