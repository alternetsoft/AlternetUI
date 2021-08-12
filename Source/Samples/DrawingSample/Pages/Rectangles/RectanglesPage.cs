using Alternet.UI;
using System.Drawing;

namespace DrawingSample
{
    internal sealed class RectanglesPage : DrawingPage
    {
        public override string Name => "Rectangles";

        public override void Draw(DrawingContext dc, RectangleF bounds)
        {
            dc.DrawRectangle(new RectangleF(10, 10, 100, 100), Color.Blue);
        }

        protected override Control CreateSettingsControl() => new RectanglesPageSettings(this);
    }
}