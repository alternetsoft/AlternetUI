using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Linq;

namespace DrawingSample.RandomArt
{
    internal class RandomArtController : IDisposable
    {
        private readonly Control canvas;
        private Model model;
        private bool drawing;

        private ToolSettings toolSettings = new ToolSettings();

        public RandomArtController(Model model, Control canvas, PathSegmentType pathSegmentType)
        {
            PathSegmentType = pathSegmentType;

            this.model = model;
            this.canvas = canvas;

            canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas.MouseMove += Canvas_MouseMove;
            canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
        }

        public Point TipPoint { get; private set; }

        public PathSegmentType PathSegmentType { get; set; }

        public void Dispose()
        {
            canvas.MouseLeftButtonDown -= Canvas_MouseLeftButtonDown;
            canvas.MouseMove -= Canvas_MouseMove;
            canvas.MouseLeftButtonUp -= Canvas_MouseLeftButtonUp;
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            canvas.CaptureMouse();
            model.Paths.Add(new Path());
            drawing = true;
            TipPoint = e.GetPosition(canvas);
            model.Paths.Last().Segments.Add(SegmentFactory.CreateSegment(PathSegmentType, TipPoint));
            canvas.Invalidate();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!drawing)
                return;

            TipPoint = e.GetPosition(canvas);
            model.Paths.Last().Segments.Last().TryAddSegmentParts(TipPoint, toolSettings);
            canvas.Invalidate();
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            canvas.ReleaseMouseCapture();
            drawing = false;
        }
    }
}