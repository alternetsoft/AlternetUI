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

            var pens = Control.Pens;

            var r = DrawClientRectangle;

            if (Control.DrawDebugPointsBefore)
                drawingContext.DrawDebugPoints(r, Pens.Yellow);

            if (pens.Width.Top > 0)
            {
                var topRectangle = new Rect(r.TopLeft, new Size(r.Width, pens.Width.Top));
                // drawingContext.DrawLine(pens.Top, r.TopLeft, r.TopRight);
                drawingContext.FillRectangle(pens.TopColor.AsBrush, topRectangle);
            }

            if (pens.Width.Bottom > 0)
            {
                var bottomRectangle = new Rect(
                    new Point(r.Left, r.Bottom - pens.Width.Bottom),
                    new Size(r.Width, pens.Width.Bottom));
                // drawingContext.DrawLine(pens.Bottom, r.BottomLeft, r.BottomRight);
                drawingContext.FillRectangle(pens.BottomColor.AsBrush, bottomRectangle);
            }

            if (pens.Width.Left > 0)
            {
                var leftRectangle = new Rect();
                // drawingContext.DrawLine(pens.Left, r.TopLeft, r.BottomLeft);
                drawingContext.FillRectangle(pens.LeftColor.AsBrush, leftRectangle);
            }

            if (pens.Width.Right > 0)
            {
                var rightRectangle = new Rect();
                // drawingContext.DrawLine(pens.Right, r.TopRight, r.BottomRight);
                drawingContext.FillRectangle(pens.RightColor.AsBrush, rightRectangle);
            }

            if(Control.DrawDebugPointsAfter)
                drawingContext.DrawDebugPoints(r);
        }

        public override Size GetPreferredSize(Size availableSize)
        {
            return base.GetPreferredSize(availableSize)/* +
                new Size(
                    Control.BorderWidth.Horizontal,
                    Control.BorderWidth.Vertical)*/;
        }
    }
}