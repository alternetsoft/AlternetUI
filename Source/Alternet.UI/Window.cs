using System;
using System.Drawing;

namespace Alternet.UI
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class Window : Control
    {
        public Window()
        {
            Bounds = new RectangleF(100, 100, 400, 400);
        }

        private string? title = null;

        public event EventHandler? TitleChanged;

        public string? Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
                TitleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public SizeF Size
        {
            get
            {
                return Bounds.Size;
            }

            set
            {
                Handler.Bounds = new RectangleF(Bounds.Location, value);
            }
        }

        public PointF Location
        {
            get
            {
                return Bounds.Location;
            }

            set
            {
                Bounds = new RectangleF(value, Bounds.Size);
            }
        }

        public RectangleF Bounds
        {
            get
            {
                return Handler.Bounds;
            }

            set
            {
                Handler.Bounds = value;
            }
        }

        protected override ControlHandler CreateHandler()
        {
            return new NativeWindowHandler(this);
        }
    }
}