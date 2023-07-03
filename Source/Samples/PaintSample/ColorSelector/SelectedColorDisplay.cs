using Alternet.Drawing;
using Alternet.UI;

namespace PaintSample
{
    internal class SelectedColorDisplay : Control
    {
        private Color selectedColor;

        public SelectedColorDisplay()
        {
            UserPaint = true;
            ToolTip = "Currently selected color";
        }

        public Color SelectedColor
        {
            get => selectedColor;
            set
            {
                selectedColor = value;
                Invalidate();
            }
        }

        public override Size GetPreferredSize(Size availableSize)
        {
            return new Size(50, 50);
        }

        Brush backgroundHatchBrush = new HatchBrush(BrushHatchStyle.BackwardDiagonal, Color.Gray);

        protected override void OnPaint(PaintEventArgs e)
        {
            var dc = e.DrawingContext;

            dc.FillRectangle(Brushes.LightGray, e.Bounds);
            dc.FillRectangle(backgroundHatchBrush, e.Bounds);
            dc.DrawLine(Pens.Gray, e.Bounds.TopRight + new Size(-1, 0), e.Bounds.BottomRight + new Size(-1, 0));

            var innerRect = e.Bounds;
            innerRect.Inflate(-10, -10);

            dc.FillRectangle(new SolidBrush(SelectedColor), innerRect);
            dc.DrawRectangle(Pens.Gray, innerRect);

            //dc.DrawLine(Pens.Black, innerRect.TopLeft + new Size(1, 1), innerRect.TopRight + new Size(-1, 1));
            //dc.DrawLine(Pens.Black, innerRect.TopLeft + new Size(1, 1), innerRect.BottomLeft + new Size(1, -1));
        }
    }
}