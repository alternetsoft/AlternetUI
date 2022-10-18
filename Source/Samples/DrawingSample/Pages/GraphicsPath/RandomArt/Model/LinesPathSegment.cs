using Alternet.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace DrawingSample.RandomArt
{
    internal class LinesPathSegment : PathSegment
    {
        public LinesPathSegment(Point start) : base(start)
        {
            Points.Add(start);
        }

        public override PathSegmentType Type => PathSegmentType.Lines;

        public List<Point> Points { get; } = new List<Point>();

        public override Point End => Points.Last();

        public override void Render(GraphicsPath path)
        {
            path.AddLines(Points.ToArray());
        }

        public override void TryAddSegmentParts(Point tipPoint, ToolSettings toolSettings)
        {
            if (Utils.IsDistanceGreaterOrEqual(End, tipPoint, toolSettings.PartLength))
            {
                Points.Add(Utils.GetJitteredPointAlongLineSegment(
                    End,
                    tipPoint,
                    toolSettings.PartLength,
                    toolSettings.Jitter,
                    toolSettings.Random));
            }
        }
    }
}