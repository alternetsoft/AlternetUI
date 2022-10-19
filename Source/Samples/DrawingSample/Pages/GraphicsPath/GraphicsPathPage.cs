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

        private RandomArt.PathSegmentType pathSegmentType = RandomArt.PathSegmentType.Curves;

        private FillMode pathFillMode;

        public override string Name => "Graphics Path";

        public RandomArt.PathSegmentType PathSegmentType
        {
            get => pathSegmentType;
            set
            {
                pathSegmentType = value;

                if (randomArtController != null)
                    randomArtController.PathSegmentType = value;
            }
        }

        public FillMode PathFillMode
        {
            get => pathFillMode;
            set
            {
                pathFillMode = value;
                Canvas?.Invalidate();
            }
        }

        public override void Draw(DrawingContext dc, Rect bounds)
        {
            DrawRandomArtModel(dc);
            DrawDemoForeground(dc, bounds);
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
                randomArtController = new RandomArt.RandomArtController(randomArtModel, value, PathSegmentType);
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

            Pen? lastPen = null;

            foreach (var path in randomArtModel.Paths)
            {
                using var graphicsPath = new GraphicsPath(dc) { FillMode = PathFillMode };

                foreach (var segment in path.Segments)
                {
                    segment.Render(graphicsPath);
                    dc.FillPath(path.GetBrush(), graphicsPath);

                    lastPen = path.GetPen();
                    dc.DrawPath(lastPen, graphicsPath);
                }
            }

            if (randomArtController.IsDrawing && lastPen != null)
            {
                var lastPoint = randomArtModel.Paths.LastOrDefault()?.Segments.LastOrDefault()?.End;
                if (lastPoint != null)
                    dc.DrawLine(lastPen, lastPoint.Value, randomArtController.TipPoint);
            }
        }

        private void DrawDemoForeground(DrawingContext dc, Rect bounds)
        {
            dc.DrawText(
                "\n\nClick and drag here to draw.\nYou can select the path segment type to draw in the combo box in the panel to the right.",
                Control.DefaultFont,
                Brushes.Black,
                bounds,
                new TextFormat { Wrapping = TextWrapping.Word, HorizontalAlignment = TextHorizontalAlignment.Center });

            using var path = new GraphicsPath(dc) { FillMode = PathFillMode };

            path.AddLines(new[] { new Point(210, 210), new Point(300, 300), new Point(300, 320) });
            path.AddLine(new Point(320, 320), new Point(340, 340));
            path.AddLineTo(new Point(340, 350));
            path.CloseFigure();

            path.AddEllipse(new Rect(260, 260, 40, 20));

            path.AddBezier(new Point(240, 210), new Point(450, 250), new Point(550, 250), new Point(310, 300));
            path.AddBezierTo(new Point(450, 250), new Point(550, 250), new Point(400, 210));
            path.CloseFigure();

            path.StartFigure(new Point(310, 210));
            path.AddArc(new Point(310, 210), 30, 295, 285);

            path.AddRectangle(new Rect(260, 260, 40, 20));
            path.AddRoundedRectangle(new Rect(250, 250, 60, 40), 5);

            dc.FillPath(Brushes.LightGreen, path);
            dc.DrawPath(Pens.Fuchsia, path);
        }

        public void Clear()
        {
            randomArtModel.Paths.Clear();
            Canvas?.Invalidate();
        }

        public void CloseLastFigure()
        {
            randomArtModel.Paths.LastOrDefault()?.Segments.LastOrDefault()?.CloseFigure();
            Canvas?.Invalidate();
        }
    }
}