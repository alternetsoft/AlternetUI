#pragma warning disable
using ApiCommon;
using System;
using System.Collections.Generic;
using System.Text;
using Alternet.Drawing;

namespace NativeApi.Api
{
    public class PropertyGridChoices
    {
        public static IntPtr CreatePropertyGridChoices() => throw new Exception();

        public static void Delete(IntPtr handle) { }

        public static void Add(IntPtr handle, string text, int value,
            ImageSet? bitmapBundle) => throw new Exception();
    }
}