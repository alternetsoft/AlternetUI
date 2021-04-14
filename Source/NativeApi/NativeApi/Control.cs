using System;
using System.Drawing;

namespace NativeApi.Api
{
    public abstract class Control : IDisposable
    {
        public event EventHandler? Paint { add => throw new Exception(); remove => throw new Exception(); }

        public SizeF Size { get; set; }

        public PointF Location { get; set; }

        public RectangleF Bounds { get; set; }

        public bool Visible { get; set; }

        public bool IsMouseDirectlyOver { get; }

        public static Control? FromScreenPoint(PointF point) => throw new Exception();

        void IDisposable.Dispose() => throw new NotImplementedException();

        public void AddChild(Control control) => throw new Exception();

        public void RemoveChild(Control control) => throw new Exception();

        public void Update() => throw new Exception();

        public virtual SizeF GetPreferredSize(SizeF availableSize) => throw new Exception();

        public DrawingContext OpenPaintDrawingContext() => throw new Exception();
    }
}