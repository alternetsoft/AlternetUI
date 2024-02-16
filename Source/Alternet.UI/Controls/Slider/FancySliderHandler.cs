#nullable disable

using System;
using Alternet.Drawing;
using Alternet.UI;

namespace Alternet.UI
{
    internal class FancySliderHandler : SliderHandler
    {
        private bool dragging = false;
        private PointD dragStartPosition;

        public override SliderOrientation Orientation { get; set; }

        public override SliderTickStyle TickStyle { get; set; }

        protected override void OnAttach()
        {
            base.OnAttach();
            Control.UserPaint = true;
            Control.ValueChanged += Control_ValueChanged;
            Control.MouseMove += Control_MouseMove;
            Control.MouseEnter += Control_MouseEnter;
            Control.MouseLeave += Control_MouseLeave;
            Control.MouseLeftButtonDown += Control_MouseLeftButtonDown;
            Control.MouseLeftButtonUp += Control_MouseLeftButtonUp;
            Control.MouseWheel += Control_MouseWheel;
        }

        protected override void OnDetach()
        {
            Control.ValueChanged -= Control_ValueChanged;
            Control.MouseMove -= Control_MouseMove;
            Control.MouseEnter -= Control_MouseEnter;
            Control.MouseLeave -= Control_MouseLeave;
            Control.MouseLeftButtonDown -= Control_MouseLeftButtonDown;
            Control.MouseLeftButtonUp -= Control_MouseLeftButtonUp;
            Control.MouseWheel -= Control_MouseWheel;

            base.OnDetach();
        }

        private void Control_MouseLeave(object sender, EventArgs e)
        {
            Control.Refresh();
        }

        private void Control_MouseEnter(object sender, EventArgs e)
        {
            Control.Refresh();
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                var location = e.GetPosition(Control);
                int opos = Control.Value;
                int pos = opos;
                var delta = dragStartPosition.Y - location.Y;
                pos += (int)delta;
                int min = Control.Minimum;
                int max = Control.Maximum;
                if (pos < min) pos = min;
                if (pos > max) pos = max;
                if (pos != opos)
                {
                    Control.Value = pos;
                    dragStartPosition = location;
                }
            }
        }

        private void Control_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            CaptureMouse();

            var location = e.GetPosition(Control);

            SetFocus();
            if (MathUtils.IsPointInCircle(
                location,
                ((FancySlider)Control).GetControlCenter(),
                ((FancySlider)Control).GetControlRadius()))
            {
                dragStartPosition = location;
                dragging = true;
            }
        }

        private void Control_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            ReleaseMouseCapture();

            dragging = false;
        }

        private void Control_MouseWheel(object sender, MouseEventArgs e)
        {
            int pos;
            int m;
            var delta = e.Delta;
            if (delta > 0)
            {
                delta = 1;
                pos = Control.Value;
                pos += delta;
                m = Control.Maximum;
                if (pos > m)
                    pos = m;
                Control.Value = pos;
            }
            else if (delta < 0)
            {
                delta = -1;
                pos = Control.Value;
                pos += delta;
                m = Control.Minimum;
                if (pos < m)
                    pos = m;
                Control.Value = pos;
            }
        }

        private void Control_ValueChanged(object sender, EventArgs e)
        {
            Control.Refresh();
        }
    }
}