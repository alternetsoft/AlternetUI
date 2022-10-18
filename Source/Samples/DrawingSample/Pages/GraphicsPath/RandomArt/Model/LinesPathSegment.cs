using Alternet.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace DrawingSample.RandomArt
{
    internal class LinesPathSegment : PathSegment
    {
        private List<Point> points = new List<Point>();

        public LinesPathSegment(Point start) : base(start)
        {
            points.Add(start);
        }

        public override PathSegmentType Type => PathSegmentType.Lines;

        public override Point End => points.Last();

        public override void Render(GraphicsPath path)
        {
            path.AddLines(points.ToArray());
        }

        public override void TryAddSegmentParts(Point tipPoint, ToolSettings toolSettings)
        {
            if (Utils.IsDistanceGreaterOrEqual(End, tipPoint, toolSettings.PartLength))
            {
                points.Add(Utils.GetJitteredPointAlongLineSegment(
                    End,
                    tipPoint,
                    toolSettings.PartLength,
                    toolSettings.Jitter,
                    toolSettings.Random));
            }
        }
    }
}