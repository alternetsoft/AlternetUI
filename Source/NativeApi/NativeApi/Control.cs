using System;
using System.Drawing;

namespace NativeApi.Api
{
    public abstract class Control : IDisposable
    {
        public event EventHandler? Paint { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? MouseEnter { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? MouseLeave { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? MouseMove { add => throw new Exception(); remove => throw new Exception(); }

        public SizeF Size { get; set; }

        public PointF Location { get; set; }

        public RectangleF Bounds { get; set; }

        public bool Visible { get; set; }

        public bool IsMouseOver { get; }

        void IDisposable.Dispose() => throw new NotImplementedException();

        public void AddChild(Control control) => throw new Exception();

        public void RemoveChild(Control control) => throw new Exception();

        public void Update() => throw new Exception();

        public virtual SizeF GetPreferredSize(SizeF availableSize) => throw new Exception();

        public DrawingContext OpenPaintDrawingContext() => throw new Exception();
    }
}