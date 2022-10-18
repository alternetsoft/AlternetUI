using Alternet.Drawing;
using System;

namespace DrawingSample.RandomArt
{
    internal static class SegmentFactory
    {
        public static PathSegment CreateSegment(PathSegmentType type, Point start) => type switch
        {
            PathSegmentType.Lines => new LinesPathSegment(start),
            PathSegmentType.Curves => new CurvesPathSegment(start),
            _ => throw new Exception(),
        };
    }
}