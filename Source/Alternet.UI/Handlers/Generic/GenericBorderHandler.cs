using System.Diagnostics;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class GenericBorderHandler : ControlHandler<Border>
    {
        protected override bool NeedsPaint => true;

        public override void OnPaint(DrawingContext drawingContext)
        {
            if (Control.Background != null)
            {
                drawingContext.FillRectangle(
                    Control.Background,
                    DrawClientRectangle);
            }

            var settings = Control.Settings;

            var r = DrawClientRectangle;

            if (Control.DrawDebugPointsBefore)
                drawingContext.DrawDebugPoints(r, Pens.Yellow);

            if(Control.HasBorder)
                settings.Draw(drawingContext, r);

            if(Control.DrawDebugPointsAfter)
                drawingContext.DrawDebugPoints(r);
        }

        public override Size GetPreferredSize(Size availableSize)
        {
            return base.GetPreferredSize(availableSize) +
                new Size(
                    Control.BorderWidth.Horizontal,
                    Control.BorderWidth.Vertical);
        }
    }
}