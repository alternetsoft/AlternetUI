using Alternet.Drawing;
using Alternet.UI;
using System;
using static CustomControlsSample.TicTacToeGame;

namespace CustomControlsSample
{
    public class TicTacToeCell : Control
    {
        private PlayerMark? mark;

        private bool isWinningCell;

        public TicTacToeCell()
        {
        }

        public PlayerMark? Mark
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

        protected override ControlHandler CreateHandler()
        {
            return new CustomHandler();
        }

        public class CustomHandler : ControlHandler<TicTacToeCell>
        {
            private SolidBrush winningCellBrush = new SolidBrush(Color.Parse("#FFD0BF"));
            private Pen xPen = new Pen(Color.Red, 2);
            private Pen oPen = new Pen(Color.Blue, 2);

            protected override bool NeedsPaint => true;

            public override void OnPaint(DrawingContext dc)
            {
                var bounds = ClientRectangle;
                dc.FillRectangle(GetBackgroundBrush(), bounds);
                dc.DrawRectangle(Pens.Gray, bounds.InflatedBy(-3, -3));

                var minBoundsSize = Math.Min(bounds.Width, bounds.Height);

                var mark = Control.Mark;
                if (mark != null)
                {
                    var markSize = minBoundsSize * 0.7;
                    var markBounds = Rect.FromCenter(bounds.Center, new Size(markSize, markSize));

                    if (mark == PlayerMark.X)
                    {
                        dc.DrawLine(xPen, markBounds.TopLeft, markBounds.BottomRight);
                        dc.DrawLine(xPen, markBounds.BottomLeft, markBounds.TopRight);
                    }
                    else if (mark == PlayerMark.O)
                    {
                        dc.DrawEllipse(oPen, markBounds);
                    }
                    else
                        throw new Exception();
                }
            }

            protected override void OnAttach()
            {
                base.OnAttach();

                UserPaint = true;
                Control.MouseMove += Control_MouseMove;
                Control.MouseEnter += Control_MouseEnter;
                Control.MouseLeave += Control_MouseLeave;
                Control.MouseLeftButtonDown += Control_MouseLeftButtonDown;
                Control.MouseLeftButtonUp += Control_MouseLeftButtonUp;
            }

            protected override void OnDetach()
            {
                Control.MouseMove -= Control_MouseMove;
                Control.MouseEnter -= Control_MouseEnter;
                Control.MouseLeave -= Control_MouseLeave;
                Control.MouseLeftButtonDown -= Control_MouseLeftButtonDown;
                Control.MouseLeftButtonUp -= Control_MouseLeftButtonUp;

                base.OnDetach();
            }

            private void Control_MouseLeave(object? sender, EventArgs e)
            {
                Refresh();
            }

            private void Control_MouseEnter(object? sender, EventArgs e)
            {
                Refresh();
            }

            private void Control_MouseMove(object sender, MouseEventArgs e)
            {
                Refresh();
            }

            private void Control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {
                Control.RaiseClick(EventArgs.Empty);
            }

            private void Control_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            {
            }

            private Brush GetBackgroundBrush()
            {
                if (Control.IsWinningCell)
                    return winningCellBrush;

                if (IsMouseOver)
                    return CustomControlsColors.BackgroundHoveredBrush;

                return CustomControlsColors.BackgroundBrush;
            }
        }
    }
}