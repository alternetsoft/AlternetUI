using Alternet.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace DrawingSample.RandomArt
{
    internal class CurvesPathSegment : PathSegment
    {
        public CurvesPathSegment(Point start) : base(start)
        {
        }

        public override PathSegmentType Type => PathSegmentType.Curves;

        List<Curve> curves = new List<Curve>();

        class Curve
        {
            public Curve(Point control1, Point control2, Point end)
            {
                Control1 = control1;
                Control2 = control2;
                End = end;
            }

            public Point Control1 { get; }
            public Point Control2 { get; }
            public Point End { get; }
        }

        public override Point End => curves.Count > 0 ? curves.Last().End : Start;

        public override void Render(GraphicsPath path)
        {
            path.AddLine(Start, Start);

            foreach (var arc in curves)
                path.AddBezierTo(arc.Control1, arc.Control2, arc.End);
        }

        public override void TryAddSegmentParts(Point tipPoint, ToolSettings toolSettings)
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
    }
}