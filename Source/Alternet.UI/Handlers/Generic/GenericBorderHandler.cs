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

        public override void OnLayout()
        {
            // todo: use padding

            var displayRectangle = DisplayRectangle;
            displayRectangle.Inflate(-5, -5);
            foreach (var control in Control.AllChildren)
            {
                var margin = control.Margin;
                control.Handler.Bounds = new RectangleF(displayRectangle.Location + new SizeF(margin.Left, margin.Top), displayRectangle.Size - margin.Size);
            }
        }
    }
}