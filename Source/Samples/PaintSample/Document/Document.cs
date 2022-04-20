using Alternet.Base.Collections;
using Alternet.Drawing;
using System;

namespace PaintSample
{
    internal class Document : IDisposable
    {
        private Collection<Shape> shapes = new Collection<Shape>();
        private Brush background = Brushes.White;

        public Document()
        {
            shapes.ItemInserted += Shapes_ItemInserted;
            shapes.ItemRemoved += Shapes_ItemRemoved;
        }

        public event EventHandler? Changed;

        public Brush Background
        {
            get => background;

            set
            {
                if (background == value)
                    return;

                background = value;
                RaiseChanged();
            }
        }

        Image? image;
        public Image Image
        {
            get
            {
                if (image == null)
                    Render();

                return image ?? throw new InvalidOperationException();
            }
        }

        private bool isDisposed;

        public void AddShape(Shape shape)
        {
            shapes.Add(shape);
        }

        public void RemoveShape(Shape shape)
        {
            shapes.Remove(shape);
        }

        void Render()
        {
            image?.Dispose();
            image = new Bitmap(new Size(600, 600));
            using var dc = DrawingContext.FromImage(image);

            dc.FillRectangle(Brushes.White, new Rect(new Point(), image.Size));

            foreach (var shape in shapes)
                shape.Paint(dc);
        }

        private void Shapes_ItemInserted(object? sender, CollectionChangeEventArgs<Shape> e)
        {
            e.Item.Changed += Shape_Changed;
            RaiseChanged();
        }

        private void Shape_Changed(object? sender, EventArgs e)
        {
            RaiseChanged();
        }

        private void Shapes_ItemRemoved(object? sender, CollectionChangeEventArgs<Shape> e)
        {
            e.Item.Changed -= Shape_Changed;
        }

        private void RaiseChanged()
        {
            Render();
            Changed?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    image?.Dispose();
                    image = null;
                }

                isDisposed = true;
            }
        }


        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}