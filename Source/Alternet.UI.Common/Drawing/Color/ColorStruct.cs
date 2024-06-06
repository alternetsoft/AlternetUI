using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Alternet.UI;
using Alternet.UI.Localization;

using SkiaSharp;

namespace Alternet.Drawing
{
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct ColorStruct
    {
        [FieldOffset(0)] public byte B;
        [FieldOffset(1)] public byte G;
        [FieldOffset(2)] public byte R;
        [FieldOffset(3)] public byte A;
        [FieldOffset(0)] public SKColor Color;
     }
}
