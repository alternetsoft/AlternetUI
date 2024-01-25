using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class TicTacToeCell : Control
    {
        private TicTacToeControl.PlayerMark? mark;

        private bool isWinningCell;

        public TicTacToeCell()
        {
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

        protected override ControlHandler CreateHandler()
        {
            return new CustomHandler();
        }

        public class CustomHandler : ControlHandler<TicTacToeCell>
        {
            private readonly SolidBrush winningCellBrush = new(Color.Parse("#FFD0BF"));
            private readonly Pen xPen = new(Color.Red, 2);
            private readonly Pen oPen = new(Color.Blue, 2);

            protected override bool NeedsPaint => true;

            public override void OnPaint(Graphics dc)
            {
                var bounds = Control.ClientRectangle;
                var brush = GetBackgroundBrush();
                if(brush != null)
                    dc.FillRectangle(brush, bounds);
                dc.DrawRectangle(Pens.Gray, bounds.InflatedBy(-3, -3));

                var minBoundsSize = Math.Min(bounds.Width, bounds.Height);

                var mark = Control.Mark;
                if (mark != null)
                {
                    var markSize = minBoundsSize * 0.7;
                    var markBounds = RectD.FromCenter(bounds.Center, new SizeD(markSize, markSize));

                    if (mark == TicTacToeControl.PlayerMark.X)
                    {
                        dc.DrawLine(xPen, markBounds.TopLeft, markBounds.BottomRight);
                        dc.DrawLine(xPen, markBounds.BottomLeft, markBounds.TopRight);
                    }
                    else if (mark == TicTacToeControl.PlayerMark.O)
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

                Control.UserPaint = true;
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
                Control.Refresh();
            }

            private void Control_MouseEnter(object? sender, EventArgs e)
            {
                Control.Refresh();
            }

            private void Control_MouseMove(object sender, MouseEventArgs e)
            {
                Control.Refresh();
            }

            private void Control_MouseLeftButtonDown(object sender, MouseEventArgs e)
            {
                Control.RaiseClick(EventArgs.Empty);
            }

            private void Control_MouseLeftButtonUp(object sender, MouseEventArgs e)
            {
            }

            private Brush? GetBackgroundBrush()
            {
                if (Control.IsWinningCell)
                    return winningCellBrush;

                if (Control.IsMouseOver)
                    return Brushes.LightYellow;

                return null;
            }
        }
    }
}