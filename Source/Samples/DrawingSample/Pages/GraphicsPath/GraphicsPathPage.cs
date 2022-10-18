using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Linq;

namespace DrawingSample
{
    internal sealed class GraphicsPathPage : DrawingPage
    {
        private RandomArt.Model randomArtModel = new RandomArt.Model();

        private RandomArt.RandomArtController? randomArtController;

        public override string Name => "Graphics Path";

        public override void Draw(DrawingContext dc, Rect bounds)
        {
            DrawDemoPath(dc);
            DrawRandomArtModel(dc);
        }

        protected override void OnCanvasChanged(Control? oldValue, Control? value)
        {
            if (oldValue != null)
            {
                if (randomArtController != null)
                {
                    randomArtController.Dispose();
                    randomArtController = null;
                }
            }

            if (value != null)
            {
                randomArtController = new RandomArt.RandomArtController(randomArtModel, value);
            }
        }

        protected override Control CreateSettingsControl()
        {
            var control = new GraphicsPathPageSettings();
            control.Initialize(this);
            return control;
        }

        private void DrawRandomArtModel(DrawingContext dc)
        {
            if (randomArtController == null)
                throw new Exception();

            var pen = Pens.Red;

            foreach (var path in randomArtModel.Paths)
            {
                using var graphicsPath = new GraphicsPath(dc);

                foreach (var segment in path.Segments)
                {
                    segment.Render(graphicsPath);
                    dc.FillPath(Brushes.LightGray, graphicsPath);
                    dc.DrawPath(pen, graphicsPath);
                }
            }

            var lastPoint = randomArtModel.Paths.LastOrDefault()?.Segments.LastOrDefault()?.End;
            if (lastPoint != null)
            {
                dc.DrawLine(pen, lastPoint.Value, randomArtController.TipPoint);
            }
        }

        private void DrawDemoPath(DrawingContext dc)
        {
            using var path = new GraphicsPath(dc);

            path.AddLines(new[] { new Point(10, 10), new Point(100, 100), new Point(100, 120) });
            path.AddLine(new Point(120, 120), new Point(140, 140));
            path.AddLineTo(new Point(140, 150));
            path.CloseFigure();

            path.AddEllipse(new Rect(60, 60, 40, 20));

            path.AddBezier(new Point(40, 10), new Point(250, 50), new Point(350, 50), new Point(110, 100));
            path.AddBezierTo(new Point(250, 50), new Point(350, 50), new Point(200, 10));
            path.CloseFigure();

            path.StartFigure();
            path.AddArc(new Point(110, 10), 100, 95, 85);

            path.AddRectangle(new Rect(60, 60, 40, 20));
            path.AddRoundedRectangle(new Rect(50, 50, 60, 40), 5);

            dc.FillPath(Brushes.LightBlue, path);
            dc.DrawPath(Pens.Fuchsia, path);
        }
    }
}