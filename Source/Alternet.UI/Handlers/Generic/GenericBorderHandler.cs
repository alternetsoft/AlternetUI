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

            var radius = Control.Settings.GetUniformCornerRadius(r);

            if (Control.Background != null)
            {
                if (radius is null)
                    dc.FillRectangle(Control.Background, r);
                else
                    dc.FillRoundedRectangle(Control.Background, r, radius.Value);
            }

            var settings = Control.Settings;

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