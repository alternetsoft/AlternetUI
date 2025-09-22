using Alternet.Drawing;
using Alternet.UI;

namespace PaintSample
{
    internal class SelectedColorDisplay : HiddenBorder
    {
        private Color selectedColor = Color.Empty;

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

        public override SizeD GetPreferredSize(PreferredSizeContext context)
        {
            return (50, 50);
        }

        private readonly Brush backgroundHatchBrush
            = new HatchBrush(BrushHatchStyle.BackwardDiagonal, Color.Gray);

        protected override void OnPaint(PaintEventArgs e)
        {
            var dc = e.Graphics;

            dc.FillRectangle(Brushes.LightGray, e.ClipRectangle);
            dc.FillRectangle(backgroundHatchBrush, e.ClipRectangle);
            dc.DrawLine(
                Pens.Gray,
                e.ClipRectangle.TopRight + new PointD(-1, 0),
                e.ClipRectangle.BottomRight + new PointD(-1, 0));

            var innerRect = e.ClipRectangle;
            innerRect.Inflate(-10, -10);

            dc.FillRectangle(new SolidBrush(SelectedColor), innerRect);
            dc.DrawRectangle(Pens.Gray, innerRect);
        }
    }
}