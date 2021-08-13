
using System;

namespace NativeApi.Api
{
    [Flags]
    public enum FontStyle
    {
        Regular = 0,
        Bold = 1 << 0,
        Italic = 1 << 1,
        Underlined = 1 << 2,
        Strikethrough = 1 << 3,
    }
}