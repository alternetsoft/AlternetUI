using System.Drawing;

namespace Alternet.UI
{
    internal class GenericTextBlockHandler : GenericControlHandler<TextBlock>
    {
        protected override bool NeedsPaint => true;

        public override void OnPaint(DrawingContext drawingContext)
        {
            if (Control.Text != null)
                drawingContext.DrawText(Control.Text, DisplayRectangle.Location, Color.Black);
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

        private void Control_TextChanged(object sender, System.EventArgs e)
        {
            Update();
        }
    }
}