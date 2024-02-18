using Alternet.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace DrawingSample.RandomArt
{
    internal class LinesPathSegment : PathSegment
    {
        private List<PointD> points = new List<PointD>();

        public LinesPathSegment(PointD start) : base(start)
        {
            points.Add(start);
        }

        public override PathSegmentType Type => PathSegmentType.Lines;

        public override PointD End => points.Last();

        public override void Render(GraphicsPath path)
        {
            path.AddLines(points.ToArray());

            if (IsClosed)
                path.CloseFigure();
        }

        public override void TryAddSegmentParts(PointD tipPoint, ToolSettings toolSettings)
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