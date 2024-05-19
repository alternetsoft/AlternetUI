
using System;

using ApiCommon;

namespace NativeApi.Api
{
    [Flags]
    [ManagedExternName("Alternet.Drawing.FontStyle")]
    [ManagedName("Alternet.Drawing.FontStyle")]
    public enum FontStyle
    {
        Regular = 0,
        Bold = 1 << 0,
        Italic = 1 << 1,
        Underlined = 1 << 2,
        Strikethrough = 1 << 3,
    }
}