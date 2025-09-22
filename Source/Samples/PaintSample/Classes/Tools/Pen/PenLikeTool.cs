using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaintSample
{
    public abstract class PenLikeTool : Tool
    {
        private State? state;

        protected PenLikeTool(Func<PaintSampleDocument> getDocument, ISelectedColors selectedColors, UndoService undoService) :
            base(getDocument, selectedColors, undoService)
        {
        }

        public Coord Thickness { get; set; } = 5;
        
        public abstract Color PenColor { get; }

        protected override AbstractControl? CreateOptionsControl()
        {
            var control = new PenLikeToolOptionsControl
            {
                Tool = this,
            };
            return control;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;

            Canvas.CaptureMouse();

            if (state != null)
                throw new InvalidOperationException();

            Pen pen = new(PenColor, Thickness)
            {
                LineCap = LineCap.Round,
                LineJoin = LineJoin.Round
            };

            state = new State(pen, Mouse.GetPosition(Canvas));
            Document.PreviewAction = Draw;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (state != null)
            {
                state.Points.Add(Mouse.GetPosition(Canvas));
                Document.UpdatePreview();
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

        protected override void OnMouseUp(MouseEventArgs e)
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

        void DrawSinglePoint(Graphics dc, PointD point)
        {
            if (state == null)
                throw new InvalidOperationException();

            var width = state.Pen.Width;
            RectD rect;
            if (width == 1)
                rect = new RectD(point, new SizeD(width, width));
            else
            {
                Coord halfWidth = width / 2;
                rect = new RectD(point - new SizeD(halfWidth, halfWidth), new SizeD(width, width));
            }

            dc.FillEllipse(new SolidBrush(state.Pen.Color), rect);
        }

        private void Draw(Graphics dc)
        {
            if (state == null)
                throw new InvalidOperationException();

            if (state.Points.Count == 1)
                DrawSinglePoint(dc, state.Points.Single());
            else
            {
                PointD? prevPoint = null;

                foreach (PointD p in state.Points)
                {
                    if(prevPoint != null)
                        dc.DrawLine(state.Pen, (PointD)prevPoint, p);
                    prevPoint = p;
                }
            }
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
            public State(Pen pen, PointD firstPoint)
            {
                Pen = pen;
                Points.Add(firstPoint);
            }

            public Pen Pen { get; }

            public List<PointD> Points { get; } = new();
        }
    }
}