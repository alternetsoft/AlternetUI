using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class DragEventData : NativeEventData
    {
        public IntPtr data;

        public DragInputState inputState;

        public MouseButton changedButton;

        public double mouseClientLocationX;
        public double mouseClientLocationY;

        public DragDropEffects allowedEffect;
        public DragDropEffects effect;
    }
}