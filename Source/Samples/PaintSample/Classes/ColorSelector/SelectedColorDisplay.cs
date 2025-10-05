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

            dc.FillRectangle(Brushes.LightGray, e.ClientRectangle);
            dc.FillRectangle(backgroundHatchBrush, e.ClientRectangle);
            dc.DrawLine(
                Pens.Gray,
                e.ClientRectangle.TopRight + new PointD(-1, 0),
                e.ClientRectangle.BottomRight + new PointD(-1, 0));

            var innerRect = e.ClientRectangle;
            innerRect.Inflate(-10, -10);

            dc.FillRectangle(new SolidBrush(SelectedColor), innerRect);
            dc.DrawRectangle(Pens.Gray, innerRect);
        }
    }
}