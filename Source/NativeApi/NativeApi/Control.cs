using System;
using System.Drawing;

namespace NativeApi.Api
{
    public abstract class Control : IDisposable
    {
        void IDisposable.Dispose() => throw new NotImplementedException();

        public void AddChild(Control control) => throw new Exception();

        public void RemoveChild(Control control) => throw new Exception();

        public SizeF Size { get; set; }

        public PointF Location { get; set; }

        public RectangleF Bounds { get; set; }

        public bool Visible { get; set; }

        public virtual SizeF GetPreferredSize(SizeF availableSize) => throw new Exception();
    }
}