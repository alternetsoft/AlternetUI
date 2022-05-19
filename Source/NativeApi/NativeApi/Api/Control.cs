using Alternet.UI;
using System;
using Alternet.Drawing;

namespace NativeApi.Api
{
    public abstract class Control
    {
        public event EventHandler? Paint { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? MouseEnter { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? MouseLeave { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? MouseClick { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? VisibleChanged { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? MouseCaptureLost { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? Destroyed { add => throw new Exception(); remove => throw new Exception(); }

        public void SetMouseCapture(bool value) => throw new Exception();

        public Control? ParentRefCounted { get; }

        public Size Size { get; set; }

        public Point Location { get; set; }

        public Rect Bounds { get; set; }

        public Size ClientSize { get; set; }

        public virtual Thickness IntrinsicLayoutPadding { get; }
        
        public virtual Thickness IntrinsicPreferredSizePadding { get; }

        public bool Visible { get; set; }

        public bool Enabled { get; set; }

        public bool UserPaint { get; set; }

        public bool IsMouseOver { get; }

        public bool HasWindowCreated { get; }

        public void AddChild(Control control) => throw new Exception();

        public void RemoveChild(Control control) => throw new Exception();

        public void Invalidate() => throw new Exception();

        public void Update() => throw new Exception();

        public Color BackgroundColor { get; set; }

        public Color ForegroundColor { get; set; }

        public Font? Font { get; set; }

        public virtual Size GetPreferredSize(Size availableSize) => throw new Exception();

        public DrawingContext OpenPaintDrawingContext() => throw new Exception();
        public DrawingContext OpenClientDrawingContext() => throw new Exception();

        public void BeginUpdate() => throw new Exception();
        public void EndUpdate() => throw new Exception();

        public static Control? GetFocusedControl() => throw new Exception();

        public static Control? HitTest(Point screenPoint) => throw new Exception();

        public Point ClientToScreen(Point point) => throw new Exception();
        public Point ScreenToClient(Point point) => throw new Exception();

        public bool IsMouseCaptured { get; }

        public bool Focus() => throw new Exception();

        public void BeginInit() => throw new Exception();
        public void EndInit() => throw new Exception();

        public void Destroy() => throw new Exception();
    }
}