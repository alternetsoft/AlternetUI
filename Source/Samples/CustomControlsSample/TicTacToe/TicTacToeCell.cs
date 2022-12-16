using Alternet.Drawing;
using Alternet.UI;
using System;
using static CustomControlsSample.TicTacToeGame;

namespace CustomControlsSample
{
    public class TicTacToeCell : Control
    {
        public TicTacToeCell()
        {
        }

        protected override ControlHandler CreateHandler()
        {
            return new CustomHandler();
        }

        private PlayerMark? mark;

        public PlayerMark? Mark
        {
            get => mark;

            set
            {
                mark = value;
                Refresh();
            }
        }

        public class CustomHandler : ControlHandler<TicTacToeCell>
        {
            protected override bool NeedsPaint => true;

            public override void OnPaint(DrawingContext dc)
            {
                var bounds = ClientRectangle;
                dc.FillRectangle(GetBackgroundBrush(), bounds);

                var minBoundsSize = Math.Min(bounds.Width, bounds.Height);

                var mark = Control.Mark;
                if (mark != null)
                {
                    var markSize = minBoundsSize * 0.7;
                    var markBounds = Rect.FromCenter(bounds.Center, new Size(markSize, markSize));

                    if (mark == PlayerMark.X)
                    {
                        var xPen = Pens.Red;
                        dc.DrawLine(xPen, markBounds.TopLeft, markBounds.BottomRight);
                        dc.DrawLine(xPen, markBounds.BottomLeft, markBounds.TopRight);
                    }
                    else if (mark == PlayerMark.O)
                    {
                        var oPen = Pens.Blue;
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
                if (IsMouseOver)
                    return CustomControlsColors.BackgroundHoveredBrush;

                return CustomControlsColors.BackgroundBrush;
            }
        }
    }
}