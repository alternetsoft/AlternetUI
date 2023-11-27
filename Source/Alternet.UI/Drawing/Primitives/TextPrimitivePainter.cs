using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    internal class TextPrimitivePainter : PrimitivePainter
    {
    }
}

/*
DrawText(string text, Font font, Brush brush, Point origin) // default format

DrawText(string text, Font font, Brush brush, Point origin, TextFormat format)

DrawText(string text, Font font, Brush brush, Rect bounds)

DrawText(
            string text,
            Font font,
            Brush brush,
            Rect bounds,
            TextFormat format)

Size MeasureText(string text, Font font)

Size MeasureText(string text, Font font, double maximumWidth)

Size MeasureText(string text, Font font, double maximumWidth, TextFormat format)
 */