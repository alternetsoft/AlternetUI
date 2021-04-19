using System.Drawing;

namespace Alternet.UI
{
    internal class GenericTextBlockHandler : ControlHandler<TextBlock>
    {
        protected override bool NeedsPaint => true;

        public override void OnPaint(DrawingContext drawingContext)
        {
            if (Control.Text != null)
                drawingContext.DrawText(Control.Text, ChildrenLayoutBounds.Location, Color.Black);
        }

        public override SizeF GetPreferredSize(SizeF availableSize)
        {
            var text = Control.Text;
            if (text == null)
                return new SizeF();

            using (var dc = Control.CreateDrawingContext())
                return dc.MeasureText(text) + Control.Padding.Size;
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            Control.TextChanged += Control_TextChanged;
        }

        protected override void OnDetach()
        {
            Control.TextChanged -= Control_TextChanged;
            base.OnDetach();
        }

        private void Control_TextChanged(object? sender, System.EventArgs? e)
        {
            Update();
        }
    }
}