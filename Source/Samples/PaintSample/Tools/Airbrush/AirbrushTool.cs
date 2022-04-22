using Alternet.Drawing;
using Alternet.UI;
using System;

namespace PaintSample
{
    internal sealed class AirbrushTool : Tool
    {
        private State? state;

        public AirbrushTool(Document document, ISelectedColors selectedColors, UndoService undoService) :
            base(document, selectedColors, undoService)
        {
        }

        public double Size { get; set; } = 20;

        public double Flow { get; set; } = 40;

        protected override Control? CreateOptionsControl()
        {
            var control = new AirbrushToolOptionsControl();
            control.Tool = this;
            return control;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;

            Canvas.CaptureMouse();

            if (state != null)
                throw new InvalidOperationException();

            state = new State(
                new SolidBrush(SelectedColors.Stroke),
                e.GetPosition(Canvas),
                GetPointsPerTick(),
                new Bitmap(Document.Bitmap));

            state.Timer.Tick += Timer_Tick;
            state.Timer.Start();
            Document.PreviewAction = Draw;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (state != null)
                state.Center = e.GetPosition(Canvas);
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
                CleanupState();
            }
        }

        private int GetPointsPerTick()
        {
            var radius = Size / 2;
            return (int)(Flow * radius * radius / 200);
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (state == null)
                throw new InvalidOperationException();

            using (var dc = DrawingContext.FromImage(state.PreviewBitmap))
            {
                for (int i = 0; i < state.PointsPerTick; i++)
                    DrawSinglePoint(dc, state.Brush, GetNextPoint());
            }

            Document.UpdatePreview();
        }

        private Point GetNextPoint()
        {
            if (state == null)
                throw new InvalidOperationException();

            static Point RandomPointInCircle(Random random, Point center, double radius)
            {
                var angle = 2.0 * Math.PI * random.NextDouble();
                var radiusX = random.NextDouble() * radius;
                var radiusY = random.NextDouble() * radius;
                return new Point(center.X + radiusX * Math.Cos(angle), center.Y + radiusY * Math.Sin(angle));
            }

            return RandomPointInCircle(state.Random, state.Center, Size / 2);
        }

        private void CleanupState()
        {
            if (state == null)
                throw new InvalidOperationException();

            state.Timer.Stop();
            state.Timer.Tick -= Timer_Tick;
            state.Dispose();
            state = null;
            Document.PreviewAction = null;
        }

        private void DrawSinglePoint(DrawingContext dc, Brush brush, Point point)
        {
            dc.FillRectangle(brush, new Rect(point, new Size(1, 1)));
        }

        private void Draw(DrawingContext dc)
        {
            if (state == null)
                throw new InvalidOperationException();

            dc.DrawImage(state.PreviewBitmap, new Point());
        }

        private void Cancel()
        {
            if (state == null)
                return;

            if (Canvas.IsMouseCaptured)
                Canvas.ReleaseMouseCapture();

            CleanupState();
        }

        private class State : IDisposable
        {
            public State(Brush brush, Point center, int pointsPerTick, Bitmap previewBitmap)
            {
                Brush = brush;
                Center = center;
                Timer = new Timer(TimeSpan.FromMilliseconds(10));
                PointsPerTick = pointsPerTick;
                PreviewBitmap = previewBitmap;
            }

            public Bitmap PreviewBitmap { get; private set; }

            public Brush Brush { get; }

            public int PointsPerTick { get; }

            public Point Center { get; set; }

            public Random Random { get; } = new Random();

            public Timer Timer { get; private set; }

            public void Dispose()
            {
                Timer.Dispose();
                Timer = null!;

                PreviewBitmap.Dispose();
                PreviewBitmap = null!;
            }
        }
    }
}