using System;
using System.Drawing;

namespace Alternet.UI
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class Window : Control
    {
        private string? title = null;

        public Window()
        {
            Bounds = new RectangleF(100, 100, 400, 400);
        }

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

        internal override bool IsTopLevel => true;

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

        public override float Width { get => Size.Width; set => Size = new SizeF(value, Height); }

        public override float Height { get => Size.Height; set => Size = new SizeF(Width, value); }

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
    }
}