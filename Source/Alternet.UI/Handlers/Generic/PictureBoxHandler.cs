using Alternet.Drawing;

namespace Alternet.UI
{
    internal class PictureBoxHandler : ControlHandler<PictureBox>
    {
        protected override bool NeedsPaint => true;

        public override void OnPaint(DrawingContext drawingContext)
        {
            if (Control.Background != null)
                drawingContext.FillRectangle(Control.Background, ClientRectangle);
            
            if (Control.Image != null)
                drawingContext.DrawImage(Control.Image, ClientRectangle);
        }

        public override Size GetPreferredSize(Size availableSize)
        {
            if (Control.Image == null)
                return base.GetPreferredSize(availableSize);

            var specifiedWidth = Control.Width;
            var specifiedHeight = Control.Height;
            if (!double.IsNaN(specifiedWidth) && !double.IsNaN(specifiedHeight))
                return new Size(specifiedWidth, specifiedHeight);

            return Control.Image.Size;
        }
    }
}