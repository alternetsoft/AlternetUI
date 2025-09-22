using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class TicTacToeCell : GraphicControl
    {
        private static readonly SolidBrush winningCellBrush = new(Color.Parse("#FFD0BF"));

        private TicTacToeControl.PlayerMark? mark;
        private bool isWinningCell;

        public TicTacToeCell()
        {
            ParentBackColor = true;
            ParentForeColor = true;
        }

        public TicTacToeControl.PlayerMark? Mark
        {
            get => mark;

            set
            {
                mark = value;
                Refresh();
            }
        }

        public bool IsWinningCell
        {
            get => isWinningCell;

            set
            {
                isWinningCell = value;
                Refresh();
            }
        }

        public override void DefaultPaint(PaintEventArgs e)
        {
            var dc = e.Graphics;
            var bounds = e.ClipRectangle;

            var brush = GetBackgroundBrush();
            if (brush != null)
                dc.FillRectangle(brush, bounds);
            else
                DrawDefaultBackground(e);

            dc.DrawRectangle(Pens.Gray, bounds.InflatedBy(-3, -3));

            var minBoundsSize = Math.Min(bounds.Width, bounds.Height);

            var mark = Mark;
            if (mark != null)
            {
                var markSize = minBoundsSize * 0.7f;
                var markBounds = RectD.FromCenter(bounds.Center, new SizeD(markSize, markSize));

                if (mark == TicTacToeControl.PlayerMark.X)
                {
                    var xPen = LightDarkColors.Red.LightOrDark(IsDarkBackground).GetAsPen(2);

                    dc.DrawLine(xPen, markBounds.TopLeft, markBounds.BottomRight);
                    dc.DrawLine(xPen, markBounds.BottomLeft, markBounds.TopRight);
                }
                else
                {
                    var oPen = LightDarkColors.Blue.LightOrDark(IsDarkBackground).GetAsPen(2);

                    dc.DrawEllipse(oPen, markBounds);
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            Refresh();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            Refresh();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            Refresh();
        }

        protected override void OnMouseLeftButtonDown(MouseEventArgs e)
        {
            RaiseClick();
        }

        private Brush? GetBackgroundBrush()
        {
            if (IsWinningCell)
                return winningCellBrush;

            if (IsMouseOver)
                return Brushes.LightYellow;

            return null;
        }
    }
}