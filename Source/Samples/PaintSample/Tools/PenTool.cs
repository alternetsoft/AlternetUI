using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;

namespace PaintSample
{
    internal class PenTool : Tool
    {
        private State? state;

        public PenTool(Document document, ISelectedColors selectedColors, UndoService undoService) :
            base(document, selectedColors, undoService)
        {
        }

        public double Thickness { get; set; } = 1;

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Canvas.CaptureMouse();

            if (state != null)
                throw new InvalidOperationException();

            state = new State(new Pen(SelectedColors.Stroke, Thickness), e.GetPosition(Canvas));
            Document.PreviewAction = Draw;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (state != null)
            {
                state.Points.Add(e.GetPosition(Canvas));
                Document.PreviewUpdated();
            }
        }

        protected override void OnMouseCaptureLost()
        {
            Cancel();
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (state != null)
            {
                Canvas.ReleaseMouseCapture();
                UndoService.Do(() => Document.Modify(Draw));
                state = null;
                Document.PreviewAction = null;
            }
        }

        private void Draw(DrawingContext dc)
        {
            if (state == null)
                throw new InvalidOperationException();

            dc.DrawLines(state.Pen, state.Points.ToArray());
        }

        private void Cancel()
        {
            if (state == null)
                return;

            if (Canvas.IsMouseCaptured)
                Canvas.ReleaseMouseCapture();

            state = null;
            Document.PreviewAction = null;
        }

        private class State
        {
            public State(Pen pen, Point firstPoint)
            {
                Pen = pen;
                Points.Add(firstPoint);
            }

            public Pen Pen { get; }

            public List<Point> Points { get; } = new List<Point>();
        }
    }
}