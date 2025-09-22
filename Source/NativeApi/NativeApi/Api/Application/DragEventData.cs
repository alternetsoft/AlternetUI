using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class DragEventData : NativeEventData
    {
        public IntPtr data;

        public float mouseClientLocationX;
        public float mouseClientLocationY;

        public DragDropEffects effect;
    }
}