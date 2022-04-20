using Alternet.Drawing;

namespace PaintSample
{
    internal class ColorSelector : ISelectedColors
    {
        public Color Stroke => Color.Blue;

        public Color Fill => Color.White;
    }
}