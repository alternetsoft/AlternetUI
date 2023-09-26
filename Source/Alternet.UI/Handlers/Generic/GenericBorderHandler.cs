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

            if (settings.Width.Top > 0)
            {
                var topRectangle = new Rect(r.TopLeft, new Size(r.Width, settings.Width.Top));
                drawingContext.FillRectangle(settings.TopBrush, topRectangle);
            }

            if (settings.Width.Bottom > 0)
            {
                var bottomRectangle = new Rect(
                    new Point(r.Left, r.Bottom - settings.Width.Bottom),
                    new Size(r.Width, settings.Width.Bottom));
                drawingContext.FillRectangle(settings.BottomBrush, bottomRectangle);
            }

            if (settings.Width.Left > 0)
            {
                var leftRectangle = new Rect(r.TopLeft, new Size(settings.Width.Left, r.Height));
                drawingContext.FillRectangle(settings.LeftBrush, leftRectangle);
            }

            if (settings.Width.Right > 0)
            {
                var rightRectangle = new Rect(
                    new Point(r.Right - settings.Width.Right, r.Top),
                    new Size(settings.Width.Right, r.Height));
                drawingContext.FillRectangle(settings.RightBrush, rightRectangle);
            }

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