using System;
using System.Drawing;

namespace NativeApi.Api
{
    public class DrawingContext : IDisposable
    {
        private DrawingContext() => throw new Exception();

        void IDisposable.Dispose() => throw new NotImplementedException();

        public void FillRectangle(RectangleF rectangle, Color color) => throw new Exception();
    }
}