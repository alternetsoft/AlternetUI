using System.Drawing;

namespace Alternet.UI
{
    internal class GenericBorderHandler : GenericControlHandler<Border>
    {
        protected override bool NeedsPaint => true;

        public override void OnPaint(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(ChildrenBounds, Control.BorderColor);
        }

        public override SizeF GetPreferredSize(SizeF availableSize)
        {
            var size = GexChildrenMaxPreferredSize(availableSize);
            return size + new SizeF(2, 2); // todo: BorderThickness
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            Control.BorderColorChanged += Control_BorderColorChanged;
        }

        protected override void OnDetach()
        {
            Control.BorderColorChanged -= Control_BorderColorChanged;
            base.OnDetach();
        }

        private void Control_BorderColorChanged(object? sender, System.EventArgs? e)
        {
            Update();
        }
    }
}