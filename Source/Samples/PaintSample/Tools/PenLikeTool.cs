using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaintSample
{
    internal abstract class PenLikeTool : Tool
    {
        private State? state;

        protected PenLikeTool(Document document, ISelectedColors selectedColors, UndoService undoService) :
            base(document, selectedColors, undoService)
        {
        }

        public double Thickness { get; set; } = 1;
        
        public abstract Color PenColor { get; }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;

            Canvas.CaptureMouse();

            if (state != null)
                throw new InvalidOperationException();

            state = new State(new Pen(PenColor, Thickness), e.GetPosition(Canvas));
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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape && state != null)
                Cancel();
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;

            if (state != null)
            {
                Canvas.ReleaseMouseCapture();
                UndoService.Do(() => Document.Modify(Draw));
                state = null;
                Document.PreviewAction = null;
            }
        }

        void DrawSinglePoint(DrawingContext dc)
        {
            if (state == null)
                throw new InvalidOperationException();

            var width = state.Pen.Width;
            Rect rect;
            var point = state.Points.Single();
            if (width == 1)
                rect = new Rect(point, new Size(width, width));
            else
            {
                double halfWidth = width / 2;
                rect = new Rect(point - new Size(halfWidth, halfWidth), new Size(width, width));
            }

            dc.FillEllipse(new SolidBrush(state.Pen.Color), rect);
        }

        private void Draw(DrawingContext dc)
        {
            if (state == null)
                throw new InvalidOperationException();

            if (state.Points.Count == 1)
                DrawSinglePoint(dc);
            else
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