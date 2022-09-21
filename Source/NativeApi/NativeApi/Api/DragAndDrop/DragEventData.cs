using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class DragEventData : NativeEventData
    {
        public IntPtr data;

        public double mouseClientLocationX;
        public double mouseClientLocationY;

        public DragDropEffects effect;
    }
}