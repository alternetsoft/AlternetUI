using Alternet.Drawing;
using System;

namespace PaintSample
{
    internal class Document : IDisposable
    {
        private bool isDisposed;

        private Bitmap? bitmap;

        private Action<DrawingContext>? previewAction;

        public Document()
        {
            Bitmap = CreateBitmap();
        }

        public Color BackgroundColor => Color.White;

        public event EventHandler? Changed;

        public Bitmap Bitmap
        {
            get => bitmap ?? throw new Exception();
            set
            {
                if (bitmap == value)
                    return;

                if (bitmap != null)
                    bitmap.Dispose();

                bitmap = value;
                RaiseChanged();
            }
        }

        public Action<DrawingContext>? PreviewAction
        {
            get => previewAction;
            set
            {
                previewAction = value;
                RaiseChanged();
            }
        }

        public void Modify(Action<DrawingContext> action)
        {
            using var dc = DrawingContext.FromImage(Bitmap);
            action(dc);
            RaiseChanged();
        }

        public void UpdatePreview()
        {
            RaiseChanged();
        }

        public void Paint(DrawingContext drawingContext)
        {
            drawingContext.DrawImage(Bitmap, new Point());
            if (previewAction != null)
                previewAction(drawingContext);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    Bitmap.Dispose();
                    Bitmap = null!;
                }

                isDisposed = true;
            }
        }

        private Bitmap CreateBitmap()
        {
            var bitmap = new Bitmap(new Size(600, 600));
            using var dc = DrawingContext.FromImage(bitmap);
            dc.FillRectangle(new SolidBrush(BackgroundColor), new Rect(new Point(), bitmap.Size));
            return bitmap;
        }

        private void RaiseChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }
}