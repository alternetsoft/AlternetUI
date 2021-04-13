using System.Drawing;

namespace Alternet.UI
{
    internal class GenericBorderHandler : GenericControlHandler<Border>
    {
        public override void OnPaint(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(DisplayRectangle, Color.Blue);
        }

        protected override bool NeedsPaint => true;
    }
}