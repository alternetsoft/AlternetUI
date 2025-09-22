using System;

using Alternet.Drawing;
using Alternet.UI;
using Alternet.UI.Extensions;

namespace PaintSample
{
    public sealed class AirbrushTool : Tool
    {
        private State? state;

        public AirbrushTool(Func<PaintSampleDocument> getDocument, ISelectedColors selectedColors, UndoService undoService) :
            base(getDocument, selectedColors, undoService)
        {
        }

        public Coord Size { get; set; } = 20;

        public Coord Flow { get; set; } = 40;

        public override string Name => "Airbrush";

        protected override AbstractControl? CreateOptionsControl()
        {
            var control = new AirbrushToolOptionsControl
            {
                Tool = this
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

            state = new State(
                new SolidBrush(SelectedColors.Stroke),
                Mouse.GetPosition(Canvas),
                GetPointsPerTick(),
                new Bitmap(Document.Bitmap));

            state.Timer.Tick += Timer_Tick;
            state.Timer.Start();
            Document.PreviewAction = Draw;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (state != null)
                state.Center = Mouse.GetPosition(Canvas);
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

            using (var dc = Graphics.FromImage(state.PreviewBitmap))
            {
                for (int i = 0; i < state.PointsPerTick; i++)
                    DrawSinglePoint(dc, state.Brush, GetNextPoint());
            }

            Document.UpdatePreview();
        }

        private PointD GetNextPoint()
        {
            if (state == null)
                throw new InvalidOperationException();

            static PointD RandomPointInCircle(Random random, PointD center, float radius)
            {
                var angle = 2.0f * MathF.PI * random.NextFloat();
                var radiusX = random.NextFloat() * radius;
                var radiusY = random.NextFloat() * radius;
                return new PointD(center.X + radiusX * MathF.Cos(angle), center.Y + radiusY * MathF.Sin(angle));
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

        private void DrawSinglePoint(Graphics dc, Brush brush, PointD point)
        {
            dc.FillRectangle(brush, new RectD(point, new SizeD(1, 1)));
        }

        private void Draw(Graphics dc)
        {
            if (state == null)
                throw new InvalidOperationException();

            dc.DrawImage(state.PreviewBitmap, new PointD());
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
            public State(Brush brush, PointD center, int pointsPerTick, Bitmap previewBitmap)
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

            public PointD Center { get; set; }

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