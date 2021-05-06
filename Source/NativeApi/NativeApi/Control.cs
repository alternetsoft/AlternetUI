using System;
using System.Drawing;

namespace NativeApi.Api
{
    public abstract class Control
    {
        public event EventHandler? Paint { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? MouseEnter { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? MouseLeave { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? MouseMove { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? MouseLeftButtonDown { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? MouseLeftButtonUp { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? MouseClick { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? VisibleChanged { add => throw new Exception(); remove => throw new Exception(); }

        public void SetMouseCapture(bool value) => throw new Exception();

        public Control? Parent { get; }

        public SizeF Size { get; set; }

        public PointF Location { get; set; }

        public RectangleF Bounds { get; set; }

        public bool Visible { get; set; }

        public bool IsMouseOver { get; }

        public void AddChild(Control control) => throw new Exception();

        public void RemoveChild(Control control) => throw new Exception();

        public void Update() => throw new Exception();

        public Color BackgroundColor { get; set; }

        public Color ForegroundColor { get; set; }

        public virtual SizeF GetPreferredSize(SizeF availableSize) => throw new Exception();

        public DrawingContext OpenPaintDrawingContext() => throw new Exception();
        public DrawingContext OpenClientDrawingContext() => throw new Exception();
    }
}