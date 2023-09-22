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

            if (Control.BorderBrush != null)
            {
                drawingContext.DrawRectangle(
                    Control.BorderBrush.AsPen,
                    DrawClientRectangle);
            }
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