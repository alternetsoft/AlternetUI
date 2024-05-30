using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui;
using Microsoft.Maui.Graphics;

using SkiaSharp;

namespace Alternet.Drawing
{
    public class SkiaFontHandler : PlessFontHandler
    {
        public SkiaFontHandler()
            : this(Microsoft.Maui.Graphics.Font.Default)
        {
        }

        public SkiaFontHandler(Microsoft.Maui.Graphics.Font font)
        {
            Name = font.Name;
            if (font.StyleType == FontStyleType.Italic)
                Style = FontStyle.Italic;
            SetNumericWeight(font.Weight);
        }
    }
}
