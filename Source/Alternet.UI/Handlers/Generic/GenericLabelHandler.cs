using Alternet.Drawing;

namespace Alternet.UI
{
    internal class GenericLabelHandler : ControlHandler<Label>
    {
        protected override bool NeedsPaint => true;

        public override void OnPaint(DrawingContext drawingContext)
        {
            if (Control.Text != null)
                drawingContext.DrawText(Control.Text, Control.Font ?? UI.Control.DefaultFont, Control.Foreground ?? Brushes.Black, ChildrenLayoutBounds.Location);
        }

        public override Size GetPreferredSize(Size availableSize)
        {
            var text = Control.Text;
            if (text == null)
                return new Size();

            using (var dc = Control.CreateDrawingContext())
                return dc.MeasureText(text, Control.Font ?? UI.Control.DefaultFont) + Control.Padding.Size;
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            Control.TextChanged += Control_TextChanged;
            Control.ForegroundChanged += Control_ForegroundColorChanged;
        }

        protected override void OnDetach()
        {
            Control.TextChanged -= Control_TextChanged;
            Control.ForegroundChanged -= Control_ForegroundColorChanged;
            base.OnDetach();
        }

        private void Control_ForegroundColorChanged(object? sender, System.EventArgs? e)
        {
            Invalidate();
        }

        private void Control_TextChanged(object? sender, System.EventArgs? e)
        {
            Invalidate();
        }
    }
}