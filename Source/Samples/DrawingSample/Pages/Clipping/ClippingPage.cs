using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;

namespace DrawingSample
{
    internal sealed class ClippingPage : DrawingPage
    {
        private readonly Timer timer;
        private readonly List<ClipAreaPart> clipAreaParts = [];

        private double x;
        private bool mouseDown;
        private ClipOperation selectedClipOperation = ClipOperation.Union;

        public ClippingPage()
        {
            timer = new Timer(TimeSpan.FromMilliseconds(5), Timer_Tick);
        }

        public enum ClipOperation
        {
            Union,
            Subtract,
        }

        public ClipOperation SelectedClipOperation
        {
            get => selectedClipOperation;
            set
            {
                selectedClipOperation = value;
            }
        }

        public override string Name => "Clipping";

        public override void OnActivated()
        {
            timer.Start();
        }

        public override void OnDeactivated()
        {
            timer.Stop();
        }

        public override void Draw(DrawingContext dc, Rect bounds)
        {
            Point[] GetPolygonPoints()
            {
                var lines = new List<Point>();
                var r = Math.Min(bounds.Width, bounds.Height) * 0.3;
                var c = bounds.Center;
                for (double a = 0; a <= 360; a += 10)
                {
                    lines.Add(MathUtils.GetPointOnCircle(c, r, a));
                }

#pragma warning disable
                return lines.ToArray();
#pragma warning restore
            }

            using var region = new Region(GetPolygonPoints());
            ApplyRegionParts(region);

            dc.Clip = region;

            if (x >= bounds.Width)
                x = 0;

            dc.FillRectangle(Brushes.SkyBlue, bounds);

            dc.PushTransform(TransformMatrix.CreateTranslation(x, 0));

            DrawScene(dc, bounds);

            dc.Clip = null;

            dc.Pop();

            dc.DrawText(
                "Click and drag the mouse to add or subtract rectangles to/from the clip region.",
                Control.DefaultFont,
                Brushes.Black,
                bounds.OffsetBy(0, 20),
                new TextFormat
                {
                    HorizontalAlignment = TextHorizontalAlignment.Center,
                    VerticalAlignment = TextVerticalAlignment.Top,
                    Wrapping = TextWrapping.Word
                });
        }

        public void ResetClipAreaParts()
        {
            clipAreaParts.Clear();
            Canvas?.Invalidate();
        }

        protected override void OnCanvasChanged(Control? oldValue, Control? value)
        {
            if (oldValue != null)
            {
                oldValue.MouseLeftButtonDown -= Canvas_MouseLeftButtonDown;
                oldValue.MouseLeftButtonUp -= Canvas_MouseLeftButtonUp;
                oldValue.MouseMove -= Canvas_MouseMove;
            }

            if (value != null)
            {
                value.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
                value.MouseMove += Canvas_MouseMove;
                value.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
            }
        }

        protected override Control CreateSettingsControl()
        {
            var control = new ClippingPageSettings();
            control.Initialize(this);
            return control;
        }

        private static void DrawScene(DrawingContext dc, Rect bounds)
        {
            var random = new Random(0);

            for (int i = 0; i < 1000; i++)
                dc.FillRectangle(
                    Brushes.White,
                    new Rect(random.Next(-(int)bounds.Width, (int)bounds.Width), random.Next(-(int)bounds.Height, (int)bounds.Height), 5, 5));

            for (int i = 0; i < 100; i++)
                dc.DrawImage(
                    Resources.LogoImage,
                    new Point(random.Next(-(int)bounds.Width, (int)bounds.Width), random.Next(-(int)bounds.Height, (int)bounds.Height)));
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
                AddClipAreaPart(e.GetPosition(Canvas));
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AddClipAreaPart(e.GetPosition(Canvas));
            Canvas!.CaptureMouse();
            mouseDown = true;
        }

        private void AddClipAreaPart(Point position)
        {
            clipAreaParts.Add(new ClipAreaPart(SelectedClipOperation, Rect.FromCenter(position, new Size(20, 20))));
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!mouseDown)
                return;
            mouseDown = false;
            Canvas!.ReleaseMouseCapture();
        }

        private void ApplyRegionParts(Region region)
        {
            foreach (var part in clipAreaParts)
            {
                switch (part.Operation)
                {
                    case ClipOperation.Union:
                        region.Union(part.Bounds);
                        break;

                    case ClipOperation.Subtract:
                        region.Subtract(part.Bounds);
                        break;

                    default:
                        throw new Exception();
                }
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            var canvas = Canvas;
            if (canvas == null)
                return;

            x++;

            canvas.Invalidate();
        }

        private class ClipAreaPart
        {
            public ClipAreaPart(ClipOperation operation, Rect bounds)
            {
                Operation = operation;
                Bounds = bounds;
            }

            public ClipOperation Operation { get; }

            public Rect Bounds { get; }
        }
    }
}