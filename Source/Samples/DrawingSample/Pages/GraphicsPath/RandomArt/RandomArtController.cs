using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Linq;

namespace DrawingSample.RandomArt
{
    internal class RandomArtController : IDisposable
    {
        private readonly Control canvas;
        private readonly Model model;
        private readonly ToolSettings toolSettings = new();

        public RandomArtController(Model model, Control canvas, PathSegmentType pathSegmentType)
        {
            PathSegmentType = pathSegmentType;

            this.model = model;
            this.canvas = canvas;

            canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas.MouseMove += Canvas_MouseMove;
            canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
        }

        public bool IsDrawing { get; private set; }

        public Point TipPoint { get; private set; }

        public PathSegmentType PathSegmentType { get; set; }

        public void Dispose()
        {
            canvas.MouseLeftButtonDown -= Canvas_MouseLeftButtonDown;
            canvas.MouseMove -= Canvas_MouseMove;
            canvas.MouseLeftButtonUp -= Canvas_MouseLeftButtonUp;
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            canvas.CaptureMouse();
            model.Paths.Add(new Path());
            IsDrawing = true;
            TipPoint = e.GetPosition(canvas);
            model.Paths.Last().Segments.Add(SegmentFactory.CreateSegment(PathSegmentType, TipPoint));
            canvas.Invalidate();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsDrawing)
                return;

            TipPoint = e.GetPosition(canvas);
            model.Paths.Last().Segments.Last().TryAddSegmentParts(TipPoint, toolSettings);
            canvas.Invalidate();
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            canvas.ReleaseMouseCapture();
            IsDrawing = false;
            canvas.Invalidate();
        }
    }
}