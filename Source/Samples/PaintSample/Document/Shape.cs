using Alternet.Drawing;
using System;

namespace PaintSample
{
    internal abstract class Shape
    {
        public event EventHandler? Changed;

        private Pen? pen;

        private Brush? brush;

        public Pen? Pen
        {
            get => pen;
            set
            {
                pen = value;
                RaiseChanged();
            }
        }

        public Brush? Brush
        {
            get => brush;
            set
            {
                brush = value;
                RaiseChanged();
            }
        }

        public abstract void Paint(DrawingContext dc);

        protected void RaiseChanged() => Changed?.Invoke(this, EventArgs.Empty);
    }
}