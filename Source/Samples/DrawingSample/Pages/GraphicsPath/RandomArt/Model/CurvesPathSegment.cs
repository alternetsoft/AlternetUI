using Alternet.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace DrawingSample.RandomArt
{
    internal class CurvesPathSegment : PathSegment
    {
        private List<Curve> curves = new List<Curve>();

        public CurvesPathSegment(PointD start) : base(start)
        {
        }

        public override PathSegmentType Type => PathSegmentType.Curves;

        public override PointD End => curves.Count > 0 ? curves.Last().End : Start;

        public override void Render(GraphicsPath path)
        {
            path.StartFigure(Start);

            foreach (var arc in curves)
                path.AddBezierTo(arc.Control1, arc.Control2, arc.End);

            if (IsClosed)
                path.CloseFigure();
        }

        public override void TryAddSegmentParts(PointD tipPoint, ToolSettings toolSettings)
        {
            if (Utils.IsDistanceGreaterOrEqual(End, tipPoint, toolSettings.PartLength))
            {
                var control1 = Utils.GetJitteredPointAlongLineSegment(
                    End,
                    tipPoint,
                    toolSettings.PartLength / 3,
                    toolSettings.Jitter,
                    toolSettings.Random);

                var control2 = Utils.GetJitteredPointAlongLineSegment(
                    End,
                    tipPoint,
                    toolSettings.PartLength - toolSettings.PartLength / 3,
                    toolSettings.Jitter,
                    toolSettings.Random);

                curves.Add(new Curve(control1, control2, tipPoint));
            }
        }

        private class Curve
        {
            public Curve(PointD control1, PointD control2, PointD end)
            {
                Control1 = control1;
                Control2 = control2;
                End = end;
            }

            public PointD Control1 { get; }

            public PointD Control2 { get; }

            public PointD End { get; }
        }
    }
}