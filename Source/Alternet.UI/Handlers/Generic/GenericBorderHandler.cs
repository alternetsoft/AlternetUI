using System.Diagnostics;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class GenericBorderHandler : ControlHandler<Border>
    {
        public override Rect ChildrenLayoutBounds
        {
            get
            {
                var bounds = base.ChildrenLayoutBounds;
                bounds.X += Control.BorderWidth.Left;
                bounds.Y += Control.BorderWidth.Top;
                bounds.Width -= Control.BorderWidth.Horizontal;
                bounds.Height -= Control.BorderWidth.Vertical;
                return bounds;
            }
        }

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