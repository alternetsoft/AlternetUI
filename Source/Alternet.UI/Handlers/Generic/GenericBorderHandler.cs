using System.Diagnostics;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class GenericBorderHandler : ControlHandler<Border>
    {
        protected override bool NeedsPaint => true;

        public override void OnPaint(DrawingContext dc)
        {
            var r = DrawClientRectangle;
            var settings = Control.GetSettings(Control.CurrentState);

            var radius = settings.GetUniformCornerRadius(r);

            var brush = Control.GetBackground(Control.CurrentState);

            if (brush != null)
            {
                if (radius is null)
                    dc.FillRectangle(brush, r);
                else
                    dc.FillRoundedRectangle(brush, r, radius.Value);
            }

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