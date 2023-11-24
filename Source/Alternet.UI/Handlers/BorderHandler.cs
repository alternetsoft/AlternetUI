using System.Diagnostics;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class BorderHandler : ControlHandler<Border>
    {
        protected override bool NeedsPaint => true;

        public override void OnPaint(DrawingContext dc)
        {
            var r = DrawClientRectangle;
            var settings = Control.GetSettings(Control.CurrentState);

            var radius = settings.GetUniformCornerRadius(r);

            Control.DrawDefaultBackground(dc, r);

            if (Control.DrawDebugPointsBefore)
                dc.DrawDebugPoints(r, Pens.Yellow);

            if(Control.HasBorder && radius is null)
                settings.Draw(dc, r);

            if(Control.DrawDebugPointsAfter)
                dc.DrawDebugPoints(r);
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