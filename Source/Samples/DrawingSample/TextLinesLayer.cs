using Alternet.UI;
using System.Drawing;

namespace DrawingSample
{
    public sealed class TextLinesLayer : Layer
    {
        private Alternet.UI.Font font = new Alternet.UI.Font("Times New Roman", 15);

        public override string Name => "Text Lines";

        public override void Draw(DrawingContext dc, RectangleF bounds)
        {
            dc.DrawText("Hello", new PointF(20, 20), font, Color.DarkViolet);
        }
    }
}