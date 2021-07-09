using System.Drawing;

namespace Alternet.UI
{
    internal class GenericBorderHandler : ControlHandler<Border>
    {
        protected override bool NeedsPaint => true;

        public override void OnPaint(DrawingContext drawingContext)
        {
            if (Control.BackgroundColor != null)
                drawingContext.FillRectangle(ClientRectangle, Control.BackgroundColor.Value);
            
            if (Control.BorderColor != null)
                drawingContext.DrawRectangle(ClientRectangle, Control.BorderColor.Value);
        }

        public override SizeF GetPreferredSize(SizeF availableSize)
        {
            var size = GetChildrenMaxPreferredSize(availableSize);
            return size + new SizeF(2, 2); // todo: BorderThickness
        }
    }
}