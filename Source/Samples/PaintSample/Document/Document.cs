using Alternet.Base.Collections;
using Alternet.Drawing;
using System;

namespace PaintSample
{
    internal class Document
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

        public void AddShape(Shape shape)
        {
            shapes.Add(shape);
        }

        public void RemoveShape(Shape shape)
        {
            shapes.Remove(shape);
        }

        public void Paint(DrawingContext dc, Rect bounds)
        {
            dc.FillRectangle(Background, bounds);

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

        private void RaiseChanged() => Changed?.Invoke(this, EventArgs.Empty);
    }
}