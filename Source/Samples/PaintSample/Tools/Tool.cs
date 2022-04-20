using Alternet.UI;
using System;

namespace PaintSample
{
    internal abstract class Tool
    {
        private Control? canvas;

        protected Tool(Document document, ISelectedColors selectedColors, UndoService undoService)
        {
            Document = document;
            SelectedColors = selectedColors;
            UndoService = undoService;
        }

        protected Document Document { get; }

        protected ISelectedColors SelectedColors { get; }

        protected UndoService UndoService { get; }

        protected Control Canvas { get => canvas ?? throw new InvalidOperationException(); private set => canvas = value; }

        public void Activate(Control canvas)
        {
            if (this.canvas != null)
                throw new InvalidOperationException();

            this.canvas = canvas;
            canvas.MouseDown += Canvas_MouseDown;
            canvas.MouseUp += Canvas_MouseUp;
            canvas.MouseMove += Canvas_MouseMove;
            canvas.MouseCaptureLost += Canvas_MouseCaptureLost;
        }

        public void Deactivate()
        {
            if (canvas == null)
                throw new InvalidOperationException();

            canvas.MouseDown -= Canvas_MouseDown;
            canvas.MouseUp -= Canvas_MouseUp;
            canvas.MouseMove -= Canvas_MouseMove;
            canvas.MouseCaptureLost -= Canvas_MouseCaptureLost;

            canvas = null;
        }

        protected virtual void OnMouseCaptureLost()
        {
        }

        protected virtual void OnMouseMove(MouseEventArgs e)
        {
        }

        protected virtual void OnMouseUp(MouseButtonEventArgs e)
        {
        }

        protected virtual void OnMouseDown(MouseButtonEventArgs e)
        {
        }

        private void Canvas_MouseCaptureLost(object? sender, EventArgs e)
        {
            OnMouseCaptureLost();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            OnMouseMove(e);
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            OnMouseUp(e);
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnMouseDown(e);
        }
    }
}