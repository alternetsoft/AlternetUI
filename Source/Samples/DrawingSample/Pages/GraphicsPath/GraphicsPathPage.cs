using Alternet.Drawing;
using Alternet.UI;

namespace DrawingSample
{
    internal sealed class GraphicsPathPage : DrawingPage
    {
        public override string Name => "Graphics Path";

        public override void Draw(DrawingContext dc, Rect bounds)
        {
            DrawInitialPath(dc);
        }

        private void DrawInitialPath(DrawingContext dc)
        {
            using var path = new GraphicsPath(dc);
            
            path.AddLines(new[] { new Point(10, 10), new Point(100, 100), new Point(100, 120) });
            path.AddLine(new Point(120, 120), new Point(140, 140));
            path.AddLineTo(new Point(140, 150));
            path.CloseFigure();

            path.AddEllipse(new Rect(60, 60, 40, 20));

            path.AddBezier(new Point(110, 100), new Point(250, 150), new Point(350, 150), new Point(200, 200));
            path.AddBezierTo(new Point(250, 150), new Point(350, 150), new Point(300, 100));
            path.CloseFigure();

            path.StartFigure();
            path.AddArc(new Point(200, 200), 100, 95, 90);

            dc.FillPath(Brushes.LightBlue, path);
            dc.DrawPath(Pens.Fuchsia, path);
        }

        protected override Control CreateSettingsControl()
        {
            var control = new GraphicsPathPageSettings();
            control.Initialize(this);
            return control;
        }
    }
}