#pragma warning disable
using System;

namespace NativeApi.Api
{
    public class Toolbar : Control
    {
        public static IntPtr CreateEx(bool mainToolbar) => throw new Exception();

        public int ItemsCount { get; }

        public void InsertItemAt(int index, ToolbarItem item) => throw new Exception();

        public void RemoveItemAt(int index) => throw new Exception();

        public bool ItemTextVisible { get; set; }

        public bool ItemImagesVisible { get; set; }

        public bool NoDivider { get; set; }
        public bool IsVertical { get; set; }

        public bool IsBottom { get; set; }
        public bool IsRight { get; set; }

        public ImageToText ImageToTextDisplayMode { get; set; }

        public void Realize() => throw new Exception();
    }
}