using System.Drawing;

namespace Alternet.UI
{
    internal class GenericBorderHandler : ControlHandler<Border>
    {
        protected override bool NeedsPaint => true;

        public override void OnPaint(DrawingContext drawingContext)
        {
            if (Control.Background != null)
                drawingContext.FillRectangle(ClientRectangle, Control.Background);
            
            if (Control.BorderBrush != null)
                drawingContext.DrawRectangle(ClientRectangle, new Pen(Control.BorderBrush));
        }

        public override SizeF GetPreferredSize(SizeF availableSize)
        {
            var size = GetChildrenMaxPreferredSize(availableSize);
            return size + new SizeF(2, 2); // todo: BorderThickness
        }
    }
}